namespace Energy.Shared.Models.V1.Finance.FinancialTransactionLine.Requests;

/// <summary>FinancialTransactionLine oluşturma isteği.</summary>
public class CreateFinancialTransactionLineRequest
{
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
}
