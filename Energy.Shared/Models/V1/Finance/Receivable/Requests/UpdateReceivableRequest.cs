namespace Energy.Shared.Models.V1.Finance.Receivable.Requests;

/// <summary>Receivable güncelleme isteği.</summary>
public class UpdateReceivableRequest
{
    /// <summary>Güncellenecek kaydın kimliği.</summary>
    public Guid Id { get; set; }

    /// <summary>PartnerId</summary>
    public Guid PartnerId { get; set; }

    /// <summary>CurrencyId</summary>
    public Guid CurrencyId { get; set; }

    /// <summary>Amount</summary>
    public decimal Amount { get; set; }

    /// <summary>RemainingAmount</summary>
    public decimal RemainingAmount { get; set; }

    /// <summary>DueDate</summary>
    public DateTime DueDate { get; set; }

    /// <summary>RelatedModule</summary>
    public string? RelatedModule { get; set; }

    /// <summary>RelatedEntityType</summary>
    public string? RelatedEntityType { get; set; }

    /// <summary>RelatedEntityId</summary>
    public Guid? RelatedEntityId { get; set; }

    /// <summary>IsClosed</summary>
    public bool IsClosed { get; set; }
}
