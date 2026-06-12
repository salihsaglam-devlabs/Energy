namespace Energy.Shared.Models.V1.Chat.Responses;

/// <summary>Aggregated reaction info for one emoji on a message.</summary>
public sealed class ChatReactionSummary
{
    public string Emoji { get; set; } = string.Empty;
    public int Count { get; set; }

    /// <summary>True when the current user is among the reactors of this emoji.</summary>
    public bool Reacted { get; set; }
}

