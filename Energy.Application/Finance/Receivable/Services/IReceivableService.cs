using Energy.Shared.Models.V1.Common.Responses;
using Energy.Shared.Models.V1.Finance.Receivable.Requests;
using Energy.Shared.Models.V1.Finance.Receivable.Responses;

namespace Energy.Application.Finance.Receivable.Services;

/// <summary>Receivable CRUD use-case sözleşmesi.</summary>
public interface IReceivableService
{
    /// <summary>Sayfalanmış Receivable listesi.</summary>
    Task<BaseResponse<PaginatedResponse<ReceivableListResponse>>> GetListAsync(GetReceivableListRequest request, CancellationToken ct = default);

    /// <summary>Kimliğe göre detay.</summary>
    Task<BaseResponse<ReceivableDetailResponse>> GetByIdAsync(Guid id, CancellationToken ct = default);

    /// <summary>Yeni kayıt oluşturur; yeni kimliği döndürür.</summary>
    Task<BaseResponse<Guid>> CreateAsync(CreateReceivableRequest request, CancellationToken ct = default);

    /// <summary>Var olan kaydı günceller.</summary>
    Task<BaseResponse<bool>> UpdateAsync(Guid id, UpdateReceivableRequest request, CancellationToken ct = default);

    /// <summary>Kaydı (gerekiyorsa soft-delete) siler.</summary>
    Task<BaseResponse<bool>> DeleteAsync(Guid id, CancellationToken ct = default);
}
