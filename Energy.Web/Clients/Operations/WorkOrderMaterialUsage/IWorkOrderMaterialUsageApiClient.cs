using Energy.Shared.Models.V1.Common.Responses;
using Energy.Shared.Models.V1.Operations.WorkOrderMaterialUsage.Requests;
using Energy.Shared.Models.V1.Operations.WorkOrderMaterialUsage.Responses;
using Energy.Web.Clients.Infrastructure;

namespace Energy.Web.Clients.Operations.WorkOrderMaterialUsage;

/// <summary>WorkOrderMaterialUsage API istemci sözleşmesi.</summary>
public interface IWorkOrderMaterialUsageApiClient
{
    Task<BaseResponse<PaginatedResponse<WorkOrderMaterialUsageListResponse>>> GetListAsync(int pageNumber, int pageSize, string? search, CancellationToken ct = default);
    Task<BaseResponse<WorkOrderMaterialUsageDetailResponse>> GetByIdAsync(Guid id, CancellationToken ct = default);
    Task<BaseResponse<IReadOnlyList<WorkOrderMaterialUsageLookupResponse>>> GetLookupAsync(string? search, bool activeOnly, CancellationToken ct = default);
    Task<BaseResponse<Guid>> CreateAsync(CreateWorkOrderMaterialUsageRequest request, CancellationToken ct = default);
    Task<BaseResponse<bool>> UpdateAsync(Guid id, UpdateWorkOrderMaterialUsageRequest request, CancellationToken ct = default);
    Task<BaseResponse<bool>> DeleteAsync(Guid id, CancellationToken ct = default);
}

/// <summary>WorkOrderMaterialUsage API istemcisi (HttpClientFactory + BaseResponse).</summary>
public sealed class WorkOrderMaterialUsageApiClient : ApiClientBase, IWorkOrderMaterialUsageApiClient
{
    private const string Base = "api/v1/operations/work-order-material-usages";

    public WorkOrderMaterialUsageApiClient(HttpClient httpClient) : base(httpClient) { }

    public Task<BaseResponse<PaginatedResponse<WorkOrderMaterialUsageListResponse>>> GetListAsync(int pageNumber, int pageSize, string? search, CancellationToken ct = default)
        => GetAsync<BaseResponse<PaginatedResponse<WorkOrderMaterialUsageListResponse>>>($"{Base}?pageNumber={pageNumber}&pageSize={pageSize}&search={Uri.EscapeDataString(search ?? string.Empty)}", ct);

    public Task<BaseResponse<WorkOrderMaterialUsageDetailResponse>> GetByIdAsync(Guid id, CancellationToken ct = default)
        => GetAsync<BaseResponse<WorkOrderMaterialUsageDetailResponse>>($"{Base}/{id}", ct);

    public Task<BaseResponse<IReadOnlyList<WorkOrderMaterialUsageLookupResponse>>> GetLookupAsync(string? search, bool activeOnly, CancellationToken ct = default)
        => GetAsync<BaseResponse<IReadOnlyList<WorkOrderMaterialUsageLookupResponse>>>($"{Base}/lookup?search={Uri.EscapeDataString(search ?? string.Empty)}&activeOnly={activeOnly}", ct);

    public Task<BaseResponse<Guid>> CreateAsync(CreateWorkOrderMaterialUsageRequest request, CancellationToken ct = default)
        => PostAsync<CreateWorkOrderMaterialUsageRequest, BaseResponse<Guid>>(Base, request, ct);

    public Task<BaseResponse<bool>> UpdateAsync(Guid id, UpdateWorkOrderMaterialUsageRequest request, CancellationToken ct = default)
        => PutAsync<UpdateWorkOrderMaterialUsageRequest, BaseResponse<bool>>($"{Base}/{id}", request, ct);

    public Task<BaseResponse<bool>> DeleteAsync(Guid id, CancellationToken ct = default)
        => DeleteAsync<BaseResponse<bool>>($"{Base}/{id}", ct);
}
