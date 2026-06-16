using Energy.Domain.Common;

namespace Energy.Domain.Modules.Requests;

/// <summary>Talep satırı.</summary>
public class RequestLine : AuditableEntity
{
    public Guid RequestId { get; set; }
    public Guid? MaterialId { get; set; }
    public string? RequestedMaterialText { get; set; }
    public decimal Quantity { get; set; }
    public Guid UnitOfMeasureId { get; set; }
    public string? Note { get; set; }
}
