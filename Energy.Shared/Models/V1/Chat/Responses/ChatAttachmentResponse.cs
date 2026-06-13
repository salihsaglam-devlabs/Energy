namespace Energy.Shared.Models.V1.Chat.Responses;

/// <summary>İstemciye akış olarak gönderilen sohbet mesajı ekinin ikili (binary) içeriği.</summary>
public sealed class ChatAttachmentResponse
{
    /// <summary>Ekin ham bayt içeriği.</summary>
    public byte[] Content { get; set; } = Array.Empty<byte>();

    /// <summary>Ekin MIME türü.</summary>
    public string ContentType { get; set; } = "application/octet-stream";

    /// <summary>Ekin dosya adı.</summary>
    public string FileName { get; set; } = "file";
}
