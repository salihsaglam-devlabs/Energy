using Energy.Domain.Common;

namespace Energy.Domain.Modules.Requests;

/// <summary>
/// Talep satırları
/// </summary>
public class RequestLine : AuditableEntity
{
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
}
