using System.Text.Json;
using Energy.Application.Identity.Services;
using Energy.Application.System.Services;
using Energy.Localization;
using Energy.Shared.Models.V1.Common.Responses;
using Microsoft.Extensions.Localization;
using Microsoft.Extensions.Logging;

namespace Energy.Api.Common.Authorization;

/// <summary>
/// Tek yetkilendirme kapısı. Akış:
/// 1. API olmayan ve anonim işaretli rotaları atla.
/// 2. Uç noktayı <see cref="IApiEndpointService"/> ile çöz.
/// 3. Rota bilinmiyorsa veya pasifse varsayılan REDDET.
/// 4. Uç nokta bir yetki bildirmiyorsa izin ver.
/// 5. Kimliği doğrulanmış bir kullanıcı ve eşleşen bir yetki talebi (claim) gerektir.
/// </summary>
public sealed class PermissionAuthorizationMiddleware
{
    private readonly RequestDelegate _next;
    private readonly ILogger<PermissionAuthorizationMiddleware> _logger;

    /// <summary>Sonraki ara katmanı ve günlükleyiciyi enjekte eder.</summary>
    public PermissionAuthorizationMiddleware(
        RequestDelegate next,
        ILogger<PermissionAuthorizationMiddleware> logger)
    {
        _next = next;
        _logger = logger;
    }

    /// <summary>İsteği uç nokta tabanlı yetki kontrolünden geçirir.</summary>
    public async Task InvokeAsync(
        HttpContext context,
        IApiEndpointService endpoints,
        IPermissionResolver permissions,
        ICurrentUser currentUser,
        IStringLocalizer<SharedResource> localizer)
    {
        var path = context.Request.Path.Value ?? string.Empty;

        // Yalnızca API yüzeyini koru; statik, Swagger ve /api olmayan rotalar geçer.
        if (!path.StartsWith("/api/", StringComparison.OrdinalIgnoreCase))
        {
            await _next(context);
            return;
        }

        _logger.LogInformation(
            "[PermAuth] {Method} {Path} IsAuth={IsAuth} User={User} AuthType={Type}",
            context.Request.Method, path,
            context.User.Identity?.IsAuthenticated,
            context.User.Identity?.Name ?? "<anon>",
            context.User.Identity?.AuthenticationType ?? "<none>");

        var endpoint = context.GetEndpoint();
        if (endpoint?.Metadata.GetMetadata<Microsoft.AspNetCore.Authorization.IAllowAnonymous>() is not null)
        {
            await _next(context);
            return;
        }

        var match = await endpoints.ResolveAsync(context.Request.Method, path);
        if (match is null)
        {
            _logger.LogWarning("[PermAuth] Endpoint not registered: {Method} {Path}", context.Request.Method, path);
            await DenyAsync(context, StatusCodes.Status403Forbidden,
                localizer[LocalizationKeys.Messages.EndpointNotRegistered].Value);
            return;
        }
        if (!match.IsActive)
        {
            _logger.LogWarning("[PermAuth] Endpoint disabled: {Method} {Path}", context.Request.Method, path);
            await DenyAsync(context, StatusCodes.Status403Forbidden,
                localizer[LocalizationKeys.Messages.EndpointDisabled].Value);
            return;
        }

        var required = match.RequiredPermissionCode;

        // Açıkça [AllowAnonymous] olmayan (yukarıda dönenler) kayıtlı ve etkin her uç
        // nokta, belirli bir yetki ekli olmasa bile (örn. "menüm") kimliği doğrulanmış
        // bir kullanıcı gerektirir.
        if (!currentUser.IsAuthenticated || currentUser.UserId is null)
        {
            _logger.LogWarning(
                "[PermAuth] DENIED 401: not authenticated. Required={Required} IsAuth={IsAuth} UserId={UserId}",
                required, currentUser.IsAuthenticated, currentUser.UserId);
            await DenyAsync(context, StatusCodes.Status401Unauthorized,
                localizer[LocalizationKeys.Messages.AuthenticationRequired].Value);
            return;
        }

        if (string.IsNullOrEmpty(required))
        {
            await _next(context);
            return;
        }

        var hasPermission = await permissions.HasPermissionAsync(currentUser.UserId.Value, required);
        if (!hasPermission)
        {
            _logger.LogWarning("[PermAuth] DENIED 403: user {UserId} missing {Required}",
                currentUser.UserId, required);
            await DenyAsync(context, StatusCodes.Status403Forbidden,
                localizer[LocalizationKeys.Messages.MissingPermission, required].Value);
            return;
        }

        await _next(context);
    }

    /// <summary>Standartlaştırılmış bir reddetme (deny) yanıtı yazar.</summary>
    private static async Task DenyAsync(HttpContext context, int status, string message)
    {
        if (context.Response.HasStarted) return;
        context.Response.StatusCode = status;
        context.Response.ContentType = "application/json";
        var payload = BaseResponse<object>.Failure(message, new[] { message });
        await context.Response.WriteAsync(JsonSerializer.Serialize(payload));
    }
}
