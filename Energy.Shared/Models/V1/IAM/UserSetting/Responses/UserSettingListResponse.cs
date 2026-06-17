namespace Energy.Shared.Models.V1.IAM.UserSetting.Responses;

/// <summary>UserSetting liste satırı.</summary>
public class UserSettingListResponse
{
    /// <summary>Kimlik.</summary>
    public Guid Id { get; set; }

    /// <summary>UserId</summary>
    public Guid UserId { get; set; }

    /// <summary>NotificationSound</summary>
    public bool NotificationSound { get; set; }

    /// <summary>CallSound</summary>
    public bool CallSound { get; set; }

    /// <summary>DesktopNotifications</summary>
    public bool DesktopNotifications { get; set; }

    /// <summary>ReadReceipts</summary>
    public bool ReadReceipts { get; set; }

    /// <summary>Theme</summary>
    public string Theme { get; set; } = string.Empty;

    /// <summary>Oluşturma zamanı.</summary>
    public DateTime CreatedAt { get; set; }
}
