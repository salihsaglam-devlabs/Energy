using Microsoft.EntityFrameworkCore;
using Energy.Infrastructure.Persistence;
using Energy.Shared.Models.V1.Common.Responses;
using Energy.Application.FieldOperations.DailySiteReportWorker.Lookups;
using Energy.Shared.Models.V1.FieldOperations.DailySiteReportWorker.Responses;

namespace Energy.Infrastructure.FieldOperations.DailySiteReportWorker.Lookups;

/// <summary>DailySiteReportWorker lookup servisi (aktif + arama filtreli projection).</summary>
public class DailySiteReportWorkerLookupService : IDailySiteReportWorkerLookupService
{
    private readonly AppDbContext _db;

    public DailySiteReportWorkerLookupService(AppDbContext db) => _db = db;

    public async Task<BaseResponse<IReadOnlyList<DailySiteReportWorkerLookupResponse>>> GetLookupAsync(string? search = null, bool activeOnly = true, CancellationToken ct = default)
    {
        var query = _db.DailySiteReportWorkers.AsNoTracking();
        var items = await query
            .OrderBy(e => e.Id)
            .Select(e => new DailySiteReportWorkerLookupResponse
            {
                Id = e.Id,
                Code = null,
                Name = null,
                DisplayName = e.Id.ToString(),
                IsActive = true
            })
            .ToListAsync(ct);
        return BaseResponse<IReadOnlyList<DailySiteReportWorkerLookupResponse>>.Success(items);
    }
}
