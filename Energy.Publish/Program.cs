using System.Diagnostics;
using FluentFTP;

// ---------------------------------------------------------------------------
// Energy.Publish
// ---------------------------------------------------------------------------
// For each selected target this tool:
//   1) Runs the matching publish shell script (shells/publish-<target>.sh),
//      which builds the Release output via `dotnet publish`.
//   2) ONLY if the script succeeds (exit code 0), connects to the FTP server
//      and recursively uploads the published output using SEVERAL PARALLEL
//      connections, OVERWRITING every existing file. Remote folders are created
//      on demand. Live progress ([done/total]) is printed to the console.
//
// Usage:
//   dotnet run --project Energy.Publish            # publish + upload BOTH api + web
//   dotnet run --project Energy.Publish -- api     # publish + upload only the API
//   dotnet run --project Energy.Publish -- web     # publish + upload only the Web
//
// Credentials / host / parallelism can be overridden with environment variables:
//   ENERGY_FTP_HOST, ENERGY_FTP_USER, ENERGY_FTP_PASSWORD, ENERGY_FTP_PARALLELISM
// ---------------------------------------------------------------------------

// FTP connection (override via environment variables in CI / production).
var host = Environment.GetEnvironmentVariable("ENERGY_FTP_HOST") ?? "31.186.11.158";
var user = Environment.GetEnvironmentVariable("ENERGY_FTP_USER") ?? "wat14bcomtr";
var password = Environment.GetEnvironmentVariable("ENERGY_FTP_PASSWORD") ?? "Wattiw@1";

// Number of parallel FTP connections used for uploading (speeds up transfers).
// Override with ENERGY_FTP_PARALLELISM. Defaults to 4.
var parallelism = int.TryParse(Environment.GetEnvironmentVariable("ENERGY_FTP_PARALLELISM"), out var p) && p > 0
    ? p
    : 4;

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
        RemoteRoot: "/energy.wattiw.com.tr",
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

Console.WriteLine($"FTP host    : {host}");
Console.WriteLine($"FTP user    : {user}");
Console.WriteLine($"Parallelism : {parallelism} connection(s)");
Console.WriteLine($"Targets     : {string.Join(", ", targets.Select(t => t.Name))}");
Console.WriteLine();

var exitCode = 0;

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

    // 2) Upload using multiple parallel FTP connections.
    try
    {
        var (uploaded, failed, total) = await UploadFolderParallelAsync(
            host, user, password, target.LocalPath, target.RemoteRoot, parallelism);
        Console.WriteLine($"  -> {uploaded}/{total} file(s) uploaded, {failed} failed.\n");
        if (failed > 0) exitCode = 1;
    }
    catch (Exception ex)
    {
        Console.Error.WriteLine($"  ! Upload failed: {ex.Message}\n");
        exitCode = 1;
    }
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
// Recursively uploads every file under localRoot to remoteRoot using several
// parallel FTP connections (one per worker). Files are overwritten and remote
// directories are created on demand. Reports live progress to the console.
// Returns (Uploaded, Failed, Total).
// ---------------------------------------------------------------------------
static async Task<(int Uploaded, int Failed, int Total)> UploadFolderParallelAsync(
    string host, string user, string password,
    string localRoot, string remoteRoot, int parallelism)
{
    var files = Directory.GetFiles(localRoot, "*", SearchOption.AllDirectories);
    var total = files.Length;
    if (total == 0)
    {
        Console.WriteLine("  (no files to upload)");
        return (0, 0, 0);
    }

    // Never open more connections than there are files.
    parallelism = Math.Max(1, Math.Min(parallelism, total));
    Console.WriteLine($"  {total} file(s) found. Uploading with {parallelism} parallel connection(s)...");

    var uploaded = 0;
    var failed = 0;
    var processed = 0;
    var consoleLock = new object();

    // Round-robin the files into one bucket per connection.
    var buckets = Enumerable.Range(0, parallelism).Select(_ => new List<string>()).ToArray();
    for (var i = 0; i < files.Length; i++)
        buckets[i % parallelism].Add(files[i]);

    var tasks = buckets.Select(async bucket =>
    {
        await using var client = new AsyncFtpClient(host, user, password);
        client.Config.RetryAttempts = 3;
        client.Config.SocketKeepAlive = true;
        await client.AutoConnect();

        foreach (var localFile in bucket)
        {
            var relative = Path.GetRelativePath(localRoot, localFile).Replace('\\', '/');
            var remotePath = $"{remoteRoot.TrimEnd('/')}/{relative}";

            FtpStatus status;
            try
            {
                status = await client.UploadFile(
                    localFile, remotePath, FtpRemoteExists.Overwrite, createRemoteDir: true, FtpVerify.None);
            }
            catch (Exception ex)
            {
                status = FtpStatus.Failed;
                lock (consoleLock)
                    Console.Error.WriteLine($"  x {localFile}  ->  {remotePath} ({ex.Message})");
            }

            var done = Interlocked.Increment(ref processed);
            if (status == FtpStatus.Success)
            {
                Interlocked.Increment(ref uploaded);
                lock (consoleLock)
                    Console.WriteLine($"  [{done}/{total}] + {localFile}  ->  {remotePath}");
            }
            else
            {
                Interlocked.Increment(ref failed);
                lock (consoleLock)
                    Console.Error.WriteLine($"  [{done}/{total}] x {localFile}  ->  {remotePath} ({status})");
            }
        }

        await client.Disconnect();
    });

    await Task.WhenAll(tasks);
    return (uploaded, failed, total);
}

internal sealed record DeployTarget(string Name, string LocalPath, string RemoteRoot, string ScriptPath);
