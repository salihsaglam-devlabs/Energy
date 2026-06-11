namespace Energy.Web.Common.Filters;

/// <summary>
/// Declares the permission code required to render an MVC page (controller or
/// action). Enforced by <see cref="PageAccessFilter"/> against the permission
/// claims written to the auth cookie at sign-in. Uses the same
/// <see cref="Energy.Shared.Identity.Permissions.PermissionCatalog"/> codes as
/// the API so naming stays consistent end-to-end.
/// </summary>
[AttributeUsage(AttributeTargets.Class | AttributeTargets.Method, AllowMultiple = false, Inherited = true)]
public sealed class PagePermissionAttribute : Attribute
{
    public PagePermissionAttribute(string permission) => Permission = permission;

    public string Permission { get; }
}

