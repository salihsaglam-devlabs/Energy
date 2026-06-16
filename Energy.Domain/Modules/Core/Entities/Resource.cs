using Energy.Domain.Common;

namespace Energy.Domain.Modules.Core;

/// <summary>
/// (Anahtar, Kültür) çifti için veritabanında saklanan yerelleştirme (localization)
/// değeri. Okuma anında burada saklanan değer, .resx dosyasındaki yedeğe göre
/// önceliklidir; yazma anında hem veritabanı hem de kaynak .resx dosyası güncellenir.
/// </summary>
public class Resource : BaseEntity
{
    /// <summary>Yerelleştirme anahtarı (ör. "Menus.Chat").</summary>
    public string Key { get; set; } = string.Empty;

    /// <summary>
    /// Kültür adı (ör. "tr-TR", "en-US") veya değişmez (invariant/nötr) kültür için
    /// boş dize. Ön ek taşımayan .resx dosya adlandırmasıyla eşleşir.
    /// </summary>
    public string Culture { get; set; } = string.Empty;

    /// <summary>Bu anahtar/kültür için çevrilmiş metin değeri.</summary>
    public string Value { get; set; } = string.Empty;
}
