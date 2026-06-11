namespace Energy.Shared.Models.V1.Chat.Responses;

/// <summary>Binary payload of a chat message attachment, streamed to the client.</summary>
public sealed class ChatAttachmentResponse
{
    public byte[] Content { get; set; } = Array.Empty<byte>();
    public string ContentType { get; set; } = "application/octet-stream";
    public string FileName { get; set; } = "file";
}

