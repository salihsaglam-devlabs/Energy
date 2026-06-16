namespace Energy.Shared.Models.V1.Finance.FinancialTransactionLine.Responses;

/// <summary>FinancialTransactionLine liste satırı.</summary>
public class FinancialTransactionLineListResponse
{
    /// <summary>Kimlik.</summary>
    public Guid Id { get; set; }

    /// <summary>FinancialTransactionId</summary>
    public Guid FinancialTransactionId { get; set; }

    /// <summary>CostCenterId</summary>
    public Guid? CostCenterId { get; set; }

    /// <summary>ProjectId</summary>
    public Guid? ProjectId { get; set; }

    /// <summary>Amount</summary>
    public decimal Amount { get; set; }

    /// <summary>Description</summary>
    public string? Description { get; set; }

    /// <summary>Oluşturma zamanı.</summary>
    public DateTime CreatedAt { get; set; }
}
