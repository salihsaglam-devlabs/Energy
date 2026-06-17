namespace Energy.Shared.Models.V1.Chat.ChatMessage.Responses;

/// <summary>ChatMessage liste satırı.</summary>
public class ChatMessageListResponse
{
    /// <summary>Kimlik.</summary>
    public Guid Id { get; set; }

    /// <summary>Users referansı</summary>
    public Guid SenderId { get; set; }

    /// <summary>Users referansı</summary>
    public Guid? RecipientId { get; set; }

    /// <summary>ChatGroups referansı</summary>
    public Guid? GroupId { get; set; }

    /// <summary>ChatMessages referansı</summary>
    public Guid? ReplyToMessageId { get; set; }

    /// <summary>Oluşturma zamanı.</summary>
    public DateTime CreatedAt { get; set; }
}
