namespace Energy.Shared.Models.V1.Projects.Reports.ProjectStatusReport.Responses;

/// <summary>ProjectStatusReport raporu satırı (salt-okunur projeksiyon).</summary>
public sealed class ProjectStatusReportRowResponse
{
    /// <summary>Kaynak kayıt kimliği.</summary>
    public Guid Id { get; set; }

    /// <summary>Code</summary>
    public string? Code { get; set; }

    /// <summary>Name</summary>
    public string? Name { get; set; }

    /// <summary>ProjectTypeId</summary>
    public Guid ProjectTypeId { get; set; }

    /// <summary>StatusId</summary>
    public Guid StatusId { get; set; }

    /// <summary>StartDate</summary>
    public DateTime? StartDate { get; set; }

    /// <summary>EndDate</summary>
    public DateTime? EndDate { get; set; }
}
