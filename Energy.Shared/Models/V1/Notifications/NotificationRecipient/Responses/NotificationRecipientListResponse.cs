namespace Energy.Shared.Models.V1.Notifications.NotificationRecipient.Responses;

/// <summary>NotificationRecipient liste satırı.</summary>
public class NotificationRecipientListResponse
{
    /// <summary>Kimlik.</summary>
    public Guid Id { get; set; }

    /// <summary>Notifications referansı</summary>
    public Guid NotificationId { get; set; }

    /// <summary>Users referansı</summary>
    public Guid UserId { get; set; }

    /// <summary>IsRead</summary>
    public bool IsRead { get; set; }

    /// <summary>ReadAt</summary>
    public DateTime? ReadAt { get; set; }

    /// <summary>Oluşturma zamanı.</summary>
    public DateTime CreatedAt { get; set; }
}
