using Energy.Domain.Common;

namespace Energy.Domain.Modules.Chat;

/// <summary>
/// Sohbet mesajları
/// </summary>
public class ChatMessage : AuditableEntity
{
    /// <summary>Users referansı</summary>
    public Guid SenderId { get; set; }

    /// <summary>Users referansı</summary>
    public Guid? RecipientId { get; set; }

    /// <summary>ChatGroups referansı</summary>
    public Guid? GroupId { get; set; }

    /// <summary>ChatMessages referansı</summary>
    public Guid? ReplyToMessageId { get; set; }
}
