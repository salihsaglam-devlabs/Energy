using Energy.Domain.Common;

namespace Energy.Domain.Modules.Notifications;

/// <summary>
/// Bildirim tercihleri
/// </summary>
public class NotificationPreference : AuditableEntity
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
