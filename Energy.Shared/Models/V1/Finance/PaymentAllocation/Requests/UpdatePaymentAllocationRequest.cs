namespace Energy.Shared.Models.V1.Finance.PaymentAllocation.Requests;

/// <summary>PaymentAllocation güncelleme isteği.</summary>
public class UpdatePaymentAllocationRequest
{
    /// <summary>Güncellenecek kaydın kimliği.</summary>
    public Guid Id { get; set; }

    /// <summary>PaymentId</summary>
    public Guid PaymentId { get; set; }

    /// <summary>PayableId</summary>
    public Guid PayableId { get; set; }

    /// <summary>Amount</summary>
    public decimal Amount { get; set; }
}
