using Energy.Shared.Models.V1.Common.Responses;
using Energy.Shared.Models.V1.Operations.WorkOrderChecklistItem.Responses;

namespace Energy.Application.Modules.Operations.WorkOrderChecklistItem.Lookups;

/// <summary>WorkOrderChecklistItem lookup sözleşmesi (aktif kayıt + arama filtreli).</summary>
public interface IWorkOrderChecklistItemLookupService
{
    /// <summary>WorkOrderChecklistItem lookup listesi döndürür.</summary>
    Task<BaseResponse<IReadOnlyList<WorkOrderChecklistItemLookupResponse>>> GetLookupAsync(string? search = null, bool activeOnly = true, CancellationToken ct = default);
}
