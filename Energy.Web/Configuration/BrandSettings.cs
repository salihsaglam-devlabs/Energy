namespace Energy.Web.Configuration;

public sealed class BrandSettings
{
    public const string SectionName = "Brand";

    public string Name { get; set; } = string.Empty;

    public string? LogoUrl { get; set; }

    public string? LogoPath { get; set; }

    public string? LogoAlt { get; set; }
}

