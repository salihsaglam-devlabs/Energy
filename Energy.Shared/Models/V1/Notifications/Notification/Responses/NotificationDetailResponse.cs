namespace Energy.Shared.Models.V1.Notifications.Notification.Responses;

/// <summary>Notification detay görünümü.</summary>
public class NotificationDetailResponse
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

    /// <summary>Title</summary>
    public string Title { get; set; } = string.Empty;

    /// <summary>Body</summary>
    public string? Body { get; set; }

    /// <summary>NotificationType</summary>
    public string NotificationType { get; set; } = string.Empty;

    /// <summary>RelatedModule</summary>
    public string? RelatedModule { get; set; }

    /// <summary>RelatedEntityType</summary>
    public string? RelatedEntityType { get; set; }

    /// <summary>RelatedEntityId</summary>
    public Guid? RelatedEntityId { get; set; }
}
