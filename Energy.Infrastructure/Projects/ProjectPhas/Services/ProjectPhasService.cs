using Microsoft.EntityFrameworkCore;
using Energy.Infrastructure.Persistence;
using Energy.Shared.Models.V1.Common.Responses;
using Energy.Application.Projects.ProjectPhas.Services;
using Energy.Shared.Models.V1.Projects.ProjectPhas.Requests;
using Energy.Shared.Models.V1.Projects.ProjectPhas.Responses;

namespace Energy.Infrastructure.Projects.ProjectPhas.Services;

/// <summary>ProjectPhas CRUD servisi (projection, pagination, soft-delete).</summary>
public class ProjectPhasService : IProjectPhasService
{
    private readonly AppDbContext _db;

    public ProjectPhasService(AppDbContext db) => _db = db;

    public async Task<BaseResponse<PaginatedResponse<ProjectPhasListResponse>>> GetListAsync(GetProjectPhasListRequest request, CancellationToken ct = default)
    {
        var query = _db.ProjectPhases.AsNoTracking();
        var total = await query.CountAsync(ct);
        var items = await query
            .OrderByDescending(e => e.Id)
            .Skip((request.PageNumber - 1) * request.PageSize)
            .Take(request.PageSize)
            .Select(e => new ProjectPhasListResponse
            {
                Id = e.Id,
                ProjectId = e.ProjectId,
                ParentPhaseId = e.ParentPhaseId,
                Code = e.Code,
                Name = e.Name,
                ProgressPercentage = e.ProgressPercentage,
                CreatedAt = e.CreatedAt
            })
            .ToListAsync(ct);
        var page = PaginatedResponse<ProjectPhasListResponse>.Create(items, request.PageNumber, request.PageSize, total);
        return BaseResponse<PaginatedResponse<ProjectPhasListResponse>>.Success(page);
    }

    public async Task<BaseResponse<ProjectPhasDetailResponse>> GetByIdAsync(Guid id, CancellationToken ct = default)
    {
        var dto = await _db.ProjectPhases.AsNoTracking().Where(e => e.Id == id)
            .Select(e => new ProjectPhasDetailResponse
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
                ParentPhaseId = e.ParentPhaseId,
                Code = e.Code,
                Name = e.Name,
                ProgressPercentage = e.ProgressPercentage
            }).FirstOrDefaultAsync(ct);
        return dto is null
            ? BaseResponse<ProjectPhasDetailResponse>.Failure("NotFound")
            : BaseResponse<ProjectPhasDetailResponse>.Success(dto);
    }

    public async Task<BaseResponse<Guid>> CreateAsync(CreateProjectPhasRequest request, CancellationToken ct = default)
    {
        var entity = new global::Energy.Domain.Projects.ProjectPhase
        {
            Id = Guid.NewGuid(),
            ProjectId = request.ProjectId,
            ParentPhaseId = request.ParentPhaseId,
            Code = request.Code,
            Name = request.Name,
            ProgressPercentage = request.ProgressPercentage,
            CreatedAt = DateTime.UtcNow,
        };
        _db.ProjectPhases.Add(entity);
        await _db.SaveChangesAsync(ct);
        return BaseResponse<Guid>.Success(entity.Id, "Created");
    }

    public async Task<BaseResponse<bool>> UpdateAsync(Guid id, UpdateProjectPhasRequest request, CancellationToken ct = default)
    {
        var entity = await _db.ProjectPhases.FirstOrDefaultAsync(e => e.Id == id, ct);
        if (entity is null) return BaseResponse<bool>.Failure("NotFound");
            entity.ProjectId = request.ProjectId;
            entity.ParentPhaseId = request.ParentPhaseId;
            entity.Code = request.Code;
            entity.Name = request.Name;
            entity.ProgressPercentage = request.ProgressPercentage;
        entity.UpdatedAt = DateTime.UtcNow;
        await _db.SaveChangesAsync(ct);
        return BaseResponse<bool>.Success(true, "Updated");
    }

    public async Task<BaseResponse<bool>> DeleteAsync(Guid id, CancellationToken ct = default)
    {
        var entity = await _db.ProjectPhases.FirstOrDefaultAsync(e => e.Id == id, ct);
        if (entity is null) return BaseResponse<bool>.Failure("NotFound");
        entity.IsDeleted = true;
        entity.DeletedAt = DateTime.UtcNow;
        await _db.SaveChangesAsync(ct);
        return BaseResponse<bool>.Success(true, "Deleted");
    }
}
