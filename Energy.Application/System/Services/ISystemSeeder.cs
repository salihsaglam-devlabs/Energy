namespace Energy.Application.System.Services;

/// <summary>
/// Veritabanını tamamen kullanılabilir bir duruma getiren idempotent sistem
/// seeder'larını çalıştırır (şema top-up'ları, yetki kataloğu, roller, kullanıcılar,
/// menüler, API endpoint kataloğu ve yerelleştirme). Tekrar tekrar çağrılması güvenlidir.
/// </summary>
public interface ISystemSeeder
{
    /// <summary>
    /// Tüm seed adımlarını sırayla çalıştırır. Mevcut veriler korunur; yalnızca
    /// eksik satırlar eklenir ve yakınsayan (convergent) değerler güncellenir.
    /// </summary>
    Task SeedAllAsync(CancellationToken cancellationToken = default);
}
