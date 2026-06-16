using Energy.Shared.Models.V1.Common.Responses;
using Energy.Shared.Models.V1.Operations.WorkOrder.Requests;
using Energy.Shared.Models.V1.Operations.WorkOrder.Responses;
using Energy.Web.Clients.Infrastructure;

namespace Energy.Web.Clients.Operations.WorkOrder;

/// <summary>WorkOrder API istemci sözleşmesi.</summary>
public interface IWorkOrderApiClient
{
    Task<BaseResponse<PaginatedResponse<WorkOrderListResponse>>> GetListAsync(int pageNumber, int pageSize, string? search, CancellationToken ct = default);
    Task<BaseResponse<WorkOrderDetailResponse>> GetByIdAsync(Guid id, CancellationToken ct = default);
    Task<BaseResponse<IReadOnlyList<WorkOrderLookupResponse>>> GetLookupAsync(string? search, bool activeOnly, CancellationToken ct = default);
    Task<BaseResponse<Guid>> CreateAsync(CreateWorkOrderRequest request, CancellationToken ct = default);
    Task<BaseResponse<bool>> UpdateAsync(Guid id, UpdateWorkOrderRequest request, CancellationToken ct = default);
    Task<BaseResponse<bool>> DeleteAsync(Guid id, CancellationToken ct = default);
}

/// <summary>WorkOrder API istemcisi (HttpClientFactory + BaseResponse).</summary>
public sealed class WorkOrderApiClient : ApiClientBase, IWorkOrderApiClient
{
    private const string Base = "api/v1/operations/work-orders";

    public WorkOrderApiClient(HttpClient httpClient) : base(httpClient) { }

    public Task<BaseResponse<PaginatedResponse<WorkOrderListResponse>>> GetListAsync(int pageNumber, int pageSize, string? search, CancellationToken ct = default)
        => GetAsync<BaseResponse<PaginatedResponse<WorkOrderListResponse>>>($"{Base}?pageNumber={pageNumber}&pageSize={pageSize}&search={Uri.EscapeDataString(search ?? string.Empty)}", ct);

    public Task<BaseResponse<WorkOrderDetailResponse>> GetByIdAsync(Guid id, CancellationToken ct = default)
        => GetAsync<BaseResponse<WorkOrderDetailResponse>>($"{Base}/{id}", ct);

    public Task<BaseResponse<IReadOnlyList<WorkOrderLookupResponse>>> GetLookupAsync(string? search, bool activeOnly, CancellationToken ct = default)
        => GetAsync<BaseResponse<IReadOnlyList<WorkOrderLookupResponse>>>($"{Base}/lookup?search={Uri.EscapeDataString(search ?? string.Empty)}&activeOnly={activeOnly}", ct);

    public Task<BaseResponse<Guid>> CreateAsync(CreateWorkOrderRequest request, CancellationToken ct = default)
        => PostAsync<CreateWorkOrderRequest, BaseResponse<Guid>>(Base, request, ct);

    public Task<BaseResponse<bool>> UpdateAsync(Guid id, UpdateWorkOrderRequest request, CancellationToken ct = default)
        => PutAsync<UpdateWorkOrderRequest, BaseResponse<bool>>($"{Base}/{id}", request, ct);

    public Task<BaseResponse<bool>> DeleteAsync(Guid id, CancellationToken ct = default)
        => DeleteAsync<BaseResponse<bool>>($"{Base}/{id}", ct);
}
