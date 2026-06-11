namespace Energy.Localization;

/// <summary>
/// Runtime settings for the localization pipeline.
/// Bound from the <c>Localization</c> configuration section.
/// </summary>
public sealed class LocalizationSettings
{
    public const string SectionName = "Localization";

    /// <summary>
    /// Logical name of the .resx files (without culture and extension).
    /// Used both to locate the file on disk and to identify the resource type.
    /// </summary>
    public string ResxBaseName { get; set; } = "SharedResource";

    /// <summary>
    /// Absolute or relative path to the folder that contains the editable
    /// .resx files. When null/empty, the resx write-through is disabled and
    /// only the database is updated (typical in production deployments where
    /// the source files are not available on disk).
    /// </summary>
    public string? ResxDirectory { get; set; }
}

