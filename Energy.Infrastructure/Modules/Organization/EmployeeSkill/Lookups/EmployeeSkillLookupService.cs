using Microsoft.EntityFrameworkCore;
using Energy.Infrastructure.Persistence;
using Energy.Shared.Models.V1.Common.Responses;
using Energy.Application.Modules.Organization.EmployeeSkill.Lookups;
using Energy.Shared.Models.V1.Organization.EmployeeSkill.Responses;

namespace Energy.Infrastructure.Modules.Organization.EmployeeSkill.Lookups;

/// <summary>EmployeeSkill lookup servisi (aktif + arama filtreli projection).</summary>
public class EmployeeSkillLookupService : IEmployeeSkillLookupService
{
    private readonly EnergyDbContext _db;

    public EmployeeSkillLookupService(EnergyDbContext db) => _db = db;

    public async Task<BaseResponse<IReadOnlyList<EmployeeSkillLookupResponse>>> GetLookupAsync(string? search = null, bool activeOnly = true, CancellationToken ct = default)
    {
        var query = _db.EmployeeSkills.AsNoTracking();
        if (activeOnly) query = query.Where(e => e.IsActive);
        if (!string.IsNullOrWhiteSpace(search)) query = query.Where(e => e.Name.Contains(search));
        var items = await query.Select(e => new EmployeeSkillLookupResponse
        {
            Id = e.Id,
            Code = e.Code,
            Name = e.Name,
            DisplayName = e.Name,
            IsActive = e.IsActive
        }).ToListAsync(ct);
        return BaseResponse<IReadOnlyList<EmployeeSkillLookupResponse>>.Success(items);
    }
}
