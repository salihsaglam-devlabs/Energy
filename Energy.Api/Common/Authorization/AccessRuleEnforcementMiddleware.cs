using System.Text.Json;
using Energy.Application.System.Services;
using Energy.Localization;
using Energy.Shared.Models.V1.Common.Responses;
using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.Localization;

namespace Energy.Api.Common.Authorization;

/// <summary>
/// Evaluates centralized access rules (scope=API) against the current request.
/// If a matching rule exists, every mapped permission must be present on the
/// authenticated principal's <c>permission</c> claims.
/// </summary>
public sealed class AccessRuleEnforcementMiddleware
{
    private readonly RequestDelegate _next;

    public AccessRuleEnforcementMiddleware(RequestDelegate next)
    {
        _next = next;
    }

    public async Task InvokeAsync(
        HttpContext context,
        IAccessRuleService accessRuleService,
        IStringLocalizer<SharedResource> localizer)
    {
        var endpoint = context.GetEndpoint();
        if (endpoint?.Metadata.GetMetadata<IAllowAnonymous>() is not null)
        {
            await _next(context);
            return;
        }

        if (context.User?.Identity?.IsAuthenticated != true)
        {
            await _next(context);
            return;
        }

        var path = Canonicalize(context.Request.Path.Value ?? string.Empty);
        var method = context.Request.Method;

        var requiredPermissions = await accessRuleService.GetRequiredPermissionCodesAsync(
            scope: "API",
            path: path,
            httpMethod: method,
            context.RequestAborted);

        if (requiredPermissions.Count == 0)
        {
            await _next(context);
            return;
        }

        var hasAllPermissions = requiredPermissions.All(permissionCode =>
            context.User.HasClaim(PermissionAuthorizationHandler.PermissionClaimType, permissionCode));

        if (hasAllPermissions)
        {
            await _next(context);
            return;
        }

        context.Response.StatusCode = StatusCodes.Status403Forbidden;
        context.Response.ContentType = "application/json";
        var payload = BaseResponse<object>.Failure(
            localizer.GetText(LocalizationKeys.Auth.AccessDeniedTitle, "Access denied."),
            new[] { localizer.GetText(LocalizationKeys.Messages.AccessRuleCentralValidationFailed, "Central access rule validation failed for this endpoint.") });
        await context.Response.WriteAsync(JsonSerializer.Serialize(payload));
    }

    private static string Canonicalize(string path)
    {
        if (string.IsNullOrWhiteSpace(path))
        {
            return "/";
        }

        var normalized = path.Trim();
        if (!normalized.StartsWith('/'))
        {
            normalized = "/" + normalized;
        }

        if (normalized.Length > 1)
        {
            normalized = normalized.TrimEnd('/');
        }

        return normalized;
    }
}

