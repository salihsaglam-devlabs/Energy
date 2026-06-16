using Energy.Shared.Models.V1.Common.Responses;
using Energy.Shared.Models.V1.Operations.WorkOrderMaterialPlan.Responses;

namespace Energy.Application.Operations.WorkOrderMaterialPlan.Lookups;

/// <summary>WorkOrderMaterialPlan lookup sözleşmesi (aktif kayıt + arama filtreli).</summary>
public interface IWorkOrderMaterialPlanLookupService
{
    /// <summary>WorkOrderMaterialPlan lookup listesi döndürür.</summary>
    Task<BaseResponse<IReadOnlyList<WorkOrderMaterialPlanLookupResponse>>> GetLookupAsync(string? search = null, bool activeOnly = true, CancellationToken ct = default);
}
