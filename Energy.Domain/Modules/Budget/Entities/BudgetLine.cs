using Energy.Domain.Common;

namespace Energy.Domain.Modules.Budget;

/// <summary>Bütçe satırı.</summary>
public class BudgetLine : AuditableEntity
{
    public Guid BudgetId { get; set; }
    public Guid? ProjectId { get; set; }
    public Guid? CostCenterId { get; set; }
    public string Description { get; set; } = string.Empty;
    public decimal PlannedAmount { get; set; }
}
