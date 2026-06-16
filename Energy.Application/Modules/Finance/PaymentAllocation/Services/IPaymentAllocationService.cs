using Energy.Shared.Models.V1.Common.Responses;
using Energy.Shared.Models.V1.Finance.PaymentAllocation.Requests;
using Energy.Shared.Models.V1.Finance.PaymentAllocation.Responses;

namespace Energy.Application.Modules.Finance.PaymentAllocation.Services;

/// <summary>PaymentAllocation CRUD use-case sözleşmesi.</summary>
public interface IPaymentAllocationService
{
    /// <summary>Sayfalanmış PaymentAllocation listesi.</summary>
    Task<BaseResponse<PaginatedResponse<PaymentAllocationListResponse>>> GetListAsync(GetPaymentAllocationListRequest request, CancellationToken ct = default);

    /// <summary>Kimliğe göre detay.</summary>
    Task<BaseResponse<PaymentAllocationDetailResponse>> GetByIdAsync(Guid id, CancellationToken ct = default);

    /// <summary>Yeni kayıt oluşturur; yeni kimliği döndürür.</summary>
    Task<BaseResponse<Guid>> CreateAsync(CreatePaymentAllocationRequest request, CancellationToken ct = default);

    /// <summary>Var olan kaydı günceller.</summary>
    Task<BaseResponse<bool>> UpdateAsync(Guid id, UpdatePaymentAllocationRequest request, CancellationToken ct = default);

    /// <summary>Kaydı (gerekiyorsa soft-delete) siler.</summary>
    Task<BaseResponse<bool>> DeleteAsync(Guid id, CancellationToken ct = default);
}
