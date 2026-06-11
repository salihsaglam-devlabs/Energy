namespace Energy.Shared.Models.V1.Chat.Responses;

/// <summary>
/// A user the current user can chat with, plus the live unread-message count
/// from that user (drives the per-contact badge and the global bell counter).
/// </summary>
public sealed class ChatContactResponse
{
    public Guid Id { get; set; }
    public string FullName { get; set; } = string.Empty;
    public string UserName { get; set; } = string.Empty;
    public bool HasProfileImage { get; set; }
    public int UnreadCount { get; set; }
    public DateTime? LastMessageAt { get; set; }
}

