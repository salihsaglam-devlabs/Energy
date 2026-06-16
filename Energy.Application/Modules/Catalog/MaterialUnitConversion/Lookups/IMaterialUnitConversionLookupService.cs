using Energy.Shared.Models.V1.Common.Responses;
using Energy.Shared.Models.V1.Catalog.MaterialUnitConversion.Responses;

namespace Energy.Application.Modules.Catalog.MaterialUnitConversion.Lookups;

/// <summary>MaterialUnitConversion lookup sözleşmesi (aktif kayıt + arama filtreli).</summary>
public interface IMaterialUnitConversionLookupService
{
    /// <summary>MaterialUnitConversion lookup listesi döndürür.</summary>
    Task<BaseResponse<IReadOnlyList<MaterialUnitConversionLookupResponse>>> GetLookupAsync(string? search = null, bool activeOnly = true, CancellationToken ct = default);
}
