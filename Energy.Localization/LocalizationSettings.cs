namespace Energy.Localization;

/// <summary>
/// Yerelleştirme hattı için çalışma zamanı ayarları.
/// <c>Localization</c> yapılandırma bölümünden bağlanır (bind edilir).
/// </summary>
public sealed class LocalizationSettings
{
    /// <summary>Bu ayarların okunduğu yapılandırma bölümünün adı.</summary>
    public const string SectionName = "Localization";

    /// <summary>
    /// .resx dosyalarının mantıksal adı (kültür ve uzantı olmadan). Hem dosyayı
    /// diskte bulmak hem de kaynak tipini tanımlamak için kullanılır.
    /// </summary>
    public string ResxBaseName { get; set; } = "SharedResource";

    /// <summary>
    /// Düzenlenebilir .resx dosyalarını içeren klasörün mutlak veya göreli yolu.
    /// null/boş olduğunda resx'e geri-yazma (write-through) devre dışı kalır ve
    /// yalnızca veritabanı güncellenir (kaynak dosyaların diskte bulunmadığı
    /// tipik üretim dağıtımlarında olduğu gibi).
    /// </summary>
    public string? ResxDirectory { get; set; }
}
