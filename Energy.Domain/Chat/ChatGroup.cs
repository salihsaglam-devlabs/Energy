using Energy.Domain.Common;

namespace Energy.Domain.Chat;

/// <summary>
/// A named chat group. The owner creates it and invites users; an invited user
/// only becomes an active participant once they accept (see
/// <see cref="ChatGroupMember"/>).
/// </summary>
public class ChatGroup : AuditableEntity
{
    public string Name { get; set; } = string.Empty;

    /// <summary>User who created and owns the group.</summary>
    public Guid OwnerId { get; set; }
}

