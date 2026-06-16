namespace Energy.Shared.Models.V1.Requests.Request.Responses;

/// <summary>Request liste satırı.</summary>
public class RequestListResponse
{
    /// <summary>Kimlik.</summary>
    public Guid Id { get; set; }

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

    /// <summary>Oluşturma zamanı.</summary>
    public DateTime CreatedAt { get; set; }
}
