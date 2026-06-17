namespace Energy.Shared.Models.V1.Finance.CollectionAllocation.Responses;

/// <summary>CollectionAllocation liste satırı.</summary>
public class CollectionAllocationListResponse
{
    /// <summary>Kimlik.</summary>
    public Guid Id { get; set; }

    /// <summary>CollectionId</summary>
    public Guid CollectionId { get; set; }

    /// <summary>ReceivableId</summary>
    public Guid ReceivableId { get; set; }

    /// <summary>Amount</summary>
    public decimal Amount { get; set; }

    /// <summary>Oluşturma zamanı.</summary>
    public DateTime CreatedAt { get; set; }
}
