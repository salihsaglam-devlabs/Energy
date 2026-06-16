namespace Energy.Shared.Models.V1.Chat.ChatMessageReaction.Requests;

/// <summary>ChatMessageReaction güncelleme isteği.</summary>
public class UpdateChatMessageReactionRequest
{
    /// <summary>Güncellenecek kaydın kimliği.</summary>
    public Guid Id { get; set; }

    /// <summary>ChatMessages referansı</summary>
    public Guid MessageId { get; set; }

    /// <summary>Users referansı</summary>
    public Guid UserId { get; set; }
}
