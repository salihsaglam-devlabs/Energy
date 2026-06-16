using Energy.Shared.Models.V1.Common.Responses;
using Energy.Shared.Models.V1.Inventory.WarehouseLocation.Requests;
using Energy.Shared.Models.V1.Inventory.WarehouseLocation.Responses;

namespace Energy.Application.Inventory.WarehouseLocation.Services;

/// <summary>WarehouseLocation CRUD use-case sözleşmesi.</summary>
public interface IWarehouseLocationService
{
    /// <summary>Sayfalanmış WarehouseLocation listesi.</summary>
    Task<BaseResponse<PaginatedResponse<WarehouseLocationListResponse>>> GetListAsync(GetWarehouseLocationListRequest request, CancellationToken ct = default);

    /// <summary>Kimliğe göre detay.</summary>
    Task<BaseResponse<WarehouseLocationDetailResponse>> GetByIdAsync(Guid id, CancellationToken ct = default);

    /// <summary>Yeni kayıt oluşturur; yeni kimliği döndürür.</summary>
    Task<BaseResponse<Guid>> CreateAsync(CreateWarehouseLocationRequest request, CancellationToken ct = default);

    /// <summary>Var olan kaydı günceller.</summary>
    Task<BaseResponse<bool>> UpdateAsync(Guid id, UpdateWarehouseLocationRequest request, CancellationToken ct = default);

    /// <summary>Kaydı (gerekiyorsa soft-delete) siler.</summary>
    Task<BaseResponse<bool>> DeleteAsync(Guid id, CancellationToken ct = default);
}
