using Microsoft.EntityFrameworkCore;
using Energy.Infrastructure.Persistence;
using Energy.Shared.Models.V1.Common.Responses;
using Energy.Application.Modules.FieldOperations.DailySiteReport.Lookups;
using Energy.Shared.Models.V1.FieldOperations.DailySiteReport.Responses;

namespace Energy.Infrastructure.Modules.FieldOperations.DailySiteReport.Lookups;

/// <summary>DailySiteReport lookup servisi (aktif + arama filtreli projection).</summary>
public class DailySiteReportLookupService : IDailySiteReportLookupService
{
    private readonly EnergyDbContext _db;

    public DailySiteReportLookupService(EnergyDbContext db) => _db = db;

    public async Task<BaseResponse<IReadOnlyList<DailySiteReportLookupResponse>>> GetLookupAsync(string? search = null, bool activeOnly = true, CancellationToken ct = default)
    {
        var query = _db.DailySiteReports.AsNoTracking();
        var items = await query.Select(e => new DailySiteReportLookupResponse
        {
            Id = e.Id,
            Code = null,
            Name = null,
            DisplayName = e.Id.ToString(),
            IsActive = true
        }).ToListAsync(ct);
        return BaseResponse<IReadOnlyList<DailySiteReportLookupResponse>>.Success(items);
    }
}
