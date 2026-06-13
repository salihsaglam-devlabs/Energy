using Energy.Shared.Models.V1.Common.Responses;
using Energy.Shared.Models.V1.Localization.Requests;
using Energy.Shared.Models.V1.Localization.Responses;

namespace Energy.Application.Localization.Services;

/// <summary>Yerelleştirme (localization) kayıtlarını okuma, yazma ve içe aktarma servisi.</summary>
public interface ILocalizationService
{
    /// <summary>Tüm yerelleştirme kayıtlarını döndürür.</summary>
    Task<IReadOnlyList<LocalizationEntryResponse>> GetAllAsync(CancellationToken cancellationToken = default);

    /// <summary>Belirtilen anahtara ait yerelleştirme kaydını döndürür; yoksa null.</summary>
    Task<LocalizationEntryResponse?> GetByKeyAsync(string key, CancellationToken cancellationToken = default);

    /// <summary>
    /// Verilen değerleri veritabanına kaydeder ve (etkinse) ilgili .resx
    /// dosyalarına da yansıtır. Yazma sonrası birleştirilmiş durumu döndürür.
    /// </summary>
    Task<LocalizationEntryResponse> UpsertAsync(
        UpsertLocalizationEntryRequest request,
        CancellationToken cancellationToken = default);

    /// <summary>Belirtilen anahtara ait kaydı siler; başarılıysa true döner.</summary>
    Task<bool> DeleteAsync(string key, CancellationToken cancellationToken = default);

    /// <summary>
    /// Tek seferlik içe aktarma: disk üzerindeki .resx dosyalarından her
    /// (kültür, anahtar, değer) üçlüsünü okuyup veritabanına ekler/günceller.
    /// Eklenen ve güncellenen satır sayısını döndürür.
    /// </summary>
    Task<SeedResultResponse> ImportFromResxAsync(CancellationToken cancellationToken = default);

    /// <summary>
    /// Veritabanını, uygulama derlemelerine GÖMÜLÜ (embedded) yerelleştirme
    /// kaynaklarından doldurur (disk üzerindeki .resx dosyaları olmadan da çalışır,
    /// yani üretim ortamında). Mevcut (anahtar, kültür) satırları gömülü değerle
    /// ÜZERİNE YAZILIR; eksik satırlar eklenir. Eklenen/güncellenen sayıları döndürür.
    /// </summary>
    Task<SeedResultResponse> SeedFromResourcesAsync(CancellationToken cancellationToken = default);
}
