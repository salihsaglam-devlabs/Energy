namespace Energy.Shared.Models.V1.Chat.Responses;

/// <summary>A pending group invitation addressed to the current user.</summary>
public sealed class ChatGroupInviteResponse
{
    public Guid GroupId { get; set; }
    public string GroupName { get; set; } = string.Empty;
    public Guid OwnerId { get; set; }
    public string InvitedByName { get; set; } = string.Empty;
    public DateTime InvitedAt { get; set; }
}

