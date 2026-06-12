using Energy.Domain.Common;

namespace Energy.Domain.Chat;

/// <summary>
/// Link row between a <see cref="ChatGroup"/> and a user. Carries the invitation
/// state so a user is only included in the group once they accept.
/// </summary>
public class ChatGroupMember : AuditableEntity
{
    public Guid GroupId { get; set; }
    public Guid UserId { get; set; }

    public ChatGroupMemberStatus Status { get; set; } = ChatGroupMemberStatus.Pending;

    /// <summary>True for the group owner (created the group).</summary>
    public bool IsOwner { get; set; }

    /// <summary>User who sent the invitation (null for the owner's own row).</summary>
    public Guid? InvitedById { get; set; }
}

