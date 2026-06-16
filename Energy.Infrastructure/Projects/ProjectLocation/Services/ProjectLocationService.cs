using Microsoft.EntityFrameworkCore;
using Energy.Infrastructure.Persistence;
using Energy.Shared.Models.V1.Common.Responses;
using Energy.Application.Projects.ProjectLocation.Services;
using Energy.Shared.Models.V1.Projects.ProjectLocation.Requests;
using Energy.Shared.Models.V1.Projects.ProjectLocation.Responses;

namespace Energy.Infrastructure.Projects.ProjectLocation.Services;

/// <summary>ProjectLocation CRUD servisi (projection, pagination, soft-delete).</summary>
public class ProjectLocationService : IProjectLocationService
{
    private readonly AppDbContext _db;

    public ProjectLocationService(AppDbContext db) => _db = db;

    public async Task<BaseResponse<PaginatedResponse<ProjectLocationListResponse>>> GetListAsync(GetProjectLocationListRequest request, CancellationToken ct = default)
    {
        var query = _db.ProjectLocations.AsNoTracking();
        var total = await query.CountAsync(ct);
        var items = await query
            .OrderByDescending(e => e.Id)
            .Skip((request.PageNumber - 1) * request.PageSize)
            .Take(request.PageSize)
            .Select(e => new ProjectLocationListResponse
            {
                Id = e.Id,
                ProjectId = e.ProjectId,
                ParentLocationId = e.ParentLocationId,
                Code = e.Code,
                Name = e.Name,
                CreatedAt = e.CreatedAt
            })
            .ToListAsync(ct);
        var page = PaginatedResponse<ProjectLocationListResponse>.Create(items, request.PageNumber, request.PageSize, total);
        return BaseResponse<PaginatedResponse<ProjectLocationListResponse>>.Success(page);
    }

    public async Task<BaseResponse<ProjectLocationDetailResponse>> GetByIdAsync(Guid id, CancellationToken ct = default)
    {
        var dto = await _db.ProjectLocations.AsNoTracking().Where(e => e.Id == id)
            .Select(e => new ProjectLocationDetailResponse
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
                ParentLocationId = e.ParentLocationId,
                Code = e.Code,
                Name = e.Name
            }).FirstOrDefaultAsync(ct);
        return dto is null
            ? BaseResponse<ProjectLocationDetailResponse>.Failure("NotFound")
            : BaseResponse<ProjectLocationDetailResponse>.Success(dto);
    }

    public async Task<BaseResponse<Guid>> CreateAsync(CreateProjectLocationRequest request, CancellationToken ct = default)
    {
        var entity = new global::Energy.Domain.Projects.ProjectLocation
        {
            Id = Guid.NewGuid(),
            ProjectId = request.ProjectId,
            ParentLocationId = request.ParentLocationId,
            Code = request.Code,
            Name = request.Name,
            CreatedAt = DateTime.UtcNow,
        };
        _db.ProjectLocations.Add(entity);
        await _db.SaveChangesAsync(ct);
        return BaseResponse<Guid>.Success(entity.Id, "Created");
    }

    public async Task<BaseResponse<bool>> UpdateAsync(Guid id, UpdateProjectLocationRequest request, CancellationToken ct = default)
    {
        var entity = await _db.ProjectLocations.FirstOrDefaultAsync(e => e.Id == id, ct);
        if (entity is null) return BaseResponse<bool>.Failure("NotFound");
            entity.ProjectId = request.ProjectId;
            entity.ParentLocationId = request.ParentLocationId;
            entity.Code = request.Code;
            entity.Name = request.Name;
        entity.UpdatedAt = DateTime.UtcNow;
        await _db.SaveChangesAsync(ct);
        return BaseResponse<bool>.Success(true, "Updated");
    }

    public async Task<BaseResponse<bool>> DeleteAsync(Guid id, CancellationToken ct = default)
    {
        var entity = await _db.ProjectLocations.FirstOrDefaultAsync(e => e.Id == id, ct);
        if (entity is null) return BaseResponse<bool>.Failure("NotFound");
        entity.IsDeleted = true;
        entity.DeletedAt = DateTime.UtcNow;
        await _db.SaveChangesAsync(ct);
        return BaseResponse<bool>.Success(true, "Deleted");
    }
}
