namespace Energy.Application.Common.Storage;

/// <summary>
/// Dosya saklama soyutlaması. Fiziksel saklama altyapısını (yerel disk, bulut vb.)
/// gizler; servisler dosya işlemlerini yalnızca bu sözleşme üzerinden yürütür.
/// Controller'lar doğrudan dosya sistemine erişmez.
/// </summary>
public interface IFileStorage
{
    /// <summary>İçeriği saklar ve daha sonra erişim için göreli (relative) yolu döndürür.</summary>
    Task<string> SaveAsync(Stream content, string fileName, CancellationToken ct = default);

    /// <summary>Göreli yola karşılık gelen içeriği açar; yoksa null döndürür.</summary>
    Task<Stream?> OpenAsync(string relativePath, CancellationToken ct = default);

    /// <summary>Göreli yola karşılık gelen dosyayı siler (varsa).</summary>
    Task DeleteAsync(string relativePath, CancellationToken ct = default);
}

