using Energy.Shared.Models.V1.Common.Responses;
using Energy.Shared.Models.V1.Inventory.StockDocumentLine.Requests;
using Energy.Shared.Models.V1.Inventory.StockDocumentLine.Responses;

namespace Energy.Application.Modules.Inventory.StockDocumentLine.Services;

/// <summary>StockDocumentLine CRUD use-case sözleşmesi.</summary>
public interface IStockDocumentLineService
{
    /// <summary>Sayfalanmış StockDocumentLine listesi.</summary>
    Task<BaseResponse<PaginatedResponse<StockDocumentLineListResponse>>> GetListAsync(GetStockDocumentLineListRequest request, CancellationToken ct = default);

    /// <summary>Kimliğe göre detay.</summary>
    Task<BaseResponse<StockDocumentLineDetailResponse>> GetByIdAsync(Guid id, CancellationToken ct = default);

    /// <summary>Yeni kayıt oluşturur; yeni kimliği döndürür.</summary>
    Task<BaseResponse<Guid>> CreateAsync(CreateStockDocumentLineRequest request, CancellationToken ct = default);

    /// <summary>Var olan kaydı günceller.</summary>
    Task<BaseResponse<bool>> UpdateAsync(Guid id, UpdateStockDocumentLineRequest request, CancellationToken ct = default);

    /// <summary>Kaydı (gerekiyorsa soft-delete) siler.</summary>
    Task<BaseResponse<bool>> DeleteAsync(Guid id, CancellationToken ct = default);
}
