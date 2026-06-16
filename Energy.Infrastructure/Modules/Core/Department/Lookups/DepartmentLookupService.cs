using Microsoft.EntityFrameworkCore;
using Energy.Infrastructure.Persistence;
using Energy.Shared.Models.V1.Common.Responses;
using Energy.Application.Modules.Core.Department.Lookups;
using Energy.Shared.Models.V1.Core.Department.Responses;

namespace Energy.Infrastructure.Modules.Core.Department.Lookups;

/// <summary>Department lookup servisi (aktif + arama filtreli projection).</summary>
public class DepartmentLookupService : IDepartmentLookupService
{
    private readonly EnergyDbContext _db;

    public DepartmentLookupService(EnergyDbContext db) => _db = db;

    public async Task<BaseResponse<IReadOnlyList<DepartmentLookupResponse>>> GetLookupAsync(string? search = null, bool activeOnly = true, CancellationToken ct = default)
    {
        var query = _db.Departments.AsNoTracking();
        if (activeOnly) query = query.Where(e => e.IsActive);
        if (!string.IsNullOrWhiteSpace(search)) query = query.Where(e => e.Name.Contains(search));
        var items = await query.Select(e => new DepartmentLookupResponse
        {
            Id = e.Id,
            Code = e.Code,
            Name = e.Name,
            DisplayName = e.Name,
            IsActive = e.IsActive
        }).ToListAsync(ct);
        return BaseResponse<IReadOnlyList<DepartmentLookupResponse>>.Success(items);
    }
}
