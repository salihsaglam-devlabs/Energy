using Energy.Shared.Models.V1.Common.Responses;
using Energy.Shared.Models.V1.Procurement.SupplierInvoiceLine.Requests;
using Energy.Shared.Models.V1.Procurement.SupplierInvoiceLine.Responses;

namespace Energy.Application.Modules.Procurement.SupplierInvoiceLine.Services;

/// <summary>SupplierInvoiceLine CRUD use-case sözleşmesi.</summary>
public interface ISupplierInvoiceLineService
{
    /// <summary>Sayfalanmış SupplierInvoiceLine listesi.</summary>
    Task<BaseResponse<PaginatedResponse<SupplierInvoiceLineListResponse>>> GetListAsync(GetSupplierInvoiceLineListRequest request, CancellationToken ct = default);

    /// <summary>Kimliğe göre detay.</summary>
    Task<BaseResponse<SupplierInvoiceLineDetailResponse>> GetByIdAsync(Guid id, CancellationToken ct = default);

    /// <summary>Yeni kayıt oluşturur; yeni kimliği döndürür.</summary>
    Task<BaseResponse<Guid>> CreateAsync(CreateSupplierInvoiceLineRequest request, CancellationToken ct = default);

    /// <summary>Var olan kaydı günceller.</summary>
    Task<BaseResponse<bool>> UpdateAsync(Guid id, UpdateSupplierInvoiceLineRequest request, CancellationToken ct = default);

    /// <summary>Kaydı (gerekiyorsa soft-delete) siler.</summary>
    Task<BaseResponse<bool>> DeleteAsync(Guid id, CancellationToken ct = default);
}
