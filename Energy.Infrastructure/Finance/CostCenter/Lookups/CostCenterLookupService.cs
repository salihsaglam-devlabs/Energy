using Microsoft.EntityFrameworkCore;
using Energy.Infrastructure.Persistence;
using Energy.Shared.Models.V1.Common.Responses;
using Energy.Application.Finance.CostCenter.Lookups;
using Energy.Shared.Models.V1.Finance.CostCenter.Responses;

namespace Energy.Infrastructure.Finance.CostCenter.Lookups;

/// <summary>CostCenter lookup servisi (aktif + arama filtreli projection).</summary>
public class CostCenterLookupService : ICostCenterLookupService
{
    private readonly AppDbContext _db;

    public CostCenterLookupService(AppDbContext db) => _db = db;

    public async Task<BaseResponse<IReadOnlyList<CostCenterLookupResponse>>> GetLookupAsync(string? search = null, bool activeOnly = true, CancellationToken ct = default)
    {
        var query = _db.CostCenters.AsNoTracking();
        if (activeOnly) query = query.Where(e => e.IsActive);
        if (!string.IsNullOrWhiteSpace(search)) query = query.Where(e => e.Name.Contains(search) || e.Code.Contains(search));
        var items = await query
            .OrderBy(e => e.Name)
            .Select(e => new CostCenterLookupResponse
            {
                Id = e.Id,
                Code = e.Code,
                Name = e.Name,
                DisplayName = e.Code + " - " + e.Name,
                IsActive = e.IsActive
            })
            .ToListAsync(ct);
        return BaseResponse<IReadOnlyList<CostCenterLookupResponse>>.Success(items);
    }
}
