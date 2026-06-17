using Energy.Shared.Models.V1.Common.Responses;
using Energy.Shared.Models.V1.Inventory.StockCount.Requests;
using Energy.Shared.Models.V1.Inventory.StockCount.Responses;

namespace Energy.Application.Inventory.StockCount.Services;

/// <summary>StockCount CRUD use-case sözleşmesi.</summary>
public interface IStockCountService
{
    /// <summary>Sayfalanmış StockCount listesi.</summary>
    Task<BaseResponse<PaginatedResponse<StockCountListResponse>>> GetListAsync(GetStockCountListRequest request, CancellationToken ct = default);

    /// <summary>Kimliğe göre detay.</summary>
    Task<BaseResponse<StockCountDetailResponse>> GetByIdAsync(Guid id, CancellationToken ct = default);

    /// <summary>Yeni kayıt oluşturur; yeni kimliği döndürür.</summary>
    Task<BaseResponse<Guid>> CreateAsync(CreateStockCountRequest request, CancellationToken ct = default);

    /// <summary>Var olan kaydı günceller.</summary>
    Task<BaseResponse<bool>> UpdateAsync(Guid id, UpdateStockCountRequest request, CancellationToken ct = default);

    /// <summary>Kaydı (gerekiyorsa soft-delete) siler.</summary>
    Task<BaseResponse<bool>> DeleteAsync(Guid id, CancellationToken ct = default);
}
