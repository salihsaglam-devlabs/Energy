namespace Energy.Shared.Models.V1.Notifications.Notification.Responses;

/// <summary>Notification liste satırı.</summary>
public class NotificationListResponse
{
    /// <summary>Kimlik.</summary>
    public Guid Id { get; set; }

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

    /// <summary>Oluşturma zamanı.</summary>
    public DateTime CreatedAt { get; set; }
}
