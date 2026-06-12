namespace Energy.Domain.Chat;

/// <summary>Membership state of a user within a chat group.</summary>
public enum ChatGroupMemberStatus
{
    /// <summary>Invited but has not yet responded.</summary>
    Pending = 0,

    /// <summary>Accepted the invitation and participates in the group.</summary>
    Accepted = 1,

    /// <summary>Declined the invitation.</summary>
    Declined = 2
}

