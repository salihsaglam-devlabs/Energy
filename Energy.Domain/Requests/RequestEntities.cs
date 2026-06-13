using Energy.Domain.Common;

namespace Energy.Domain.Requests;

/// <summary>Talep türü.</summary>
public class RequestType : AuditableEntity
{
    public string Code { get; set; } = string.Empty;
    public string Name { get; set; } = string.Empty;
    /// <summary>Material, Service, Equipment, Personnel, Other.</summary>
    public string Category { get; set; } = "Material";
    public bool IsActive { get; set; } = true;
}

/// <summary>Genel talep başlığı.</summary>
public class Request : AuditableEntity
{
    public Guid RequestTypeId { get; set; }
    public Guid? ProjectId { get; set; }
    public Guid RequestedByUserId { get; set; }
    public RequestStatus Status { get; set; } = RequestStatus.Draft;
    public string RequestNo { get; set; } = string.Empty;
    public DateTime RequestDate { get; set; }
    public string? Description { get; set; }
    public Guid? ApprovalRequestId { get; set; }
}

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

