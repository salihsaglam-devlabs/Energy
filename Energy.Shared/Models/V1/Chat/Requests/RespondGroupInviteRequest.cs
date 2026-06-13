namespace Energy.Shared.Models.V1.Chat.Requests;

/// <summary>Beklemedeki bir grup davetini kabul eder veya reddeder.</summary>
public sealed class RespondGroupInviteRequest
{
    /// <summary>Daveti kabul etmek için true, reddetmek için false.</summary>
    public bool Accept { get; set; }
}
