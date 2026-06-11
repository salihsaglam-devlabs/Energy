namespace Energy.Domain.Identity;

/// <summary>
/// THE single source of truth for "who can do what". No other table maps
/// permissions to principals.
/// </summary>
public class RolePermission
{
    public Guid RoleId { get; set; }
    public string PermissionCode { get; set; } = string.Empty;
}

