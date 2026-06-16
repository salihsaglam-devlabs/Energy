using Energy.Shared.Models.V1.Common.Responses;
using Energy.Shared.Models.V1.IAM.UserSetting.Responses;

namespace Energy.Application.Modules.IAM.UserSetting.Lookups;

/// <summary>UserSetting lookup sözleşmesi (aktif kayıt + arama filtreli).</summary>
public interface IUserSettingLookupService
{
    /// <summary>UserSetting lookup listesi döndürür.</summary>
    Task<BaseResponse<IReadOnlyList<UserSettingLookupResponse>>> GetLookupAsync(string? search = null, bool activeOnly = true, CancellationToken ct = default);
}
