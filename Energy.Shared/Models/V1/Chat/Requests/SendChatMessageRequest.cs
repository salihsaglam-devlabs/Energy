namespace Energy.Shared.Models.V1.Chat.Requests;

/// <summary>Bir kullanıcının başka bir kullanıcıya veya gruba mesaj göndermek için ilettiği veri.</summary>
public sealed class SendChatMessageRequest
{
    /// <summary>Doğrudan mesaj için hedef kullanıcı. Gruba gönderilirken null/boş olur.</summary>
    public Guid? RecipientId { get; set; }

    /// <summary>Hedef grup. Doğrudan mesaj gönderilirken null olur.</summary>
    public Guid? GroupId { get; set; }

    /// <summary>Mesaj metni.</summary>
    public string Text { get; set; } = string.Empty;

    /// <summary>Yanıtlanan mesajın isteğe bağlı kimliği (alıntı).</summary>
    public Guid? ReplyToMessageId { get; set; }

    /// <summary>İsteğe bağlı ekli dosya adı (dosya paylaşılırken).</summary>
    public string? AttachmentFileName { get; set; }

    /// <summary>İsteğe bağlı ekli dosyanın MIME türü.</summary>
    public string? AttachmentContentType { get; set; }

    /// <summary>İsteğe bağlı ekli dosya içeriği, Base64 olarak kodlanmış.</summary>
    public string? AttachmentContentBase64 { get; set; }
}
