using Energy.Shared.Models.V1.Common.Responses;
using Energy.Shared.Models.V1.Requests.RequestLine.Requests;
using Energy.Shared.Models.V1.Requests.RequestLine.Responses;

namespace Energy.Application.Modules.Requests.RequestLine.Services;

/// <summary>RequestLine CRUD use-case sözleşmesi.</summary>
public interface IRequestLineService
{
    /// <summary>Sayfalanmış RequestLine listesi.</summary>
    Task<BaseResponse<PaginatedResponse<RequestLineListResponse>>> GetListAsync(GetRequestLineListRequest request, CancellationToken ct = default);

    /// <summary>Kimliğe göre detay.</summary>
    Task<BaseResponse<RequestLineDetailResponse>> GetByIdAsync(Guid id, CancellationToken ct = default);

    /// <summary>Yeni kayıt oluşturur; yeni kimliği döndürür.</summary>
    Task<BaseResponse<Guid>> CreateAsync(CreateRequestLineRequest request, CancellationToken ct = default);

    /// <summary>Var olan kaydı günceller.</summary>
    Task<BaseResponse<bool>> UpdateAsync(Guid id, UpdateRequestLineRequest request, CancellationToken ct = default);

    /// <summary>Kaydı (gerekiyorsa soft-delete) siler.</summary>
    Task<BaseResponse<bool>> DeleteAsync(Guid id, CancellationToken ct = default);
}
