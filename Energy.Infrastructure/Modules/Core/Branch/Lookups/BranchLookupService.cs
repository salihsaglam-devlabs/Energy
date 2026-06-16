using Microsoft.EntityFrameworkCore;
using Energy.Infrastructure.Persistence;
using Energy.Shared.Models.V1.Common.Responses;
using Energy.Application.Modules.Core.Branch.Lookups;
using Energy.Shared.Models.V1.Core.Branch.Responses;

namespace Energy.Infrastructure.Modules.Core.Branch.Lookups;

/// <summary>Branch lookup servisi (aktif + arama filtreli projection).</summary>
public class BranchLookupService : IBranchLookupService
{
    private readonly AppDbContext _db;

    public BranchLookupService(AppDbContext db) => _db = db;

    public async Task<BaseResponse<IReadOnlyList<BranchLookupResponse>>> GetLookupAsync(string? search = null, bool activeOnly = true, CancellationToken ct = default)
    {
        var query = _db.Branches.AsNoTracking();
        if (activeOnly) query = query.Where(e => e.IsActive);
        if (!string.IsNullOrWhiteSpace(search)) query = query.Where(e => e.Name.Contains(search) || e.Code.Contains(search));
        var items = await query
            .OrderBy(e => e.Name)
            .Select(e => new BranchLookupResponse
            {
                Id = e.Id,
                Code = e.Code,
                Name = e.Name,
                DisplayName = e.Code + " - " + e.Name,
                IsActive = e.IsActive
            })
            .ToListAsync(ct);
        return BaseResponse<IReadOnlyList<BranchLookupResponse>>.Success(items);
    }
}
