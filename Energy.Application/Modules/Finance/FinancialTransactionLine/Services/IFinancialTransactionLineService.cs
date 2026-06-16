using Energy.Shared.Models.V1.Common.Responses;
using Energy.Shared.Models.V1.Finance.FinancialTransactionLine.Requests;
using Energy.Shared.Models.V1.Finance.FinancialTransactionLine.Responses;

namespace Energy.Application.Modules.Finance.FinancialTransactionLine.Services;

/// <summary>FinancialTransactionLine CRUD use-case sözleşmesi.</summary>
public interface IFinancialTransactionLineService
{
    /// <summary>Sayfalanmış FinancialTransactionLine listesi.</summary>
    Task<BaseResponse<PaginatedResponse<FinancialTransactionLineListResponse>>> GetListAsync(GetFinancialTransactionLineListRequest request, CancellationToken ct = default);

    /// <summary>Kimliğe göre detay.</summary>
    Task<BaseResponse<FinancialTransactionLineDetailResponse>> GetByIdAsync(Guid id, CancellationToken ct = default);

    /// <summary>Yeni kayıt oluşturur; yeni kimliği döndürür.</summary>
    Task<BaseResponse<Guid>> CreateAsync(CreateFinancialTransactionLineRequest request, CancellationToken ct = default);

    /// <summary>Var olan kaydı günceller.</summary>
    Task<BaseResponse<bool>> UpdateAsync(Guid id, UpdateFinancialTransactionLineRequest request, CancellationToken ct = default);

    /// <summary>Kaydı (gerekiyorsa soft-delete) siler.</summary>
    Task<BaseResponse<bool>> DeleteAsync(Guid id, CancellationToken ct = default);
}
