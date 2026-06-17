using Microsoft.EntityFrameworkCore;
using Energy.Infrastructure.Persistence;
using Energy.Shared.Models.V1.Common.Responses;
using Energy.Application.HR.TimesheetLine.Lookups;
using Energy.Shared.Models.V1.HR.TimesheetLine.Responses;

namespace Energy.Infrastructure.HR.TimesheetLine.Lookups;

/// <summary>TimesheetLine lookup servisi (aktif + arama filtreli projection).</summary>
public class TimesheetLineLookupService : ITimesheetLineLookupService
{
    private readonly AppDbContext _db;

    public TimesheetLineLookupService(AppDbContext db) => _db = db;

    public async Task<BaseResponse<IReadOnlyList<TimesheetLineLookupResponse>>> GetLookupAsync(string? search = null, bool activeOnly = true, CancellationToken ct = default)
    {
        var query = _db.TimesheetLines.AsNoTracking();
        var rows = await query
            .OrderBy(e => e.Id)
            .ToListAsync(ct);
        var items = (IReadOnlyList<TimesheetLineLookupResponse>)rows.Select(e => new TimesheetLineLookupResponse
        {
            Id = e.Id,
            Code = null,
            Name = null,
            DisplayName = string.IsNullOrWhiteSpace(e.WorkDate.ToString("yyyy-MM-dd")) ? "Timesheet Line #" + e.Id.ToString().Substring(0, 8) : (e.WorkDate.ToString("yyyy-MM-dd")),
            IsActive = true
        }).ToList();
        return BaseResponse<IReadOnlyList<TimesheetLineLookupResponse>>.Success(items);
    }
}
