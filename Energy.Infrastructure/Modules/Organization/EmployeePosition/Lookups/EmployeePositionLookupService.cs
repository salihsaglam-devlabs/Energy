using Microsoft.EntityFrameworkCore;
using Energy.Infrastructure.Persistence;
using Energy.Shared.Models.V1.Common.Responses;
using Energy.Application.Modules.Organization.EmployeePosition.Lookups;
using Energy.Shared.Models.V1.Organization.EmployeePosition.Responses;

namespace Energy.Infrastructure.Modules.Organization.EmployeePosition.Lookups;

/// <summary>EmployeePosition lookup servisi (aktif + arama filtreli projection).</summary>
public class EmployeePositionLookupService : IEmployeePositionLookupService
{
    private readonly EnergyDbContext _db;

    public EmployeePositionLookupService(EnergyDbContext db) => _db = db;

    public async Task<BaseResponse<IReadOnlyList<EmployeePositionLookupResponse>>> GetLookupAsync(string? search = null, bool activeOnly = true, CancellationToken ct = default)
    {
        var query = _db.EmployeePositions.AsNoTracking();
        if (activeOnly) query = query.Where(e => e.IsActive);
        if (!string.IsNullOrWhiteSpace(search)) query = query.Where(e => e.Name.Contains(search));
        var items = await query.Select(e => new EmployeePositionLookupResponse
        {
            Id = e.Id,
            Code = e.Code,
            Name = e.Name,
            DisplayName = e.Name,
            IsActive = e.IsActive
        }).ToListAsync(ct);
        return BaseResponse<IReadOnlyList<EmployeePositionLookupResponse>>.Success(items);
    }
}
