namespace Energy.Shared.Models.V1.Chat.ChatGroup.Requests;

/// <summary>ChatGroup güncelleme isteği.</summary>
public class UpdateChatGroupRequest
{
    /// <summary>Güncellenecek kaydın kimliği.</summary>
    public Guid Id { get; set; }

    /// <summary>Users referansı</summary>
    public Guid OwnerId { get; set; }

    /// <summary>Name</summary>
    public string Name { get; set; } = string.Empty;
}
