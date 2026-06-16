using Energy.Shared.Models.V1.Common.Responses;
using Energy.Shared.Models.V1.Core.UnitOfMeasure.Responses;

namespace Energy.Application.Core.UnitOfMeasure.Lookups;

/// <summary>UnitOfMeasure lookup sözleşmesi (aktif kayıt + arama filtreli).</summary>
public interface IUnitOfMeasureLookupService
{
    /// <summary>UnitOfMeasure lookup listesi döndürür.</summary>
    Task<BaseResponse<IReadOnlyList<UnitOfMeasureLookupResponse>>> GetLookupAsync(string? search = null, bool activeOnly = true, CancellationToken ct = default);
}
