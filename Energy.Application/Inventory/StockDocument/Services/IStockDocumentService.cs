using Energy.Shared.Models.V1.Common.Responses;
using Energy.Shared.Models.V1.Inventory.StockDocument.Requests;
using Energy.Shared.Models.V1.Inventory.StockDocument.Responses;

namespace Energy.Application.Inventory.StockDocument.Services;

/// <summary>StockDocument CRUD use-case sözleşmesi.</summary>
public interface IStockDocumentService
{
    /// <summary>Sayfalanmış StockDocument listesi.</summary>
    Task<BaseResponse<PaginatedResponse<StockDocumentListResponse>>> GetListAsync(GetStockDocumentListRequest request, CancellationToken ct = default);

    /// <summary>Kimliğe göre detay.</summary>
    Task<BaseResponse<StockDocumentDetailResponse>> GetByIdAsync(Guid id, CancellationToken ct = default);

    /// <summary>Yeni kayıt oluşturur; yeni kimliği döndürür.</summary>
    Task<BaseResponse<Guid>> CreateAsync(CreateStockDocumentRequest request, CancellationToken ct = default);

    /// <summary>Var olan kaydı günceller.</summary>
    Task<BaseResponse<bool>> UpdateAsync(Guid id, UpdateStockDocumentRequest request, CancellationToken ct = default);

    /// <summary>Kaydı (gerekiyorsa soft-delete) siler.</summary>
    Task<BaseResponse<bool>> DeleteAsync(Guid id, CancellationToken ct = default);
}
