using Microsoft.EntityFrameworkCore;
using Energy.Infrastructure.Persistence;
using Energy.Shared.Models.V1.Common.Responses;
using Energy.Application.Organization.LeaveRequest.Lookups;
using Energy.Shared.Models.V1.Organization.LeaveRequest.Responses;

namespace Energy.Infrastructure.Organization.LeaveRequest.Lookups;

/// <summary>LeaveRequest lookup servisi (aktif + arama filtreli projection).</summary>
public class LeaveRequestLookupService : ILeaveRequestLookupService
{
    private readonly AppDbContext _db;

    public LeaveRequestLookupService(AppDbContext db) => _db = db;

    public async Task<BaseResponse<IReadOnlyList<LeaveRequestLookupResponse>>> GetLookupAsync(string? search = null, bool activeOnly = true, CancellationToken ct = default)
    {
        var query = _db.LeaveRequests.AsNoTracking();
        var rows = await query
            .OrderBy(e => e.Id)
            .ToListAsync(ct);
        var items = (IReadOnlyList<LeaveRequestLookupResponse>)rows.Select(e => new LeaveRequestLookupResponse
        {
            Id = e.Id,
            Code = null,
            Name = null,
            DisplayName = string.IsNullOrWhiteSpace((e.LeaveType ?? "") + " - " + e.Status.ToString()) ? "Leave Request #" + e.Id.ToString().Substring(0, 8) : ((e.LeaveType ?? "") + " - " + e.Status.ToString()),
            IsActive = true
        }).ToList();
        return BaseResponse<IReadOnlyList<LeaveRequestLookupResponse>>.Success(items);
    }
}
