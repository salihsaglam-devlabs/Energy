namespace Energy.Shared.Models.V1.Chat.ChatGroup.Responses;

/// <summary>ChatGroup liste satırı.</summary>
public class ChatGroupListResponse
{
    /// <summary>Kimlik.</summary>
    public Guid Id { get; set; }

    /// <summary>Users referansı</summary>
    public Guid OwnerId { get; set; }

    /// <summary>Name</summary>
    public string Name { get; set; } = string.Empty;

    /// <summary>Oluşturma zamanı.</summary>
    public DateTime CreatedAt { get; set; }
}
