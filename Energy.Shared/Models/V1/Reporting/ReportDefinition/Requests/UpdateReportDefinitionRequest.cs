namespace Energy.Shared.Models.V1.Reporting.ReportDefinition.Requests;

/// <summary>ReportDefinition güncelleme isteği.</summary>
public class UpdateReportDefinitionRequest
{
    /// <summary>Güncellenecek kaydın kimliği.</summary>
    public Guid Id { get; set; }

    /// <summary>Code</summary>
    public string Code { get; set; } = string.Empty;

    /// <summary>Name</summary>
    public string Name { get; set; } = string.Empty;

    /// <summary>Module</summary>
    public string Module { get; set; } = string.Empty;

    /// <summary>QueryKey</summary>
    public string QueryKey { get; set; } = string.Empty;

    /// <summary>RequiredPermissionCode</summary>
    public string? RequiredPermissionCode { get; set; }

    /// <summary>IsActive</summary>
    public bool IsActive { get; set; }
}
