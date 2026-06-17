using Energy.Shared.Models.V1.Common.Responses;
using Energy.Shared.Models.V1.Inventory.StockLot.Requests;
using Energy.Shared.Models.V1.Inventory.StockLot.Responses;

namespace Energy.Application.Inventory.StockLot.Services;

/// <summary>StockLot CRUD use-case sözleşmesi.</summary>
public interface IStockLotService
{
    /// <summary>Sayfalanmış StockLot listesi.</summary>
    Task<BaseResponse<PaginatedResponse<StockLotListResponse>>> GetListAsync(GetStockLotListRequest request, CancellationToken ct = default);

    /// <summary>Kimliğe göre detay.</summary>
    Task<BaseResponse<StockLotDetailResponse>> GetByIdAsync(Guid id, CancellationToken ct = default);

    /// <summary>Yeni kayıt oluşturur; yeni kimliği döndürür.</summary>
    Task<BaseResponse<Guid>> CreateAsync(CreateStockLotRequest request, CancellationToken ct = default);

    /// <summary>Var olan kaydı günceller.</summary>
    Task<BaseResponse<bool>> UpdateAsync(Guid id, UpdateStockLotRequest request, CancellationToken ct = default);

    /// <summary>Kaydı (gerekiyorsa soft-delete) siler.</summary>
    Task<BaseResponse<bool>> DeleteAsync(Guid id, CancellationToken ct = default);
}
