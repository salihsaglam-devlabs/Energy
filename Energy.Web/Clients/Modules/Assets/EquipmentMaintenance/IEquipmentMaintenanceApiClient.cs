using Energy.Shared.Models.V1.Common.Responses;
using Energy.Shared.Models.V1.Assets.EquipmentMaintenance.Requests;
using Energy.Shared.Models.V1.Assets.EquipmentMaintenance.Responses;
using Energy.Web.Clients.Infrastructure;

namespace Energy.Web.Clients.Modules.Assets.EquipmentMaintenance;

/// <summary>EquipmentMaintenance API istemci sözleşmesi.</summary>
public interface IEquipmentMaintenanceApiClient
{
    Task<BaseResponse<PaginatedResponse<EquipmentMaintenanceListResponse>>> GetListAsync(int pageNumber, int pageSize, string? search, CancellationToken ct = default);
    Task<BaseResponse<EquipmentMaintenanceDetailResponse>> GetByIdAsync(Guid id, CancellationToken ct = default);
    Task<BaseResponse<IReadOnlyList<EquipmentMaintenanceLookupResponse>>> GetLookupAsync(string? search, bool activeOnly, CancellationToken ct = default);
    Task<BaseResponse<Guid>> CreateAsync(CreateEquipmentMaintenanceRequest request, CancellationToken ct = default);
    Task<BaseResponse<bool>> UpdateAsync(Guid id, UpdateEquipmentMaintenanceRequest request, CancellationToken ct = default);
    Task<BaseResponse<bool>> DeleteAsync(Guid id, CancellationToken ct = default);
}

/// <summary>EquipmentMaintenance API istemcisi (HttpClientFactory + BaseResponse).</summary>
public sealed class EquipmentMaintenanceApiClient : ApiClientBase, IEquipmentMaintenanceApiClient
{
    private const string Base = "api/v1/assets/equipment-maintenances";

    public EquipmentMaintenanceApiClient(HttpClient httpClient) : base(httpClient) { }

    public Task<BaseResponse<PaginatedResponse<EquipmentMaintenanceListResponse>>> GetListAsync(int pageNumber, int pageSize, string? search, CancellationToken ct = default)
        => GetAsync<BaseResponse<PaginatedResponse<EquipmentMaintenanceListResponse>>>($"{Base}?pageNumber={pageNumber}&pageSize={pageSize}&search={Uri.EscapeDataString(search ?? string.Empty)}", ct);

    public Task<BaseResponse<EquipmentMaintenanceDetailResponse>> GetByIdAsync(Guid id, CancellationToken ct = default)
        => GetAsync<BaseResponse<EquipmentMaintenanceDetailResponse>>($"{Base}/{id}", ct);

    public Task<BaseResponse<IReadOnlyList<EquipmentMaintenanceLookupResponse>>> GetLookupAsync(string? search, bool activeOnly, CancellationToken ct = default)
        => GetAsync<BaseResponse<IReadOnlyList<EquipmentMaintenanceLookupResponse>>>($"{Base}/lookup?search={Uri.EscapeDataString(search ?? string.Empty)}&activeOnly={activeOnly}", ct);

    public Task<BaseResponse<Guid>> CreateAsync(CreateEquipmentMaintenanceRequest request, CancellationToken ct = default)
        => PostAsync<CreateEquipmentMaintenanceRequest, BaseResponse<Guid>>(Base, request, ct);

    public Task<BaseResponse<bool>> UpdateAsync(Guid id, UpdateEquipmentMaintenanceRequest request, CancellationToken ct = default)
        => PutAsync<UpdateEquipmentMaintenanceRequest, BaseResponse<bool>>($"{Base}/{id}", request, ct);

    public Task<BaseResponse<bool>> DeleteAsync(Guid id, CancellationToken ct = default)
        => DeleteAsync<BaseResponse<bool>>($"{Base}/{id}", ct);
}
