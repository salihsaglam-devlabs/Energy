using Energy.Shared.Models.V1.Common.Responses;
using Energy.Shared.Models.V1.FieldOperations.DailySiteReportEquipment.Requests;
using Energy.Shared.Models.V1.FieldOperations.DailySiteReportEquipment.Responses;
using Energy.Web.Clients.Infrastructure;

namespace Energy.Web.Clients.Modules.FieldOperations.DailySiteReportEquipment;

/// <summary>DailySiteReportEquipment API istemci sözleşmesi.</summary>
public interface IDailySiteReportEquipmentApiClient
{
    Task<BaseResponse<PaginatedResponse<DailySiteReportEquipmentListResponse>>> GetListAsync(int pageNumber, int pageSize, string? search, CancellationToken ct = default);
    Task<BaseResponse<DailySiteReportEquipmentDetailResponse>> GetByIdAsync(Guid id, CancellationToken ct = default);
    Task<BaseResponse<IReadOnlyList<DailySiteReportEquipmentLookupResponse>>> GetLookupAsync(string? search, bool activeOnly, CancellationToken ct = default);
    Task<BaseResponse<Guid>> CreateAsync(CreateDailySiteReportEquipmentRequest request, CancellationToken ct = default);
    Task<BaseResponse<bool>> UpdateAsync(Guid id, UpdateDailySiteReportEquipmentRequest request, CancellationToken ct = default);
    Task<BaseResponse<bool>> DeleteAsync(Guid id, CancellationToken ct = default);
}

/// <summary>DailySiteReportEquipment API istemcisi (HttpClientFactory + BaseResponse).</summary>
public sealed class DailySiteReportEquipmentApiClient : ApiClientBase, IDailySiteReportEquipmentApiClient
{
    private const string Base = "api/v1/field-operations/daily-site-report-equipments";

    public DailySiteReportEquipmentApiClient(HttpClient httpClient) : base(httpClient) { }

    public Task<BaseResponse<PaginatedResponse<DailySiteReportEquipmentListResponse>>> GetListAsync(int pageNumber, int pageSize, string? search, CancellationToken ct = default)
        => GetAsync<BaseResponse<PaginatedResponse<DailySiteReportEquipmentListResponse>>>($"{Base}?pageNumber={pageNumber}&pageSize={pageSize}&search={Uri.EscapeDataString(search ?? string.Empty)}", ct);

    public Task<BaseResponse<DailySiteReportEquipmentDetailResponse>> GetByIdAsync(Guid id, CancellationToken ct = default)
        => GetAsync<BaseResponse<DailySiteReportEquipmentDetailResponse>>($"{Base}/{id}", ct);

    public Task<BaseResponse<IReadOnlyList<DailySiteReportEquipmentLookupResponse>>> GetLookupAsync(string? search, bool activeOnly, CancellationToken ct = default)
        => GetAsync<BaseResponse<IReadOnlyList<DailySiteReportEquipmentLookupResponse>>>($"{Base}/lookup?search={Uri.EscapeDataString(search ?? string.Empty)}&activeOnly={activeOnly}", ct);

    public Task<BaseResponse<Guid>> CreateAsync(CreateDailySiteReportEquipmentRequest request, CancellationToken ct = default)
        => PostAsync<CreateDailySiteReportEquipmentRequest, BaseResponse<Guid>>(Base, request, ct);

    public Task<BaseResponse<bool>> UpdateAsync(Guid id, UpdateDailySiteReportEquipmentRequest request, CancellationToken ct = default)
        => PutAsync<UpdateDailySiteReportEquipmentRequest, BaseResponse<bool>>($"{Base}/{id}", request, ct);

    public Task<BaseResponse<bool>> DeleteAsync(Guid id, CancellationToken ct = default)
        => DeleteAsync<BaseResponse<bool>>($"{Base}/{id}", ct);
}
