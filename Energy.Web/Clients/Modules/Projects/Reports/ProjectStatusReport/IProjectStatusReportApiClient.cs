using Energy.Shared.Models.V1.Common.Responses;
using Energy.Shared.Models.V1.Projects.Reports.ProjectStatusReport.Responses;
using Energy.Web.Clients.Infrastructure;

namespace Energy.Web.Clients.Modules.Projects.Reports.ProjectStatusReport;

/// <summary>ProjectStatusReport raporu API istemci sözleşmesi.</summary>
public interface IProjectStatusReportApiClient
{
    Task<BaseResponse<PaginatedResponse<ProjectStatusReportRowResponse>>> GetDataAsync(string query, CancellationToken ct = default);
}

/// <summary>ProjectStatusReport raporu API istemcisi (HttpClientFactory + BaseResponse).</summary>
public sealed class ProjectStatusReportApiClient : ApiClientBase, IProjectStatusReportApiClient
{
    private const string Base = "api/v1/projects/reports/project-status-report";

    public ProjectStatusReportApiClient(HttpClient httpClient) : base(httpClient) { }

    public Task<BaseResponse<PaginatedResponse<ProjectStatusReportRowResponse>>> GetDataAsync(string query, CancellationToken ct = default)
        => GetAsync<BaseResponse<PaginatedResponse<ProjectStatusReportRowResponse>>>(string.IsNullOrEmpty(query) ? Base : $"{Base}?{query}", ct);
}
