using Microsoft.EntityFrameworkCore;
using Energy.Infrastructure.Persistence;
using Energy.Shared.Models.V1.Common.Responses;
using Energy.Application.Modules.Core.Company.Lookups;
using Energy.Shared.Models.V1.Core.Company.Responses;

namespace Energy.Infrastructure.Modules.Core.Company.Lookups;

/// <summary>Company lookup servisi (aktif + arama filtreli projection).</summary>
public class CompanyLookupService : ICompanyLookupService
{
    private readonly EnergyDbContext _db;

    public CompanyLookupService(EnergyDbContext db) => _db = db;

    public async Task<BaseResponse<IReadOnlyList<CompanyLookupResponse>>> GetLookupAsync(string? search = null, bool activeOnly = true, CancellationToken ct = default)
    {
        var query = _db.Companies.AsNoTracking();
        if (activeOnly) query = query.Where(e => e.IsActive);
        if (!string.IsNullOrWhiteSpace(search)) query = query.Where(e => e.Name.Contains(search));
        var items = await query.Select(e => new CompanyLookupResponse
        {
            Id = e.Id,
            Code = e.Code,
            Name = e.Name,
            DisplayName = e.Name,
            IsActive = e.IsActive
        }).ToListAsync(ct);
        return BaseResponse<IReadOnlyList<CompanyLookupResponse>>.Success(items);
    }
}
