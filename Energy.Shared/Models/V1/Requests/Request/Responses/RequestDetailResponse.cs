using Energy.Shared.Common;
namespace Energy.Shared.Models.V1.Requests.Request.Responses;

/// <summary>Request detay görünümü.</summary>
public class RequestDetailResponse
{
    /// <summary>Kimlik.</summary>
    public Guid Id { get; set; }

    /// <summary>Oluşturma zamanı</summary>
    public DateTime CreatedAt { get; set; }

    /// <summary>Oluşturan kullanıcı</summary>
    public Guid? CreatedBy { get; set; }

    /// <summary>Son güncelleme zamanı</summary>
    public DateTime? UpdatedAt { get; set; }

    /// <summary>Güncelleyen kullanıcı</summary>
    public Guid? UpdatedBy { get; set; }

    /// <summary>Soft delete bayrağı</summary>
    public bool IsDeleted { get; set; }

    /// <summary>Silinme zamanı</summary>
    public DateTime? DeletedAt { get; set; }

    /// <summary>Silen kullanıcı</summary>
    public Guid? DeletedBy { get; set; }

    /// <summary>Talep türü</summary>
    public Guid RequestTypeId { get; set; }

    /// <summary>Opsiyonel proje</summary>
    public Guid? ProjectId { get; set; }

    /// <summary>Talep sahibi</summary>
    public Guid RequestedByUserId { get; set; }

    /// <summary>Durum</summary>
    public RequestStatus Status { get; set; }

    /// <summary>Talep no</summary>
    public string RequestNo { get; set; } = string.Empty;

    /// <summary>RequestDate</summary>
    public DateTime RequestDate { get; set; }

    /// <summary>Description</summary>
    public string? Description { get; set; }

    /// <summary>ApprovalRequestId</summary>
    public Guid? ApprovalRequestId { get; set; }
}
