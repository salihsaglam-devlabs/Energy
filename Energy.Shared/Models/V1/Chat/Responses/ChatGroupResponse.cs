namespace Energy.Shared.Models.V1.Chat.Responses;

/// <summary>Geçerli kullanıcının üyesi olduğu (kabul edilmiş üye veya sahip) bir sohbet grubu.</summary>
public sealed class ChatGroupResponse
{
    /// <summary>Grubun kimliği.</summary>
    public Guid Id { get; set; }

    /// <summary>Grubun adı.</summary>
    public string Name { get; set; } = string.Empty;

    /// <summary>Grup sahibinin kullanıcı kimliği.</summary>
    public Guid OwnerId { get; set; }

    /// <summary>Geçerli kullanıcının grubun sahibi olup olmadığı.</summary>
    public bool IsOwner { get; set; }

    /// <summary>Geçerli kullanıcı grubu yönetebiliyorsa true (sahip veya yönetici).</summary>
    public bool IsAdmin { get; set; }

    /// <summary>Gruptaki üye sayısı.</summary>
    public int MemberCount { get; set; }

    /// <summary>Grupta okunmamış mesaj sayısı.</summary>
    public int UnreadCount { get; set; }

    /// <summary>Grupta son mesajlaşma zamanı.</summary>
    public DateTime? LastMessageAt { get; set; }
}
