using System.Diagnostics;
using FluentFTP;

// ---------------------------------------------------------------------------
// Energy.Publish
// ---------------------------------------------------------------------------
// For each selected target this tool:
//   1) Runs the matching publish shell script (shells/publish-<target>.sh),
//      which builds the Release output via `dotnet publish`.
//   2) ONLY if the script succeeds (exit code 0), connects to the FTP server
//      and recursively uploads the published output, OVERWRITING every existing
//      file. Remote folders are created on demand.
//
// Usage:
//   dotnet run --project Energy.Publish            # publish + upload BOTH api + web
//   dotnet run --project Energy.Publish -- api     # publish + upload only the API
//   dotnet run --project Energy.Publish -- web     # publish + upload only the Web
//
// Credentials / host can be overridden with environment variables:
//   ENERGY_FTP_HOST, ENERGY_FTP_USER, ENERGY_FTP_PASSWORD
// ---------------------------------------------------------------------------

// FTP connection (override via environment variables in CI / production).
var host = Environment.GetEnvironmentVariable("ENERGY_FTP_HOST") ?? "31.186.11.158";
var user = Environment.GetEnvironmentVariable("ENERGY_FTP_USER") ?? "wat14bcomtr";
var password = Environment.GetEnvironmentVariable("ENERGY_FTP_PASSWORD") ?? "Wattiw@1";

// Repository root: ../ relative to this project's build output folder.
var repoRoot = Path.GetFullPath(Path.Combine(AppContext.BaseDirectory, "..", "..", "..", ".."));

// Deployment targets: local publish folder -> remote site root on the FTP server.
var allTargets = new[]
{
    new DeployTarget(
        Name: "api",
        LocalPath: Path.Combine(repoRoot, "Energy.Api", "bin", "Release", "net10.0", "win-x64", "publish"),
        RemoteRoot: "/energyapi.wattiw.com.tr",
        ScriptPath: Path.Combine(repoRoot, "Energy.Publish", "shells", "publish-api.sh")),
    new DeployTarget(
        Name: "web",
        LocalPath: Path.Combine(repoRoot, "Energy.Web", "bin", "Release", "net10.0", "win-x64", "publish"),
        RemoteRoot: "/energ.wattiw.com.tr",
        ScriptPath: Path.Combine(repoRoot, "Energy.Publish", "shells", "publish-web.sh")),
};

// Select which targets to publish based on the first CLI argument.
var selector = args.FirstOrDefault()?.Trim().ToLowerInvariant();
var targets = selector switch
{
    null or "" or "all" => allTargets,
    "api" => allTargets.Where(t => t.Name == "api").ToArray(),
    "web" => allTargets.Where(t => t.Name == "web").ToArray(),
    _ => Array.Empty<DeployTarget>()
};

if (targets.Length == 0)
{
    Console.Error.WriteLine($"Unknown target '{selector}'. Use: api | web | all (default).");
    return 1;
}

Console.WriteLine($"FTP host : {host}");
Console.WriteLine($"FTP user : {user}");
Console.WriteLine($"Targets  : {string.Join(", ", targets.Select(t => t.Name))}");
Console.WriteLine();

var exitCode = 0;

await using var client = new AsyncFtpClient(host, user, password);
client.Config.RetryAttempts = 3;
client.Config.SocketKeepAlive = true;

foreach (var target in targets)
{
    Console.WriteLine($"===== {target.Name.ToUpperInvariant()} =====");
    Console.WriteLine($"Script: {target.ScriptPath}");
    Console.WriteLine($"Local : {target.LocalPath}");
    Console.WriteLine($"Remote: {target.RemoteRoot}");

    // 1) Run the publish shell script. Only continue to the FTP upload if it
    //    completes successfully (exit code 0).
    var scriptExit = await RunScriptAsync(target.ScriptPath);
    if (scriptExit != 0)
    {
        Console.Error.WriteLine($"  ! Publish script failed (exit code {scriptExit}). Skipping FTP upload.\n");
        exitCode = 1;
        continue;
    }

    if (!Directory.Exists(target.LocalPath))
    {
        Console.Error.WriteLine($"  ! Publish folder not found after script ran. Skipping.\n");
        exitCode = 1;
        continue;
    }

    // 2) Connect (lazily, after the script succeeded) and upload.
    try
    {
        if (!client.IsConnected)
        {
            Console.WriteLine("Connecting...");
            await client.AutoConnect();
            Console.WriteLine("Connected.");
        }

        var (uploaded, failed) = await UploadFolderAsync(client, target.LocalPath, target.RemoteRoot);
        Console.WriteLine($"  -> {uploaded} file(s) uploaded, {failed} failed.\n");
        if (failed > 0) exitCode = 1;
    }
    catch (Exception ex)
    {
        Console.Error.WriteLine($"  ! Upload failed: {ex.Message}\n");
        exitCode = 1;
    }
}

if (client.IsConnected)
{
    Console.WriteLine("Disconnecting...");
    await client.Disconnect();
}
Console.WriteLine(exitCode == 0 ? "Done." : "Completed with errors.");
return exitCode;

// ---------------------------------------------------------------------------
// Runs a shell (.sh) script via zsh and streams its output. Returns the
// script's exit code (0 = success).
// ---------------------------------------------------------------------------
static async Task<int> RunScriptAsync(string scriptPath)
{
    if (!File.Exists(scriptPath))
    {
        Console.Error.WriteLine($"  ! Script not found: {scriptPath}");
        return 127;
    }

    var psi = new ProcessStartInfo
    {
        FileName = "/bin/zsh",
        UseShellExecute = false,
        RedirectStandardOutput = true,
        RedirectStandardError = true,
    };
    psi.ArgumentList.Add(scriptPath);

    using var process = new Process { StartInfo = psi, EnableRaisingEvents = true };
    process.OutputDataReceived += (_, e) => { if (e.Data is not null) Console.WriteLine(e.Data); };
    process.ErrorDataReceived += (_, e) => { if (e.Data is not null) Console.Error.WriteLine(e.Data); };

    process.Start();
    process.BeginOutputReadLine();
    process.BeginErrorReadLine();
    await process.WaitForExitAsync();

    return process.ExitCode;
}

// ---------------------------------------------------------------------------
// Recursively uploads every file under localRoot to remoteRoot, overwriting
// existing files and creating remote directories on demand.
// ---------------------------------------------------------------------------
static async Task<(int Uploaded, int Failed)> UploadFolderAsync(
    AsyncFtpClient client, string localRoot, string remoteRoot)
{
    var files = Directory.GetFiles(localRoot, "*", SearchOption.AllDirectories);
    var uploaded = 0;
    var failed = 0;

    foreach (var localFile in files)
    {
        var relative = Path.GetRelativePath(localRoot, localFile).Replace('\\', '/');
        var remotePath = $"{remoteRoot.TrimEnd('/')}/{relative}";

        var status = await client.UploadFile(
            localFile,
            remotePath,
            FtpRemoteExists.Overwrite,
            createRemoteDir: true,
            FtpVerify.None);

        if (status == FtpStatus.Success)
        {
            uploaded++;
            Console.WriteLine($"  + {relative}");
        }
        else
        {
            failed++;
            Console.Error.WriteLine($"  x {relative} ({status})");
        }
    }

    return (uploaded, failed);
}

internal sealed record DeployTarget(string Name, string LocalPath, string RemoteRoot, string ScriptPath);
