using Energy.Shared.Models.V1.Common.Responses;
using Energy.Shared.Models.V1.Reporting.ReportDefinition.Requests;
using Energy.Shared.Models.V1.Reporting.ReportDefinition.Responses;
using Energy.Web.Clients.Infrastructure;

namespace Energy.Web.Clients.Reporting.ReportDefinition;

/// <summary>ReportDefinition API istemci sözleşmesi.</summary>
public interface IReportDefinitionApiClient
{
    Task<BaseResponse<PaginatedResponse<ReportDefinitionListResponse>>> GetListAsync(int pageNumber, int pageSize, string? search, CancellationToken ct = default);
    Task<BaseResponse<ReportDefinitionDetailResponse>> GetByIdAsync(Guid id, CancellationToken ct = default);
    Task<BaseResponse<IReadOnlyList<ReportDefinitionLookupResponse>>> GetLookupAsync(string? search, bool activeOnly, CancellationToken ct = default);
    Task<BaseResponse<Guid>> CreateAsync(CreateReportDefinitionRequest request, CancellationToken ct = default);
    Task<BaseResponse<bool>> UpdateAsync(Guid id, UpdateReportDefinitionRequest request, CancellationToken ct = default);
    Task<BaseResponse<bool>> DeleteAsync(Guid id, CancellationToken ct = default);
}

/// <summary>ReportDefinition API istemcisi (HttpClientFactory + BaseResponse).</summary>
public sealed class ReportDefinitionApiClient : ApiClientBase, IReportDefinitionApiClient
{
    private const string Base = "api/v1/reporting/report-definitions";

    public ReportDefinitionApiClient(HttpClient httpClient) : base(httpClient) { }

    public Task<BaseResponse<PaginatedResponse<ReportDefinitionListResponse>>> GetListAsync(int pageNumber, int pageSize, string? search, CancellationToken ct = default)
        => GetAsync<BaseResponse<PaginatedResponse<ReportDefinitionListResponse>>>($"{Base}?pageNumber={pageNumber}&pageSize={pageSize}&search={Uri.EscapeDataString(search ?? string.Empty)}", ct);

    public Task<BaseResponse<ReportDefinitionDetailResponse>> GetByIdAsync(Guid id, CancellationToken ct = default)
        => GetAsync<BaseResponse<ReportDefinitionDetailResponse>>($"{Base}/{id}", ct);

    public Task<BaseResponse<IReadOnlyList<ReportDefinitionLookupResponse>>> GetLookupAsync(string? search, bool activeOnly, CancellationToken ct = default)
        => GetAsync<BaseResponse<IReadOnlyList<ReportDefinitionLookupResponse>>>($"{Base}/lookup?search={Uri.EscapeDataString(search ?? string.Empty)}&activeOnly={activeOnly}", ct);

    public Task<BaseResponse<Guid>> CreateAsync(CreateReportDefinitionRequest request, CancellationToken ct = default)
        => PostAsync<CreateReportDefinitionRequest, BaseResponse<Guid>>(Base, request, ct);

    public Task<BaseResponse<bool>> UpdateAsync(Guid id, UpdateReportDefinitionRequest request, CancellationToken ct = default)
        => PutAsync<UpdateReportDefinitionRequest, BaseResponse<bool>>($"{Base}/{id}", request, ct);

    public Task<BaseResponse<bool>> DeleteAsync(Guid id, CancellationToken ct = default)
        => DeleteAsync<BaseResponse<bool>>($"{Base}/{id}", ct);
}
