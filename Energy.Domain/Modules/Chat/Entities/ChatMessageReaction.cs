using Energy.Domain.Common;

namespace Energy.Domain.Modules.Chat;

/// <summary>
/// Mesaj tepkileri
/// </summary>
public class ChatMessageReaction : AuditableEntity
{
    /// <summary>ChatMessages referansı</summary>
    public Guid MessageId { get; set; }

    /// <summary>Users referansı</summary>
    public Guid UserId { get; set; }
}
