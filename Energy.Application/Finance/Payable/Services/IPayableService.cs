using Energy.Shared.Models.V1.Common.Responses;
using Energy.Shared.Models.V1.Finance.Payable.Requests;
using Energy.Shared.Models.V1.Finance.Payable.Responses;

namespace Energy.Application.Finance.Payable.Services;

/// <summary>Payable CRUD use-case sözleşmesi.</summary>
public interface IPayableService
{
    /// <summary>Sayfalanmış Payable listesi.</summary>
    Task<BaseResponse<PaginatedResponse<PayableListResponse>>> GetListAsync(GetPayableListRequest request, CancellationToken ct = default);

    /// <summary>Kimliğe göre detay.</summary>
    Task<BaseResponse<PayableDetailResponse>> GetByIdAsync(Guid id, CancellationToken ct = default);

    /// <summary>Yeni kayıt oluşturur; yeni kimliği döndürür.</summary>
    Task<BaseResponse<Guid>> CreateAsync(CreatePayableRequest request, CancellationToken ct = default);

    /// <summary>Var olan kaydı günceller.</summary>
    Task<BaseResponse<bool>> UpdateAsync(Guid id, UpdatePayableRequest request, CancellationToken ct = default);

    /// <summary>Kaydı (gerekiyorsa soft-delete) siler.</summary>
    Task<BaseResponse<bool>> DeleteAsync(Guid id, CancellationToken ct = default);
}
