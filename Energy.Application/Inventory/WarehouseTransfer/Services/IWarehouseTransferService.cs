using Energy.Shared.Models.V1.Common.Responses;
using Energy.Shared.Models.V1.Inventory.WarehouseTransfer.Requests;
using Energy.Shared.Models.V1.Inventory.WarehouseTransfer.Responses;

namespace Energy.Application.Inventory.WarehouseTransfer.Services;

/// <summary>WarehouseTransfer CRUD use-case sözleşmesi.</summary>
public interface IWarehouseTransferService
{
    /// <summary>Sayfalanmış WarehouseTransfer listesi.</summary>
    Task<BaseResponse<PaginatedResponse<WarehouseTransferListResponse>>> GetListAsync(GetWarehouseTransferListRequest request, CancellationToken ct = default);

    /// <summary>Kimliğe göre detay.</summary>
    Task<BaseResponse<WarehouseTransferDetailResponse>> GetByIdAsync(Guid id, CancellationToken ct = default);

    /// <summary>Yeni kayıt oluşturur; yeni kimliği döndürür.</summary>
    Task<BaseResponse<Guid>> CreateAsync(CreateWarehouseTransferRequest request, CancellationToken ct = default);

    /// <summary>Var olan kaydı günceller.</summary>
    Task<BaseResponse<bool>> UpdateAsync(Guid id, UpdateWarehouseTransferRequest request, CancellationToken ct = default);

    /// <summary>Kaydı (gerekiyorsa soft-delete) siler.</summary>
    Task<BaseResponse<bool>> DeleteAsync(Guid id, CancellationToken ct = default);
}
