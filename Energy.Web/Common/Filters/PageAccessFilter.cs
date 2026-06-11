using Energy.Shared.Identity;
using Energy.Shared.Models.V1.Common.Responses;
using Energy.Web.Clients.System;
using Energy.Web.Common.Exceptions;
using Energy.Web.Services.Navigation;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace Energy.Web.Common.Filters;

/// <summary>
/// Checks menu-based page access before rendering HTML screens.
/// Non-HTML (AJAX/API) requests are ignored and remain protected by API-side authorization.
/// </summary>
public sealed class PageAccessFilter : IAsyncAuthorizationFilter
{
    private readonly INavigationService _navigationService;
    private readonly IAccessRuleApiClient _accessRuleApiClient;

    public PageAccessFilter(INavigationService navigationService, IAccessRuleApiClient accessRuleApiClient)
    {
        _navigationService = navigationService;
        _accessRuleApiClient = accessRuleApiClient;
    }

    public async Task OnAuthorizationAsync(AuthorizationFilterContext context)
    {
        var endpoint = context.HttpContext.GetEndpoint();
        if (endpoint?.Metadata.GetMetadata<IAllowAnonymous>() is not null)
        {
            return;
        }

        var user = context.HttpContext.User;
        if (user.Identity?.IsAuthenticated != true)
        {
            return;
        }

        var request = context.HttpContext.Request;
        if (!HttpMethods.IsGet(request.Method) || !IsHtmlNavigation(request))
        {
            return;
        }

        var requestedUrl = Canonicalize(request.Path.Value ?? string.Empty);
        if (string.IsNullOrEmpty(requestedUrl))
        {
            return;
        }

        // Per-user pages that every authenticated caller must always be able to
        // reach, regardless of the role/menu assignment. Skipping the menu lookup
        // here also avoids a redirect loop while the seed catches up on startup
        // (e.g. immediately after a fresh deployment that just introduced /profile).
        if (IsAlwaysAllowed(requestedUrl))
        {
            return;
        }

        var items = await _navigationService.GetMenuForUserAsync(user, context.HttpContext.RequestAborted);
        if (!HasAccess(items, requestedUrl))
        {

            var permissionHint = "Required permission";

            context.Result = new RedirectToActionResult(
                actionName: "AccessDenied",
                controllerName: "Account",
                routeValues: new { path = requestedUrl, permission = permissionHint });
            return;
        }

        var rulePermissionsEnvelope = await SafeGetRequiredPermissionsAsync(
            requestedUrl,
            request.Method,
            context.HttpContext.RequestAborted);

        if (rulePermissionsEnvelope is null
            || !rulePermissionsEnvelope.IsSuccess
            || rulePermissionsEnvelope.Data is null
            || rulePermissionsEnvelope.Data.Count == 0)
        {
            return;
        }

        var hasAllRulePermissions = rulePermissionsEnvelope.Data.All(user.HasPermission);
        if (hasAllRulePermissions)
        {
            return;
        }

        context.Result = new RedirectToActionResult(
            actionName: "AccessDenied",
            controllerName: "Account",
            routeValues: new
            {
                path = requestedUrl,
                permission = string.Join(", ", rulePermissionsEnvelope.Data)
            });
    }

    private async Task<BaseResponse<IReadOnlyList<string>>?> SafeGetRequiredPermissionsAsync(
        string requestedUrl,
        string httpMethod,
        CancellationToken cancellationToken)
    {
        try
        {
            return await _accessRuleApiClient.GetRequiredPermissionsAsync(
                scope: "PAGE",
                path: requestedUrl,
                httpMethod: httpMethod,
                cancellationToken);
        }
        catch (ApiForbiddenException)
        {
            // Caller is not allowed to read required permissions — fall back to
            // the menu-based check that already succeeded above.
            return null;
        }
        catch (ApiUnauthorizedException)
        {
            return null;
        }
    }

    private static readonly string[] AlwaysAllowedPrefixes =
    [
        "/profile",
        "/dashboard"
    ];

    private static bool IsAlwaysAllowed(string requestedUrl)
    {
        foreach (var prefix in AlwaysAllowedPrefixes)
        {
            if (string.Equals(requestedUrl, prefix, StringComparison.OrdinalIgnoreCase))
            {
                return true;
            }

            if (requestedUrl.StartsWith(prefix + "/", StringComparison.OrdinalIgnoreCase))
            {
                return true;
            }
        }

        return false;
    }

    private static bool HasAccess(IReadOnlyList<NavigationItem> items, string requestedUrl)    {
        if (items.Count == 0)
        {
            return false;
        }

        var parentIds = items
            .Where(item => item.ParentId.HasValue)
            .Select(item => item.ParentId!.Value)
            .ToHashSet();

        foreach (var item in items)
        {
            if (string.IsNullOrWhiteSpace(item.Url))
            {
                continue;
            }

            var allowedUrl = Canonicalize(item.Url);
            if (string.IsNullOrEmpty(allowedUrl))
            {
                continue;
            }

            if (string.Equals(requestedUrl, allowedUrl, StringComparison.OrdinalIgnoreCase))
            {
                return true;
            }

            var isContainer = parentIds.Contains(item.Id);
            if (!isContainer && requestedUrl.StartsWith(allowedUrl + "/", StringComparison.OrdinalIgnoreCase))
            {
                return true;
            }
        }

        return false;
    }

    private static bool IsHtmlNavigation(HttpRequest request)
    {
        if (string.Equals(request.Headers.XRequestedWith, "XMLHttpRequest", StringComparison.OrdinalIgnoreCase))
        {
            return false;
        }

        var accept = request.Headers.Accept.ToString();
        return accept.Contains("text/html", StringComparison.OrdinalIgnoreCase);
    }

    private static string Canonicalize(string path)
    {
        if (string.IsNullOrWhiteSpace(path))
        {
            return string.Empty;
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

        if (normalized == "/")
        {
            return "/dashboard";
        }

        return RemapAlias(normalized);
    }

    private static string RemapAlias(string path)
    {
        // Support controller aliases (/users, /roles, ...) while menu urls use /system/*.
        var aliasMap = new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase)
        {
            ["/users"] = "/system/users",
            ["/roles"] = "/system/roles",
            ["/permissions"] = "/system/permissions",
            ["/menus"] = "/system/menus",
            ["/access-rules"] = "/system/access-rules",
            ["/localization"] = "/system/localization"
        };

        foreach (var (aliasPrefix, canonicalPrefix) in aliasMap)
        {
            if (string.Equals(path, aliasPrefix, StringComparison.OrdinalIgnoreCase))
            {
                return canonicalPrefix;
            }

            if (path.StartsWith(aliasPrefix + "/", StringComparison.OrdinalIgnoreCase))
            {
                return canonicalPrefix + path.Substring(aliasPrefix.Length);
            }
        }

        return path;
    }
}

