using Microsoft.EntityFrameworkCore;
using Energy.Infrastructure.Persistence;
using Energy.Shared.Models.V1.Common.Responses;
using Energy.Application.Projects.ProjectNote.Services;
using Energy.Shared.Models.V1.Projects.ProjectNote.Requests;
using Energy.Shared.Models.V1.Projects.ProjectNote.Responses;

namespace Energy.Infrastructure.Projects.ProjectNote.Services;

/// <summary>ProjectNote CRUD servisi (projection, pagination, soft-delete).</summary>
public class ProjectNoteService : IProjectNoteService
{
    private readonly AppDbContext _db;

    public ProjectNoteService(AppDbContext db) => _db = db;

    public async Task<BaseResponse<PaginatedResponse<ProjectNoteListResponse>>> GetListAsync(GetProjectNoteListRequest request, CancellationToken ct = default)
    {
        var query = _db.ProjectNotes.AsNoTracking();
        var total = await query.CountAsync(ct);
        var items = await query
            .OrderByDescending(e => e.Id)
            .Skip((request.PageNumber - 1) * request.PageSize)
            .Take(request.PageSize)
            .Select(e => new ProjectNoteListResponse
            {
                Id = e.Id,
                ProjectId = e.ProjectId,
                Title = e.Title,
                Body = e.Body,
                CreatedAt = e.CreatedAt
            })
            .ToListAsync(ct);
        var page = PaginatedResponse<ProjectNoteListResponse>.Create(items, request.PageNumber, request.PageSize, total);
        return BaseResponse<PaginatedResponse<ProjectNoteListResponse>>.Success(page);
    }

    public async Task<BaseResponse<ProjectNoteDetailResponse>> GetByIdAsync(Guid id, CancellationToken ct = default)
    {
        var dto = await _db.ProjectNotes.AsNoTracking().Where(e => e.Id == id)
            .Select(e => new ProjectNoteDetailResponse
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
                Title = e.Title,
                Body = e.Body
            }).FirstOrDefaultAsync(ct);
        return dto is null
            ? BaseResponse<ProjectNoteDetailResponse>.Failure("NotFound")
            : BaseResponse<ProjectNoteDetailResponse>.Success(dto);
    }

    public async Task<BaseResponse<Guid>> CreateAsync(CreateProjectNoteRequest request, CancellationToken ct = default)
    {
        var entity = new global::Energy.Domain.Projects.ProjectNote
        {
            Id = Guid.NewGuid(),
            ProjectId = request.ProjectId,
            Title = request.Title,
            Body = request.Body,
            CreatedAt = DateTime.UtcNow,
        };
        _db.ProjectNotes.Add(entity);
        await _db.SaveChangesAsync(ct);
        return BaseResponse<Guid>.Success(entity.Id, "Created");
    }

    public async Task<BaseResponse<bool>> UpdateAsync(Guid id, UpdateProjectNoteRequest request, CancellationToken ct = default)
    {
        var entity = await _db.ProjectNotes.FirstOrDefaultAsync(e => e.Id == id, ct);
        if (entity is null) return BaseResponse<bool>.Failure("NotFound");
            entity.ProjectId = request.ProjectId;
            entity.Title = request.Title;
            entity.Body = request.Body;
        entity.UpdatedAt = DateTime.UtcNow;
        await _db.SaveChangesAsync(ct);
        return BaseResponse<bool>.Success(true, "Updated");
    }

    public async Task<BaseResponse<bool>> DeleteAsync(Guid id, CancellationToken ct = default)
    {
        var entity = await _db.ProjectNotes.FirstOrDefaultAsync(e => e.Id == id, ct);
        if (entity is null) return BaseResponse<bool>.Failure("NotFound");
        entity.IsDeleted = true;
        entity.DeletedAt = DateTime.UtcNow;
        await _db.SaveChangesAsync(ct);
        return BaseResponse<bool>.Success(true, "Deleted");
    }
}
