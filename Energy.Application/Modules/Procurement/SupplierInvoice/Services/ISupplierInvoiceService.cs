using Energy.Shared.Models.V1.Common.Responses;
using Energy.Shared.Models.V1.Procurement.SupplierInvoice.Requests;
using Energy.Shared.Models.V1.Procurement.SupplierInvoice.Responses;

namespace Energy.Application.Modules.Procurement.SupplierInvoice.Services;

/// <summary>SupplierInvoice CRUD use-case sözleşmesi.</summary>
public interface ISupplierInvoiceService
{
    /// <summary>Sayfalanmış SupplierInvoice listesi.</summary>
    Task<BaseResponse<PaginatedResponse<SupplierInvoiceListResponse>>> GetListAsync(GetSupplierInvoiceListRequest request, CancellationToken ct = default);

    /// <summary>Kimliğe göre detay.</summary>
    Task<BaseResponse<SupplierInvoiceDetailResponse>> GetByIdAsync(Guid id, CancellationToken ct = default);

    /// <summary>Yeni kayıt oluşturur; yeni kimliği döndürür.</summary>
    Task<BaseResponse<Guid>> CreateAsync(CreateSupplierInvoiceRequest request, CancellationToken ct = default);

    /// <summary>Var olan kaydı günceller.</summary>
    Task<BaseResponse<bool>> UpdateAsync(Guid id, UpdateSupplierInvoiceRequest request, CancellationToken ct = default);

    /// <summary>Kaydı (gerekiyorsa soft-delete) siler.</summary>
    Task<BaseResponse<bool>> DeleteAsync(Guid id, CancellationToken ct = default);
}
