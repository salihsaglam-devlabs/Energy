namespace Energy.Shared.Models.V1.Chat.Requests;

/// <summary>Payload sent by a user to deliver a message to another user or group.</summary>
public sealed class SendChatMessageRequest
{
    /// <summary>Target user for a direct message. Null/empty when sending to a group.</summary>
    public Guid? RecipientId { get; set; }

    /// <summary>Target group. Null when sending a direct message.</summary>
    public Guid? GroupId { get; set; }

    public string Text { get; set; } = string.Empty;

    /// <summary>Optional id of the message being replied to (quote).</summary>
    public Guid? ReplyToMessageId { get; set; }

    /// <summary>Optional attached file name (when sharing a file).</summary>
    public string? AttachmentFileName { get; set; }

    /// <summary>Optional attached file MIME type.</summary>
    public string? AttachmentContentType { get; set; }

    /// <summary>Optional attached file content, Base64 encoded.</summary>
    public string? AttachmentContentBase64 { get; set; }
}

