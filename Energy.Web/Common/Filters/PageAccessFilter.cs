using Energy.Web.Common;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace Energy.Web.Common.Filters;

/// <summary>
/// Enforces page-level access for HTML screens. A controller/action decorated
/// with <see cref="PagePermissionAttribute"/> is only reachable when the signed
/// in user holds the matching permission claim. Authentication itself is handled
/// by the cookie fallback policy; this filter only adds the permission gate so
/// an unauthorized user is redirected to the access-denied page instead of
/// seeing an empty screen whose data calls later fail with 403. API-side
/// authorization remains the source of truth for every actual data operation.
/// </summary>
public sealed class PageAccessFilter : IAsyncAuthorizationFilter
{
    public Task OnAuthorizationAsync(AuthorizationFilterContext context)
    {
        // Anonymous pages (login, access-denied, ...) are never gated.
        if (context.ActionDescriptor.EndpointMetadata.OfType<IAllowAnonymous>().Any())
        {
            return Task.CompletedTask;
        }

        var attribute = context.ActionDescriptor.EndpointMetadata
            .OfType<PagePermissionAttribute>()
            .LastOrDefault();

        if (attribute is null)
        {
            return Task.CompletedTask;
        }

        var user = context.HttpContext.User;

        // Unauthenticated requests are left for the cookie challenge to handle
        // (redirect to /account/login); we only add the permission gate once a
        // user is actually present.
        if (user.Identity?.IsAuthenticated != true)
        {
            return Task.CompletedTask;
        }

        if (!user.HasPermission(attribute.Permission))
        {
            context.Result = new RedirectToActionResult("AccessDenied", "Account", null);
        }

        return Task.CompletedTask;
    }
}
