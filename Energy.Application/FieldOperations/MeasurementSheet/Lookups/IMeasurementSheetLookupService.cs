using Energy.Shared.Models.V1.Common.Responses;
using Energy.Shared.Models.V1.FieldOperations.MeasurementSheet.Responses;

namespace Energy.Application.FieldOperations.MeasurementSheet.Lookups;

/// <summary>MeasurementSheet lookup sözleşmesi (aktif kayıt + arama filtreli).</summary>
public interface IMeasurementSheetLookupService
{
    /// <summary>MeasurementSheet lookup listesi döndürür.</summary>
    Task<BaseResponse<IReadOnlyList<MeasurementSheetLookupResponse>>> GetLookupAsync(string? search = null, bool activeOnly = true, CancellationToken ct = default);
}
