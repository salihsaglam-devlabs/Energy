using Energy.Domain.Common;

namespace Energy.Domain.Chat;

/// <summary>
/// An emoji reaction placed by a user on a chat message. A user can hold at most
/// one reaction per message (toggling/replacing the emoji).
/// </summary>
public class ChatMessageReaction : AuditableEntity
{
    public Guid MessageId { get; set; }
    public Guid UserId { get; set; }

    /// <summary>The reaction emoji (e.g. "👍", "❤️", "😂").</summary>
    public string Emoji { get; set; } = string.Empty;
}

