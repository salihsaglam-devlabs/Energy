namespace Energy.Shared.Models.V1.Notifications.NotificationPreference.Responses;

/// <summary>NotificationPreference liste satırı.</summary>
public class NotificationPreferenceListResponse
{
    /// <summary>Kimlik.</summary>
    public Guid Id { get; set; }

    /// <summary>UserId</summary>
    public Guid UserId { get; set; }

    /// <summary>NotificationType</summary>
    public string NotificationType { get; set; } = string.Empty;

    /// <summary>InAppEnabled</summary>
    public bool InAppEnabled { get; set; }

    /// <summary>EmailEnabled</summary>
    public bool EmailEnabled { get; set; }

    /// <summary>Oluşturma zamanı.</summary>
    public DateTime CreatedAt { get; set; }
}
