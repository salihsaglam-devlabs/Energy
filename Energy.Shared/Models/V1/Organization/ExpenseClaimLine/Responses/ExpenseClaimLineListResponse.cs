namespace Energy.Shared.Models.V1.Organization.ExpenseClaimLine.Responses;

/// <summary>ExpenseClaimLine liste satırı.</summary>
public class ExpenseClaimLineListResponse
{
    /// <summary>Kimlik.</summary>
    public Guid Id { get; set; }

    /// <summary>ExpenseClaimId</summary>
    public Guid ExpenseClaimId { get; set; }

    /// <summary>Description</summary>
    public string Description { get; set; } = string.Empty;

    /// <summary>ExpenseDate</summary>
    public DateTime ExpenseDate { get; set; }

    /// <summary>Amount</summary>
    public decimal Amount { get; set; }

    /// <summary>Category</summary>
    public string? Category { get; set; }

    /// <summary>Oluşturma zamanı.</summary>
    public DateTime CreatedAt { get; set; }
}
