namespace Energy.Shared.Models.V1.Chat.Responses;

/// <summary>Geçerli kullanıcıya gönderilmiş, beklemedeki bir grup daveti.</summary>
public sealed class ChatGroupInviteResponse
{
    /// <summary>Davet edilen grubun kimliği.</summary>
    public Guid GroupId { get; set; }

    /// <summary>Grubun adı.</summary>
    public string GroupName { get; set; } = string.Empty;

    /// <summary>Grup sahibinin kullanıcı kimliği.</summary>
    public Guid OwnerId { get; set; }

    /// <summary>Daveti gönderen kişinin adı.</summary>
    public string InvitedByName { get; set; } = string.Empty;

    /// <summary>Davetin gönderildiği zaman.</summary>
    public DateTime InvitedAt { get; set; }
}
