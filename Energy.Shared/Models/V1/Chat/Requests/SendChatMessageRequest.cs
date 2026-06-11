namespace Energy.Shared.Models.V1.Chat.Requests;

/// <summary>Payload sent by a user to deliver a message to another user.</summary>
public sealed class SendChatMessageRequest
{
    public Guid RecipientId { get; set; }
    public string Text { get; set; } = string.Empty;

    /// <summary>Optional attached file name (when sharing a file).</summary>
    public string? AttachmentFileName { get; set; }

    /// <summary>Optional attached file MIME type.</summary>
    public string? AttachmentContentType { get; set; }

    /// <summary>Optional attached file content, Base64 encoded.</summary>
    public string? AttachmentContentBase64 { get; set; }
}

