namespace Energy.Domain.Identity;

/// <summary>
/// A direct user→permission grant that is layered ON TOP of the permissions a
/// user already inherits through their roles. Removing the row revokes the
/// direct grant; permissions still owned through a role are unaffected.
/// </summary>
public class UserPermission
{
    public Guid UserId { get; set; }
    public string PermissionCode { get; set; } = string.Empty;
}

