namespace Energy.Shared.Models.V1.Chat.Requests;

/// <summary>Forwards an existing message (its text + attachment) to a new target.</summary>
public sealed class ForwardChatMessageRequest
{
    public Guid MessageId { get; set; }

    /// <summary>Target user for a direct forward. Null when forwarding to a group.</summary>
    public Guid? RecipientId { get; set; }

    /// <summary>Target group. Null when forwarding to a user.</summary>
    public Guid? GroupId { get; set; }
}

