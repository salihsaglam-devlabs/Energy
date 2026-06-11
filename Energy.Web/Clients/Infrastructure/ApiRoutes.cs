using Energy.Shared.Versioning;

namespace Energy.Web.Clients.Infrastructure;

/// <summary>
/// Centralised, versioned API endpoint paths used by all <see cref="ApiClientBase"/>
/// implementations. Keeping the routes in one place makes them trivial to audit
/// and refactor when the API surface changes.
/// </summary>
internal static class ApiRoutes
{
    private static readonly string V1 = $"api/v{ApiVersions.V1UrlSegment}";

    public static class Auth
    {
        public static readonly string Base = $"{V1}/auth";
        public static readonly string Login = $"{Base}/login";
        public static readonly string ValidateCredentials = $"{Base}/validate-credentials";
    }

    public static class Users
    {
        public static readonly string Base = $"{V1}/users";
        public static readonly string Me = $"{Base}/me";
        public static readonly string MyProfileImage = $"{Base}/me/profile-image";
        public static string ById(Guid id) => $"{Base}/{id}";
        public static string Roles(Guid id) => $"{Base}/{id}/roles";
        public static string Password(Guid id) => $"{Base}/{id}/password";
        public static string ProfileImage(Guid id) => $"{Base}/{id}/profile-image";
        public static readonly string SeedAdmin = $"{Base}/seed-admin";
    }

    public static class Roles
    {
        public static readonly string Base = $"{V1}/roles";
        public static string ById(Guid id) => $"{Base}/{id}";
        public static string Permissions(Guid id) => $"{Base}/{id}/permissions";
        public static string Menus(Guid id) => $"{Base}/{id}/menus";
    }

    public static class Permissions
    {
        public static readonly string Base = $"{V1}/permissions";
        public static string ById(Guid id) => $"{Base}/{id}";
        public static readonly string SeedDefaults = $"{Base}/seed-defaults";
    }

    public static class Menus
    {
        public static readonly string Base = $"{V1}/menus";
        public static readonly string Tree = $"{Base}/tree";
        public static string ById(Guid id) => $"{Base}/{id}";
        public static string Permissions(Guid id) => $"{Base}/{id}/permissions";
        public static readonly string SeedDefaults = $"{Base}/seed-defaults";
    }

    public static class AccessRules
    {
        public static readonly string Base = $"{V1}/access-rules";
        public static string ById(Guid id) => $"{Base}/{id}";
        public static string Permissions(Guid id) => $"{Base}/{id}/permissions";
        public static readonly string RequiredPermissions = $"{Base}/required-permissions";
    }

    public static class Home
    {
        public static readonly string Dashboard = $"{V1}/home/dashboard";
    }

    public static class Localization
    {
        public static readonly string Base = $"{V1}/localization";
        public static string ByKey(string key) => $"{Base}/{Uri.EscapeDataString(key)}";
        public static readonly string ImportFromResx = $"{Base}/import-from-resx";
        public static readonly string Test = $"{Base}/test";
    }
}

