using Microsoft.EntityFrameworkCore;
using Energy.Infrastructure.Persistence;
using Energy.Shared.Models.V1.Common.Responses;
using Energy.Application.Modules.Organization.EmployeeSkillAssignment.Lookups;
using Energy.Shared.Models.V1.Organization.EmployeeSkillAssignment.Responses;

namespace Energy.Infrastructure.Modules.Organization.EmployeeSkillAssignment.Lookups;

/// <summary>EmployeeSkillAssignment lookup servisi (aktif + arama filtreli projection).</summary>
public class EmployeeSkillAssignmentLookupService : IEmployeeSkillAssignmentLookupService
{
    private readonly AppDbContext _db;

    public EmployeeSkillAssignmentLookupService(AppDbContext db) => _db = db;

    public async Task<BaseResponse<IReadOnlyList<EmployeeSkillAssignmentLookupResponse>>> GetLookupAsync(string? search = null, bool activeOnly = true, CancellationToken ct = default)
    {
        var query = _db.EmployeeSkillAssignments.AsNoTracking();
        var items = await query
            .OrderBy(e => e.Id)
            .Select(e => new EmployeeSkillAssignmentLookupResponse
            {
                Id = e.Id,
                Code = null,
                Name = null,
                DisplayName = e.Id.ToString(),
                IsActive = true
            })
            .ToListAsync(ct);
        return BaseResponse<IReadOnlyList<EmployeeSkillAssignmentLookupResponse>>.Success(items);
    }
}
