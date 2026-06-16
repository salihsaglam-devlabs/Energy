namespace Energy.Shared.Models.V1.Notifications.NotificationRecipient.Requests;

/// <summary>NotificationRecipient oluşturma isteği.</summary>
public class CreateNotificationRecipientRequest
{
    /// <summary>Notifications referansı</summary>
    public Guid NotificationId { get; set; }

    /// <summary>Users referansı</summary>
    public Guid UserId { get; set; }

    /// <summary>IsRead</summary>
    public bool IsRead { get; set; }

    /// <summary>ReadAt</summary>
    public DateTime? ReadAt { get; set; }
}
