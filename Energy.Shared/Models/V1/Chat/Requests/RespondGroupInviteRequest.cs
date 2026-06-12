namespace Energy.Shared.Models.V1.Chat.Requests;

/// <summary>Accept or decline a pending group invitation.</summary>
public sealed class RespondGroupInviteRequest
{
    public bool Accept { get; set; }
}

