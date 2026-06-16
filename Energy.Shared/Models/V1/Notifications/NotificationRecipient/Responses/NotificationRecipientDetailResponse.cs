namespace Energy.Shared.Models.V1.Notifications.NotificationRecipient.Responses;

/// <summary>NotificationRecipient detay görünümü.</summary>
public class NotificationRecipientDetailResponse
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

    /// <summary>Notifications referansı</summary>
    public Guid NotificationId { get; set; }

    /// <summary>Users referansı</summary>
    public Guid UserId { get; set; }

    /// <summary>IsRead</summary>
    public bool IsRead { get; set; }

    /// <summary>ReadAt</summary>
    public DateTime? ReadAt { get; set; }
}
