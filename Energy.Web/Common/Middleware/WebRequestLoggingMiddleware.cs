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
/// Maskelenmiş bir istek/yanıt kaydını API'ye ileterek her Web katmanı isteğini
/// (sayfa gezinmeleri ve MVC/JSON eylemleri) tek denetim havuzunda kaydeder. Statik
/// varlıklar ve SignalR taşıması atlanır. İstekler HER ZAMAN denetlenir: gönderim
/// çağrısı, API'ye etkileşimsiz sistem servis hesabı olarak kimlik doğrular (asla
/// oturum açmış kullanıcının jetonuyla değil); böylece denetim günlüğü, kullanıcının
/// yetkileri, süresi dolmuş/geçersiz bir kullanıcı jetonu veya başka bir Web tarafı
/// kısıtlamasıyla asla engellenemez. Gerçek oturum açmış aktör, kaydın doğru
/// ilişkilendirilmesi için istek gövdesinde iletilir. Günlükleme isteği asla bozmaz:
/// her hata yutulur.
/// </summary>
public sealed class WebRequestLoggingMiddleware
{
    private static readonly string[] SkippedPrefixes =
    [
        "/css", "/js", "/lib", "/images", "/img", "/fonts", "/favicon", "/_", "/health",
        // SignalR taşıması: hub negotiate / WebSocket / SSE / uzun yoklama isteklerinin
        // yanıt gövdesini asla sarma — bunları tamponlamak akış bağlantısını bozar ve
        // gerçek zamanlı iletim sessizce başarısız olur.
        "/hubs"
    ];

    private static readonly string[] SkippedExtensions =
    [
        ".css", ".js", ".map", ".png", ".jpg", ".jpeg", ".gif", ".svg", ".ico",
        ".woff", ".woff2", ".ttf", ".eot", ".webp"
    ];

    private readonly RequestDelegate _next;
    private readonly ILogger<WebRequestLoggingMiddleware> _logger;

    /// <summary>Sonraki ara katmanı ve günlükleyiciyi enjekte eder.</summary>
    public WebRequestLoggingMiddleware(RequestDelegate next, ILogger<WebRequestLoggingMiddleware> logger)
    {
        _next = next;
        _logger = logger;
    }

    /// <summary>İsteği maskelenmiş denetim günlüğü ve istisna yakalama ile sarmalar.</summary>
    public async Task InvokeAsync(
        HttpContext context,
        IAuditLogIngestClient ingest,
        IUserApiTokenProvider userTokens,
        IServiceApiTokenProvider serviceTokens)
    {
        // Akış/yükseltme yanıtlarını (SignalR WebSocket/SSE) asla tamponlama:
        // Response.Body'yi bir MemoryStream ile değiştirmek bağlantıyı bozar.
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
            // İstisnayla BİRLİKTE günlükle; böylece tam yığın izi (hatanın oluştuğu
            // kesin metot + satır) framework hata işleyicisine yükselmeden önce yakalanır.
            _logger.LogError(ex,
                "Unhandled exception for {Method} {Path}. CorrelationId: {CorrelationId}.",
                context.Request.Method, context.Request.Path.Value, correlationId);
            throw;
        }
        finally
        {
            stopwatch.Stop();
            var responseBody = ReadResponseBody(context, buffer);

            // İş kuralı başarısızlıklarını da yüzeye çıkar: success=false içeren bir
            // BaseResponse, hiçbir istisna fırlatılmamış olsa bile mantıksal bir başarısızlıktır.
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

    /// <summary>Denetim kaydını güvenli şekilde (hata fırlatmadan) servis jetonuyla API'ye iletir.</summary>
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
            // Denetim günlüğü API'ye HER ZAMAN etkileşimsiz sistem servis hesabı olarak
            // kimlik doğrular — asla oturum açmış kullanıcının jetonuyla değil. Bu,
            // denetim izinin kullanıcının yetkileri, süresi dolmuş/geçersiz bir kullanıcı
            // jetonu veya başka bir Web tarafı kısıtlamasıyla asla engellenememesini
            // garanti eder: HER istek (anonim giriş denemeleri dahil) yakalanır. Gerçek
            // aktör, kaydın yine oturum açmış kullanıcıya ilişkilendirilmesi için istek
            // gövdesinde iletilir.
            var serviceToken = await serviceTokens.GetAccessTokenAsync(context.RequestAborted);
            if (string.IsNullOrEmpty(serviceToken))
            {
                // Servis jetonu kullanılamıyor (ör. API kapalı). İsteği sessizce kaybetme
                // — nedeni kaydet ve yalnızca bu tek girdiyi atla.
                _logger.LogWarning("Skipping audit for {Path}: no service token available.",
                    context.Request.Path);
                return;
            }

            // Gerçek oturum açmış aktörü (varsa) çerez kimliğinden çöz.
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
            // Denetim, kullanıcı isteğini asla bozmamalıdır.
            _logger.LogWarning(ex, "Failed to forward Web audit log entry for {Path}.", context.Request.Path);
        }
    }

    /// <summary>
    /// Oturum açmış kullanıcının kimliğini ve adını çerez kimliğinden çıkarır; böylece
    /// gönderim çağrısının kendisi sistem servis hesabı olarak kimlik doğrulasa bile
    /// denetim kaydı doğru ilişkilendirilir. Anonim istekler için (ör. giriş POST'u)
    /// <c>null</c> döner.
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
    /// Yakalanan yanıt zarfını inceler ve eğer bir istisnadan KAYNAKLANMAYAN bir iş
    /// kuralı başarısızlığını (<c>success:false</c>) temsil ediyorsa kaydeder; böylece
    /// başarısız sonuçlar günlüklerde sessizce kaybolmaz.
    /// </summary>
    private void LogFailedEnvelope(HttpContext context, Guid correlationId, string? responseBody, Exception? exception)
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

    /// <summary>İstek gövdesini güvenli ve maskelenmiş şekilde yakalar (yeniden okunabilir).</summary>
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

    /// <summary>Yakalanan yanıt gövdesini okur ve maskeleyerek döndürür.</summary>
    private static string? ReadResponseBody(HttpContext context, MemoryStream buffer)
    {
        if (buffer.Length == 0) return null;
        if (!IsTextCapturable(context.Response.ContentType)) return $"[skipped:{context.Response.ContentType}]";

        buffer.Position = 0;
        using var reader = new StreamReader(buffer, Encoding.UTF8, detectEncodingFromByteOrderMarks: false, leaveOpen: true);
        var raw = reader.ReadToEnd();
        return SensitiveDataMasker.MaskBody(raw, context.Response.ContentType);
    }

    /// <summary>İçerik türünün metin olarak güvenle yakalanabilir olup olmadığını belirler.</summary>
    private static bool IsTextCapturable(string? contentType)
    {
        if (string.IsNullOrEmpty(contentType)) return true;
        var ct = contentType.ToLowerInvariant();
        return ct.Contains("json")
               || ct.Contains("xml")
               || ct.Contains("text/")
               || ct.Contains("x-www-form-urlencoded");
    }

    /// <summary>Yolun, denetimden atlanması gereken bir statik/akış yolu olup olmadığını belirler.</summary>
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

