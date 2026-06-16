using Microsoft.EntityFrameworkCore;
using Energy.Infrastructure.Persistence;
using Energy.Shared.Models.V1.Common.Responses;
using Energy.Application.Projects.Project.Services;
using Energy.Shared.Models.V1.Projects.Project.Requests;
using Energy.Shared.Models.V1.Projects.Project.Responses;

namespace Energy.Infrastructure.Projects.Project.Services;

/// <summary>Project CRUD servisi (projection, pagination, soft-delete).</summary>
public class ProjectService : IProjectService
{
    private readonly AppDbContext _db;

    public ProjectService(AppDbContext db) => _db = db;

    public async Task<BaseResponse<PaginatedResponse<ProjectListResponse>>> GetListAsync(GetProjectListRequest request, CancellationToken ct = default)
    {
        var query = _db.Projects.AsNoTracking();
        var total = await query.CountAsync(ct);
        var items = await query
            .OrderByDescending(e => e.Id)
            .Skip((request.PageNumber - 1) * request.PageSize)
            .Take(request.PageSize)
            .Select(e => new ProjectListResponse
            {
                Id = e.Id,
                CompanyId = e.CompanyId,
                BranchId = e.BranchId,
                ProjectTypeId = e.ProjectTypeId,
                StatusId = e.StatusId,
                CustomerId = e.CustomerId,
                ManagerUserId = e.ManagerUserId,
                Code = e.Code,
                Name = e.Name,
                StartDate = e.StartDate,
                EndDate = e.EndDate,
                Description = e.Description,
                CreatedAt = e.CreatedAt
            })
            .ToListAsync(ct);
        var page = PaginatedResponse<ProjectListResponse>.Create(items, request.PageNumber, request.PageSize, total);
        return BaseResponse<PaginatedResponse<ProjectListResponse>>.Success(page);
    }

    public async Task<BaseResponse<ProjectDetailResponse>> GetByIdAsync(Guid id, CancellationToken ct = default)
    {
        var dto = await _db.Projects.AsNoTracking().Where(e => e.Id == id)
            .Select(e => new ProjectDetailResponse
            {
                Id = e.Id,
                CreatedAt = e.CreatedAt,
                CreatedBy = e.CreatedBy,
                UpdatedAt = e.UpdatedAt,
                UpdatedBy = e.UpdatedBy,
                IsDeleted = e.IsDeleted,
                DeletedAt = e.DeletedAt,
                DeletedBy = e.DeletedBy,
                CompanyId = e.CompanyId,
                BranchId = e.BranchId,
                ProjectTypeId = e.ProjectTypeId,
                StatusId = e.StatusId,
                CustomerId = e.CustomerId,
                ManagerUserId = e.ManagerUserId,
                Code = e.Code,
                Name = e.Name,
                StartDate = e.StartDate,
                EndDate = e.EndDate,
                Description = e.Description
            }).FirstOrDefaultAsync(ct);
        return dto is null
            ? BaseResponse<ProjectDetailResponse>.Failure("NotFound")
            : BaseResponse<ProjectDetailResponse>.Success(dto);
    }

    public async Task<BaseResponse<Guid>> CreateAsync(CreateProjectRequest request, CancellationToken ct = default)
    {
        var entity = new global::Energy.Domain.Projects.Project
        {
            Id = Guid.NewGuid(),
            CompanyId = request.CompanyId,
            BranchId = request.BranchId,
            ProjectTypeId = request.ProjectTypeId,
            StatusId = request.StatusId,
            CustomerId = request.CustomerId,
            ManagerUserId = request.ManagerUserId,
            Code = request.Code,
            Name = request.Name,
            StartDate = request.StartDate,
            EndDate = request.EndDate,
            Description = request.Description,
            CreatedAt = DateTime.UtcNow,
        };
        _db.Projects.Add(entity);
        await _db.SaveChangesAsync(ct);
        return BaseResponse<Guid>.Success(entity.Id, "Created");
    }

    public async Task<BaseResponse<bool>> UpdateAsync(Guid id, UpdateProjectRequest request, CancellationToken ct = default)
    {
        var entity = await _db.Projects.FirstOrDefaultAsync(e => e.Id == id, ct);
        if (entity is null) return BaseResponse<bool>.Failure("NotFound");
            entity.CompanyId = request.CompanyId;
            entity.BranchId = request.BranchId;
            entity.ProjectTypeId = request.ProjectTypeId;
            entity.StatusId = request.StatusId;
            entity.CustomerId = request.CustomerId;
            entity.ManagerUserId = request.ManagerUserId;
            entity.Code = request.Code;
            entity.Name = request.Name;
            entity.StartDate = request.StartDate;
            entity.EndDate = request.EndDate;
            entity.Description = request.Description;
        entity.UpdatedAt = DateTime.UtcNow;
        await _db.SaveChangesAsync(ct);
        return BaseResponse<bool>.Success(true, "Updated");
    }

    public async Task<BaseResponse<bool>> DeleteAsync(Guid id, CancellationToken ct = default)
    {
        var entity = await _db.Projects.FirstOrDefaultAsync(e => e.Id == id, ct);
        if (entity is null) return BaseResponse<bool>.Failure("NotFound");
        entity.IsDeleted = true;
        entity.DeletedAt = DateTime.UtcNow;
        await _db.SaveChangesAsync(ct);
        return BaseResponse<bool>.Success(true, "Deleted");
    }
}
