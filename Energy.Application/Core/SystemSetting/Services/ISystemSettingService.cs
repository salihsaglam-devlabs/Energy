using Energy.Shared.Models.V1.Common.Responses;
using Energy.Shared.Models.V1.Core.SystemSetting.Requests;
using Energy.Shared.Models.V1.Core.SystemSetting.Responses;

namespace Energy.Application.Core.SystemSetting.Services;

/// <summary>SystemSetting CRUD use-case sözleşmesi.</summary>
public interface ISystemSettingService
{
    /// <summary>Sayfalanmış SystemSetting listesi.</summary>
    Task<BaseResponse<PaginatedResponse<SystemSettingListResponse>>> GetListAsync(GetSystemSettingListRequest request, CancellationToken ct = default);

    /// <summary>Kimliğe göre detay.</summary>
    Task<BaseResponse<SystemSettingDetailResponse>> GetByIdAsync(Guid id, CancellationToken ct = default);

    /// <summary>Yeni kayıt oluşturur; yeni kimliği döndürür.</summary>
    Task<BaseResponse<Guid>> CreateAsync(CreateSystemSettingRequest request, CancellationToken ct = default);

    /// <summary>Var olan kaydı günceller.</summary>
    Task<BaseResponse<bool>> UpdateAsync(Guid id, UpdateSystemSettingRequest request, CancellationToken ct = default);

    /// <summary>Kaydı (gerekiyorsa soft-delete) siler.</summary>
    Task<BaseResponse<bool>> DeleteAsync(Guid id, CancellationToken ct = default);
}
