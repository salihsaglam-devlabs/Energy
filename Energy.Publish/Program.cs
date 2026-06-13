using System.Collections.Concurrent;
using System.Diagnostics;
using FluentFTP;

// ---------------------------------------------------------------------------
// Energy.Publish
// ---------------------------------------------------------------------------
// Seçilen her hedef için bu araç:
//   1) Eşleşen yayınlama shell betiğini (shells/publish-<target>.sh) çalıştırır;
//      bu betik, `dotnet publish` ile Release çıktısını üretir.
//   2) YALNIZCA betik başarılı olursa (çıkış kodu 0), FTP sunucusuna bağlanır ve
//      yayınlanan çıktıyı BİRDEN ÇOK PARALEL bağlantı kullanarak özyinelemeli
//      şekilde yükler; var olan her dosyanın ÜZERİNE YAZAR. Uzak klasörler gerektikçe
//      oluşturulur. Canlı ilerleme ([yapılan/toplam]) konsola yazdırılır.
//
// Kullanım:
//   dotnet run --project Energy.Publish            # api + web HER İKİSİNİ yayınla + yükle
//   dotnet run --project Energy.Publish -- api     # yalnızca API'yi yayınla + yükle
//   dotnet run --project Energy.Publish -- web     # yalnızca Web'i yayınla + yükle
//
// Kimlik bilgileri / host / paralellik ortam değişkenleriyle geçersiz kılınabilir:
//   ENERGY_FTP_HOST, ENERGY_FTP_USER, ENERGY_FTP_PASSWORD, ENERGY_FTP_PARALLELISM
// ---------------------------------------------------------------------------

// FTP bağlantısı (CI / üretimde ortam değişkenleriyle geçersiz kılın).
var host = Environment.GetEnvironmentVariable("ENERGY_FTP_HOST") ?? "31.186.11.158";
var user = Environment.GetEnvironmentVariable("ENERGY_FTP_USER") ?? "wat14bcomtr";
var password = Environment.GetEnvironmentVariable("ENERGY_FTP_PASSWORD") ?? "Wattiw@1";

// Yükleme için kullanılan paralel FTP bağlantısı sayısı (aktarımları hızlandırır).
// ENERGY_FTP_PARALLELISM ile geçersiz kılın. Varsayılan 4.
var parallelism = int.TryParse(Environment.GetEnvironmentVariable("ENERGY_FTP_PARALLELISM"), out var p) && p > 0
    ? p
    : 4;

// Depo kökü: bu projenin derleme çıktısı klasörüne göre ../.
var repoRoot = Path.GetFullPath(Path.Combine(AppContext.BaseDirectory, "..", "..", "..", ".."));

// Dağıtım hedefleri: yerel yayınlama klasörü -> FTP sunucusundaki uzak site kökü.
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

// İlk CLI argümanına göre hangi hedeflerin yayınlanacağını seç.
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

// Tüm hedefler boyunca tek denemede + bir kez yeniden denemede de yüklenemeyen
// dosyalar. En sonda tek bir liste hâlinde raporlanır.
var allFailures = new List<(string Target, UploadFailure Failure)>();

foreach (var target in targets)
{
    Console.WriteLine($"===== {target.Name.ToUpperInvariant()} =====");
    Console.WriteLine($"Script: {target.ScriptPath}");
    Console.WriteLine($"Local : {target.LocalPath}");
    Console.WriteLine($"Remote: {target.RemoteRoot}");

    // 1) Yayınlama shell betiğini çalıştır. Yalnızca başarıyla tamamlanırsa
    //    (çıkış kodu 0) FTP yüklemesine devam et.
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
        var (uploaded, failed, total, failures) = await UploadFolderParallelAsync(
            host, user, password, target.LocalPath, target.RemoteRoot, parallelism);
        Console.WriteLine($"  -> {uploaded}/{total} file(s) uploaded, {failed} failed.\n");
        if (failed > 0)
        {
            exitCode = 1;
            foreach (var failure in failures)
                allFailures.Add((target.Name, failure));
        }
    }
    catch (Exception ex)
    {
        Console.Error.WriteLine($"  ! Upload failed: {ex.Message}\n");
        exitCode = 1;
    }
}

// Yeniden denemeden sonra da yüklenemeyen dosyaları sonda tek liste hâlinde göster.
if (allFailures.Count > 0)
{
    Console.WriteLine();
    Console.WriteLine($"===== FAILED TRANSFERS ({allFailures.Count}) =====");
    Console.WriteLine("Aşağıdaki dosyalar yeniden denemeye rağmen aktarılamadı:");
    foreach (var (targetName, failure) in allFailures)
        Console.Error.WriteLine($"  [{targetName}] {failure.LocalFile}  ->  {failure.RemotePath}  ({failure.Reason})");
    Console.WriteLine();
}

Console.WriteLine(exitCode == 0 ? "Done." : "Completed with errors.");
return exitCode;

// ---------------------------------------------------------------------------
// Bir shell (.sh) betiğini zsh ile çalıştırır ve çıktısını akıtır. Betiğin çıkış
// kodunu döndürür (0 = başarı).
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
// localRoot altındaki her dosyayı, birden çok paralel FTP bağlantısı (her işçi için
// bir tane) kullanarak remoteRoot'a özyinelemeli şekilde yükler. Dosyaların üzerine
// yazılır ve uzak dizinler gerektikçe oluşturulur. Bir dosya yüklenemezse BİR KEZ
// DAHA denenir; yine olmazsa başarısız olarak işaretlenir. Konsola canlı ilerleme
// raporlar. (Yüklenen, Başarısız, Toplam, BaşarısızDosyalar) döndürür.
// ---------------------------------------------------------------------------
static async Task<(int Uploaded, int Failed, int Total, IReadOnlyList<UploadFailure> Failures)> UploadFolderParallelAsync(
    string host, string user, string password,
    string localRoot, string remoteRoot, int parallelism)
{
    var files = Directory.GetFiles(localRoot, "*", SearchOption.AllDirectories);
    var total = files.Length;
    if (total == 0)
    {
        Console.WriteLine("  (no files to upload)");
        return (0, 0, 0, Array.Empty<UploadFailure>());
    }

    // Never open more connections than there are files.
    parallelism = Math.Max(1, Math.Min(parallelism, total));
    Console.WriteLine($"  {total} file(s) found. Uploading with {parallelism} parallel connection(s)...");

    var uploaded = 0;
    var failed = 0;
    var processed = 0;
    var consoleLock = new object();
    var failures = new ConcurrentBag<UploadFailure>();

    // Dosyaları bağlantı başına bir kovaya round-robin (sırayla) dağıt.
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

            // İlk deneme + başarısız olursa bir kez daha dene (toplam 2 deneme).
            var (status, reason) = await TryUploadWithRetryAsync(
                client, localFile, remotePath, attempts: 2, consoleLock);

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
                failures.Add(new UploadFailure(localFile, remotePath, reason ?? status.ToString()));
                lock (consoleLock)
                    Console.Error.WriteLine($"  [{done}/{total}] x {localFile}  ->  {remotePath} ({reason ?? status.ToString()})");
            }
        }

        await client.Disconnect();
    });

    await Task.WhenAll(tasks);

    // Başarısız dosyaları hedef yoluna göre sıralayıp döndür (deterministik liste).
    var orderedFailures = failures.OrderBy(f => f.RemotePath, StringComparer.OrdinalIgnoreCase).ToList();
    return (uploaded, failed, total, orderedFailures);
}

// ---------------------------------------------------------------------------
// Tek bir dosyayı en çok `attempts` kez yüklemeyi dener (ilk deneme + yeniden
// denemeler). Başarılı olursa FtpStatus.Success döner. Bir deneme hata verir veya
// başarısız statü dönerse kısa bir bekleme sonrası tekrar denenir; bağlantı kopmuşsa
// yeniden bağlanılır. Tüm denemeler tükenirse son hata nedeniyle Failed döner.
// ---------------------------------------------------------------------------
static async Task<(FtpStatus Status, string? Reason)> TryUploadWithRetryAsync(
    AsyncFtpClient client,
    string localFile, string remotePath, int attempts, object consoleLock)
{
    string? lastReason = null;

    for (var attempt = 1; attempt <= attempts; attempt++)
    {
        try
        {
            var status = await client.UploadFile(
                localFile, remotePath, FtpRemoteExists.Overwrite, createRemoteDir: true, FtpVerify.None);
            if (status == FtpStatus.Success)
                return (status, null);

            lastReason = status.ToString();
        }
        catch (Exception ex)
        {
            lastReason = ex.Message;
            // Bağlantı kopmuş olabilir; yeniden denemeden önce bağlantıyı tazele.
            try { if (!client.IsConnected) await client.AutoConnect(); }
            catch { /* yeniden bağlanma başarısız olursa sonraki deneme yine başarısız olur */ }
        }

        if (attempt < attempts)
        {
            lock (consoleLock)
                Console.Error.WriteLine($"  ~ retry {attempt}/{attempts - 1}: {localFile} ({lastReason})");
            await Task.Delay(1000);
        }
    }

    return (FtpStatus.Failed, lastReason);
}

internal sealed record DeployTarget(string Name, string LocalPath, string RemoteRoot, string ScriptPath);

// Yeniden denemeye rağmen aktarılamayan bir dosyanın kaydı.
internal sealed record UploadFailure(string LocalFile, string RemotePath, string? Reason);

