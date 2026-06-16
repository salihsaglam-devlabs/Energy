namespace Energy.Shared.Models.V1.Chat.ChatMessage.Requests;

/// <summary>ChatMessage güncelleme isteği.</summary>
public class UpdateChatMessageRequest
{
    /// <summary>Güncellenecek kaydın kimliği.</summary>
    public Guid Id { get; set; }

    /// <summary>Users referansı</summary>
    public Guid SenderId { get; set; }

    /// <summary>Users referansı</summary>
    public Guid? RecipientId { get; set; }

    /// <summary>ChatGroups referansı</summary>
    public Guid? GroupId { get; set; }

    /// <summary>ChatMessages referansı</summary>
    public Guid? ReplyToMessageId { get; set; }
}
