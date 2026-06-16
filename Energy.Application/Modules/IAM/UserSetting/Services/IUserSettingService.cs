using Energy.Shared.Models.V1.Common.Responses;
using Energy.Shared.Models.V1.IAM.UserSetting.Requests;
using Energy.Shared.Models.V1.IAM.UserSetting.Responses;

namespace Energy.Application.Modules.IAM.UserSetting.Services;

/// <summary>UserSetting CRUD use-case sözleşmesi.</summary>
public interface IUserSettingService
{
    /// <summary>Sayfalanmış UserSetting listesi.</summary>
    Task<BaseResponse<PaginatedResponse<UserSettingListResponse>>> GetListAsync(GetUserSettingListRequest request, CancellationToken ct = default);

    /// <summary>Kimliğe göre detay.</summary>
    Task<BaseResponse<UserSettingDetailResponse>> GetByIdAsync(Guid id, CancellationToken ct = default);

    /// <summary>Yeni kayıt oluşturur; yeni kimliği döndürür.</summary>
    Task<BaseResponse<Guid>> CreateAsync(CreateUserSettingRequest request, CancellationToken ct = default);

    /// <summary>Var olan kaydı günceller.</summary>
    Task<BaseResponse<bool>> UpdateAsync(Guid id, UpdateUserSettingRequest request, CancellationToken ct = default);

    /// <summary>Kaydı (gerekiyorsa soft-delete) siler.</summary>
    Task<BaseResponse<bool>> DeleteAsync(Guid id, CancellationToken ct = default);
}
