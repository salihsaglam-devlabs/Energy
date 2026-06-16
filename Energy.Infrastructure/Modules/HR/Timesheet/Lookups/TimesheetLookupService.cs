using Microsoft.EntityFrameworkCore;
using Energy.Infrastructure.Persistence;
using Energy.Shared.Models.V1.Common.Responses;
using Energy.Application.Modules.HR.Timesheet.Lookups;
using Energy.Shared.Models.V1.HR.Timesheet.Responses;

namespace Energy.Infrastructure.Modules.HR.Timesheet.Lookups;

/// <summary>Timesheet lookup servisi (aktif + arama filtreli projection).</summary>
public class TimesheetLookupService : ITimesheetLookupService
{
    private readonly EnergyDbContext _db;

    public TimesheetLookupService(EnergyDbContext db) => _db = db;

    public async Task<BaseResponse<IReadOnlyList<TimesheetLookupResponse>>> GetLookupAsync(string? search = null, bool activeOnly = true, CancellationToken ct = default)
    {
        var query = _db.Timesheets.AsNoTracking();
        var items = await query.Select(e => new TimesheetLookupResponse
        {
            Id = e.Id,
            Code = null,
            Name = null,
            DisplayName = e.Id.ToString(),
            IsActive = true
        }).ToListAsync(ct);
        return BaseResponse<IReadOnlyList<TimesheetLookupResponse>>.Success(items);
    }
}
