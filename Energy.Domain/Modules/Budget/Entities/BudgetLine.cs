using Energy.Domain.Common;

namespace Energy.Domain.Modules.Budget;

/// <summary>
/// Bütçe satırları
/// </summary>
public class BudgetLine : AuditableEntity
{
    /// <summary>Budgets referansı</summary>
    public Guid BudgetId { get; set; }

    /// <summary>Projects referansı</summary>
    public Guid? ProjectId { get; set; }

    /// <summary>CostCenterId</summary>
    public Guid? CostCenterId { get; set; }

    /// <summary>Description</summary>
    public string Description { get; set; } = string.Empty;

    /// <summary>PlannedAmount</summary>
    public decimal PlannedAmount { get; set; }
}
