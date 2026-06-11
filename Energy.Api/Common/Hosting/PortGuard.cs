using System.Diagnostics;
using System.Runtime.InteropServices;

namespace Energy.Api.Common.Hosting;

/// <summary>
/// Development helper that frees the configured listen ports before Kestrel
/// binds. If a previous instance (or any other process) is still holding a
/// port, it is killed first so the app can start instead of failing with
/// "address already in use". Cross-platform: uses <c>lsof</c> on macOS/Linux
/// and <c>netstat</c> on Windows. Never throws — best effort only.
/// </summary>
public static class PortGuard
{
    public static void FreeConfiguredPorts(IConfiguration configuration, ILogger logger)
    {
        foreach (var port in ResolvePorts(configuration))
        {
            FreePort(port, logger);
        }
    }

    private static IEnumerable<int> ResolvePorts(IConfiguration configuration)
    {
        var ports = new HashSet<int>();

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

    private static string SafeName(Process p)
    {
        try { return p.ProcessName; } catch { return "?"; }
    }

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

    private static IEnumerable<int> ParseWindows(string netstatOutput, int port)
    {
        var marker = ":" + port;
        var pids = new HashSet<int>();
        foreach (var line in netstatOutput.Split(['\n', '\r'], StringSplitOptions.RemoveEmptyEntries))
        {
            if (!line.Contains("LISTENING", StringComparison.OrdinalIgnoreCase)) continue;
            var parts = line.Split(' ', StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries);
            if (parts.Length < 4) continue;
            // local address is parts[1]; ensure the port matches exactly
            if (!parts[1].EndsWith(marker, StringComparison.Ordinal)) continue;
            if (int.TryParse(parts[^1], out var pid) && pid > 0) pids.Add(pid);
        }
        return pids;
    }

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

