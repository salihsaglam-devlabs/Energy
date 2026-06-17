using Microsoft.EntityFrameworkCore;
using Energy.Infrastructure.Persistence;
using Energy.Shared.Models.V1.Common.Responses;
using Energy.Application.Projects.ProjectMember.Lookups;
using Energy.Shared.Models.V1.Projects.ProjectMember.Responses;

namespace Energy.Infrastructure.Projects.ProjectMember.Lookups;

/// <summary>ProjectMember lookup servisi (aktif + arama filtreli projection).</summary>
public class ProjectMemberLookupService : IProjectMemberLookupService
{
    private readonly AppDbContext _db;

    public ProjectMemberLookupService(AppDbContext db) => _db = db;

    public async Task<BaseResponse<IReadOnlyList<ProjectMemberLookupResponse>>> GetLookupAsync(string? search = null, bool activeOnly = true, CancellationToken ct = default)
    {
        var query = _db.ProjectMembers.AsNoTracking();
        var rows = await query
            .OrderBy(e => e.Id)
            .ToListAsync(ct);
        var items = (IReadOnlyList<ProjectMemberLookupResponse>)rows.Select(e => new ProjectMemberLookupResponse
        {
            Id = e.Id,
            Code = null,
            Name = null,
            DisplayName = string.IsNullOrWhiteSpace((e.ProjectRole ?? "")) ? "Project Member #" + e.Id.ToString().Substring(0, 8) : ((e.ProjectRole ?? "")),
            IsActive = true
        }).ToList();
        return BaseResponse<IReadOnlyList<ProjectMemberLookupResponse>>.Success(items);
    }
}
