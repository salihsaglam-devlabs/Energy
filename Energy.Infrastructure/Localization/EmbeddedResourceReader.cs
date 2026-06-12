using System.Collections;
using System.Globalization;
using System.Resources;
using Energy.Localization;

namespace Energy.Infrastructure.Localization;

/// <summary>
/// Reads localization entries from the <c>SharedResource</c> .resx files that
/// ship EMBEDDED in the Energy.Localization assembly (the neutral/invariant set)
/// and its satellite assemblies (culture-specific sets such as tr-TR / en-US).
/// <para>
/// Unlike <see cref="ResxFileWriter"/> this reader does not need the source
/// .resx files to exist on disk, so it works in production deployments where
/// only the compiled assemblies are present.
/// </para>
/// </summary>
public sealed class EmbeddedResourceReader
{
    // Manifest resource base name produced by the .csproj for
    // Resources/SharedResource.resx (see LocalizationServiceExtensions).
    private const string BaseName = "Energy.Localization.Resources.SharedResource";

    /// <summary>
    /// Returns every (culture, key, value) tuple defined in the embedded
    /// resources. The invariant culture is reported with an empty culture name,
    /// matching the unprefixed .resx naming and the DB <c>Resource.Culture</c>.
    /// </summary>
    public IReadOnlyList<(string Culture, string Key, string Value)> ReadAll()
    {
        var results = new List<(string Culture, string Key, string Value)>();
        var manager = new ResourceManager(BaseName, typeof(SharedResource).Assembly);

        // Invariant (neutral) culture -> stored as empty string in the database.
        ReadCulture(manager, CultureInfo.InvariantCulture, string.Empty, results);

        // Each explicitly supported culture (tr-TR, en-US, ...).
        foreach (var culture in CultureConstants.SupportedCultures)
        {
            ReadCulture(manager, culture, culture.Name, results);
        }

        return results;
    }

    private static void ReadCulture(
        ResourceManager manager,
        CultureInfo culture,
        string cultureName,
        List<(string Culture, string Key, string Value)> results)
    {
        ResourceSet? set;
        try
        {
            // tryParents:false so each set yields ONLY the entries defined for
            // that exact culture, mirroring the per-file content of the .resx
            // sources (and avoiding duplicating neutral keys onto every culture).
            set = manager.GetResourceSet(culture, createIfNotExists: true, tryParents: false);
        }
        catch (MissingManifestResourceException)
        {
            // No satellite assembly for this culture – nothing to import.
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

