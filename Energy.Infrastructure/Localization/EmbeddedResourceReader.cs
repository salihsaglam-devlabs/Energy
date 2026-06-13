using System.Collections;
using System.Globalization;
using System.Resources;
using Energy.Localization;

namespace Energy.Infrastructure.Localization;

/// <summary>
/// Yerelleştirme girdilerini, Energy.Localization derlemesine GÖMÜLÜ olarak gelen
/// <c>SharedResource</c> .resx dosyalarından (nötr/invariant küme) ve uydu
/// derlemelerinden (tr-TR / en-US gibi kültüre özgü kümeler) okur.
/// <para>
/// <see cref="ResxFileWriter"/>'in aksine bu okuyucu kaynak .resx dosyalarının
/// diskte bulunmasına ihtiyaç duymaz; bu yüzden yalnızca derlenmiş derlemelerin
/// bulunduğu üretim dağıtımlarında da çalışır.
/// </para>
/// </summary>
public sealed class EmbeddedResourceReader
{
    // Resources/SharedResource.resx için .csproj tarafından üretilen manifest
    // kaynak temel adı (bkz. LocalizationServiceExtensions).
    private const string BaseName = "Energy.Localization.Resources.SharedResource";

    /// <summary>
    /// Gömülü kaynaklarda tanımlı her (kültür, anahtar, değer) üçlüsünü döndürür.
    /// Invariant kültür boş bir kültür adıyla raporlanır; bu, ön ek almayan .resx
    /// adlandırmasıyla ve veritabanındaki <c>Resource.Culture</c> ile eşleşir.
    /// </summary>
    public IReadOnlyList<(string Culture, string Key, string Value)> ReadAll()
    {
        var results = new List<(string Culture, string Key, string Value)>();
        var manager = new ResourceManager(BaseName, typeof(SharedResource).Assembly);

        // Invariant (nötr) kültür -> veritabanında boş dize olarak saklanır.
        ReadCulture(manager, CultureInfo.InvariantCulture, string.Empty, results);

        // Açıkça desteklenen her kültür (tr-TR, en-US, ...).
        foreach (var culture in CultureConstants.SupportedCultures)
        {
            ReadCulture(manager, culture, culture.Name, results);
        }

        return results;
    }

    /// <summary>Tek bir kültüre ait kaynak kümesini okuyup sonuç listesine ekler.</summary>
    private static void ReadCulture(
        ResourceManager manager,
        CultureInfo culture,
        string cultureName,
        List<(string Culture, string Key, string Value)> results)
    {
        ResourceSet? set;
        try
        {
            // tryParents:false; böylece her küme YALNIZCA o kesin kültür için tanımlı
            // girdileri verir, .resx kaynaklarının dosya başına içeriğini yansıtır
            // (ve nötr anahtarların her kültüre kopyalanmasını önler).
            set = manager.GetResourceSet(culture, createIfNotExists: true, tryParents: false);
        }
        catch (MissingManifestResourceException)
        {
            // Bu kültür için uydu derlemesi yok – içe aktarılacak bir şey yok.
            return;
        }

        if (set is null)
        {
            return;
        }

        using (set)
        {
            foreach (DictionaryEntry entry in set)
            {
                if (entry.Key is string key && entry.Value is string value)
                {
                    results.Add((cultureName, key, value));
                }
            }
        }
    }
}

