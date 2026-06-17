using Energy.Shared.Models.V1.Common.Responses;
using Energy.Shared.Models.V1.Operations.WorkOrderStatusHistory.Requests;
using Energy.Shared.Models.V1.Operations.WorkOrderStatusHistory.Responses;
using Energy.Web.Clients.Infrastructure;

namespace Energy.Web.Clients.Operations.WorkOrderStatusHistory;

/// <summary>WorkOrderStatusHistory API istemci sözleşmesi.</summary>
public interface IWorkOrderStatusHistoryApiClient
{
    Task<BaseResponse<PaginatedResponse<WorkOrderStatusHistoryListResponse>>> GetListAsync(int pageNumber, int pageSize, string? search, CancellationToken ct = default);
    Task<BaseResponse<WorkOrderStatusHistoryDetailResponse>> GetByIdAsync(Guid id, CancellationToken ct = default);
    Task<BaseResponse<IReadOnlyList<WorkOrderStatusHistoryLookupResponse>>> GetLookupAsync(string? search, bool activeOnly, CancellationToken ct = default);
    Task<BaseResponse<Guid>> CreateAsync(CreateWorkOrderStatusHistoryRequest request, CancellationToken ct = default);
    Task<BaseResponse<bool>> UpdateAsync(Guid id, UpdateWorkOrderStatusHistoryRequest request, CancellationToken ct = default);
    Task<BaseResponse<bool>> DeleteAsync(Guid id, CancellationToken ct = default);
}

/// <summary>WorkOrderStatusHistory API istemcisi (HttpClientFactory + BaseResponse).</summary>
public sealed class WorkOrderStatusHistoryApiClient : ApiClientBase, IWorkOrderStatusHistoryApiClient
{
    private const string Base = "api/v1/operations/work-order-status-histories";

    public WorkOrderStatusHistoryApiClient(HttpClient httpClient) : base(httpClient) { }

    public Task<BaseResponse<PaginatedResponse<WorkOrderStatusHistoryListResponse>>> GetListAsync(int pageNumber, int pageSize, string? search, CancellationToken ct = default)
        => GetAsync<BaseResponse<PaginatedResponse<WorkOrderStatusHistoryListResponse>>>($"{Base}?pageNumber={pageNumber}&pageSize={pageSize}&search={Uri.EscapeDataString(search ?? string.Empty)}", ct);

    public Task<BaseResponse<WorkOrderStatusHistoryDetailResponse>> GetByIdAsync(Guid id, CancellationToken ct = default)
        => GetAsync<BaseResponse<WorkOrderStatusHistoryDetailResponse>>($"{Base}/{id}", ct);

    public Task<BaseResponse<IReadOnlyList<WorkOrderStatusHistoryLookupResponse>>> GetLookupAsync(string? search, bool activeOnly, CancellationToken ct = default)
        => GetAsync<BaseResponse<IReadOnlyList<WorkOrderStatusHistoryLookupResponse>>>($"{Base}/lookup?search={Uri.EscapeDataString(search ?? string.Empty)}&activeOnly={activeOnly}", ct);

    public Task<BaseResponse<Guid>> CreateAsync(CreateWorkOrderStatusHistoryRequest request, CancellationToken ct = default)
        => PostAsync<CreateWorkOrderStatusHistoryRequest, BaseResponse<Guid>>(Base, request, ct);

    public Task<BaseResponse<bool>> UpdateAsync(Guid id, UpdateWorkOrderStatusHistoryRequest request, CancellationToken ct = default)
        => PutAsync<UpdateWorkOrderStatusHistoryRequest, BaseResponse<bool>>($"{Base}/{id}", request, ct);

    public Task<BaseResponse<bool>> DeleteAsync(Guid id, CancellationToken ct = default)
        => DeleteAsync<BaseResponse<bool>>($"{Base}/{id}", ct);
}
