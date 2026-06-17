using Energy.Shared.Models.V1.Common.Responses;
using Energy.Shared.Models.V1.FieldOperations.DailySiteReport.Requests;
using Energy.Shared.Models.V1.FieldOperations.DailySiteReport.Responses;
using Energy.Web.Clients.Infrastructure;

namespace Energy.Web.Clients.FieldOperations.DailySiteReport;

/// <summary>DailySiteReport API istemci sözleşmesi.</summary>
public interface IDailySiteReportApiClient
{
    Task<BaseResponse<PaginatedResponse<DailySiteReportListResponse>>> GetListAsync(int pageNumber, int pageSize, string? search, CancellationToken ct = default);
    Task<BaseResponse<DailySiteReportDetailResponse>> GetByIdAsync(Guid id, CancellationToken ct = default);
    Task<BaseResponse<IReadOnlyList<DailySiteReportLookupResponse>>> GetLookupAsync(string? search, bool activeOnly, CancellationToken ct = default);
    Task<BaseResponse<Guid>> CreateAsync(CreateDailySiteReportRequest request, CancellationToken ct = default);
    Task<BaseResponse<bool>> UpdateAsync(Guid id, UpdateDailySiteReportRequest request, CancellationToken ct = default);
    Task<BaseResponse<bool>> DeleteAsync(Guid id, CancellationToken ct = default);
}

/// <summary>DailySiteReport API istemcisi (HttpClientFactory + BaseResponse).</summary>
public sealed class DailySiteReportApiClient : ApiClientBase, IDailySiteReportApiClient
{
    private const string Base = "api/v1/field-operations/daily-site-reports";

    public DailySiteReportApiClient(HttpClient httpClient) : base(httpClient) { }

    public Task<BaseResponse<PaginatedResponse<DailySiteReportListResponse>>> GetListAsync(int pageNumber, int pageSize, string? search, CancellationToken ct = default)
        => GetAsync<BaseResponse<PaginatedResponse<DailySiteReportListResponse>>>($"{Base}?pageNumber={pageNumber}&pageSize={pageSize}&search={Uri.EscapeDataString(search ?? string.Empty)}", ct);

    public Task<BaseResponse<DailySiteReportDetailResponse>> GetByIdAsync(Guid id, CancellationToken ct = default)
        => GetAsync<BaseResponse<DailySiteReportDetailResponse>>($"{Base}/{id}", ct);

    public Task<BaseResponse<IReadOnlyList<DailySiteReportLookupResponse>>> GetLookupAsync(string? search, bool activeOnly, CancellationToken ct = default)
        => GetAsync<BaseResponse<IReadOnlyList<DailySiteReportLookupResponse>>>($"{Base}/lookup?search={Uri.EscapeDataString(search ?? string.Empty)}&activeOnly={activeOnly}", ct);

    public Task<BaseResponse<Guid>> CreateAsync(CreateDailySiteReportRequest request, CancellationToken ct = default)
        => PostAsync<CreateDailySiteReportRequest, BaseResponse<Guid>>(Base, request, ct);

    public Task<BaseResponse<bool>> UpdateAsync(Guid id, UpdateDailySiteReportRequest request, CancellationToken ct = default)
        => PutAsync<UpdateDailySiteReportRequest, BaseResponse<bool>>($"{Base}/{id}", request, ct);

    public Task<BaseResponse<bool>> DeleteAsync(Guid id, CancellationToken ct = default)
        => DeleteAsync<BaseResponse<bool>>($"{Base}/{id}", ct);
}
