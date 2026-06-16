using Microsoft.EntityFrameworkCore;
using Energy.Infrastructure.Persistence;
using Energy.Shared.Models.V1.Common.Responses;
using Energy.Application.Modules.Projects.ProjectMember.Services;
using Energy.Shared.Models.V1.Projects.ProjectMember.Requests;
using Energy.Shared.Models.V1.Projects.ProjectMember.Responses;

namespace Energy.Infrastructure.Modules.Projects.ProjectMember.Services;

/// <summary>ProjectMember CRUD servisi (projection, pagination, soft-delete).</summary>
public class ProjectMemberService : IProjectMemberService
{
    private readonly EnergyDbContext _db;

    public ProjectMemberService(EnergyDbContext db) => _db = db;

    public async Task<BaseResponse<PaginatedResponse<ProjectMemberListResponse>>> GetListAsync(GetProjectMemberListRequest request, CancellationToken ct = default)
    {
        var query = _db.ProjectMembers.AsNoTracking();
        var total = await query.CountAsync(ct);
        var items = await query
            .OrderByDescending(e => e.Id)
            .Skip((request.PageNumber - 1) * request.PageSize)
            .Take(request.PageSize)
            .Select(e => new ProjectMemberListResponse
            {
                Id = e.Id,
                ProjectId = e.ProjectId,
                UserId = e.UserId,
                EmployeeId = e.EmployeeId,
                ProjectRole = e.ProjectRole,
                CreatedAt = e.CreatedAt
            })
            .ToListAsync(ct);
        var page = PaginatedResponse<ProjectMemberListResponse>.Create(items, request.PageNumber, request.PageSize, total);
        return BaseResponse<PaginatedResponse<ProjectMemberListResponse>>.Success(page);
    }

    public async Task<BaseResponse<ProjectMemberDetailResponse>> GetByIdAsync(Guid id, CancellationToken ct = default)
    {
        var dto = await _db.ProjectMembers.AsNoTracking().Where(e => e.Id == id)
            .Select(e => new ProjectMemberDetailResponse
            {
                Id = e.Id,
                CreatedAt = e.CreatedAt,
                CreatedBy = e.CreatedBy,
                UpdatedAt = e.UpdatedAt,
                UpdatedBy = e.UpdatedBy,
                IsDeleted = e.IsDeleted,
                DeletedAt = e.DeletedAt,
                DeletedBy = e.DeletedBy,
                ProjectId = e.ProjectId,
                UserId = e.UserId,
                EmployeeId = e.EmployeeId,
                ProjectRole = e.ProjectRole
            }).FirstOrDefaultAsync(ct);
        return dto is null
            ? BaseResponse<ProjectMemberDetailResponse>.Failure("NotFound")
            : BaseResponse<ProjectMemberDetailResponse>.Success(dto);
    }

    public async Task<BaseResponse<Guid>> CreateAsync(CreateProjectMemberRequest request, CancellationToken ct = default)
    {
        var entity = new global::Energy.Domain.Modules.Projects.ProjectMember
        {
            Id = Guid.NewGuid(),
            ProjectId = request.ProjectId,
            UserId = request.UserId,
            EmployeeId = request.EmployeeId,
            ProjectRole = request.ProjectRole,
            CreatedAt = DateTime.UtcNow,
        };
        _db.ProjectMembers.Add(entity);
        await _db.SaveChangesAsync(ct);
        return BaseResponse<Guid>.Success(entity.Id, "Created");
    }

    public async Task<BaseResponse<bool>> UpdateAsync(Guid id, UpdateProjectMemberRequest request, CancellationToken ct = default)
    {
        var entity = await _db.ProjectMembers.FirstOrDefaultAsync(e => e.Id == id, ct);
        if (entity is null) return BaseResponse<bool>.Failure("NotFound");
            entity.ProjectId = request.ProjectId;
            entity.UserId = request.UserId;
            entity.EmployeeId = request.EmployeeId;
            entity.ProjectRole = request.ProjectRole;
        entity.UpdatedAt = DateTime.UtcNow;
        await _db.SaveChangesAsync(ct);
        return BaseResponse<bool>.Success(true, "Updated");
    }

    public async Task<BaseResponse<bool>> DeleteAsync(Guid id, CancellationToken ct = default)
    {
        var entity = await _db.ProjectMembers.FirstOrDefaultAsync(e => e.Id == id, ct);
        if (entity is null) return BaseResponse<bool>.Failure("NotFound");
        entity.IsDeleted = true;
        entity.DeletedAt = DateTime.UtcNow;
        await _db.SaveChangesAsync(ct);
        return BaseResponse<bool>.Success(true, "Deleted");
    }
}
