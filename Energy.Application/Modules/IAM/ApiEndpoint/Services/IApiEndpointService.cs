using Energy.Shared.Models.V1.Common.Responses;
using Energy.Shared.Models.V1.IAM.ApiEndpoint.Requests;
using Energy.Shared.Models.V1.IAM.ApiEndpoint.Responses;

namespace Energy.Application.Modules.IAM.ApiEndpoint.Services;

/// <summary>ApiEndpoint CRUD use-case sözleşmesi.</summary>
public interface IApiEndpointService
{
    /// <summary>Sayfalanmış ApiEndpoint listesi.</summary>
    Task<BaseResponse<PaginatedResponse<ApiEndpointListResponse>>> GetListAsync(GetApiEndpointListRequest request, CancellationToken ct = default);

    /// <summary>Kimliğe göre detay.</summary>
    Task<BaseResponse<ApiEndpointDetailResponse>> GetByIdAsync(Guid id, CancellationToken ct = default);

    /// <summary>Yeni kayıt oluşturur; yeni kimliği döndürür.</summary>
    Task<BaseResponse<Guid>> CreateAsync(CreateApiEndpointRequest request, CancellationToken ct = default);

    /// <summary>Var olan kaydı günceller.</summary>
    Task<BaseResponse<bool>> UpdateAsync(Guid id, UpdateApiEndpointRequest request, CancellationToken ct = default);

    /// <summary>Kaydı (gerekiyorsa soft-delete) siler.</summary>
    Task<BaseResponse<bool>> DeleteAsync(Guid id, CancellationToken ct = default);
}
