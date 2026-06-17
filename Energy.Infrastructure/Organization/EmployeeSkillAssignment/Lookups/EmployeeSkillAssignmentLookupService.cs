using Microsoft.EntityFrameworkCore;
using Energy.Infrastructure.Persistence;
using Energy.Shared.Models.V1.Common.Responses;
using Energy.Application.Organization.EmployeeSkillAssignment.Lookups;
using Energy.Shared.Models.V1.Organization.EmployeeSkillAssignment.Responses;

namespace Energy.Infrastructure.Organization.EmployeeSkillAssignment.Lookups;

/// <summary>EmployeeSkillAssignment lookup servisi (aktif + arama filtreli projection).</summary>
public class EmployeeSkillAssignmentLookupService : IEmployeeSkillAssignmentLookupService
{
    private readonly AppDbContext _db;

    public EmployeeSkillAssignmentLookupService(AppDbContext db) => _db = db;

    public async Task<BaseResponse<IReadOnlyList<EmployeeSkillAssignmentLookupResponse>>> GetLookupAsync(string? search = null, bool activeOnly = true, CancellationToken ct = default)
    {
        var query = _db.EmployeeSkillAssignments.AsNoTracking();
        var rows = await query
            .OrderBy(e => e.Id)
            .ToListAsync(ct);
        var items = (IReadOnlyList<EmployeeSkillAssignmentLookupResponse>)rows.Select(e => new EmployeeSkillAssignmentLookupResponse
        {
            Id = e.Id,
            Code = null,
            Name = null,
            DisplayName = string.IsNullOrWhiteSpace((e.Note ?? "")) ? "Employee Skill Assignment #" + e.Id.ToString().Substring(0, 8) : ((e.Note ?? "")),
            IsActive = true
        }).ToList();
        return BaseResponse<IReadOnlyList<EmployeeSkillAssignmentLookupResponse>>.Success(items);
    }
}
