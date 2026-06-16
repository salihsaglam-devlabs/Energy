namespace Energy.Shared.Models.V1.Requests.RequestLine.Responses;

/// <summary>RequestLine liste satırı.</summary>
public class RequestLineListResponse
{
    /// <summary>Kimlik.</summary>
    public Guid Id { get; set; }

    /// <summary>Talep</summary>
    public Guid RequestId { get; set; }

    /// <summary>Opsiyonel katalog malzemesi</summary>
    public Guid? MaterialId { get; set; }

    /// <summary>Serbest malzeme açıklaması</summary>
    public string? RequestedMaterialText { get; set; }

    /// <summary>Miktar</summary>
    public decimal Quantity { get; set; }

    /// <summary>Birim</summary>
    public Guid UnitOfMeasureId { get; set; }

    /// <summary>Note</summary>
    public string? Note { get; set; }

    /// <summary>Oluşturma zamanı.</summary>
    public DateTime CreatedAt { get; set; }
}
