using Energy.Shared.Models.V1.Common.Responses;
using Energy.Shared.Models.V1.Operations.WorkOrderChecklist.Responses;

namespace Energy.Application.Modules.Operations.WorkOrderChecklist.Lookups;

/// <summary>WorkOrderChecklist lookup sözleşmesi (aktif kayıt + arama filtreli).</summary>
public interface IWorkOrderChecklistLookupService
{
    /// <summary>WorkOrderChecklist lookup listesi döndürür.</summary>
    Task<BaseResponse<IReadOnlyList<WorkOrderChecklistLookupResponse>>> GetLookupAsync(string? search = null, bool activeOnly = true, CancellationToken ct = default);
}
