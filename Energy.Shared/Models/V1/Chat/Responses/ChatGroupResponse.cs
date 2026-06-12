namespace Energy.Shared.Models.V1.Chat.Responses;

/// <summary>A chat group the current user belongs to (accepted member or owner).</summary>
public sealed class ChatGroupResponse
{
    public Guid Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public Guid OwnerId { get; set; }
    public bool IsOwner { get; set; }
    public int MemberCount { get; set; }
    public int UnreadCount { get; set; }
    public DateTime? LastMessageAt { get; set; }
}

