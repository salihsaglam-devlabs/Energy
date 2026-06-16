using Energy.Domain.Common;

namespace Energy.Domain.Modules.Chat;

/// <summary>
/// Bir kullanıcının bir sohbet mesajına bıraktığı emoji tepkisi (reaction).
/// Bir kullanıcı, bir mesaj için en fazla tek bir tepki tutabilir (emoji
/// değiştirilerek/aç-kapa yapılarak güncellenir).
/// </summary>
public class ChatMessageReaction : AuditableEntity
{
    /// <summary>Tepki verilen mesajın kimliği.</summary>
    public Guid MessageId { get; set; }

    /// <summary>Tepkiyi veren kullanıcının kimliği.</summary>
    public Guid UserId { get; set; }

    /// <summary>Tepki emojisi (ör. "👍", "❤️", "😂").</summary>
    public string Emoji { get; set; } = string.Empty;
}
