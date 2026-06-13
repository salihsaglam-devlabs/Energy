using System.Diagnostics;
using System.Runtime.InteropServices;

namespace Energy.Api.Common.Hosting;

/// <summary>
/// Kestrel bağlanmadan önce yapılandırılan dinleme portlarını serbest bırakan
/// geliştirme yardımcısı. Önceki bir örnek (veya başka bir süreç) bir portu hâlâ
/// tutuyorsa, uygulama "address already in use" hatasıyla başarısız olmak yerine
/// başlayabilsin diye önce o süreç sonlandırılır. Çapraz platform: macOS/Linux'ta
/// <c>lsof</c>, Windows'ta <c>netstat</c> kullanır. Asla hata fırlatmaz — yalnızca en iyi çaba.
/// </summary>
public static class PortGuard
{
    /// <summary>Yapılandırmada belirtilen tüm portları serbest bırakır.</summary>
    public static void FreeConfiguredPorts(IConfiguration configuration, ILogger logger)
    {
        foreach (var port in ResolvePorts(configuration))
        {
            FreePort(port, logger);
        }
    }

    /// <summary>Yapılandırma kaynaklarından dinlenecek port numaralarını çözer.</summary>
    private static IEnumerable<int> ResolvePorts(IConfiguration configuration)
    {        var ports = new HashSet<int>();

        var sources = new[]
        {
            configuration["urls"],
            Environment.GetEnvironmentVariable("ASPNETCORE_URLS"),
            Environment.GetEnvironmentVariable("DOTNET_URLS")
        };

        foreach (var source in sources)
        {
            if (string.IsNullOrWhiteSpace(source)) continue;
            foreach (var url in source.Split(';', StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries))
            {
                if (Uri.TryCreate(url.Replace("*", "localhost").Replace("+", "localhost"), UriKind.Absolute, out var uri) && uri.Port > 0)
                {
                    ports.Add(uri.Port);
                }
            }
        }

        // Kestrel:Endpoints:*:Url
        foreach (var endpoint in configuration.GetSection("Kestrel:Endpoints").GetChildren())
        {
            var url = endpoint["Url"];
            if (!string.IsNullOrWhiteSpace(url)
                && Uri.TryCreate(url.Replace("*", "localhost").Replace("+", "localhost"), UriKind.Absolute, out var uri)
                && uri.Port > 0)
            {
                ports.Add(uri.Port);
            }
        }

        return ports;
    }

    /// <summary>Belirtilen portu tutan süreçleri (varsa) sonlandırarak portu serbest bırakır.</summary>
    private static void FreePort(int port, ILogger logger)
    {
        try
        {
            var currentPid = Environment.ProcessId;
            foreach (var pid in GetListeningPids(port))
            {
                if (pid == currentPid) continue;
                try
                {
                    using var process = Process.GetProcessById(pid);
                    logger.LogWarning("[PortGuard] Port {Port} is held by PID {Pid} ({Name}); killing it before bind.",
                        port, pid, SafeName(process));
                    process.Kill(entireProcessTree: true);
                    process.WaitForExit(3000);
                }
                catch (Exception ex)
                {
                    logger.LogWarning(ex, "[PortGuard] Could not kill PID {Pid} on port {Port}.", pid, port);
                }
            }
        }
        catch (Exception ex)
        {
            logger.LogWarning(ex, "[PortGuard] Failed to inspect port {Port}.", port);
        }
    }

    /// <summary>Bir sürecin adını güvenli şekilde (hata fırlatmadan) döndürür.</summary>
    private static string SafeName(Process p)
    {
        try { return p.ProcessName; } catch { return "?"; }
    }

    /// <summary>Belirtilen portu dinleyen süreç kimliklerini (PID) işletim sistemine göre bulur.</summary>
    private static IEnumerable<int> GetListeningPids(int port)
    {
        if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
        {
            return ParseWindows(RunCommand("netstat", "-ano"), port);
        }

        // macOS / Linux
        var output = RunCommand("lsof", $"-nP -iTCP:{port} -sTCP:LISTEN -t");
        return output
            .Split(['\n', '\r'], StringSplitOptions.RemoveEmptyEntries)
            .Select(line => int.TryParse(line.Trim(), out var pid) ? pid : -1)
            .Where(pid => pid > 0)
            .Distinct();
    }

    /// <summary>Windows <c>netstat</c> çıktısını ayrıştırarak portu dinleyen PID'leri çıkarır.</summary>
    private static IEnumerable<int> ParseWindows(string netstatOutput, int port)
    {
        var marker = ":" + port;
        var pids = new HashSet<int>();
        foreach (var line in netstatOutput.Split(['\n', '\r'], StringSplitOptions.RemoveEmptyEntries))
        {
            if (!line.Contains("LISTENING", StringComparison.OrdinalIgnoreCase)) continue;
            var parts = line.Split(' ', StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries);
            if (parts.Length < 4) continue;
            // yerel adres parts[1]'dir; portun tam olarak eşleştiğinden emin ol
            if (!parts[1].EndsWith(marker, StringComparison.Ordinal)) continue;
            if (int.TryParse(parts[^1], out var pid) && pid > 0) pids.Add(pid);
        }
        return pids;
    }

    /// <summary>Harici bir komutu çalıştırır ve standart çıktısını güvenli şekilde döndürür.</summary>
    private static string RunCommand(string fileName, string arguments)
    {
        try
        {
            using var process = new Process
            {
                StartInfo = new ProcessStartInfo
                {
                    FileName = fileName,
                    Arguments = arguments,
                    RedirectStandardOutput = true,
                    RedirectStandardError = true,
                    UseShellExecute = false,
                    CreateNoWindow = true
                }
            };
            process.Start();
            var output = process.StandardOutput.ReadToEnd();
            process.WaitForExit(3000);
            return output;
        }
        catch
        {
            return string.Empty;
        }
    }
}

