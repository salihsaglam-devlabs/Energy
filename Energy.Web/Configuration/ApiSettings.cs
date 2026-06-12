namespace Energy.Web.Configuration;

public sealed class ApiSettings
{
    public const string SectionName = "Api";

    public string BaseUrl { get; set; } = string.Empty;

    /// <summary>
    /// When true, the HTTP client accepts the API server's TLS certificate even
    /// if it is invalid (name mismatch / untrusted chain / self-signed).
    /// Intended ONLY for local/dev or temporary setups where the server cert
    /// cannot be trusted. Leave false in real production.
    /// </summary>
    public bool AllowInvalidCertificate { get; set; }
}
