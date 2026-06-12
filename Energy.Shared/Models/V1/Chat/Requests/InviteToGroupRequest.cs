namespace Energy.Shared.Models.V1.Chat.Requests;

/// <summary>Invites one or more users to an existing group.</summary>
public sealed class InviteToGroupRequest
{
    public IReadOnlyList<Guid> UserIds { get; set; } = [];
}

