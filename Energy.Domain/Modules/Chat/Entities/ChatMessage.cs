using Energy.Domain.Common;

namespace Energy.Domain.Modules.Chat;

/// <summary>
/// İki kullanıcı arasında gönderilen tek bir mesaj. Herhangi bir kullanıcı çifti
/// arasındaki sohbet, iki yönde de gönderen/alıcı oldukları mesajların sıralı
/// kümesidir. <see cref="AuditableEntity.CreatedAt"/> mesajın gönderim zamanını
/// (asıl referans zaman damgası) tutar.
/// </summary>
public class ChatMessage : AuditableEntity
{
    /// <summary>Mesajı gönderen kullanıcının kimliği.</summary>
    public Guid SenderId { get; set; }

    /// <summary>
    /// Birebir (1-1) mesajın hedef kullanıcısı. Mesaj bir gruba gönderildiğinde
    /// null olur (bkz. <see cref="GroupId"/>).
    /// </summary>
    public Guid? RecipientId { get; set; }

    /// <summary>Grup mesajının hedef grubu. Birebir mesajlarda null'dır.</summary>
    public Guid? GroupId { get; set; }

    /// <summary>Mesaj metni.</summary>
    public string Text { get; set; } = string.Empty;

    /// <summary>Mesajın okunup okunmadığı.</summary>
    public bool IsRead { get; set; }

    /// <summary>Mesajın okunduğu UTC zaman damgası (okunmadıysa null).</summary>
    public DateTime? ReadAt { get; set; }

    /// <summary>Bu mesajın yanıt verdiği (alıntıladığı) mesaj; varsa.</summary>
    public Guid? ReplyToMessageId { get; set; }

    /// <summary>Mesaj bir dosya taşıyorsa, ekli dosyanın özgün adı.</summary>
    public string? AttachmentFileName { get; set; }

    /// <summary>Ekli dosyanın MIME türü (ör. image/png, application/pdf).</summary>
    public string? AttachmentContentType { get; set; }

    /// <summary>Ekli dosyanın ham byte içeriği. Mesajla birlikte saklanır.</summary>
    public byte[]? AttachmentData { get; set; }
}
