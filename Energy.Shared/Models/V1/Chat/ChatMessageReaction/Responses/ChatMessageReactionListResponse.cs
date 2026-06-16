namespace Energy.Shared.Models.V1.Chat.ChatMessageReaction.Responses;

/// <summary>ChatMessageReaction liste satırı.</summary>
public class ChatMessageReactionListResponse
{
    /// <summary>Kimlik.</summary>
    public Guid Id { get; set; }

    /// <summary>ChatMessages referansı</summary>
    public Guid MessageId { get; set; }

    /// <summary>Users referansı</summary>
    public Guid UserId { get; set; }

    /// <summary>Oluşturma zamanı.</summary>
    public DateTime CreatedAt { get; set; }
}
