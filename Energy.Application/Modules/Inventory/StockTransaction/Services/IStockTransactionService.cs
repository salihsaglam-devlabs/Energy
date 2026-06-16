using Energy.Shared.Models.V1.Common.Responses;
using Energy.Shared.Models.V1.Inventory.StockTransaction.Requests;
using Energy.Shared.Models.V1.Inventory.StockTransaction.Responses;

namespace Energy.Application.Modules.Inventory.StockTransaction.Services;

/// <summary>StockTransaction CRUD use-case sözleşmesi.</summary>
public interface IStockTransactionService
{
    /// <summary>Sayfalanmış StockTransaction listesi.</summary>
    Task<BaseResponse<PaginatedResponse<StockTransactionListResponse>>> GetListAsync(GetStockTransactionListRequest request, CancellationToken ct = default);

    /// <summary>Kimliğe göre detay.</summary>
    Task<BaseResponse<StockTransactionDetailResponse>> GetByIdAsync(Guid id, CancellationToken ct = default);

    /// <summary>Yeni kayıt oluşturur; yeni kimliği döndürür.</summary>
    Task<BaseResponse<Guid>> CreateAsync(CreateStockTransactionRequest request, CancellationToken ct = default);

    /// <summary>Var olan kaydı günceller.</summary>
    Task<BaseResponse<bool>> UpdateAsync(Guid id, UpdateStockTransactionRequest request, CancellationToken ct = default);

    /// <summary>Kaydı (gerekiyorsa soft-delete) siler.</summary>
    Task<BaseResponse<bool>> DeleteAsync(Guid id, CancellationToken ct = default);
}
