using System.Diagnostics;
using System.Security.Claims;
using System.Text;
using System.Text.Json;
using Energy.Application.Common.Exceptions;
using Energy.Application.Identity.Services;
using Energy.Application.Logger.Services;
using Energy.Domain.Core;
using Energy.Localization;
using Energy.Shared.Logging;
using Energy.Shared.Models.V1.Common.Responses;
using Microsoft.Extensions.Localization;

namespace Energy.Api.Common.Middleware;

/// <summary>
/// Her isteği bir denetim bağlamına sarar: (maskelenmiş) istek ve yanıt gövdelerini
/// yakalar, istek başına tek ve değiştirilemez bir <see cref="AuditLog"/> satırı
/// yazar — ASLA atlamadan — ve işlenmemiş istisnaları standartlaştırılmış,
/// YERELLEŞTİRİLMİŞ bir <see cref="BaseResponse{T}"/> yüküne dönüştürür. Hassas
/// alanlar <see cref="SensitiveDataMasker"/> ile maskelenir.
/// </summary>
public sealed class RequestLoggingMiddleware
{
    private const string CorrelationHeader = "X-Correlation-Id";
    private const string AuditSource = "API";

    private readonly RequestDelegate _next;
    private readonly ILogger<RequestLoggingMiddleware> _logger;

    /// <summary>Sonraki ara katmanı ve günlükleyiciyi enjekte eder.</summary>
    public RequestLoggingMiddleware(RequestDelegate next, ILogger<RequestLoggingMiddleware> logger)
    {
        _next = next;
        _logger = logger;
    }

    /// <summary>İstek ardışık düzenini denetim, istisna işleme ve günlükleme ile sarmalar.</summary>
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

        // İstek gövdesini önceden yakala (tamponlama, model bağlamanın yeniden okumasını sağlar).
        var requestBody = await CaptureRequestBodyAsync(context);

        // Yanıt akışını değiştir; böylece alt katmanın ürettiğini okuyabilir ve
        // sonrasında gerçek istemci bağlantısına aktarabiliriz.
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
            // Beklenen/işlenen domain sonucu: tam yığın izi (hatanın oluştuğu kesin
            // metot + satır) yakalansın diye istisnayla BİRLİKTE günlükle.
            _logger.LogWarning(ex,
                "Handled {ExceptionType} for {Method} {Path} -> 404. CorrelationId: {CorrelationId}.",
                nameof(NotFoundException), context.Request.Method, context.Request.Path.Value, correlationId);
            var message = localizer[ex.MessageKey, ex.Arguments].Value;
            await WriteFailureAsync(context, StatusCodes.Status404NotFound, message, new[] { message });
        }
        catch (ConflictException ex)
        {
            exception = ex;
            _logger.LogWarning(ex,
                "Handled {ExceptionType} for {Method} {Path} -> 409. CorrelationId: {CorrelationId}.",
                nameof(ConflictException), context.Request.Method, context.Request.Path.Value, correlationId);
            var message = localizer[ex.MessageKey, ex.Arguments].Value;
            await WriteFailureAsync(context, StatusCodes.Status409Conflict, message, new[] { message });
        }
        catch (Exception ex)
        {
            exception = ex;
            // Beklenmeyen hata: tam yığın iziyle (metot/satır) hatanın NEREDE oluştuğunu
            // ve istek bağlamını belirterek Error seviyesinde günlükle.
            _logger.LogError(ex,
                "Unhandled exception for {Method} {Path} -> 500. CorrelationId: {CorrelationId}.",
                context.Request.Method, context.Request.Path.Value, correlationId);
            await WriteFailureAsync(context, StatusCodes.Status500InternalServerError,
                localizer[LocalizationKeys.Messages.UnexpectedError].Value,
                new[] { localizer[LocalizationKeys.Messages.UnexpectedError].Value });
        }
        finally
        {
            stopwatch.Stop();

            var responseBody = ReadResponseBody(context, buffer);

            // İş kuralı başarısızlıklarını da yüzeye çıkar: success=false içeren bir
            // BaseResponse, hiçbir istisna fırlatılmamış olsa bile (ör. doğrulama)
            // mantıksal bir başarısızlıktır.
            LogFailedEnvelope(context, correlationId, responseBody, exception);

            // Yakalanan yanıtı her zaman gerçek bağlantıya geri aktar.
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

    /// <summary>İstek gövdesini güvenli ve maskelenmiş şekilde yakalar (tamponlamayla yeniden okunabilir).</summary>
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

    /// <summary>Yakalanan yanıt gövdesini okur ve maskeleyerek döndürür.</summary>
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

    /// <summary>İçerik türünün metin olarak güvenle yakalanabilir olup olmadığını belirler.</summary>
    private static bool IsTextCapturable(string? contentType)
    {
        if (string.IsNullOrEmpty(contentType)) return true; // bilinmeyen küçük yükleri okumak güvenlidir
        var ct = contentType.ToLowerInvariant();
        return ct.Contains("json")
               || ct.Contains("xml")
               || ct.Contains("text/")
               || ct.Contains("x-www-form-urlencoded");
    }

    /// <summary>İlişkilendirme (correlation) kimliğini başlıktan çözer veya yeni bir tane üretir.</summary>
    private static string ResolveCorrelationId(HttpContext context)
    {
        var id = context.Request.Headers[CorrelationHeader].FirstOrDefault();
        if (string.IsNullOrWhiteSpace(id)) id = Guid.NewGuid().ToString("N");
        context.Response.Headers[CorrelationHeader] = id;
        return id;
    }

    /// <summary>Standartlaştırılmış bir başarısızlık yanıtı (BaseResponse) yazar.</summary>
    private static async Task WriteFailureAsync(HttpContext context, int status, string message, IEnumerable<string> errors)
    {
        if (context.Response.HasStarted) return;
        context.Response.StatusCode = status;
        context.Response.ContentType = "application/json";
        await context.Response.WriteAsync(JsonSerializer.Serialize(BaseResponse<object>.Failure(message, errors)));
    }

    /// <summary>
    /// Yakalanan yanıt zarfını inceler ve eğer bir istisnadan KAYNAKLANMAYAN bir iş
    /// kuralı başarısızlığını (<c>success:false</c>) temsil ediyorsa kaydeder; böylece
    /// başarısız sonuçlar günlüklerde sessizce kaybolmaz.
    /// </summary>
    private void LogFailedEnvelope(HttpContext context, string correlationId, string? responseBody, Exception? exception)
    {
        // İstisna yolları yukarıda zaten tam yığın iziyle günlüğe yazılmıştır.
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
            // JSON olmayan veya kısmi yük — çıkarılacak bir şey yok; yok say.
        }
    }

    /// <summary>Denetim günlüğü kaydını güvenli şekilde (hata fırlatmadan) kalıcılaştırır.</summary>
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
