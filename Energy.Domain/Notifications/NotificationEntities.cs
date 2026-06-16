using Energy.Domain.Common;

namespace Energy.Domain.Notifications;

/// <summary>Bildirim başlığı.</summary>
public class Notification : AuditableEntity
{
    public string Title { get; set; } = string.Empty;
    public string? Body { get; set; }
    /// <summary>LowStock, BudgetOverrun, WorkOrderDelay, PendingApproval, UpcomingPayment, ProgressPaymentStatus, ChatMessage vb.</summary>
    public string NotificationType { get; set; } = string.Empty;
    public string? RelatedModule { get; set; }
    public string? RelatedEntityType { get; set; }
    public Guid? RelatedEntityId { get; set; }
}

/// <summary>Bildirim alıcısı.</summary>
public class NotificationRecipient : AuditableEntity
{
    public Guid NotificationId { get; set; }
    public Guid UserId { get; set; }
    public bool IsRead { get; set; }
    public DateTime? ReadAt { get; set; }
}

/// <summary>Kullanıcı bazlı bildirim tercihi.</summary>
public class NotificationPreference : AuditableEntity
{
    public Guid UserId { get; set; }
    public string NotificationType { get; set; } = string.Empty;
    public bool InAppEnabled { get; set; } = true;
    public bool EmailEnabled { get; set; }
}

