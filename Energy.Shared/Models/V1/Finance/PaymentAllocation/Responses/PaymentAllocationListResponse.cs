namespace Energy.Shared.Models.V1.Finance.PaymentAllocation.Responses;

/// <summary>PaymentAllocation liste satırı.</summary>
public class PaymentAllocationListResponse
{
    /// <summary>Kimlik.</summary>
    public Guid Id { get; set; }

    /// <summary>PaymentId</summary>
    public Guid PaymentId { get; set; }

    /// <summary>PayableId</summary>
    public Guid PayableId { get; set; }

    /// <summary>Amount</summary>
    public decimal Amount { get; set; }

    /// <summary>Oluşturma zamanı.</summary>
    public DateTime CreatedAt { get; set; }
}
