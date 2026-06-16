using Energy.Domain.Common;

namespace Energy.Domain.Modules.Notifications;

/// <summary>
/// Bildirim alıcıları
/// </summary>
public class NotificationRecipient : AuditableEntity
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
