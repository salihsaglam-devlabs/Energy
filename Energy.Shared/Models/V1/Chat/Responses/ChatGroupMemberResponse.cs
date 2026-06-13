namespace Energy.Shared.Models.V1.Chat.Responses;

/// <summary>Bir sohbet grubunun üye satırı (davet durumu ile birlikte).</summary>
public sealed class ChatGroupMemberResponse
{
    /// <summary>Üyenin kullanıcı kimliği.</summary>
    public Guid UserId { get; set; }

    /// <summary>Üyenin ad soyadı.</summary>
    public string FullName { get; set; } = string.Empty;

    /// <summary>Üyenin kullanıcı adı.</summary>
    public string UserName { get; set; } = string.Empty;

    /// <summary>Üyenin profil resmi olup olmadığı.</summary>
    public bool HasProfileImage { get; set; }

    /// <summary>Bu üyenin grubun sahibi olup olmadığı.</summary>
    public bool IsOwner { get; set; }

    /// <summary>Bu üye grup yöneticisiyse true (sahip her zaman yöneticidir).</summary>
    public bool IsAdmin { get; set; }

    /// <summary>0 = Beklemede, 1 = Kabul edildi, 2 = Reddedildi (domain enum'unu yansıtır).</summary>
    public int Status { get; set; }
}
