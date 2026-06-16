using Energy.Shared.Models.V1.Common.Responses;
using Energy.Shared.Models.V1.FieldOperations.MeasurementSheetLine.Responses;

namespace Energy.Application.FieldOperations.MeasurementSheetLine.Lookups;

/// <summary>MeasurementSheetLine lookup sözleşmesi (aktif kayıt + arama filtreli).</summary>
public interface IMeasurementSheetLineLookupService
{
    /// <summary>MeasurementSheetLine lookup listesi döndürür.</summary>
    Task<BaseResponse<IReadOnlyList<MeasurementSheetLineLookupResponse>>> GetLookupAsync(string? search = null, bool activeOnly = true, CancellationToken ct = default);
}
