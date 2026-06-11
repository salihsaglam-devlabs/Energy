using Energy.Domain.Common;

namespace Energy.Domain.Localization;

/// <summary>
/// Persisted localization override for a (Key, Culture) pair.
/// At read time the value stored here takes precedence over the .resx fallback;
/// at write time both the database and the source .resx file are updated.
/// </summary>
public class Resource : BaseEntity
{
    public string Key { get; set; } = string.Empty;

    /// <summary>
    /// Culture name (e.g. "tr-TR", "en-US") or empty string for the invariant
    /// (neutral) culture, matching the unprefixed .resx file naming.
    /// </summary>
    public string Culture { get; set; } = string.Empty;

    public string Value { get; set; } = string.Empty;
}

