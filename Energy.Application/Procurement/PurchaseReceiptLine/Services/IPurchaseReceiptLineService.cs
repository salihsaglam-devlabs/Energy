using Energy.Shared.Models.V1.Common.Responses;
using Energy.Shared.Models.V1.Procurement.PurchaseReceiptLine.Requests;
using Energy.Shared.Models.V1.Procurement.PurchaseReceiptLine.Responses;

namespace Energy.Application.Procurement.PurchaseReceiptLine.Services;

/// <summary>PurchaseReceiptLine CRUD use-case sözleşmesi.</summary>
public interface IPurchaseReceiptLineService
{
    /// <summary>Sayfalanmış PurchaseReceiptLine listesi.</summary>
    Task<BaseResponse<PaginatedResponse<PurchaseReceiptLineListResponse>>> GetListAsync(GetPurchaseReceiptLineListRequest request, CancellationToken ct = default);

    /// <summary>Kimliğe göre detay.</summary>
    Task<BaseResponse<PurchaseReceiptLineDetailResponse>> GetByIdAsync(Guid id, CancellationToken ct = default);

    /// <summary>Yeni kayıt oluşturur; yeni kimliği döndürür.</summary>
    Task<BaseResponse<Guid>> CreateAsync(CreatePurchaseReceiptLineRequest request, CancellationToken ct = default);

    /// <summary>Var olan kaydı günceller.</summary>
    Task<BaseResponse<bool>> UpdateAsync(Guid id, UpdatePurchaseReceiptLineRequest request, CancellationToken ct = default);

    /// <summary>Kaydı (gerekiyorsa soft-delete) siler.</summary>
    Task<BaseResponse<bool>> DeleteAsync(Guid id, CancellationToken ct = default);
}
