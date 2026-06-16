using Energy.Shared.Models.V1.Common.Responses;
using Energy.Shared.Models.V1.ProgressPayments.ProgressPayment.Requests;
using Energy.Shared.Models.V1.ProgressPayments.ProgressPayment.Responses;

namespace Energy.Application.Modules.ProgressPayments.ProgressPayment.Services;

/// <summary>ProgressPayment CRUD use-case sözleşmesi.</summary>
public interface IProgressPaymentService
{
    /// <summary>Sayfalanmış ProgressPayment listesi.</summary>
    Task<BaseResponse<PaginatedResponse<ProgressPaymentListResponse>>> GetListAsync(GetProgressPaymentListRequest request, CancellationToken ct = default);

    /// <summary>Kimliğe göre detay.</summary>
    Task<BaseResponse<ProgressPaymentDetailResponse>> GetByIdAsync(Guid id, CancellationToken ct = default);

    /// <summary>Yeni kayıt oluşturur; yeni kimliği döndürür.</summary>
    Task<BaseResponse<Guid>> CreateAsync(CreateProgressPaymentRequest request, CancellationToken ct = default);

    /// <summary>Var olan kaydı günceller.</summary>
    Task<BaseResponse<bool>> UpdateAsync(Guid id, UpdateProgressPaymentRequest request, CancellationToken ct = default);

    /// <summary>Kaydı (gerekiyorsa soft-delete) siler.</summary>
    Task<BaseResponse<bool>> DeleteAsync(Guid id, CancellationToken ct = default);
}
