namespace Energy.Shared.Models.V1.Chat.ChatGroupMember.Requests;

/// <summary>ChatGroupMember güncelleme isteği.</summary>
public class UpdateChatGroupMemberRequest
{
    /// <summary>Güncellenecek kaydın kimliği.</summary>
    public Guid Id { get; set; }

    /// <summary>ChatGroups referansı</summary>
    public Guid GroupId { get; set; }

    /// <summary>Users referansı</summary>
    public Guid UserId { get; set; }
}
