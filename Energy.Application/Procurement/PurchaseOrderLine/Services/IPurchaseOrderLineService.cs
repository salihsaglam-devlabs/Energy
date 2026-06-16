using Energy.Shared.Models.V1.Common.Responses;
using Energy.Shared.Models.V1.Procurement.PurchaseOrderLine.Requests;
using Energy.Shared.Models.V1.Procurement.PurchaseOrderLine.Responses;

namespace Energy.Application.Procurement.PurchaseOrderLine.Services;

/// <summary>PurchaseOrderLine CRUD use-case sözleşmesi.</summary>
public interface IPurchaseOrderLineService
{
    /// <summary>Sayfalanmış PurchaseOrderLine listesi.</summary>
    Task<BaseResponse<PaginatedResponse<PurchaseOrderLineListResponse>>> GetListAsync(GetPurchaseOrderLineListRequest request, CancellationToken ct = default);

    /// <summary>Kimliğe göre detay.</summary>
    Task<BaseResponse<PurchaseOrderLineDetailResponse>> GetByIdAsync(Guid id, CancellationToken ct = default);

    /// <summary>Yeni kayıt oluşturur; yeni kimliği döndürür.</summary>
    Task<BaseResponse<Guid>> CreateAsync(CreatePurchaseOrderLineRequest request, CancellationToken ct = default);

    /// <summary>Var olan kaydı günceller.</summary>
    Task<BaseResponse<bool>> UpdateAsync(Guid id, UpdatePurchaseOrderLineRequest request, CancellationToken ct = default);

    /// <summary>Kaydı (gerekiyorsa soft-delete) siler.</summary>
    Task<BaseResponse<bool>> DeleteAsync(Guid id, CancellationToken ct = default);
}
