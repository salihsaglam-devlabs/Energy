using System.Xml.Linq;
using Energy.Localization;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace Energy.Infrastructure.Localization;

/// <summary>
/// Yerelleştirme yedeği olarak kullanılan disk üzerindeki .resx dosyalarını okur
/// ve yazar. <see cref="LocalizationSettings.ResxDirectory"/> yapılandırılmadığında
/// yazıcı işlemsiz (no-op) hâle gelir ve veritabanı tek doğruluk kaynağı olarak kalır.
/// </summary>
public sealed class ResxFileWriter
{
    private static readonly XNamespace XmlNs = "http://www.w3.org/XML/1998/namespace";

    private readonly LocalizationSettings _settings;
    private readonly ILogger<ResxFileWriter> _logger;
    private readonly object _ioLock = new();
    private readonly string? _resolvedDirectory;

    /// <summary>Ayarları ve günlükleyiciyi enjekte eder, hedef dizini çözümler.</summary>
    public ResxFileWriter(IOptions<LocalizationSettings> settings, ILogger<ResxFileWriter> logger)
    {
        _settings = settings.Value;
        _logger = logger;
        _resolvedDirectory = ResolveDirectory(_settings.ResxDirectory);
    }

    /// <summary>Yazıcının etkin olup olmadığı (dizin yapılandırılmış ve mevcutsa).</summary>
    public bool IsEnabled
        => !string.IsNullOrWhiteSpace(_resolvedDirectory)
           && Directory.Exists(_resolvedDirectory);

    /// <summary>Yapılandırılan yolu mutlak bir dizin yoluna çözümler.</summary>
    private static string? ResolveDirectory(string? configured)
    {
        if (string.IsNullOrWhiteSpace(configured))
        {
            return null;
        }

        // Mutlak yollar olduğu gibi kullanılır; göreli yollar uygulama temel
        // dizinine göre çözümlenir; böylece yazıcı başlatma anındaki geçerli
        // çalışma dizininden bağımsız olarak çalışmaya devam eder.
        return Path.IsPathRooted(configured)
            ? Path.GetFullPath(configured)
            : Path.GetFullPath(Path.Combine(AppContext.BaseDirectory, configured));
    }

    /// <summary>
    /// <see cref="LocalizationSettings.ResxDirectory"/> içindeki herhangi bir
    /// SharedResource resx dosyasında saklanan her (kültür, anahtar, değer) üçlüsünü döndürür.
    /// </summary>
    public IReadOnlyList<(string Culture, string Key, string Value)> ReadAll()
    {
        if (!IsEnabled)
        {
            return Array.Empty<(string, string, string)>();
        }

        var results = new List<(string Culture, string Key, string Value)>();

        foreach (var path in EnumerateFiles())
        {
            var culture = ExtractCulture(Path.GetFileName(path));
            var doc = XDocument.Load(path);
            foreach (var data in doc.Root!.Elements("data"))
            {
                var key = (string?)data.Attribute("name");
                var value = data.Element("value")?.Value;
                if (!string.IsNullOrEmpty(key) && value is not null)
                {
                    results.Add((culture, key, value));
                }
            }
        }

        return results;
    }

    /// <summary>Verilen kültür/anahtar için resx girdisini ekler veya günceller (upsert).</summary>
    public void Upsert(string culture, string key, string value)
    {
        if (!IsEnabled)
        {
            return;
        }

        var path = ResolveFilePath(culture);

        lock (_ioLock)
        {
            try
            {
                EnsureResxFile(path);
                var doc = XDocument.Load(path, LoadOptions.PreserveWhitespace);
                var root = doc.Root!;

                var existing = FindData(root, key);
                if (existing is null)
                {
                    root.Add(BuildDataElement(key, value));
                }
                else
                {
                    var valueElement = existing.Element("value") ?? new XElement("value");
                    valueElement.Value = value;
                    if (valueElement.Parent is null)
                    {
                        existing.Add(valueElement);
                    }
                }

                doc.Save(path);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed to upsert resx entry '{Key}' for culture '{Culture}' in '{Path}'.", key, culture, path);
            }
        }
    }

    /// <summary>Verilen anahtarı tüm kültür resx dosyalarından siler.</summary>
    public void Delete(string key)
    {
        if (!IsEnabled)
        {
            return;
        }

        lock (_ioLock)
        {
            foreach (var path in EnumerateFiles())
            {
                try
                {
                    var doc = XDocument.Load(path, LoadOptions.PreserveWhitespace);
                    var data = FindData(doc.Root!, key);
                    if (data is null)
                    {
                        continue;
                    }

                    data.Remove();
                    doc.Save(path);
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "Failed to delete resx entry '{Key}' from '{Path}'.", key, path);
                }
            }
        }
    }

    /// <summary>Verilen kültür için resx dosya yolunu oluşturur.</summary>
    private string ResolveFilePath(string culture)
    {
        var fileName = string.IsNullOrEmpty(culture)
            ? $"{_settings.ResxBaseName}.resx"
            : $"{_settings.ResxBaseName}.{culture}.resx";

        return Path.Combine(_resolvedDirectory!, fileName);
    }

    /// <summary>Hedef dizindeki tüm SharedResource resx dosyalarını listeler.</summary>
    private IEnumerable<string> EnumerateFiles()
        => Directory.EnumerateFiles(_resolvedDirectory!, $"{_settings.ResxBaseName}*.resx");

    /// <summary>Kök öğe içinde verilen anahtara sahip <c>data</c> elemanını bulur.</summary>
    private static XElement? FindData(XElement root, string key)
        => root.Elements("data").FirstOrDefault(e => (string?)e.Attribute("name") == key);

    /// <summary>Verilen anahtar/değer için yeni bir <c>data</c> elemanı oluşturur.</summary>
    private static XElement BuildDataElement(string key, string value)
        => new("data",
            new XAttribute("name", key),
            new XAttribute(XmlNs + "space", "preserve"),
            new XElement("value", value));

    /// <summary>Dosya yoksa geçerli bir boş .resx iskeleti oluşturur.</summary>
    private static void EnsureResxFile(string path)
    {
        if (File.Exists(path))
        {
            return;
        }

        var document = new XDocument(
            new XDeclaration("1.0", "utf-8", null),
            new XElement("root",
                new XElement("resheader", new XAttribute("name", "resmimetype"), new XElement("value", "text/microsoft-resx")),
                new XElement("resheader", new XAttribute("name", "version"), new XElement("value", "1.3")),
                new XElement("resheader", new XAttribute("name", "reader"),
                    new XElement("value", "System.Resources.ResXResourceReader, System.Windows.Forms, Version=2.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089")),
                new XElement("resheader", new XAttribute("name", "writer"),
                    new XElement("value", "System.Resources.ResXResourceWriter, System.Windows.Forms, Version=2.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089"))));

        Directory.CreateDirectory(Path.GetDirectoryName(path)!);
        document.Save(path);
    }

    /// <summary>Dosya adından kültür kodunu ayıklar (ör. "SharedResource.tr-TR.resx" → "tr-TR").</summary>
    private string ExtractCulture(string fileName)
    {
        // "SharedResource.tr-TR.resx" → "tr-TR"
        // "SharedResource.resx"        → ""        (sabit/invariant)
        var withoutExtension = Path.GetFileNameWithoutExtension(fileName);
        if (!withoutExtension.StartsWith(_settings.ResxBaseName + ".", StringComparison.Ordinal))
        {
            return string.Empty;
        }

        return withoutExtension[(_settings.ResxBaseName.Length + 1)..];
    }
}

