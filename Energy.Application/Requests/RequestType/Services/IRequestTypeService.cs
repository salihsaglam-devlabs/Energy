using Energy.Shared.Models.V1.Common.Responses;
using Energy.Shared.Models.V1.Requests.RequestType.Requests;
using Energy.Shared.Models.V1.Requests.RequestType.Responses;

namespace Energy.Application.Requests.RequestType.Services;

/// <summary>RequestType CRUD use-case sözleşmesi.</summary>
public interface IRequestTypeService
{
    /// <summary>Sayfalanmış RequestType listesi.</summary>
    Task<BaseResponse<PaginatedResponse<RequestTypeListResponse>>> GetListAsync(GetRequestTypeListRequest request, CancellationToken ct = default);

    /// <summary>Kimliğe göre detay.</summary>
    Task<BaseResponse<RequestTypeDetailResponse>> GetByIdAsync(Guid id, CancellationToken ct = default);

    /// <summary>Yeni kayıt oluşturur; yeni kimliği döndürür.</summary>
    Task<BaseResponse<Guid>> CreateAsync(CreateRequestTypeRequest request, CancellationToken ct = default);

    /// <summary>Var olan kaydı günceller.</summary>
    Task<BaseResponse<bool>> UpdateAsync(Guid id, UpdateRequestTypeRequest request, CancellationToken ct = default);

    /// <summary>Kaydı (gerekiyorsa soft-delete) siler.</summary>
    Task<BaseResponse<bool>> DeleteAsync(Guid id, CancellationToken ct = default);
}
