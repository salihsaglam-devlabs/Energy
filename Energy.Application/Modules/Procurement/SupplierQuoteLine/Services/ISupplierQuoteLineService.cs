using Energy.Shared.Models.V1.Common.Responses;
using Energy.Shared.Models.V1.Procurement.SupplierQuoteLine.Requests;
using Energy.Shared.Models.V1.Procurement.SupplierQuoteLine.Responses;

namespace Energy.Application.Modules.Procurement.SupplierQuoteLine.Services;

/// <summary>SupplierQuoteLine CRUD use-case sözleşmesi.</summary>
public interface ISupplierQuoteLineService
{
    /// <summary>Sayfalanmış SupplierQuoteLine listesi.</summary>
    Task<BaseResponse<PaginatedResponse<SupplierQuoteLineListResponse>>> GetListAsync(GetSupplierQuoteLineListRequest request, CancellationToken ct = default);

    /// <summary>Kimliğe göre detay.</summary>
    Task<BaseResponse<SupplierQuoteLineDetailResponse>> GetByIdAsync(Guid id, CancellationToken ct = default);

    /// <summary>Yeni kayıt oluşturur; yeni kimliği döndürür.</summary>
    Task<BaseResponse<Guid>> CreateAsync(CreateSupplierQuoteLineRequest request, CancellationToken ct = default);

    /// <summary>Var olan kaydı günceller.</summary>
    Task<BaseResponse<bool>> UpdateAsync(Guid id, UpdateSupplierQuoteLineRequest request, CancellationToken ct = default);

    /// <summary>Kaydı (gerekiyorsa soft-delete) siler.</summary>
    Task<BaseResponse<bool>> DeleteAsync(Guid id, CancellationToken ct = default);
}
