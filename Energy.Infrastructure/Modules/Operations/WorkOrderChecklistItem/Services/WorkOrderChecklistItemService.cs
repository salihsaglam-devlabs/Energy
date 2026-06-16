using Microsoft.EntityFrameworkCore;
using Energy.Infrastructure.Persistence;
using Energy.Shared.Models.V1.Common.Responses;
using Energy.Application.Modules.Operations.WorkOrderChecklistItem.Services;
using Energy.Shared.Models.V1.Operations.WorkOrderChecklistItem.Requests;
using Energy.Shared.Models.V1.Operations.WorkOrderChecklistItem.Responses;

namespace Energy.Infrastructure.Modules.Operations.WorkOrderChecklistItem.Services;

/// <summary>WorkOrderChecklistItem CRUD servisi (projection, pagination, soft-delete).</summary>
public class WorkOrderChecklistItemService : IWorkOrderChecklistItemService
{
    private readonly AppDbContext _db;

    public WorkOrderChecklistItemService(AppDbContext db) => _db = db;

    public async Task<BaseResponse<PaginatedResponse<WorkOrderChecklistItemListResponse>>> GetListAsync(GetWorkOrderChecklistItemListRequest request, CancellationToken ct = default)
    {
        var query = _db.WorkOrderChecklistItems.AsNoTracking();
        var total = await query.CountAsync(ct);
        var items = await query
            .OrderByDescending(e => e.Id)
            .Skip((request.PageNumber - 1) * request.PageSize)
            .Take(request.PageSize)
            .Select(e => new WorkOrderChecklistItemListResponse
            {
                Id = e.Id,
                WorkOrderChecklistId = e.WorkOrderChecklistId,
                Description = e.Description,
                IsRequired = e.IsRequired,
                IsCompleted = e.IsCompleted,
                CreatedAt = e.CreatedAt
            })
            .ToListAsync(ct);
        var page = PaginatedResponse<WorkOrderChecklistItemListResponse>.Create(items, request.PageNumber, request.PageSize, total);
        return BaseResponse<PaginatedResponse<WorkOrderChecklistItemListResponse>>.Success(page);
    }

    public async Task<BaseResponse<WorkOrderChecklistItemDetailResponse>> GetByIdAsync(Guid id, CancellationToken ct = default)
    {
        var dto = await _db.WorkOrderChecklistItems.AsNoTracking().Where(e => e.Id == id)
            .Select(e => new WorkOrderChecklistItemDetailResponse
            {
                Id = e.Id,
                CreatedAt = e.CreatedAt,
                CreatedBy = e.CreatedBy,
                UpdatedAt = e.UpdatedAt,
                UpdatedBy = e.UpdatedBy,
                IsDeleted = e.IsDeleted,
                DeletedAt = e.DeletedAt,
                DeletedBy = e.DeletedBy,
                WorkOrderChecklistId = e.WorkOrderChecklistId,
                Description = e.Description,
                IsRequired = e.IsRequired,
                IsCompleted = e.IsCompleted
            }).FirstOrDefaultAsync(ct);
        return dto is null
            ? BaseResponse<WorkOrderChecklistItemDetailResponse>.Failure("NotFound")
            : BaseResponse<WorkOrderChecklistItemDetailResponse>.Success(dto);
    }

    public async Task<BaseResponse<Guid>> CreateAsync(CreateWorkOrderChecklistItemRequest request, CancellationToken ct = default)
    {
        var entity = new global::Energy.Domain.Modules.Operations.WorkOrderChecklistItem
        {
            Id = Guid.NewGuid(),
            WorkOrderChecklistId = request.WorkOrderChecklistId,
            Description = request.Description,
            IsRequired = request.IsRequired,
            IsCompleted = request.IsCompleted,
            CreatedAt = DateTime.UtcNow,
        };
        _db.WorkOrderChecklistItems.Add(entity);
        await _db.SaveChangesAsync(ct);
        return BaseResponse<Guid>.Success(entity.Id, "Created");
    }

    public async Task<BaseResponse<bool>> UpdateAsync(Guid id, UpdateWorkOrderChecklistItemRequest request, CancellationToken ct = default)
    {
        var entity = await _db.WorkOrderChecklistItems.FirstOrDefaultAsync(e => e.Id == id, ct);
        if (entity is null) return BaseResponse<bool>.Failure("NotFound");
            entity.WorkOrderChecklistId = request.WorkOrderChecklistId;
            entity.Description = request.Description;
            entity.IsRequired = request.IsRequired;
            entity.IsCompleted = request.IsCompleted;
        entity.UpdatedAt = DateTime.UtcNow;
        await _db.SaveChangesAsync(ct);
        return BaseResponse<bool>.Success(true, "Updated");
    }

    public async Task<BaseResponse<bool>> DeleteAsync(Guid id, CancellationToken ct = default)
    {
        var entity = await _db.WorkOrderChecklistItems.FirstOrDefaultAsync(e => e.Id == id, ct);
        if (entity is null) return BaseResponse<bool>.Failure("NotFound");
        entity.IsDeleted = true;
        entity.DeletedAt = DateTime.UtcNow;
        await _db.SaveChangesAsync(ct);
        return BaseResponse<bool>.Success(true, "Deleted");
    }
}
