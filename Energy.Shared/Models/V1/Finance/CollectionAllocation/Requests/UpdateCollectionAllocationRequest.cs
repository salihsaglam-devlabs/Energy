namespace Energy.Shared.Models.V1.Finance.CollectionAllocation.Requests;

/// <summary>CollectionAllocation güncelleme isteği.</summary>
public class UpdateCollectionAllocationRequest
{
    /// <summary>Güncellenecek kaydın kimliği.</summary>
    public Guid Id { get; set; }

    /// <summary>CollectionId</summary>
    public Guid CollectionId { get; set; }

    /// <summary>ReceivableId</summary>
    public Guid ReceivableId { get; set; }

    /// <summary>Amount</summary>
    public decimal Amount { get; set; }
}
