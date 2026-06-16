using Energy.Shared.Models.V1.Common.Responses;
using Energy.Shared.Models.V1.Finance.FinancialTransaction.Requests;
using Energy.Shared.Models.V1.Finance.FinancialTransaction.Responses;

namespace Energy.Application.Finance.FinancialTransaction.Services;

/// <summary>FinancialTransaction CRUD use-case sözleşmesi.</summary>
public interface IFinancialTransactionService
{
    /// <summary>Sayfalanmış FinancialTransaction listesi.</summary>
    Task<BaseResponse<PaginatedResponse<FinancialTransactionListResponse>>> GetListAsync(GetFinancialTransactionListRequest request, CancellationToken ct = default);

    /// <summary>Kimliğe göre detay.</summary>
    Task<BaseResponse<FinancialTransactionDetailResponse>> GetByIdAsync(Guid id, CancellationToken ct = default);

    /// <summary>Yeni kayıt oluşturur; yeni kimliği döndürür.</summary>
    Task<BaseResponse<Guid>> CreateAsync(CreateFinancialTransactionRequest request, CancellationToken ct = default);

    /// <summary>Var olan kaydı günceller.</summary>
    Task<BaseResponse<bool>> UpdateAsync(Guid id, UpdateFinancialTransactionRequest request, CancellationToken ct = default);

    /// <summary>Kaydı (gerekiyorsa soft-delete) siler.</summary>
    Task<BaseResponse<bool>> DeleteAsync(Guid id, CancellationToken ct = default);
}
