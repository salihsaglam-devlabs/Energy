using Energy.Shared.Models.V1.Common.Responses;
using Energy.Shared.Models.V1.Operations.WorkOrderType.Responses;

namespace Energy.Application.Operations.WorkOrderType.Lookups;

/// <summary>WorkOrderType lookup sözleşmesi (aktif kayıt + arama filtreli).</summary>
public interface IWorkOrderTypeLookupService
{
    /// <summary>WorkOrderType lookup listesi döndürür.</summary>
    Task<BaseResponse<IReadOnlyList<WorkOrderTypeLookupResponse>>> GetLookupAsync(string? search = null, bool activeOnly = true, CancellationToken ct = default);
}
