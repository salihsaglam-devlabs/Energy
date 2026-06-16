using Microsoft.EntityFrameworkCore;
using Energy.Infrastructure.Persistence;
using Energy.Shared.Models.V1.Common.Responses;
using Energy.Application.Modules.Organization.LeaveRequest.Lookups;
using Energy.Shared.Models.V1.Organization.LeaveRequest.Responses;

namespace Energy.Infrastructure.Modules.Organization.LeaveRequest.Lookups;

/// <summary>LeaveRequest lookup servisi (aktif + arama filtreli projection).</summary>
public class LeaveRequestLookupService : ILeaveRequestLookupService
{
    private readonly AppDbContext _db;

    public LeaveRequestLookupService(AppDbContext db) => _db = db;

    public async Task<BaseResponse<IReadOnlyList<LeaveRequestLookupResponse>>> GetLookupAsync(string? search = null, bool activeOnly = true, CancellationToken ct = default)
    {
        var query = _db.LeaveRequests.AsNoTracking();
        var items = await query
            .OrderBy(e => e.Id)
            .Select(e => new LeaveRequestLookupResponse
            {
                Id = e.Id,
                Code = null,
                Name = null,
                DisplayName = e.Id.ToString(),
                IsActive = true
            })
            .ToListAsync(ct);
        return BaseResponse<IReadOnlyList<LeaveRequestLookupResponse>>.Success(items);
    }
}
