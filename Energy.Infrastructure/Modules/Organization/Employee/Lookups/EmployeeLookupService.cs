using Microsoft.EntityFrameworkCore;
using Energy.Infrastructure.Persistence;
using Energy.Shared.Models.V1.Common.Responses;
using Energy.Application.Modules.Organization.Employee.Lookups;
using Energy.Shared.Models.V1.Organization.Employee.Responses;

namespace Energy.Infrastructure.Modules.Organization.Employee.Lookups;

/// <summary>Employee lookup servisi (aktif + arama filtreli projection).</summary>
public class EmployeeLookupService : IEmployeeLookupService
{
    private readonly AppDbContext _db;

    public EmployeeLookupService(AppDbContext db) => _db = db;

    public async Task<BaseResponse<IReadOnlyList<EmployeeLookupResponse>>> GetLookupAsync(string? search = null, bool activeOnly = true, CancellationToken ct = default)
    {
        var query = _db.Employees.AsNoTracking();
        if (activeOnly) query = query.Where(e => e.IsActive);
        if (!string.IsNullOrWhiteSpace(search)) query = query.Where(e => e.Email.Contains(search) || e.Code.Contains(search));
        var items = await query
            .OrderBy(e => e.Email)
            .Select(e => new EmployeeLookupResponse
            {
                Id = e.Id,
                Code = e.Code,
                Name = e.Email,
                DisplayName = e.Code + " - " + e.Email,
                IsActive = e.IsActive
            })
            .ToListAsync(ct);
        return BaseResponse<IReadOnlyList<EmployeeLookupResponse>>.Success(items);
    }
}
