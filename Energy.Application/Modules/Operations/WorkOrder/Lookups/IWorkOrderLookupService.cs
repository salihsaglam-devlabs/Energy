using Energy.Shared.Models.V1.Common.Responses;
using Energy.Shared.Models.V1.Operations.WorkOrder.Responses;

namespace Energy.Application.Modules.Operations.WorkOrder.Lookups;

/// <summary>WorkOrder lookup sözleşmesi (aktif kayıt + arama filtreli).</summary>
public interface IWorkOrderLookupService
{
    /// <summary>WorkOrder lookup listesi döndürür.</summary>
    Task<BaseResponse<IReadOnlyList<WorkOrderLookupResponse>>> GetLookupAsync(string? search = null, bool activeOnly = true, CancellationToken ct = default);
}
