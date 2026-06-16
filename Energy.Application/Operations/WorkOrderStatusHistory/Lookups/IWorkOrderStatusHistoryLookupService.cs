using Energy.Shared.Models.V1.Common.Responses;
using Energy.Shared.Models.V1.Operations.WorkOrderStatusHistory.Responses;

namespace Energy.Application.Operations.WorkOrderStatusHistory.Lookups;

/// <summary>WorkOrderStatusHistory lookup sözleşmesi (aktif kayıt + arama filtreli).</summary>
public interface IWorkOrderStatusHistoryLookupService
{
    /// <summary>WorkOrderStatusHistory lookup listesi döndürür.</summary>
    Task<BaseResponse<IReadOnlyList<WorkOrderStatusHistoryLookupResponse>>> GetLookupAsync(string? search = null, bool activeOnly = true, CancellationToken ct = default);
}
