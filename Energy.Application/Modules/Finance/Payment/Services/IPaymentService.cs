using Energy.Shared.Models.V1.Common.Responses;
using Energy.Shared.Models.V1.Finance.Payment.Requests;
using Energy.Shared.Models.V1.Finance.Payment.Responses;

namespace Energy.Application.Modules.Finance.Payment.Services;

/// <summary>Payment CRUD use-case sözleşmesi.</summary>
public interface IPaymentService
{
    /// <summary>Sayfalanmış Payment listesi.</summary>
    Task<BaseResponse<PaginatedResponse<PaymentListResponse>>> GetListAsync(GetPaymentListRequest request, CancellationToken ct = default);

    /// <summary>Kimliğe göre detay.</summary>
    Task<BaseResponse<PaymentDetailResponse>> GetByIdAsync(Guid id, CancellationToken ct = default);

    /// <summary>Yeni kayıt oluşturur; yeni kimliği döndürür.</summary>
    Task<BaseResponse<Guid>> CreateAsync(CreatePaymentRequest request, CancellationToken ct = default);

    /// <summary>Var olan kaydı günceller.</summary>
    Task<BaseResponse<bool>> UpdateAsync(Guid id, UpdatePaymentRequest request, CancellationToken ct = default);

    /// <summary>Kaydı (gerekiyorsa soft-delete) siler.</summary>
    Task<BaseResponse<bool>> DeleteAsync(Guid id, CancellationToken ct = default);
}
