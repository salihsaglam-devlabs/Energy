using Energy.Domain.Common;

namespace Energy.Domain.Notifications;

/// <summary>Kullanıcı bazlı bildirim tercihi.</summary>
public class NotificationPreference : AuditableEntity
{
    public Guid UserId { get; set; }
    public string NotificationType { get; set; } = string.Empty;
    public bool InAppEnabled { get; set; } = true;
    public bool EmailEnabled { get; set; }
}
