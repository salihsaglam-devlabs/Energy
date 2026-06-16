using Energy.Domain.Common;

namespace Energy.Domain.Modules.Budget;

/// <summary>
/// Bütçe başlıkları
/// </summary>
public class Budget : AuditableEntity
{
    /// <summary>ProjectId</summary>
    public Guid? ProjectId { get; set; }

    /// <summary>CostCenterId</summary>
    public Guid? CostCenterId { get; set; }

    /// <summary>CurrencyId</summary>
    public Guid CurrencyId { get; set; }

    /// <summary>Name</summary>
    public string Name { get; set; } = string.Empty;

    /// <summary>Year</summary>
    public int Year { get; set; }

    /// <summary>IsActive</summary>
    public bool IsActive { get; set; }
}
