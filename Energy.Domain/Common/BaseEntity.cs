namespace Energy.Domain.Common;

/// <summary>
/// Base class for all persisted entities. Provides the surrogate key and the
/// audit columns that are managed uniformly across the schema.
/// </summary>
public abstract class BaseEntity
{
    public Guid Id { get; set; }

    public DateTime CreatedAt { get; set; }

    public Guid? CreatedBy { get; set; }

    public DateTime? UpdatedAt { get; set; }

    public Guid? UpdatedBy { get; set; }

    public bool IsDeleted { get; set; }
}
