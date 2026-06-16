namespace Energy.Shared.Models.V1.Budget.Budget.Requests;

/// <summary>Budget oluşturma isteği.</summary>
public class CreateBudgetRequest
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
