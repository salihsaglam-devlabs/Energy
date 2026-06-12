namespace Energy.Shared.Models.V1.Chat.Responses;

/// <summary>A member row of a chat group (with invitation state).</summary>
public sealed class ChatGroupMemberResponse
{
    public Guid UserId { get; set; }
    public string FullName { get; set; } = string.Empty;
    public string UserName { get; set; } = string.Empty;
    public bool HasProfileImage { get; set; }
    public bool IsOwner { get; set; }

    /// <summary>0 = Pending, 1 = Accepted, 2 = Declined (mirrors the domain enum).</summary>
    public int Status { get; set; }
}

