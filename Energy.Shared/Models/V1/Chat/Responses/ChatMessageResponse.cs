namespace Energy.Shared.Models.V1.Chat.Responses;

/// <summary>Kullanıcı arayüzü / SignalR taşıması için izdüşürülmüş tek bir sohbet mesajı.</summary>
public sealed class ChatMessageResponse
{
    /// <summary>Mesajın kimliği.</summary>
    public Guid Id { get; set; }

    /// <summary>Gönderenin kullanıcı kimliği.</summary>
    public Guid SenderId { get; set; }

    /// <summary>Gönderenin ad soyadı.</summary>
    public string SenderName { get; set; } = string.Empty;

    /// <summary>Gönderenin profil resmi olup olmadığı (sohbet avatarını besler).</summary>
    public bool SenderHasProfileImage { get; set; }

    /// <summary>Doğrudan mesaj için hedef kullanıcı; grup mesajlarında null.</summary>
    public Guid? RecipientId { get; set; }

    /// <summary>Grup mesajı için hedef grup; doğrudan mesajlarda null.</summary>
    public Guid? GroupId { get; set; }

    /// <summary>Mesaj metni.</summary>
    public string Text { get; set; } = string.Empty;

    /// <summary>Mesajın gönderildiği zaman.</summary>
    public DateTime SentAt { get; set; }

    /// <summary>Mesajın okunup okunmadığı.</summary>
    public bool IsRead { get; set; }

    /// <summary>Mesaj herkesten silindiyse true.</summary>
    public bool IsDeleted { get; set; }

    /// <summary>Ayarlandığında, bu mesajın yanıtladığı mesajın kimliği.</summary>
    public Guid? ReplyToId { get; set; }

    /// <summary>Yanıtlanan mesajın metni.</summary>
    public string? ReplyToText { get; set; }

    /// <summary>Yanıtlanan mesajın gönderen adı.</summary>
    public string? ReplyToSenderName { get; set; }

    /// <summary>Bu mesaja konulan emoji tepkisi özetleri.</summary>
    public IReadOnlyList<ChatReactionSummary> Reactions { get; set; } = [];

    /// <summary>Bu mesaj paylaşılan bir dosya taşıyorsa true.</summary>
    public bool HasAttachment { get; set; }

    /// <summary>Varsa, ekin orijinal dosya adı.</summary>
    public string? AttachmentFileName { get; set; }

    /// <summary>Varsa, ekin MIME türü.</summary>
    public string? AttachmentContentType { get; set; }
}
