namespace Energy.Shared.Models.V1.IAM.UserSetting.Responses;

/// <summary>UserSetting detay görünümü.</summary>
public class UserSettingDetailResponse
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
