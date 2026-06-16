namespace Energy.Shared.Models.V1.Organization.ExpenseClaimLine.Requests;

/// <summary>ExpenseClaimLine güncelleme isteği.</summary>
public class UpdateExpenseClaimLineRequest
{
    /// <summary>Güncellenecek kaydın kimliği.</summary>
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
}
