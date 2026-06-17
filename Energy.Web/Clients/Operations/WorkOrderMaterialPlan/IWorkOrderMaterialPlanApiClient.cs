using Energy.Shared.Models.V1.Common.Responses;
using Energy.Shared.Models.V1.Operations.WorkOrderMaterialPlan.Requests;
using Energy.Shared.Models.V1.Operations.WorkOrderMaterialPlan.Responses;
using Energy.Web.Clients.Infrastructure;

namespace Energy.Web.Clients.Operations.WorkOrderMaterialPlan;

/// <summary>WorkOrderMaterialPlan API istemci sözleşmesi.</summary>
public interface IWorkOrderMaterialPlanApiClient
{
    Task<BaseResponse<PaginatedResponse<WorkOrderMaterialPlanListResponse>>> GetListAsync(int pageNumber, int pageSize, string? search, CancellationToken ct = default);
    Task<BaseResponse<WorkOrderMaterialPlanDetailResponse>> GetByIdAsync(Guid id, CancellationToken ct = default);
    Task<BaseResponse<IReadOnlyList<WorkOrderMaterialPlanLookupResponse>>> GetLookupAsync(string? search, bool activeOnly, CancellationToken ct = default);
    Task<BaseResponse<Guid>> CreateAsync(CreateWorkOrderMaterialPlanRequest request, CancellationToken ct = default);
    Task<BaseResponse<bool>> UpdateAsync(Guid id, UpdateWorkOrderMaterialPlanRequest request, CancellationToken ct = default);
    Task<BaseResponse<bool>> DeleteAsync(Guid id, CancellationToken ct = default);
}

/// <summary>WorkOrderMaterialPlan API istemcisi (HttpClientFactory + BaseResponse).</summary>
public sealed class WorkOrderMaterialPlanApiClient : ApiClientBase, IWorkOrderMaterialPlanApiClient
{
    private const string Base = "api/v1/operations/work-order-material-plans";

    public WorkOrderMaterialPlanApiClient(HttpClient httpClient) : base(httpClient) { }

    public Task<BaseResponse<PaginatedResponse<WorkOrderMaterialPlanListResponse>>> GetListAsync(int pageNumber, int pageSize, string? search, CancellationToken ct = default)
        => GetAsync<BaseResponse<PaginatedResponse<WorkOrderMaterialPlanListResponse>>>($"{Base}?pageNumber={pageNumber}&pageSize={pageSize}&search={Uri.EscapeDataString(search ?? string.Empty)}", ct);

    public Task<BaseResponse<WorkOrderMaterialPlanDetailResponse>> GetByIdAsync(Guid id, CancellationToken ct = default)
        => GetAsync<BaseResponse<WorkOrderMaterialPlanDetailResponse>>($"{Base}/{id}", ct);

    public Task<BaseResponse<IReadOnlyList<WorkOrderMaterialPlanLookupResponse>>> GetLookupAsync(string? search, bool activeOnly, CancellationToken ct = default)
        => GetAsync<BaseResponse<IReadOnlyList<WorkOrderMaterialPlanLookupResponse>>>($"{Base}/lookup?search={Uri.EscapeDataString(search ?? string.Empty)}&activeOnly={activeOnly}", ct);

    public Task<BaseResponse<Guid>> CreateAsync(CreateWorkOrderMaterialPlanRequest request, CancellationToken ct = default)
        => PostAsync<CreateWorkOrderMaterialPlanRequest, BaseResponse<Guid>>(Base, request, ct);

    public Task<BaseResponse<bool>> UpdateAsync(Guid id, UpdateWorkOrderMaterialPlanRequest request, CancellationToken ct = default)
        => PutAsync<UpdateWorkOrderMaterialPlanRequest, BaseResponse<bool>>($"{Base}/{id}", request, ct);

    public Task<BaseResponse<bool>> DeleteAsync(Guid id, CancellationToken ct = default)
        => DeleteAsync<BaseResponse<bool>>($"{Base}/{id}", ct);
}
