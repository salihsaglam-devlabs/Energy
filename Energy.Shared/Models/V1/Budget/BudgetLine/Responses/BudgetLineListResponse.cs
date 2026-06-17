namespace Energy.Shared.Models.V1.Budget.BudgetLine.Responses;

/// <summary>BudgetLine liste satırı.</summary>
public class BudgetLineListResponse
{
    /// <summary>Kimlik.</summary>
    public Guid Id { get; set; }

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

    /// <summary>Oluşturma zamanı.</summary>
    public DateTime CreatedAt { get; set; }
}
