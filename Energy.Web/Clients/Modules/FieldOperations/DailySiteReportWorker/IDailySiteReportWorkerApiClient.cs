using Energy.Shared.Models.V1.Common.Responses;
using Energy.Shared.Models.V1.FieldOperations.DailySiteReportWorker.Requests;
using Energy.Shared.Models.V1.FieldOperations.DailySiteReportWorker.Responses;
using Energy.Web.Clients.Infrastructure;

namespace Energy.Web.Clients.Modules.FieldOperations.DailySiteReportWorker;

/// <summary>DailySiteReportWorker API istemci sözleşmesi.</summary>
public interface IDailySiteReportWorkerApiClient
{
    Task<BaseResponse<PaginatedResponse<DailySiteReportWorkerListResponse>>> GetListAsync(int pageNumber, int pageSize, string? search, CancellationToken ct = default);
    Task<BaseResponse<DailySiteReportWorkerDetailResponse>> GetByIdAsync(Guid id, CancellationToken ct = default);
    Task<BaseResponse<IReadOnlyList<DailySiteReportWorkerLookupResponse>>> GetLookupAsync(string? search, bool activeOnly, CancellationToken ct = default);
    Task<BaseResponse<Guid>> CreateAsync(CreateDailySiteReportWorkerRequest request, CancellationToken ct = default);
    Task<BaseResponse<bool>> UpdateAsync(Guid id, UpdateDailySiteReportWorkerRequest request, CancellationToken ct = default);
    Task<BaseResponse<bool>> DeleteAsync(Guid id, CancellationToken ct = default);
}

/// <summary>DailySiteReportWorker API istemcisi (HttpClientFactory + BaseResponse).</summary>
public sealed class DailySiteReportWorkerApiClient : ApiClientBase, IDailySiteReportWorkerApiClient
{
    private const string Base = "api/v1/field-operations/daily-site-report-workers";

    public DailySiteReportWorkerApiClient(HttpClient httpClient) : base(httpClient) { }

    public Task<BaseResponse<PaginatedResponse<DailySiteReportWorkerListResponse>>> GetListAsync(int pageNumber, int pageSize, string? search, CancellationToken ct = default)
        => GetAsync<BaseResponse<PaginatedResponse<DailySiteReportWorkerListResponse>>>($"{Base}?pageNumber={pageNumber}&pageSize={pageSize}&search={Uri.EscapeDataString(search ?? string.Empty)}", ct);

    public Task<BaseResponse<DailySiteReportWorkerDetailResponse>> GetByIdAsync(Guid id, CancellationToken ct = default)
        => GetAsync<BaseResponse<DailySiteReportWorkerDetailResponse>>($"{Base}/{id}", ct);

    public Task<BaseResponse<IReadOnlyList<DailySiteReportWorkerLookupResponse>>> GetLookupAsync(string? search, bool activeOnly, CancellationToken ct = default)
        => GetAsync<BaseResponse<IReadOnlyList<DailySiteReportWorkerLookupResponse>>>($"{Base}/lookup?search={Uri.EscapeDataString(search ?? string.Empty)}&activeOnly={activeOnly}", ct);

    public Task<BaseResponse<Guid>> CreateAsync(CreateDailySiteReportWorkerRequest request, CancellationToken ct = default)
        => PostAsync<CreateDailySiteReportWorkerRequest, BaseResponse<Guid>>(Base, request, ct);

    public Task<BaseResponse<bool>> UpdateAsync(Guid id, UpdateDailySiteReportWorkerRequest request, CancellationToken ct = default)
        => PutAsync<UpdateDailySiteReportWorkerRequest, BaseResponse<bool>>($"{Base}/{id}", request, ct);

    public Task<BaseResponse<bool>> DeleteAsync(Guid id, CancellationToken ct = default)
        => DeleteAsync<BaseResponse<bool>>($"{Base}/{id}", ct);
}
