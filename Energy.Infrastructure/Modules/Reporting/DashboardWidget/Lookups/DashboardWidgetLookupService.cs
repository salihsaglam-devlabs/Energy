using Microsoft.EntityFrameworkCore;
using Energy.Infrastructure.Persistence;
using Energy.Shared.Models.V1.Common.Responses;
using Energy.Application.Modules.Reporting.DashboardWidget.Lookups;
using Energy.Shared.Models.V1.Reporting.DashboardWidget.Responses;

namespace Energy.Infrastructure.Modules.Reporting.DashboardWidget.Lookups;

/// <summary>DashboardWidget lookup servisi (aktif + arama filtreli projection).</summary>
public class DashboardWidgetLookupService : IDashboardWidgetLookupService
{
    private readonly AppDbContext _db;

    public DashboardWidgetLookupService(AppDbContext db) => _db = db;

    public async Task<BaseResponse<IReadOnlyList<DashboardWidgetLookupResponse>>> GetLookupAsync(string? search = null, bool activeOnly = true, CancellationToken ct = default)
    {
        var query = _db.DashboardWidgets.AsNoTracking();
        if (activeOnly) query = query.Where(e => e.IsActive);
        if (!string.IsNullOrWhiteSpace(search)) query = query.Where(e => e.Name.Contains(search) || e.Code.Contains(search));
        var items = await query
            .OrderBy(e => e.Name)
            .Select(e => new DashboardWidgetLookupResponse
            {
                Id = e.Id,
                Code = e.Code,
                Name = e.Name,
                DisplayName = e.Code + " - " + e.Name,
                IsActive = e.IsActive
            })
            .ToListAsync(ct);
        return BaseResponse<IReadOnlyList<DashboardWidgetLookupResponse>>.Success(items);
    }
}
