namespace Energy.Shared.Models.V1.Settings.Responses;

/// <summary>Geçerli kullanıcının veritabanında saklanan tercihleri.</summary>
public sealed class UserSettingsResponse
{
    /// <summary>Yeni mesaj geldiğinde bildirim sesi çalsın mı.</summary>
    public bool NotificationSound { get; set; } = true;

    /// <summary>Gelen aramalarda sesli bildirim çalsın mı.</summary>
    public bool CallSound { get; set; } = true;

    /// <summary>Masaüstü bildirimleri gösterilsin mi.</summary>
    public bool DesktopNotifications { get; set; } = true;

    /// <summary>Okundu bilgisi (görüldü) gönderilsin mi.</summary>
    public bool ReadReceipts { get; set; } = true;

    /// <summary>Arayüz teması ("system", "light" veya "dark").</summary>
    public string Theme { get; set; } = "system";
}
