using Energy.Domain.Common;

namespace Energy.Domain.Modules.Requests;

/// <summary>
/// Genel talep başlıkları
/// </summary>
public class Request : AuditableEntity
{
    /// <summary>Talep türü</summary>
    public Guid RequestTypeId { get; set; }

    /// <summary>Opsiyonel proje</summary>
    public Guid? ProjectId { get; set; }

    /// <summary>Talep sahibi</summary>
    public Guid RequestedByUserId { get; set; }

    /// <summary>Durum</summary>
    public string Status { get; set; } = string.Empty;

    /// <summary>Talep no</summary>
    public string RequestNo { get; set; } = string.Empty;

    /// <summary>RequestDate</summary>
    public DateTime RequestDate { get; set; }

    /// <summary>Description</summary>
    public string? Description { get; set; }

    /// <summary>ApprovalRequestId</summary>
    public Guid? ApprovalRequestId { get; set; }
}
