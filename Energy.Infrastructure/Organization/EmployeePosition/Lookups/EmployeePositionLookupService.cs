using Microsoft.EntityFrameworkCore;
using Energy.Infrastructure.Persistence;
using Energy.Shared.Models.V1.Common.Responses;
using Energy.Application.Organization.EmployeePosition.Lookups;
using Energy.Shared.Models.V1.Organization.EmployeePosition.Responses;

namespace Energy.Infrastructure.Organization.EmployeePosition.Lookups;

/// <summary>EmployeePosition lookup servisi (aktif + arama filtreli projection).</summary>
public class EmployeePositionLookupService : IEmployeePositionLookupService
{
    private readonly AppDbContext _db;

    public EmployeePositionLookupService(AppDbContext db) => _db = db;

    public async Task<BaseResponse<IReadOnlyList<EmployeePositionLookupResponse>>> GetLookupAsync(string? search = null, bool activeOnly = true, CancellationToken ct = default)
    {
        var query = _db.EmployeePositions.AsNoTracking();
        if (activeOnly) query = query.Where(e => e.IsActive);
        if (!string.IsNullOrWhiteSpace(search)) query = query.Where(e => e.Name.Contains(search) || e.Code.Contains(search));
        var items = await query
            .OrderBy(e => e.Name)
            .Select(e => new EmployeePositionLookupResponse
            {
                Id = e.Id,
                Code = e.Code,
                Name = e.Name,
                DisplayName = e.Code + " - " + e.Name,
                IsActive = e.IsActive
            })
            .ToListAsync(ct);
        return BaseResponse<IReadOnlyList<EmployeePositionLookupResponse>>.Success(items);
    }
}
