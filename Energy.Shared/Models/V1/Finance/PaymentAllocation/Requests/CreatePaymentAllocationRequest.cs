namespace Energy.Shared.Models.V1.Finance.PaymentAllocation.Requests;

/// <summary>PaymentAllocation oluşturma isteği.</summary>
public class CreatePaymentAllocationRequest
{
    /// <summary>PaymentId</summary>
    public Guid PaymentId { get; set; }

    /// <summary>PayableId</summary>
    public Guid PayableId { get; set; }

    /// <summary>Amount</summary>
    public decimal Amount { get; set; }
}
