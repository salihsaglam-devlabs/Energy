using Energy.Shared.Models.V1.Common.Responses;
using Energy.Shared.Models.V1.Core.UnitConversion.Responses;

namespace Energy.Application.Modules.Core.UnitConversion.Lookups;

/// <summary>UnitConversion lookup sözleşmesi (aktif kayıt + arama filtreli).</summary>
public interface IUnitConversionLookupService
{
    /// <summary>UnitConversion lookup listesi döndürür.</summary>
    Task<BaseResponse<IReadOnlyList<UnitConversionLookupResponse>>> GetLookupAsync(string? search = null, bool activeOnly = true, CancellationToken ct = default);
}
