using Energy.Application.Home.Services;
using Energy.Infrastructure.Persistence;
using Energy.Shared.Models.V1.Home.Requests;
using Energy.Shared.Models.V1.Home.Responses;
using Microsoft.EntityFrameworkCore;

namespace Energy.Infrastructure.Home.Services;

public sealed class HomeService : IHomeService
{
    private readonly AppDbContext _db;
    public HomeService(AppDbContext db) { _db = db; }

    public async Task<HomeDashboardResponse> GetDashboardAsync(GetHomeDashboardRequest request, CancellationToken ct = default)
    {
        var since = DateTime.UtcNow.AddHours(-24);
        return new HomeDashboardResponse
        {
            ActiveUsers = await _db.Users.AsNoTracking().CountAsync(u => u.IsActive, ct),
            TotalRoles = await _db.Roles.AsNoTracking().CountAsync(ct),
            TotalPermissions = await _db.Permissions.AsNoTracking().CountAsync(ct),
            TotalMenus = await _db.Menus.AsNoTracking().CountAsync(ct),
            TotalApiEndpoints = await _db.ApiEndpoints.AsNoTracking().CountAsync(ct),
            FailedLogins24h = await _db.AuditLogs.AsNoTracking().CountAsync(l => l.OccurredAt >= since && l.StatusCode == 401, ct)
        };
    }
}
