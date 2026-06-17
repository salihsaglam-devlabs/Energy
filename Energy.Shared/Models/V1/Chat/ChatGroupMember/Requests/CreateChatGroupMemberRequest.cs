namespace Energy.Shared.Models.V1.Chat.ChatGroupMember.Requests;

/// <summary>ChatGroupMember oluşturma isteği.</summary>
public class CreateChatGroupMemberRequest
{
    /// <summary>ChatGroups referansı</summary>
    public Guid GroupId { get; set; }

    /// <summary>Users referansı</summary>
    public Guid UserId { get; set; }
}
