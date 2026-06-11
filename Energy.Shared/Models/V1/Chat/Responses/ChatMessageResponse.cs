namespace Energy.Shared.Models.V1.Chat.Responses;

/// <summary>A single chat message projected for the UI / SignalR transport.</summary>
public sealed class ChatMessageResponse
{
    public Guid Id { get; set; }
    public Guid SenderId { get; set; }
    public string SenderName { get; set; } = string.Empty;

    /// <summary>Whether the sender has a profile image (drives the chat avatar).</summary>
    public bool SenderHasProfileImage { get; set; }

    public Guid RecipientId { get; set; }
    public string Text { get; set; } = string.Empty;
    public DateTime SentAt { get; set; }
    public bool IsRead { get; set; }

    /// <summary>True when this message carries a shared file.</summary>
    public bool HasAttachment { get; set; }

    /// <summary>Original file name of the attachment, when present.</summary>
    public string? AttachmentFileName { get; set; }

    /// <summary>MIME type of the attachment, when present.</summary>
    public string? AttachmentContentType { get; set; }
}

