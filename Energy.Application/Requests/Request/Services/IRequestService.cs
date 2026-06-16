using Energy.Shared.Models.V1.Common.Responses;
using Energy.Shared.Models.V1.Requests.Request.Requests;
using Energy.Shared.Models.V1.Requests.Request.Responses;

namespace Energy.Application.Requests.Request.Services;

/// <summary>Request CRUD use-case sözleşmesi.</summary>
public interface IRequestService
{
    /// <summary>Sayfalanmış Request listesi.</summary>
    Task<BaseResponse<PaginatedResponse<RequestListResponse>>> GetListAsync(GetRequestListRequest request, CancellationToken ct = default);

    /// <summary>Kimliğe göre detay.</summary>
    Task<BaseResponse<RequestDetailResponse>> GetByIdAsync(Guid id, CancellationToken ct = default);

    /// <summary>Yeni kayıt oluşturur; yeni kimliği döndürür.</summary>
    Task<BaseResponse<Guid>> CreateAsync(CreateRequestRequest request, CancellationToken ct = default);

    /// <summary>Var olan kaydı günceller.</summary>
    Task<BaseResponse<bool>> UpdateAsync(Guid id, UpdateRequestRequest request, CancellationToken ct = default);

    /// <summary>Kaydı (gerekiyorsa soft-delete) siler.</summary>
    Task<BaseResponse<bool>> DeleteAsync(Guid id, CancellationToken ct = default);
}
