using System.Diagnostics;
using System.Security.Claims;
using System.Text;
using System.Text.Json;
using Energy.Application.Common.Exceptions;
using Energy.Application.Identity.Services;
using Energy.Application.Logger.Services;
using Energy.Domain.Logger;
using Energy.Localization;
using Energy.Shared.Logging;
using Energy.Shared.Models.V1.Common.Responses;
using Microsoft.Extensions.Localization;

namespace Energy.Api.Common.Middleware;

/// <summary>
/// Wraps every request in an audit context: captures the (masked) request and
/// response bodies, writes a single immutable <see cref="AuditLog"/> row per
/// request — NEVER skipping any — and converts unhandled exceptions into a
/// standardized, LOCALIZED <see cref="BaseResponse{T}"/> payload. Sensitive
/// fields are redacted via <see cref="SensitiveDataMasker"/>.
/// </summary>
public sealed class RequestLoggingMiddleware
{
    private const string CorrelationHeader = "X-Correlation-Id";
    private const string AuditSource = "API";

    private readonly RequestDelegate _next;
    private readonly ILogger<RequestLoggingMiddleware> _logger;

    public RequestLoggingMiddleware(RequestDelegate next, ILogger<RequestLoggingMiddleware> logger)
    {
        _next = next;
        _logger = logger;
    }

    public async Task InvokeAsync(
        HttpContext context,
        IAuditLogService auditLogs,
        ICurrentUser currentUser,
        IStringLocalizer<SharedResource> localizer)
    {
        var startedAt = DateTime.UtcNow;
        var stopwatch = Stopwatch.StartNew();
        var correlationId = ResolveCorrelationId(context);
        Exception? exception = null;

        // Capture the request body up-front (buffering lets model binding re-read it).
        var requestBody = await CaptureRequestBodyAsync(context);

        // Swap the response stream so we can read what downstream produced and
        // still flush it to the real client connection afterwards.
        var originalBody = context.Response.Body;
        await using var buffer = new MemoryStream();
        context.Response.Body = buffer;

        try
        {
            await _next(context);
        }
        catch (NotFoundException ex)
        {
            exception = ex;
            var message = localizer[ex.MessageKey, ex.Arguments].Value;
            await WriteFailureAsync(context, StatusCodes.Status404NotFound, message, new[] { message });
        }
        catch (ConflictException ex)
        {
            exception = ex;
            var message = localizer[ex.MessageKey, ex.Arguments].Value;
            await WriteFailureAsync(context, StatusCodes.Status409Conflict, message, new[] { message });
        }
        catch (Exception ex)
        {
            exception = ex;
            _logger.LogError(ex, "Unhandled exception in pipeline.");
            await WriteFailureAsync(context, StatusCodes.Status500InternalServerError,
                localizer[LocalizationKeys.Messages.UnexpectedError].Value,
                new[] { localizer[LocalizationKeys.Messages.UnexpectedError].Value });
        }
        finally
        {
            stopwatch.Stop();

            var responseBody = ReadResponseBody(context, buffer);

            // Always flush the captured response back to the real connection.
            try
            {
                buffer.Position = 0;
                await buffer.CopyToAsync(originalBody);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed to flush captured response body.");
            }
            finally
            {
                context.Response.Body = originalBody;
            }

            await SafeWriteLogAsync(auditLogs, context, currentUser, correlationId, startedAt,
                stopwatch.ElapsedMilliseconds, exception, requestBody, responseBody);
        }
    }

    private static async Task<string?> CaptureRequestBodyAsync(HttpContext context)
    {
        var request = context.Request;
        if (request.ContentLength is null or 0) return null;

        var contentType = request.ContentType;
        if (!IsTextCapturable(contentType))
        {
            return $"[skipped:{contentType}]";
        }

        request.EnableBuffering();
        request.Body.Position = 0;
        using var reader = new StreamReader(request.Body, Encoding.UTF8, detectEncodingFromByteOrderMarks: false, leaveOpen: true);
        var raw = await reader.ReadToEndAsync();
        request.Body.Position = 0;

        return SensitiveDataMasker.MaskBody(raw, contentType);
    }

    private static string? ReadResponseBody(HttpContext context, MemoryStream buffer)
    {
        if (buffer.Length == 0) return null;

        var contentType = context.Response.ContentType;
        if (!IsTextCapturable(contentType))
        {
            return $"[skipped:{contentType}]";
        }

        buffer.Position = 0;
        using var reader = new StreamReader(buffer, Encoding.UTF8, detectEncodingFromByteOrderMarks: false, leaveOpen: true);
        var raw = reader.ReadToEnd();
        return SensitiveDataMasker.MaskBody(raw, contentType);
    }

    private static bool IsTextCapturable(string? contentType)
    {
        if (string.IsNullOrEmpty(contentType)) return true; // unknown small payloads are safe to read
        var ct = contentType.ToLowerInvariant();
        return ct.Contains("json")
               || ct.Contains("xml")
               || ct.Contains("text/")
               || ct.Contains("x-www-form-urlencoded");
    }

    private static string ResolveCorrelationId(HttpContext context)
    {
        var id = context.Request.Headers[CorrelationHeader].FirstOrDefault();
        if (string.IsNullOrWhiteSpace(id)) id = Guid.NewGuid().ToString("N");
        context.Response.Headers[CorrelationHeader] = id;
        return id;
    }

    private static async Task WriteFailureAsync(HttpContext context, int status, string message, IEnumerable<string> errors)
    {
        if (context.Response.HasStarted) return;
        context.Response.StatusCode = status;
        context.Response.ContentType = "application/json";
        await context.Response.WriteAsync(JsonSerializer.Serialize(BaseResponse<object>.Failure(message, errors)));
    }

    private async Task SafeWriteLogAsync(
        IAuditLogService auditLogs,
        HttpContext context,
        ICurrentUser currentUser,
        string correlationId,
        DateTime startedAt,
        long durationMs,
        Exception? exception,
        string? requestBody,
        string? responseBody)
    {
        try
        {
            var status = context.Response.StatusCode;
            await auditLogs.WriteAsync(new AuditLog
            {
                OccurredAt = startedAt,
                UserId = currentUser.UserId,
                UserName = currentUser.UserName,
                IpAddress = context.Connection.RemoteIpAddress?.ToString(),
                HttpMethod = context.Request.Method,
                Path = context.Request.Path.Value,
                QueryString = SensitiveDataMasker.MaskQueryString(context.Request.QueryString.Value),
                StatusCode = status,
                IsSuccess = exception is null && status >= 200 && status < 400,
                Source = AuditSource,
                RequestBody = requestBody,
                ResponseBody = responseBody,
                HasException = exception is not null,
                ExceptionType = exception?.GetType().FullName,
                ExceptionMessage = exception?.Message,
                CorrelationId = Guid.TryParseExact(correlationId, "N", out var parsed) ? parsed : null,
                DurationMs = (int)durationMs
            });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Failed to persist audit log entry.");
        }
    }
}
