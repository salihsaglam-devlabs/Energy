using Energy.Shared.Models.V1.Common.Responses;
using Energy.Shared.Models.V1.Inventory.StockCountLine.Requests;
using Energy.Shared.Models.V1.Inventory.StockCountLine.Responses;

namespace Energy.Application.Inventory.StockCountLine.Services;

/// <summary>StockCountLine CRUD use-case sözleşmesi.</summary>
public interface IStockCountLineService
{
    /// <summary>Sayfalanmış StockCountLine listesi.</summary>
    Task<BaseResponse<PaginatedResponse<StockCountLineListResponse>>> GetListAsync(GetStockCountLineListRequest request, CancellationToken ct = default);

    /// <summary>Kimliğe göre detay.</summary>
    Task<BaseResponse<StockCountLineDetailResponse>> GetByIdAsync(Guid id, CancellationToken ct = default);

    /// <summary>Yeni kayıt oluşturur; yeni kimliği döndürür.</summary>
    Task<BaseResponse<Guid>> CreateAsync(CreateStockCountLineRequest request, CancellationToken ct = default);

    /// <summary>Var olan kaydı günceller.</summary>
    Task<BaseResponse<bool>> UpdateAsync(Guid id, UpdateStockCountLineRequest request, CancellationToken ct = default);

    /// <summary>Kaydı (gerekiyorsa soft-delete) siler.</summary>
    Task<BaseResponse<bool>> DeleteAsync(Guid id, CancellationToken ct = default);
}
