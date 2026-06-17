namespace Energy.Shared.Models.V1.Finance.CollectionAllocation.Requests;

/// <summary>CollectionAllocation oluşturma isteği.</summary>
public class CreateCollectionAllocationRequest
{
    /// <summary>CollectionId</summary>
    public Guid CollectionId { get; set; }

    /// <summary>ReceivableId</summary>
    public Guid ReceivableId { get; set; }

    /// <summary>Amount</summary>
    public decimal Amount { get; set; }
}
