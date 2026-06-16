using System.Xml.Linq;
using Xunit;

namespace Energy.Tests;

/// <summary>
/// Localization key kapsama testi (spec §13 + §22). Üç kültür resx dosyasının
/// (neutral, tr-TR, en-US) AYNI anahtar kümesine sahip olduğunu doğrular; böylece
/// bir kültürde eksik kalan çeviri anahtarı derleme/CI aşamasında görünür olur.
/// </summary>
public sealed class LocalizationCoverageTests
{
    private static string ResourcesDir()
    {
        var dir = AppContext.BaseDirectory;
        for (var i = 0; i < 12 && dir is not null; i++)
        {
            var candidate = Path.Combine(dir, "Energy.Localization", "Resources");
            if (Directory.Exists(candidate))
            {
                return candidate;
            }
            dir = Directory.GetParent(dir)?.FullName;
        }
        throw new DirectoryNotFoundException("Energy.Localization/Resources not found from test base directory.");
    }

    private static HashSet<string> KeysOf(string path)
    {
        var doc = XDocument.Load(path);
        return doc.Root!
            .Elements("data")
            .Select(d => (string?)d.Attribute("name"))
            .Where(n => !string.IsNullOrEmpty(n))
            .Select(n => n!)
            .ToHashSet(StringComparer.Ordinal);
    }

    [Fact]
    public void All_Cultures_Cover_The_Same_Keys()
    {
        var dir = ResourcesDir();
        var neutral = KeysOf(Path.Combine(dir, "SharedResource.resx"));
        var tr = KeysOf(Path.Combine(dir, "SharedResource.tr-TR.resx"));
        var en = KeysOf(Path.Combine(dir, "SharedResource.en-US.resx"));

        var missingTr = neutral.Except(tr).ToList();
        var missingEn = neutral.Except(en).ToList();

        Assert.True(missingTr.Count == 0, "Keys missing from tr-TR: " + string.Join(", ", missingTr.Take(20)));
        Assert.True(missingEn.Count == 0, "Keys missing from en-US: " + string.Join(", ", missingEn.Take(20)));
    }

    [Fact]
    public void Neutral_Resource_Is_Not_Empty()
    {
        var dir = ResourcesDir();
        Assert.NotEmpty(KeysOf(Path.Combine(dir, "SharedResource.resx")));
    }
}

