using Microsoft.EntityFrameworkCore;
using Energy.Infrastructure.Persistence;
using Energy.Shared.Models.V1.Common.Responses;
using Energy.Application.Modules.IAM.Role.Lookups;
using Energy.Shared.Models.V1.IAM.Role.Responses;

namespace Energy.Infrastructure.Modules.IAM.Role.Lookups;

/// <summary>Role lookup servisi (aktif + arama filtreli projection).</summary>
public class RoleLookupService : IRoleLookupService
{
    private readonly AppDbContext _db;

    public RoleLookupService(AppDbContext db) => _db = db;

    public async Task<BaseResponse<IReadOnlyList<RoleLookupResponse>>> GetLookupAsync(string? search = null, bool activeOnly = true, CancellationToken ct = default)
    {
        var query = _db.Roles.AsNoTracking();
        if (!string.IsNullOrWhiteSpace(search)) query = query.Where(e => e.Name.Contains(search));
        var items = await query
            .OrderBy(e => e.Name)
            .Select(e => new RoleLookupResponse
            {
                Id = e.Id,
                Code = null,
                Name = e.Name,
                DisplayName = e.Name,
                IsActive = true
            })
            .ToListAsync(ct);
        return BaseResponse<IReadOnlyList<RoleLookupResponse>>.Success(items);
    }
}
