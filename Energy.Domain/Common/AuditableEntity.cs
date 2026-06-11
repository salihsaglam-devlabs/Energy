namespace Energy.Domain.Common;

/// <summary>
/// Single audit + soft-delete contract shared by every aggregate root.
/// Join tables and append-only logs do not inherit from this type.
/// </summary>
public abstract class AuditableEntity
{
    public Guid Id { get; set; }

    public DateTime CreatedAt { get; set; }
    public Guid? CreatedBy { get; set; }

    public DateTime? UpdatedAt { get; set; }
    public Guid? UpdatedBy { get; set; }

    public bool IsDeleted { get; set; }
    public DateTime? DeletedAt { get; set; }
    public Guid? DeletedBy { get; set; }
}

