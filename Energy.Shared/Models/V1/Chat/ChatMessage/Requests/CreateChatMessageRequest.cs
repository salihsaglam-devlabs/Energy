namespace Energy.Shared.Models.V1.Chat.ChatMessage.Requests;

/// <summary>ChatMessage oluşturma isteği.</summary>
public class CreateChatMessageRequest
{
    /// <summary>Users referansı</summary>
    public Guid SenderId { get; set; }

    /// <summary>Users referansı</summary>
    public Guid? RecipientId { get; set; }

    /// <summary>ChatGroups referansı</summary>
    public Guid? GroupId { get; set; }

    /// <summary>ChatMessages referansı</summary>
    public Guid? ReplyToMessageId { get; set; }
}
