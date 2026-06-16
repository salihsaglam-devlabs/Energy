using Energy.Domain.Common;

namespace Energy.Domain.Modules.Notifications;

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
