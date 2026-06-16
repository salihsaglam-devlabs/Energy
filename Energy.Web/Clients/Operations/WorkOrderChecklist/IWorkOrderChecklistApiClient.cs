using Energy.Shared.Models.V1.Common.Responses;
using Energy.Shared.Models.V1.Operations.WorkOrderChecklist.Requests;
using Energy.Shared.Models.V1.Operations.WorkOrderChecklist.Responses;
using Energy.Web.Clients.Infrastructure;

namespace Energy.Web.Clients.Operations.WorkOrderChecklist;

/// <summary>WorkOrderChecklist API istemci sözleşmesi.</summary>
public interface IWorkOrderChecklistApiClient
{
    Task<BaseResponse<PaginatedResponse<WorkOrderChecklistListResponse>>> GetListAsync(int pageNumber, int pageSize, string? search, CancellationToken ct = default);
    Task<BaseResponse<WorkOrderChecklistDetailResponse>> GetByIdAsync(Guid id, CancellationToken ct = default);
    Task<BaseResponse<IReadOnlyList<WorkOrderChecklistLookupResponse>>> GetLookupAsync(string? search, bool activeOnly, CancellationToken ct = default);
    Task<BaseResponse<Guid>> CreateAsync(CreateWorkOrderChecklistRequest request, CancellationToken ct = default);
    Task<BaseResponse<bool>> UpdateAsync(Guid id, UpdateWorkOrderChecklistRequest request, CancellationToken ct = default);
    Task<BaseResponse<bool>> DeleteAsync(Guid id, CancellationToken ct = default);
}

/// <summary>WorkOrderChecklist API istemcisi (HttpClientFactory + BaseResponse).</summary>
public sealed class WorkOrderChecklistApiClient : ApiClientBase, IWorkOrderChecklistApiClient
{
    private const string Base = "api/v1/operations/work-order-checklists";

    public WorkOrderChecklistApiClient(HttpClient httpClient) : base(httpClient) { }

    public Task<BaseResponse<PaginatedResponse<WorkOrderChecklistListResponse>>> GetListAsync(int pageNumber, int pageSize, string? search, CancellationToken ct = default)
        => GetAsync<BaseResponse<PaginatedResponse<WorkOrderChecklistListResponse>>>($"{Base}?pageNumber={pageNumber}&pageSize={pageSize}&search={Uri.EscapeDataString(search ?? string.Empty)}", ct);

    public Task<BaseResponse<WorkOrderChecklistDetailResponse>> GetByIdAsync(Guid id, CancellationToken ct = default)
        => GetAsync<BaseResponse<WorkOrderChecklistDetailResponse>>($"{Base}/{id}", ct);

    public Task<BaseResponse<IReadOnlyList<WorkOrderChecklistLookupResponse>>> GetLookupAsync(string? search, bool activeOnly, CancellationToken ct = default)
        => GetAsync<BaseResponse<IReadOnlyList<WorkOrderChecklistLookupResponse>>>($"{Base}/lookup?search={Uri.EscapeDataString(search ?? string.Empty)}&activeOnly={activeOnly}", ct);

    public Task<BaseResponse<Guid>> CreateAsync(CreateWorkOrderChecklistRequest request, CancellationToken ct = default)
        => PostAsync<CreateWorkOrderChecklistRequest, BaseResponse<Guid>>(Base, request, ct);

    public Task<BaseResponse<bool>> UpdateAsync(Guid id, UpdateWorkOrderChecklistRequest request, CancellationToken ct = default)
        => PutAsync<UpdateWorkOrderChecklistRequest, BaseResponse<bool>>($"{Base}/{id}", request, ct);

    public Task<BaseResponse<bool>> DeleteAsync(Guid id, CancellationToken ct = default)
        => DeleteAsync<BaseResponse<bool>>($"{Base}/{id}", ct);
}
