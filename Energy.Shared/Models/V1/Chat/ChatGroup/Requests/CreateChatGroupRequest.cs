namespace Energy.Shared.Models.V1.Chat.ChatGroup.Requests;

/// <summary>ChatGroup oluşturma isteği.</summary>
public class CreateChatGroupRequest
{
    /// <summary>Users referansı</summary>
    public Guid OwnerId { get; set; }

    /// <summary>Name</summary>
    public string Name { get; set; } = string.Empty;
}
