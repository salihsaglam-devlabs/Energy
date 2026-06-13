namespace Energy.Shared.Models.V1.Chat.Responses;

/// <summary>
/// Geçerli kullanıcının sohbet edebileceği bir kullanıcı ile o kullanıcıdan gelen
/// canlı okunmamış mesaj sayısı (kişi başına rozeti ve global zil sayacını besler).
/// </summary>
public sealed class ChatContactResponse
{
    /// <summary>Kişinin kullanıcı kimliği.</summary>
    public Guid Id { get; set; }

    /// <summary>Kişinin ad soyadı.</summary>
    public string FullName { get; set; } = string.Empty;

    /// <summary>Kişinin kullanıcı adı.</summary>
    public string UserName { get; set; } = string.Empty;

    /// <summary>Kişinin profil resmi olup olmadığı.</summary>
    public bool HasProfileImage { get; set; }

    /// <summary>Bu kişiden gelen okunmamış mesaj sayısı.</summary>
    public int UnreadCount { get; set; }

    /// <summary>Bu kişiyle son mesajlaşma zamanı.</summary>
    public DateTime? LastMessageAt { get; set; }
}
