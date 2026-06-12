using System.Diagnostics;
using System.Security.Claims;
using System.Text;
using System.Text.Json;
using Energy.Shared.Logging;
using Energy.Shared.Models.V1.Logger.Requests;
using Energy.Web.Clients.Infrastructure.Authentication;
using Energy.Web.Clients.Logger;

namespace Energy.Web.Common.Middleware;

/// <summary>
/// Records every Web-tier request (page navigations and MVC/JSON actions) in
/// the single audit sink by forwarding a masked request/response entry to the
/// API. Static assets and SignalR transport are skipped. Requests are ALWAYS
/// audited: the ingest call authenticates to the API as the non-interactive
/// system service account (never with the signed-in user's token), so audit
/// logging can never be blocked by the user's permissions, an expired/invalid
/// user token, or any other Web-side restriction. The real signed-in actor is
/// forwarded in the request body so the entry is still attributed correctly.
/// Logging never breaks the request: every failure is swallowed.
/// </summary>
public sealed class WebRequestLoggingMiddleware
{
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

    public async Task InvokeAsync(
        HttpContext context,
        IAuditLogIngestClient ingest,
        IUserApiTokenProvider userTokens,
        IServiceApiTokenProvider serviceTokens)
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
            // Log WITH the exception so the full stack trace (the exact method +
            // line where it was raised) is captured before it bubbles up to the
            // framework error handler.
            _logger.LogError(ex,
                "Unhandled exception for {Method} {Path}. CorrelationId: {CorrelationId}.",
                context.Request.Method, context.Request.Path.Value, correlationId);
            throw;
        }
        finally
        {
            stopwatch.Stop();
            var responseBody = ReadResponseBody(context, buffer);

            // Surface business failures too: a BaseResponse with success=false is a
            // logical failure even when no exception was thrown.
            LogFailedEnvelope(context, correlationId, responseBody, exception);

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

            await SafeIngestAsync(context, ingest, userTokens, serviceTokens, startedAt,
                stopwatch.ElapsedMilliseconds, correlationId, exception, requestBody, responseBody);
        }
    }

    private async Task SafeIngestAsync(
        HttpContext context,
        IAuditLogIngestClient ingest,
        IUserApiTokenProvider userTokens,
        IServiceApiTokenProvider serviceTokens,
        DateTime startedAt,
        long durationMs,
        Guid correlationId,
        Exception? exception,
        string? requestBody,
        string? responseBody)
    {
        try
        {
            // Audit logging ALWAYS authenticates to the API as the non-interactive
            // system service account — never with the signed-in user's token. This
            // guarantees the audit trail can never be blocked by the user's
            // permissions, an expired/invalid user token, or any other Web-side
            // restriction: EVERY request (anonymous login attempts included) is
            // captured. The real actor is forwarded in the request body so the
            // entry is still attributed to the signed-in user.
            var serviceToken = await serviceTokens.GetAccessTokenAsync(context.RequestAborted);
            if (string.IsNullOrEmpty(serviceToken))
            {
                // Service token unavailable (e.g. API down). Do not lose the request
                // silently — record the reason and skip this single entry.
                _logger.LogWarning("Skipping audit for {Path}: no service token available.",
                    context.Request.Path);
                return;
            }

            // Resolve the real signed-in actor (if any) from the cookie principal.
            ResolveActor(context, out var actorId, out var actorName);

            using (userTokens.UseAccessToken(serviceToken))
            {
                var status = context.Response.StatusCode;
                await ingest.IngestAsync(new CreateAuditLogRequest
                {
                    OccurredAt = startedAt,
                    UserId = actorId,
                    UserName = actorName,
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
        }
        catch (Exception ex)
        {
            // Auditing must never break the user request.
            _logger.LogWarning(ex, "Failed to forward Web audit log entry for {Path}.", context.Request.Path);
        }
    }

    /// <summary>
    /// Extracts the signed-in user's id and name from the cookie principal so the
    /// audit entry is attributed correctly even though the ingest call itself is
    /// authenticated as the system service account. Returns <c>null</c> for
    /// anonymous requests (e.g. the login POST).
    /// </summary>
    private static void ResolveActor(HttpContext context, out Guid? userId, out string? userName)
    {
        userId = null;
        userName = null;

        if (context.User.Identity?.IsAuthenticated != true) return;

        var idValue = context.User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        if (Guid.TryParse(idValue, out var parsed)) userId = parsed;
        userName = context.User.Identity?.Name;
    }

    /// <summary>
    /// Inspects the captured response envelope and, if it represents a business
    /// failure (<c>success:false</c>) that did NOT originate from an exception,
    /// records it so failed outcomes are never silently lost in the logs.
    /// </summary>
    private void LogFailedEnvelope(HttpContext context, Guid correlationId, string? responseBody, Exception? exception)
    {
        // Exception paths are already logged above with their full stack trace.
        if (exception is not null) return;
        if (string.IsNullOrEmpty(responseBody)) return;
        if (!responseBody.Contains("\"success\"", StringComparison.OrdinalIgnoreCase)) return;

        try
        {
            using var doc = JsonDocument.Parse(responseBody);
            if (doc.RootElement.ValueKind != JsonValueKind.Object) return;
            if (!doc.RootElement.TryGetProperty("success", out var success)) return;
            if (success.ValueKind != JsonValueKind.False) return;

            var message = doc.RootElement.TryGetProperty("message", out var m) ? m.GetString() : null;
            _logger.LogWarning(
                "Business failure (success=false) for {Method} {Path} -> {StatusCode}. Message: {Message}. CorrelationId: {CorrelationId}.",
                context.Request.Method, context.Request.Path.Value, context.Response.StatusCode, message, correlationId);
        }
        catch (JsonException)
        {
            // Non-JSON or partial payload — nothing to extract; ignore.
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

