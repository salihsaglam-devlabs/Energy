namespace Energy.Domain.Identity;

/// <summary>Hard-deleted join: removing the row unassigns the role.</summary>
public class UserRole
{
    public Guid UserId { get; set; }
    public Guid RoleId { get; set; }
}

