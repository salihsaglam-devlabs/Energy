using Energy.Domain.Common;

namespace Energy.Domain.Notifications;

/// <summary>Bildirim alıcısı.</summary>
public class NotificationRecipient : AuditableEntity
{
    public Guid NotificationId { get; set; }
    public Guid UserId { get; set; }
    public bool IsRead { get; set; }
    public DateTime? ReadAt { get; set; }
}
