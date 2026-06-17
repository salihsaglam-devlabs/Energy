using Energy.Shared.Models.V1.Common.Responses;
using Energy.Shared.Models.V1.Finance.CostCenter.Requests;
using Energy.Shared.Models.V1.Finance.CostCenter.Responses;
using Energy.Web.Clients.Infrastructure;

namespace Energy.Web.Clients.Finance.CostCenter;

/// <summary>CostCenter API istemci sözleşmesi.</summary>
public interface ICostCenterApiClient
{
    Task<BaseResponse<PaginatedResponse<CostCenterListResponse>>> GetListAsync(int pageNumber, int pageSize, string? search, CancellationToken ct = default);
    Task<BaseResponse<CostCenterDetailResponse>> GetByIdAsync(Guid id, CancellationToken ct = default);
    Task<BaseResponse<IReadOnlyList<CostCenterLookupResponse>>> GetLookupAsync(string? search, bool activeOnly, CancellationToken ct = default);
    Task<BaseResponse<Guid>> CreateAsync(CreateCostCenterRequest request, CancellationToken ct = default);
    Task<BaseResponse<bool>> UpdateAsync(Guid id, UpdateCostCenterRequest request, CancellationToken ct = default);
    Task<BaseResponse<bool>> DeleteAsync(Guid id, CancellationToken ct = default);
}

/// <summary>CostCenter API istemcisi (HttpClientFactory + BaseResponse).</summary>
public sealed class CostCenterApiClient : ApiClientBase, ICostCenterApiClient
{
    private const string Base = "api/v1/finance/cost-centers";

    public CostCenterApiClient(HttpClient httpClient) : base(httpClient) { }

    public Task<BaseResponse<PaginatedResponse<CostCenterListResponse>>> GetListAsync(int pageNumber, int pageSize, string? search, CancellationToken ct = default)
        => GetAsync<BaseResponse<PaginatedResponse<CostCenterListResponse>>>($"{Base}?pageNumber={pageNumber}&pageSize={pageSize}&search={Uri.EscapeDataString(search ?? string.Empty)}", ct);

    public Task<BaseResponse<CostCenterDetailResponse>> GetByIdAsync(Guid id, CancellationToken ct = default)
        => GetAsync<BaseResponse<CostCenterDetailResponse>>($"{Base}/{id}", ct);

    public Task<BaseResponse<IReadOnlyList<CostCenterLookupResponse>>> GetLookupAsync(string? search, bool activeOnly, CancellationToken ct = default)
        => GetAsync<BaseResponse<IReadOnlyList<CostCenterLookupResponse>>>($"{Base}/lookup?search={Uri.EscapeDataString(search ?? string.Empty)}&activeOnly={activeOnly}", ct);

    public Task<BaseResponse<Guid>> CreateAsync(CreateCostCenterRequest request, CancellationToken ct = default)
        => PostAsync<CreateCostCenterRequest, BaseResponse<Guid>>(Base, request, ct);

    public Task<BaseResponse<bool>> UpdateAsync(Guid id, UpdateCostCenterRequest request, CancellationToken ct = default)
        => PutAsync<UpdateCostCenterRequest, BaseResponse<bool>>($"{Base}/{id}", request, ct);

    public Task<BaseResponse<bool>> DeleteAsync(Guid id, CancellationToken ct = default)
        => DeleteAsync<BaseResponse<bool>>($"{Base}/{id}", ct);
}
