using Energy.Shared.Models.V1.Common.Responses;
using Energy.Shared.Models.V1.Core.SystemSetting.Responses;

namespace Energy.Application.Core.SystemSetting.Lookups;

/// <summary>SystemSetting lookup sözleşmesi (aktif kayıt + arama filtreli).</summary>
public interface ISystemSettingLookupService
{
    /// <summary>SystemSetting lookup listesi döndürür.</summary>
    Task<BaseResponse<IReadOnlyList<SystemSettingLookupResponse>>> GetLookupAsync(string? search = null, bool activeOnly = true, CancellationToken ct = default);
}
