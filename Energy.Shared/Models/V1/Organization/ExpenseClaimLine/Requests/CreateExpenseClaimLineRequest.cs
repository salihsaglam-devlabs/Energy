namespace Energy.Shared.Models.V1.Organization.ExpenseClaimLine.Requests;

/// <summary>ExpenseClaimLine oluşturma isteği.</summary>
public class CreateExpenseClaimLineRequest
{
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
}
