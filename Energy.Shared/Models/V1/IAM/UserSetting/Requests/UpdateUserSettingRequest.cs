namespace Energy.Shared.Models.V1.IAM.UserSetting.Requests;

/// <summary>UserSetting güncelleme isteği.</summary>
public class UpdateUserSettingRequest
{
    /// <summary>Güncellenecek kaydın kimliği.</summary>
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
}
