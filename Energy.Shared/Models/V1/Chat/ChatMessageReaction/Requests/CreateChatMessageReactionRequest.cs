namespace Energy.Shared.Models.V1.Chat.ChatMessageReaction.Requests;

/// <summary>ChatMessageReaction oluşturma isteği.</summary>
public class CreateChatMessageReactionRequest
{
    /// <summary>ChatMessages referansı</summary>
    public Guid MessageId { get; set; }

    /// <summary>Users referansı</summary>
    public Guid UserId { get; set; }
}
