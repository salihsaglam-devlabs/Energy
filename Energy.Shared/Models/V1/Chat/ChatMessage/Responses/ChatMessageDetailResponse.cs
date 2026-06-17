namespace Energy.Shared.Models.V1.Chat.ChatMessage.Responses;

/// <summary>ChatMessage detay görünümü.</summary>
public class ChatMessageDetailResponse
{
    /// <summary>Kimlik.</summary>
    public Guid Id { get; set; }

    /// <summary>Oluşturma zamanı</summary>
    public DateTime CreatedAt { get; set; }

    /// <summary>Oluşturan kullanıcı</summary>
    public Guid? CreatedBy { get; set; }

    /// <summary>Son güncelleme zamanı</summary>
    public DateTime? UpdatedAt { get; set; }

    /// <summary>Güncelleyen kullanıcı</summary>
    public Guid? UpdatedBy { get; set; }

    /// <summary>Soft delete bayrağı</summary>
    public bool IsDeleted { get; set; }

    /// <summary>Silinme zamanı</summary>
    public DateTime? DeletedAt { get; set; }

    /// <summary>Silen kullanıcı</summary>
    public Guid? DeletedBy { get; set; }

    /// <summary>Users referansı</summary>
    public Guid SenderId { get; set; }

    /// <summary>Users referansı</summary>
    public Guid? RecipientId { get; set; }

    /// <summary>ChatGroups referansı</summary>
    public Guid? GroupId { get; set; }

    /// <summary>ChatMessages referansı</summary>
    public Guid? ReplyToMessageId { get; set; }
}
