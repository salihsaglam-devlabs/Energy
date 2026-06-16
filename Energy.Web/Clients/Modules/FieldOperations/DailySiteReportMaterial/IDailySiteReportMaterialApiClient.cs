using Energy.Shared.Models.V1.Common.Responses;
using Energy.Shared.Models.V1.FieldOperations.DailySiteReportMaterial.Requests;
using Energy.Shared.Models.V1.FieldOperations.DailySiteReportMaterial.Responses;
using Energy.Web.Clients.Infrastructure;

namespace Energy.Web.Clients.Modules.FieldOperations.DailySiteReportMaterial;

/// <summary>DailySiteReportMaterial API istemci sözleşmesi.</summary>
public interface IDailySiteReportMaterialApiClient
{
    Task<BaseResponse<PaginatedResponse<DailySiteReportMaterialListResponse>>> GetListAsync(int pageNumber, int pageSize, string? search, CancellationToken ct = default);
    Task<BaseResponse<DailySiteReportMaterialDetailResponse>> GetByIdAsync(Guid id, CancellationToken ct = default);
    Task<BaseResponse<IReadOnlyList<DailySiteReportMaterialLookupResponse>>> GetLookupAsync(string? search, bool activeOnly, CancellationToken ct = default);
    Task<BaseResponse<Guid>> CreateAsync(CreateDailySiteReportMaterialRequest request, CancellationToken ct = default);
    Task<BaseResponse<bool>> UpdateAsync(Guid id, UpdateDailySiteReportMaterialRequest request, CancellationToken ct = default);
    Task<BaseResponse<bool>> DeleteAsync(Guid id, CancellationToken ct = default);
}

/// <summary>DailySiteReportMaterial API istemcisi (HttpClientFactory + BaseResponse).</summary>
public sealed class DailySiteReportMaterialApiClient : ApiClientBase, IDailySiteReportMaterialApiClient
{
    private const string Base = "api/v1/field-operations/daily-site-report-materials";

    public DailySiteReportMaterialApiClient(HttpClient httpClient) : base(httpClient) { }

    public Task<BaseResponse<PaginatedResponse<DailySiteReportMaterialListResponse>>> GetListAsync(int pageNumber, int pageSize, string? search, CancellationToken ct = default)
        => GetAsync<BaseResponse<PaginatedResponse<DailySiteReportMaterialListResponse>>>($"{Base}?pageNumber={pageNumber}&pageSize={pageSize}&search={Uri.EscapeDataString(search ?? string.Empty)}", ct);

    public Task<BaseResponse<DailySiteReportMaterialDetailResponse>> GetByIdAsync(Guid id, CancellationToken ct = default)
        => GetAsync<BaseResponse<DailySiteReportMaterialDetailResponse>>($"{Base}/{id}", ct);

    public Task<BaseResponse<IReadOnlyList<DailySiteReportMaterialLookupResponse>>> GetLookupAsync(string? search, bool activeOnly, CancellationToken ct = default)
        => GetAsync<BaseResponse<IReadOnlyList<DailySiteReportMaterialLookupResponse>>>($"{Base}/lookup?search={Uri.EscapeDataString(search ?? string.Empty)}&activeOnly={activeOnly}", ct);

    public Task<BaseResponse<Guid>> CreateAsync(CreateDailySiteReportMaterialRequest request, CancellationToken ct = default)
        => PostAsync<CreateDailySiteReportMaterialRequest, BaseResponse<Guid>>(Base, request, ct);

    public Task<BaseResponse<bool>> UpdateAsync(Guid id, UpdateDailySiteReportMaterialRequest request, CancellationToken ct = default)
        => PutAsync<UpdateDailySiteReportMaterialRequest, BaseResponse<bool>>($"{Base}/{id}", request, ct);

    public Task<BaseResponse<bool>> DeleteAsync(Guid id, CancellationToken ct = default)
        => DeleteAsync<BaseResponse<bool>>($"{Base}/{id}", ct);
}
