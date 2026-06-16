namespace Energy.Shared.Models.V1.Chat.ChatGroupMember.Responses;

/// <summary>ChatGroupMember liste satırı.</summary>
public class ChatGroupMemberListResponse
{
    /// <summary>Kimlik.</summary>
    public Guid Id { get; set; }

    /// <summary>ChatGroups referansı</summary>
    public Guid GroupId { get; set; }

    /// <summary>Users referansı</summary>
    public Guid UserId { get; set; }

    /// <summary>Oluşturma zamanı.</summary>
    public DateTime CreatedAt { get; set; }
}
