using Energy.Shared.Models.V1.Common.Responses;
using Energy.Shared.Models.V1.Core.LocalizationResource.Requests;
using Energy.Shared.Models.V1.Core.LocalizationResource.Responses;

namespace Energy.Application.Core.LocalizationResource.Services;

/// <summary>LocalizationResource CRUD use-case sözleşmesi.</summary>
public interface ILocalizationResourceService
{
    /// <summary>Sayfalanmış LocalizationResource listesi.</summary>
    Task<BaseResponse<PaginatedResponse<LocalizationResourceListResponse>>> GetListAsync(GetLocalizationResourceListRequest request, CancellationToken ct = default);

    /// <summary>Kimliğe göre detay.</summary>
    Task<BaseResponse<LocalizationResourceDetailResponse>> GetByIdAsync(Guid id, CancellationToken ct = default);

    /// <summary>Yeni kayıt oluşturur; yeni kimliği döndürür.</summary>
    Task<BaseResponse<Guid>> CreateAsync(CreateLocalizationResourceRequest request, CancellationToken ct = default);

    /// <summary>Var olan kaydı günceller.</summary>
    Task<BaseResponse<bool>> UpdateAsync(Guid id, UpdateLocalizationResourceRequest request, CancellationToken ct = default);

    /// <summary>Kaydı (gerekiyorsa soft-delete) siler.</summary>
    Task<BaseResponse<bool>> DeleteAsync(Guid id, CancellationToken ct = default);
}
