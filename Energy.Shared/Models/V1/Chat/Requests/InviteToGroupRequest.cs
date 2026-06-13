namespace Energy.Shared.Models.V1.Chat.Requests;

/// <summary>Var olan bir gruba bir veya daha fazla kullanıcı davet eder.</summary>
public sealed class InviteToGroupRequest
{
    /// <summary>Gruba davet edilecek kullanıcıların kimlikleri.</summary>
    public IReadOnlyList<Guid> UserIds { get; set; } = [];
}
