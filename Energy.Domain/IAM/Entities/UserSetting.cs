namespace Energy.Domain.IAM;

/// <summary>
/// Kullanıcı bazlı tercihler (her kullanıcı için tek satır, <see cref="UserId"/>
/// ile anahtarlanır). Oturumlar ve cihazlar arasında korunması gereken
/// self-service sohbet/bildirim ayarlarını tutar. İlk okunduğunda varsayılan
/// değerlerle tembel (lazy) olarak oluşturulur.
/// </summary>
public class UserSetting
{
    /// <summary>Sahibi olan kullanıcı. Birincil anahtar ve <c>Users</c> tablosuna yabancı anahtar.</summary>
    public Guid UserId { get; set; }

    /// <summary>Yeni bir sohbet mesajı geldiğinde bildirim sesi çal.</summary>
    public bool NotificationSound { get; set; } = true;

    /// <summary>Gelen sesli aramada zil sesi çal.</summary>
    public bool CallSound { get; set; } = true;

    /// <summary>Yeni mesajlar için masaüstü/merkez bildirim baloncuğu göster.</summary>
    public bool DesktopNotifications { get; set; } = true;

    /// <summary>Sohbet ettiğimiz kişilere okundu bilgisi (mavi tik) gönder.</summary>
    public bool ReadReceipts { get; set; } = true;

    /// <summary>Arayüz tema tercihi: <c>system</c> | <c>light</c> | <c>dark</c>.</summary>
    public string Theme { get; set; } = "system";

    /// <summary>Ayarların en son güncellendiği UTC zaman damgası.</summary>
    public DateTime? UpdatedAt { get; set; }
}
