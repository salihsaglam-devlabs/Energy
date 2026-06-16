using Microsoft.EntityFrameworkCore;
using Energy.Infrastructure.Persistence;
using Energy.Shared.Models.V1.Common.Responses;
using Energy.Application.Core.Company.Lookups;
using Energy.Shared.Models.V1.Core.Company.Responses;

namespace Energy.Infrastructure.Core.Company.Lookups;

/// <summary>Company lookup servisi (aktif + arama filtreli projection).</summary>
public class CompanyLookupService : ICompanyLookupService
{
    private readonly AppDbContext _db;

    public CompanyLookupService(AppDbContext db) => _db = db;

    public async Task<BaseResponse<IReadOnlyList<CompanyLookupResponse>>> GetLookupAsync(string? search = null, bool activeOnly = true, CancellationToken ct = default)
    {
        var query = _db.Companies.AsNoTracking();
        if (activeOnly) query = query.Where(e => e.IsActive);
        if (!string.IsNullOrWhiteSpace(search)) query = query.Where(e => e.Name.Contains(search) || e.Code.Contains(search));
        var items = await query
            .OrderBy(e => e.Name)
            .Select(e => new CompanyLookupResponse
            {
                Id = e.Id,
                Code = e.Code,
                Name = e.Name,
                DisplayName = e.Code + " - " + e.Name,
                IsActive = e.IsActive
            })
            .ToListAsync(ct);
        return BaseResponse<IReadOnlyList<CompanyLookupResponse>>.Success(items);
    }
}
