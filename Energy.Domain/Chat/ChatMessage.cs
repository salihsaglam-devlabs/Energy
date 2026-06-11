using Energy.Domain.Common;

namespace Energy.Domain.Chat;

/// <summary>
/// A single direct message exchanged between two users. The conversation
/// between any pair of users is the ordered set of messages where they are
/// sender/recipient in either direction. <see cref="AuditableEntity.CreatedAt"/>
/// is the authoritative send timestamp.
/// </summary>
public class ChatMessage : AuditableEntity
{
    public Guid SenderId { get; set; }
    public Guid RecipientId { get; set; }
    public string Text { get; set; } = string.Empty;
    public bool IsRead { get; set; }
    public DateTime? ReadAt { get; set; }

    /// <summary>Original file name of an attached file, when the message carries one.</summary>
    public string? AttachmentFileName { get; set; }

    /// <summary>MIME type of the attached file (e.g. image/png, application/pdf).</summary>
    public string? AttachmentContentType { get; set; }

    /// <summary>Raw bytes of the attached file. Stored alongside the message.</summary>
    public byte[]? AttachmentData { get; set; }
}

