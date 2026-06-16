using Energy.Shared.Models.V1.Common.Responses;
using Energy.Shared.Models.V1.ProgressPayments.ProgressPaymentDeduction.Requests;
using Energy.Shared.Models.V1.ProgressPayments.ProgressPaymentDeduction.Responses;

namespace Energy.Application.Modules.ProgressPayments.ProgressPaymentDeduction.Services;

/// <summary>ProgressPaymentDeduction CRUD use-case sözleşmesi.</summary>
public interface IProgressPaymentDeductionService
{
    /// <summary>Sayfalanmış ProgressPaymentDeduction listesi.</summary>
    Task<BaseResponse<PaginatedResponse<ProgressPaymentDeductionListResponse>>> GetListAsync(GetProgressPaymentDeductionListRequest request, CancellationToken ct = default);

    /// <summary>Kimliğe göre detay.</summary>
    Task<BaseResponse<ProgressPaymentDeductionDetailResponse>> GetByIdAsync(Guid id, CancellationToken ct = default);

    /// <summary>Yeni kayıt oluşturur; yeni kimliği döndürür.</summary>
    Task<BaseResponse<Guid>> CreateAsync(CreateProgressPaymentDeductionRequest request, CancellationToken ct = default);

    /// <summary>Var olan kaydı günceller.</summary>
    Task<BaseResponse<bool>> UpdateAsync(Guid id, UpdateProgressPaymentDeductionRequest request, CancellationToken ct = default);

    /// <summary>Kaydı (gerekiyorsa soft-delete) siler.</summary>
    Task<BaseResponse<bool>> DeleteAsync(Guid id, CancellationToken ct = default);
}
