namespace Energy.Shared.Models.V1.Notifications.Notification.Requests;

/// <summary>Notification oluşturma isteği.</summary>
public class CreateNotificationRequest
{
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
