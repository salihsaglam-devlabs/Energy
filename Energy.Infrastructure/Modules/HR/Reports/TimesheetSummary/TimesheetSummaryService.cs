using Microsoft.EntityFrameworkCore;
using Energy.Infrastructure.Persistence;
using Energy.Shared.Models.V1.Common.Responses;
using Energy.Application.Modules.HR.Reports.TimesheetSummary.Services;
using Energy.Shared.Models.V1.HR.Reports.TimesheetSummary.Requests;
using Energy.Shared.Models.V1.HR.Reports.TimesheetSummary.Responses;

namespace Energy.Infrastructure.Modules.HR.Reports.TimesheetSummary;

/// <summary>TimesheetSummary raporu servisi (AsNoTracking, projection, filtre, sayfalama).</summary>
public sealed class TimesheetSummaryService : ITimesheetSummaryService
{
    private readonly EnergyDbContext _db;

    public TimesheetSummaryService(EnergyDbContext db) => _db = db;

    public async Task<BaseResponse<PaginatedResponse<TimesheetSummaryRowResponse>>> GetDataAsync(TimesheetSummaryRequest request, CancellationToken ct = default)
    {
        var query = _db.Timesheets.AsNoTracking();
        if (request.StartDate.HasValue) query = query.Where(e => e.PeriodStart >= request.StartDate.Value);
        if (request.EndDate.HasValue) query = query.Where(e => e.PeriodStart <= request.EndDate.Value);
        if (!string.IsNullOrWhiteSpace(request.Status)) query = query.Where(e => e.Status == request.Status);
        var total = await query.CountAsync(ct);
        var pageSize = request.PageSize <= 0 ? 50 : request.PageSize;
        var pageNumber = request.PageNumber <= 0 ? 1 : request.PageNumber;
        var items = await query
            .OrderByDescending(e => e.PeriodStart)
            .Skip((pageNumber - 1) * pageSize)
            .Take(pageSize)
            .Select(e => new TimesheetSummaryRowResponse
            {
                Id = e.Id,
                TimesheetNo = e.TimesheetNo,
                PeriodStart = e.PeriodStart,
                PeriodEnd = e.PeriodEnd,
                Status = e.Status
            })
            .ToListAsync(ct);
        var page = PaginatedResponse<TimesheetSummaryRowResponse>.Create(items, pageNumber, pageSize, total);
        return BaseResponse<PaginatedResponse<TimesheetSummaryRowResponse>>.Success(page);
    }
}
