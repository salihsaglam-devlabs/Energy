namespace Energy.Shared.Models.V1.Chat.Requests;

/// <summary>Toggles an emoji reaction by the current user on a message.</summary>
public sealed class ReactChatMessageRequest
{
    /// <summary>The reaction emoji. Sending the same emoji again removes it.</summary>
    public string Emoji { get; set; } = string.Empty;
}

