using Energy.Domain.Common;

namespace Energy.Domain.Modules.IAM;

/// <summary>
/// API endpoint permission eşleştirmeleri
/// </summary>
public class ApiEndpoint : AuditableEntity
{
    /// <summary>Endpoint yolu</summary>
    public string Path { get; set; } = string.Empty;

    /// <summary>HTTP metodu</summary>
    public string HttpMethod { get; set; } = string.Empty;

    /// <summary>Gerekli permission</summary>
    public string? RequiredPermissionCode { get; set; }
}
