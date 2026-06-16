using Energy.Shared.Models.V1.Common.Responses;
using Energy.Shared.Models.V1.Operations.WorkOrderAssignment.Responses;

namespace Energy.Application.Modules.Operations.WorkOrderAssignment.Lookups;

/// <summary>WorkOrderAssignment lookup sözleşmesi (aktif kayıt + arama filtreli).</summary>
public interface IWorkOrderAssignmentLookupService
{
    /// <summary>WorkOrderAssignment lookup listesi döndürür.</summary>
    Task<BaseResponse<IReadOnlyList<WorkOrderAssignmentLookupResponse>>> GetLookupAsync(string? search = null, bool activeOnly = true, CancellationToken ct = default);
}
