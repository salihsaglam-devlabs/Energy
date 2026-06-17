namespace Energy.Domain.Chat;

/// <summary>Bir kullanıcının bir sohbet grubundaki üyelik durumu.</summary>
public enum ChatGroupMemberStatus
{
    /// <summary>Davet edildi ancak henüz yanıt vermedi.</summary>
    Pending = 0,

    /// <summary>Daveti kabul etti ve gruba katılıyor.</summary>
    Accepted = 1,

    /// <summary>Daveti reddetti.</summary>
    Declined = 2
}
