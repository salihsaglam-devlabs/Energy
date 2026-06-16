using Microsoft.EntityFrameworkCore;
using Energy.Infrastructure.Persistence;
using Energy.Shared.Models.V1.Common.Responses;
using Energy.Application.Modules.FieldOperations.DailySiteReport.Lookups;
using Energy.Shared.Models.V1.FieldOperations.DailySiteReport.Responses;

namespace Energy.Infrastructure.Modules.FieldOperations.DailySiteReport.Lookups;

/// <summary>DailySiteReport lookup servisi (aktif + arama filtreli projection).</summary>
public class DailySiteReportLookupService : IDailySiteReportLookupService
{
    private readonly AppDbContext _db;

    public DailySiteReportLookupService(AppDbContext db) => _db = db;

    public async Task<BaseResponse<IReadOnlyList<DailySiteReportLookupResponse>>> GetLookupAsync(string? search = null, bool activeOnly = true, CancellationToken ct = default)
    {
        var query = _db.DailySiteReports.AsNoTracking();
        if (!string.IsNullOrWhiteSpace(search)) query = query.Where(e => e.ReportNo.Contains(search));
        var items = await query
            .OrderBy(e => e.ReportNo)
            .Select(e => new DailySiteReportLookupResponse
            {
                Id = e.Id,
                Code = null,
                Name = e.ReportNo,
                DisplayName = e.ReportNo,
                IsActive = true
            })
            .ToListAsync(ct);
        return BaseResponse<IReadOnlyList<DailySiteReportLookupResponse>>.Success(items);
    }
}
