using Energy.Shared.Models.V1.Common.Responses;
using Energy.Shared.Models.V1.Inventory.StockBalance.Requests;
using Energy.Shared.Models.V1.Inventory.StockBalance.Responses;

namespace Energy.Application.Modules.Inventory.StockBalance.Services;

/// <summary>StockBalance CRUD use-case sözleşmesi.</summary>
public interface IStockBalanceService
{
    /// <summary>Sayfalanmış StockBalance listesi.</summary>
    Task<BaseResponse<PaginatedResponse<StockBalanceListResponse>>> GetListAsync(GetStockBalanceListRequest request, CancellationToken ct = default);

    /// <summary>Kimliğe göre detay.</summary>
    Task<BaseResponse<StockBalanceDetailResponse>> GetByIdAsync(Guid id, CancellationToken ct = default);

    /// <summary>Yeni kayıt oluşturur; yeni kimliği döndürür.</summary>
    Task<BaseResponse<Guid>> CreateAsync(CreateStockBalanceRequest request, CancellationToken ct = default);

    /// <summary>Var olan kaydı günceller.</summary>
    Task<BaseResponse<bool>> UpdateAsync(Guid id, UpdateStockBalanceRequest request, CancellationToken ct = default);

    /// <summary>Kaydı (gerekiyorsa soft-delete) siler.</summary>
    Task<BaseResponse<bool>> DeleteAsync(Guid id, CancellationToken ct = default);
}
