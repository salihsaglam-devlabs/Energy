using System.Diagnostics;
using System.Text;
using Energy.Shared.Logging;
using Energy.Shared.Models.V1.Logger.Requests;
using Energy.Web.Clients.Logger;

namespace Energy.Web.Common.Middleware;

/// <summary>
/// Records every Web-tier request (page navigations and MVC/JSON actions) in
/// the single audit sink by forwarding a masked request/response entry to the
/// API. Static assets are skipped; anonymous requests are skipped because they
/// cannot authenticate against the API (the corresponding API call — e.g. the
/// login POST — is already audited on the API side). Logging never breaks the
/// request: every failure is swallowed.
/// </summary>
public sealed class WebRequestLoggingMiddleware
{
    private const string Source = "Web";

    private static readonly string[] SkippedPrefixes =
    [
        "/css", "/js", "/lib", "/images", "/img", "/fonts", "/favicon", "/_", "/health",
        // SignalR transport: never wrap the response body of hub negotiate /
        // WebSocket / SSE / long-polling requests — buffering them breaks the
        // streaming connection so real-time delivery silently fails.
        "/hubs"
    ];

    private static readonly string[] SkippedExtensions =
    [
        ".css", ".js", ".map", ".png", ".jpg", ".jpeg", ".gif", ".svg", ".ico",
        ".woff", ".woff2", ".ttf", ".eot", ".webp"
    ];

    private readonly RequestDelegate _next;
    private readonly ILogger<WebRequestLoggingMiddleware> _logger;

    public WebRequestLoggingMiddleware(RequestDelegate next, ILogger<WebRequestLoggingMiddleware> logger)
    {
        _next = next;
        _logger = logger;
    }

    public async Task InvokeAsync(HttpContext context, IAuditLogIngestClient ingest)
    {
        // Never buffer streaming/upgrade responses (SignalR WebSocket/SSE):
        // swapping Response.Body for a MemoryStream would break the connection.
        if (context.WebSockets.IsWebSocketRequest || ShouldSkip(context.Request.Path))
        {
            await _next(context);
            return;
        }

        var startedAt = DateTime.UtcNow;
        var stopwatch = Stopwatch.StartNew();
        var correlationId = Guid.NewGuid();
        Exception? exception = null;

        var requestBody = await CaptureRequestBodyAsync(context);

        var originalBody = context.Response.Body;
        await using var buffer = new MemoryStream();
        context.Response.Body = buffer;

        try
        {
            await _next(context);
        }
        catch (Exception ex)
        {
            exception = ex;
            throw;
        }
        finally
        {
            stopwatch.Stop();
            var responseBody = ReadResponseBody(context, buffer);

            try
            {
                buffer.Position = 0;
                await buffer.CopyToAsync(originalBody);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed to flush captured Web response body.");
            }
            finally
            {
                context.Response.Body = originalBody;
            }

            await SafeIngestAsync(context, ingest, startedAt, stopwatch.ElapsedMilliseconds,
                correlationId, exception, requestBody, responseBody);
        }
    }

    private async Task SafeIngestAsync(
        HttpContext context,
        IAuditLogIngestClient ingest,
        DateTime startedAt,
        long durationMs,
        Guid correlationId,
        Exception? exception,
        string? requestBody,
        string? responseBody)
    {
        // Anonymous requests cannot authenticate against the API ingest endpoint;
        // their equivalent API call (e.g. login) is audited on the API side.
        if (context.User.Identity?.IsAuthenticated != true) return;

        try
        {
            var status = context.Response.StatusCode;
            await ingest.IngestAsync(new CreateAuditLogRequest
            {
                OccurredAt = startedAt,
                HttpMethod = context.Request.Method,
                Path = context.Request.Path.Value,
                QueryString = SensitiveDataMasker.MaskQueryString(context.Request.QueryString.Value),
                StatusCode = status,
                IsSuccess = exception is null && status is >= 200 and < 400,
                RequestBody = requestBody,
                ResponseBody = responseBody,
                HasException = exception is not null,
                ExceptionType = exception?.GetType().FullName,
                ExceptionMessage = exception?.Message,
                CorrelationId = correlationId,
                DurationMs = (int)durationMs
            }, context.RequestAborted);
        }
        catch (Exception ex)
        {
            // Auditing must never break the user request.
            _logger.LogWarning(ex, "Failed to forward Web audit log entry for {Path}.", context.Request.Path);
        }
    }

    private static async Task<string?> CaptureRequestBodyAsync(HttpContext context)
    {
        var request = context.Request;
        if (request.ContentLength is null or 0) return null;
        if (!IsTextCapturable(request.ContentType)) return $"[skipped:{request.ContentType}]";

        request.EnableBuffering();
        request.Body.Position = 0;
        using var reader = new StreamReader(request.Body, Encoding.UTF8, detectEncodingFromByteOrderMarks: false, leaveOpen: true);
        var raw = await reader.ReadToEndAsync();
        request.Body.Position = 0;
        return SensitiveDataMasker.MaskBody(raw, request.ContentType);
    }

    private static string? ReadResponseBody(HttpContext context, MemoryStream buffer)
    {
        if (buffer.Length == 0) return null;
        if (!IsTextCapturable(context.Response.ContentType)) return $"[skipped:{context.Response.ContentType}]";

        buffer.Position = 0;
        using var reader = new StreamReader(buffer, Encoding.UTF8, detectEncodingFromByteOrderMarks: false, leaveOpen: true);
        var raw = reader.ReadToEnd();
        return SensitiveDataMasker.MaskBody(raw, context.Response.ContentType);
    }

    private static bool IsTextCapturable(string? contentType)
    {
        if (string.IsNullOrEmpty(contentType)) return true;
        var ct = contentType.ToLowerInvariant();
        return ct.Contains("json")
               || ct.Contains("xml")
               || ct.Contains("text/")
               || ct.Contains("x-www-form-urlencoded");
    }

    private static bool ShouldSkip(PathString path)
    {
        var value = path.Value;
        if (string.IsNullOrEmpty(value)) return false;

        foreach (var prefix in SkippedPrefixes)
        {
            if (value.StartsWith(prefix, StringComparison.OrdinalIgnoreCase)) return true;
        }

        foreach (var ext in SkippedExtensions)
        {
            if (value.EndsWith(ext, StringComparison.OrdinalIgnoreCase)) return true;
        }

        return false;
    }
}

