using Energy.Shared.Models.V1.Common.Responses;
using Energy.Shared.Models.V1.Inventory.Warehouse.Requests;
using Energy.Shared.Models.V1.Inventory.Warehouse.Responses;

namespace Energy.Application.Inventory.Warehouse.Services;

/// <summary>Warehouse CRUD use-case sözleşmesi.</summary>
public interface IWarehouseService
{
    /// <summary>Sayfalanmış Warehouse listesi.</summary>
    Task<BaseResponse<PaginatedResponse<WarehouseListResponse>>> GetListAsync(GetWarehouseListRequest request, CancellationToken ct = default);

    /// <summary>Kimliğe göre detay.</summary>
    Task<BaseResponse<WarehouseDetailResponse>> GetByIdAsync(Guid id, CancellationToken ct = default);

    /// <summary>Yeni kayıt oluşturur; yeni kimliği döndürür.</summary>
    Task<BaseResponse<Guid>> CreateAsync(CreateWarehouseRequest request, CancellationToken ct = default);

    /// <summary>Var olan kaydı günceller.</summary>
    Task<BaseResponse<bool>> UpdateAsync(Guid id, UpdateWarehouseRequest request, CancellationToken ct = default);

    /// <summary>Kaydı (gerekiyorsa soft-delete) siler.</summary>
    Task<BaseResponse<bool>> DeleteAsync(Guid id, CancellationToken ct = default);
}
