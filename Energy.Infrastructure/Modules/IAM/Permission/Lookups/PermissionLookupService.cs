using Microsoft.EntityFrameworkCore;
using Energy.Infrastructure.Persistence;
using Energy.Shared.Models.V1.Common.Responses;
using Energy.Application.Modules.IAM.Permission.Lookups;
using Energy.Shared.Models.V1.IAM.Permission.Responses;

namespace Energy.Infrastructure.Modules.IAM.Permission.Lookups;

/// <summary>Permission lookup servisi (aktif + arama filtreli projection).</summary>
public class PermissionLookupService : IPermissionLookupService
{
    private readonly AppDbContext _db;

    public PermissionLookupService(AppDbContext db) => _db = db;

    public async Task<BaseResponse<IReadOnlyList<PermissionLookupResponse>>> GetLookupAsync(string? search = null, bool activeOnly = true, CancellationToken ct = default)
    {
        var query = _db.Permissions.AsNoTracking();
        if (!string.IsNullOrWhiteSpace(search)) query = query.Where(e => e.Code.Contains(search) || e.Code.Contains(search));
        var items = await query
            .OrderBy(e => e.Code)
            .Select(e => new PermissionLookupResponse
            {
                Id = Guid.Empty,
                Code = e.Code,
                Name = e.Code,
                DisplayName = e.Code,
                IsActive = true
            })
            .ToListAsync(ct);
        return BaseResponse<IReadOnlyList<PermissionLookupResponse>>.Success(items);
    }
}
