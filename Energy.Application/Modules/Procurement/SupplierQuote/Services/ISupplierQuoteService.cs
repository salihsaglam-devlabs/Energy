using Energy.Shared.Models.V1.Common.Responses;
using Energy.Shared.Models.V1.Procurement.SupplierQuote.Requests;
using Energy.Shared.Models.V1.Procurement.SupplierQuote.Responses;

namespace Energy.Application.Modules.Procurement.SupplierQuote.Services;

/// <summary>SupplierQuote CRUD use-case sözleşmesi.</summary>
public interface ISupplierQuoteService
{
    /// <summary>Sayfalanmış SupplierQuote listesi.</summary>
    Task<BaseResponse<PaginatedResponse<SupplierQuoteListResponse>>> GetListAsync(GetSupplierQuoteListRequest request, CancellationToken ct = default);

    /// <summary>Kimliğe göre detay.</summary>
    Task<BaseResponse<SupplierQuoteDetailResponse>> GetByIdAsync(Guid id, CancellationToken ct = default);

    /// <summary>Yeni kayıt oluşturur; yeni kimliği döndürür.</summary>
    Task<BaseResponse<Guid>> CreateAsync(CreateSupplierQuoteRequest request, CancellationToken ct = default);

    /// <summary>Var olan kaydı günceller.</summary>
    Task<BaseResponse<bool>> UpdateAsync(Guid id, UpdateSupplierQuoteRequest request, CancellationToken ct = default);

    /// <summary>Kaydı (gerekiyorsa soft-delete) siler.</summary>
    Task<BaseResponse<bool>> DeleteAsync(Guid id, CancellationToken ct = default);
}
