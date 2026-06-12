namespace Energy.Shared.Models.V1.Chat.Requests;

/// <summary>Creates a chat group and optionally invites the supplied users.</summary>
public sealed class CreateChatGroupRequest
{
    public string Name { get; set; } = string.Empty;

    /// <summary>Users to invite on creation (each receives a pending invitation).</summary>
    public IReadOnlyList<Guid> MemberUserIds { get; set; } = [];
}

