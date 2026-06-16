using Energy.Shared.Models.V1.Common.Responses;
using Energy.Shared.Models.V1.Operations.WorkOrderMaterialUsage.Responses;

namespace Energy.Application.Operations.WorkOrderMaterialUsage.Lookups;

/// <summary>WorkOrderMaterialUsage lookup sözleşmesi (aktif kayıt + arama filtreli).</summary>
public interface IWorkOrderMaterialUsageLookupService
{
    /// <summary>WorkOrderMaterialUsage lookup listesi döndürür.</summary>
    Task<BaseResponse<IReadOnlyList<WorkOrderMaterialUsageLookupResponse>>> GetLookupAsync(string? search = null, bool activeOnly = true, CancellationToken ct = default);
}
