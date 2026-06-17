using Microsoft.EntityFrameworkCore;
using Energy.Infrastructure.Persistence;
using Energy.Shared.Models.V1.Common.Responses;
using Energy.Application.FieldOperations.ProgressEntry.Services;
using Energy.Shared.Models.V1.FieldOperations.ProgressEntry.Requests;
using Energy.Shared.Models.V1.FieldOperations.ProgressEntry.Responses;

namespace Energy.Infrastructure.FieldOperations.ProgressEntry.Services;

/// <summary>ProgressEntry CRUD servisi (projection, pagination, soft-delete).</summary>
public class ProgressEntryService : IProgressEntryService
{
    private readonly AppDbContext _db;

    public ProgressEntryService(AppDbContext db) => _db = db;

    public async Task<BaseResponse<PaginatedResponse<ProgressEntryListResponse>>> GetListAsync(GetProgressEntryListRequest request, CancellationToken ct = default)
    {
        var query = _db.ProgressEntries.AsNoTracking();
        var total = await query.CountAsync(ct);
        var items = await query
            .OrderByDescending(e => e.Id)
            .Skip((request.PageNumber - 1) * request.PageSize)
            .Take(request.PageSize)
            .Select(e => new ProgressEntryListResponse
            {
                Id = e.Id,
                ProjectId = e.ProjectId,
                ProjectPhaseId = e.ProjectPhaseId,
                EntryDate = e.EntryDate,
                Quantity = e.Quantity,
                Percentage = e.Percentage,
                Note = e.Note,
                CreatedAt = e.CreatedAt
            })
            .ToListAsync(ct);
        var page = PaginatedResponse<ProgressEntryListResponse>.Create(items, request.PageNumber, request.PageSize, total);
        return BaseResponse<PaginatedResponse<ProgressEntryListResponse>>.Success(page);
    }

    public async Task<BaseResponse<ProgressEntryDetailResponse>> GetByIdAsync(Guid id, CancellationToken ct = default)
    {
        var dto = await _db.ProgressEntries.AsNoTracking().Where(e => e.Id == id)
            .Select(e => new ProgressEntryDetailResponse
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
                ProjectPhaseId = e.ProjectPhaseId,
                EntryDate = e.EntryDate,
                Quantity = e.Quantity,
                Percentage = e.Percentage,
                Note = e.Note
            }).FirstOrDefaultAsync(ct);
        return dto is null
            ? BaseResponse<ProgressEntryDetailResponse>.Failure("NotFound")
            : BaseResponse<ProgressEntryDetailResponse>.Success(dto);
    }

    public async Task<BaseResponse<Guid>> CreateAsync(CreateProgressEntryRequest request, CancellationToken ct = default)
    {
        var entity = new global::Energy.Domain.FieldOperations.ProgressEntry
        {
            Id = Guid.NewGuid(),
            ProjectId = request.ProjectId,
            ProjectPhaseId = request.ProjectPhaseId,
            EntryDate = request.EntryDate,
            Quantity = request.Quantity,
            Percentage = request.Percentage,
            Note = request.Note,
            CreatedAt = DateTime.UtcNow,
        };
        _db.ProgressEntries.Add(entity);
        await _db.SaveChangesAsync(ct);
        return BaseResponse<Guid>.Success(entity.Id, "Created");
    }

    public async Task<BaseResponse<bool>> UpdateAsync(Guid id, UpdateProgressEntryRequest request, CancellationToken ct = default)
    {
        var entity = await _db.ProgressEntries.FirstOrDefaultAsync(e => e.Id == id, ct);
        if (entity is null) return BaseResponse<bool>.Failure("NotFound");
            entity.ProjectId = request.ProjectId;
            entity.ProjectPhaseId = request.ProjectPhaseId;
            entity.EntryDate = request.EntryDate;
            entity.Quantity = request.Quantity;
            entity.Percentage = request.Percentage;
            entity.Note = request.Note;
        entity.UpdatedAt = DateTime.UtcNow;
        await _db.SaveChangesAsync(ct);
        return BaseResponse<bool>.Success(true, "Updated");
    }

    public async Task<BaseResponse<bool>> DeleteAsync(Guid id, CancellationToken ct = default)
    {
        var entity = await _db.ProgressEntries.FirstOrDefaultAsync(e => e.Id == id, ct);
        if (entity is null) return BaseResponse<bool>.Failure("NotFound");
        entity.IsDeleted = true;
        entity.DeletedAt = DateTime.UtcNow;
        await _db.SaveChangesAsync(ct);
        return BaseResponse<bool>.Success(true, "Deleted");
    }
}
