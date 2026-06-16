using Microsoft.EntityFrameworkCore;
using Energy.Infrastructure.Persistence;
using Energy.Shared.Models.V1.Common.Responses;
using Energy.Application.Modules.Projects.ProjectStatus.Services;
using Energy.Shared.Models.V1.Projects.ProjectStatus.Requests;
using Energy.Shared.Models.V1.Projects.ProjectStatus.Responses;

namespace Energy.Infrastructure.Modules.Projects.ProjectStatus.Services;

/// <summary>ProjectStatus CRUD servisi (projection, pagination, soft-delete).</summary>
public class ProjectStatusService : IProjectStatusService
{
    private readonly EnergyDbContext _db;

    public ProjectStatusService(EnergyDbContext db) => _db = db;

    public async Task<BaseResponse<PaginatedResponse<ProjectStatusListResponse>>> GetListAsync(GetProjectStatusListRequest request, CancellationToken ct = default)
    {
        var query = _db.ProjectStatuses.AsNoTracking();
        var total = await query.CountAsync(ct);
        var items = await query
            .OrderByDescending(e => e.Id)
            .Skip((request.PageNumber - 1) * request.PageSize)
            .Take(request.PageSize)
            .Select(e => new ProjectStatusListResponse
            {
                Id = e.Id,
                Code = e.Code,
                Name = e.Name,
                DisplayOrder = e.DisplayOrder,
                IsClosedState = e.IsClosedState,
                IsActive = e.IsActive,
                CreatedAt = e.CreatedAt
            })
            .ToListAsync(ct);
        var page = PaginatedResponse<ProjectStatusListResponse>.Create(items, request.PageNumber, request.PageSize, total);
        return BaseResponse<PaginatedResponse<ProjectStatusListResponse>>.Success(page);
    }

    public async Task<BaseResponse<ProjectStatusDetailResponse>> GetByIdAsync(Guid id, CancellationToken ct = default)
    {
        var dto = await _db.ProjectStatuses.AsNoTracking().Where(e => e.Id == id)
            .Select(e => new ProjectStatusDetailResponse
            {
                Id = e.Id,
                CreatedAt = e.CreatedAt,
                CreatedBy = e.CreatedBy,
                UpdatedAt = e.UpdatedAt,
                UpdatedBy = e.UpdatedBy,
                IsDeleted = e.IsDeleted,
                DeletedAt = e.DeletedAt,
                DeletedBy = e.DeletedBy,
                Code = e.Code,
                Name = e.Name,
                DisplayOrder = e.DisplayOrder,
                IsClosedState = e.IsClosedState,
                IsActive = e.IsActive
            }).FirstOrDefaultAsync(ct);
        return dto is null
            ? BaseResponse<ProjectStatusDetailResponse>.Failure("NotFound")
            : BaseResponse<ProjectStatusDetailResponse>.Success(dto);
    }

    public async Task<BaseResponse<Guid>> CreateAsync(CreateProjectStatusRequest request, CancellationToken ct = default)
    {
        var entity = new global::Energy.Domain.Modules.Projects.ProjectStatus
        {
            Id = Guid.NewGuid(),
            Code = request.Code,
            Name = request.Name,
            DisplayOrder = request.DisplayOrder,
            IsClosedState = request.IsClosedState,
            IsActive = request.IsActive,
            CreatedAt = DateTime.UtcNow,
        };
        _db.ProjectStatuses.Add(entity);
        await _db.SaveChangesAsync(ct);
        return BaseResponse<Guid>.Success(entity.Id, "Created");
    }

    public async Task<BaseResponse<bool>> UpdateAsync(Guid id, UpdateProjectStatusRequest request, CancellationToken ct = default)
    {
        var entity = await _db.ProjectStatuses.FirstOrDefaultAsync(e => e.Id == id, ct);
        if (entity is null) return BaseResponse<bool>.Failure("NotFound");
            entity.Code = request.Code;
            entity.Name = request.Name;
            entity.DisplayOrder = request.DisplayOrder;
            entity.IsClosedState = request.IsClosedState;
            entity.IsActive = request.IsActive;
        entity.UpdatedAt = DateTime.UtcNow;
        await _db.SaveChangesAsync(ct);
        return BaseResponse<bool>.Success(true, "Updated");
    }

    public async Task<BaseResponse<bool>> DeleteAsync(Guid id, CancellationToken ct = default)
    {
        var entity = await _db.ProjectStatuses.FirstOrDefaultAsync(e => e.Id == id, ct);
        if (entity is null) return BaseResponse<bool>.Failure("NotFound");
        entity.IsDeleted = true;
        entity.DeletedAt = DateTime.UtcNow;
        await _db.SaveChangesAsync(ct);
        return BaseResponse<bool>.Success(true, "Deleted");
    }
}
