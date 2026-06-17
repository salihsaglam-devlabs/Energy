namespace Energy.Shared.Models.V1.Notifications.NotificationPreference.Requests;

/// <summary>NotificationPreference oluşturma isteği.</summary>
public class CreateNotificationPreferenceRequest
{
    /// <summary>UserId</summary>
    public Guid UserId { get; set; }

    /// <summary>NotificationType</summary>
    public string NotificationType { get; set; } = string.Empty;

    /// <summary>InAppEnabled</summary>
    public bool InAppEnabled { get; set; }

    /// <summary>EmailEnabled</summary>
    public bool EmailEnabled { get; set; }
}
