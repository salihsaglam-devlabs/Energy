using Microsoft.EntityFrameworkCore;
using Energy.Infrastructure.Persistence;
using Energy.Shared.Models.V1.Common.Responses;
using Energy.Application.Modules.Operations.WorkOrderStatusHistory.Services;
using Energy.Shared.Models.V1.Operations.WorkOrderStatusHistory.Requests;
using Energy.Shared.Models.V1.Operations.WorkOrderStatusHistory.Responses;

namespace Energy.Infrastructure.Modules.Operations.WorkOrderStatusHistory.Services;

/// <summary>WorkOrderStatusHistory CRUD servisi (projection, pagination, soft-delete).</summary>
public class WorkOrderStatusHistoryService : IWorkOrderStatusHistoryService
{
    private readonly AppDbContext _db;

    public WorkOrderStatusHistoryService(AppDbContext db) => _db = db;

    public async Task<BaseResponse<PaginatedResponse<WorkOrderStatusHistoryListResponse>>> GetListAsync(GetWorkOrderStatusHistoryListRequest request, CancellationToken ct = default)
    {
        var query = _db.WorkOrderStatusHistories.AsNoTracking();
        var total = await query.CountAsync(ct);
        var items = await query
            .OrderByDescending(e => e.Id)
            .Skip((request.PageNumber - 1) * request.PageSize)
            .Take(request.PageSize)
            .Select(e => new WorkOrderStatusHistoryListResponse
            {
                Id = e.Id,
                WorkOrderId = e.WorkOrderId,
                FromStatus = e.FromStatus,
                ToStatus = e.ToStatus,
                ChangedAt = e.ChangedAt,
                Note = e.Note,
                CreatedAt = e.CreatedAt
            })
            .ToListAsync(ct);
        var page = PaginatedResponse<WorkOrderStatusHistoryListResponse>.Create(items, request.PageNumber, request.PageSize, total);
        return BaseResponse<PaginatedResponse<WorkOrderStatusHistoryListResponse>>.Success(page);
    }

    public async Task<BaseResponse<WorkOrderStatusHistoryDetailResponse>> GetByIdAsync(Guid id, CancellationToken ct = default)
    {
        var dto = await _db.WorkOrderStatusHistories.AsNoTracking().Where(e => e.Id == id)
            .Select(e => new WorkOrderStatusHistoryDetailResponse
            {
                Id = e.Id,
                CreatedAt = e.CreatedAt,
                CreatedBy = e.CreatedBy,
                UpdatedAt = e.UpdatedAt,
                UpdatedBy = e.UpdatedBy,
                IsDeleted = e.IsDeleted,
                DeletedAt = e.DeletedAt,
                DeletedBy = e.DeletedBy,
                WorkOrderId = e.WorkOrderId,
                FromStatus = e.FromStatus,
                ToStatus = e.ToStatus,
                ChangedAt = e.ChangedAt,
                Note = e.Note
            }).FirstOrDefaultAsync(ct);
        return dto is null
            ? BaseResponse<WorkOrderStatusHistoryDetailResponse>.Failure("NotFound")
            : BaseResponse<WorkOrderStatusHistoryDetailResponse>.Success(dto);
    }

    public async Task<BaseResponse<Guid>> CreateAsync(CreateWorkOrderStatusHistoryRequest request, CancellationToken ct = default)
    {
        var entity = new global::Energy.Domain.Modules.Operations.WorkOrderStatusHistory
        {
            Id = Guid.NewGuid(),
            WorkOrderId = request.WorkOrderId,
            FromStatus = request.FromStatus,
            ToStatus = request.ToStatus,
            ChangedAt = request.ChangedAt,
            Note = request.Note,
            CreatedAt = DateTime.UtcNow,
        };
        _db.WorkOrderStatusHistories.Add(entity);
        await _db.SaveChangesAsync(ct);
        return BaseResponse<Guid>.Success(entity.Id, "Created");
    }

    public async Task<BaseResponse<bool>> UpdateAsync(Guid id, UpdateWorkOrderStatusHistoryRequest request, CancellationToken ct = default)
    {
        var entity = await _db.WorkOrderStatusHistories.FirstOrDefaultAsync(e => e.Id == id, ct);
        if (entity is null) return BaseResponse<bool>.Failure("NotFound");
            entity.WorkOrderId = request.WorkOrderId;
            entity.FromStatus = request.FromStatus;
            entity.ToStatus = request.ToStatus;
            entity.ChangedAt = request.ChangedAt;
            entity.Note = request.Note;
        entity.UpdatedAt = DateTime.UtcNow;
        await _db.SaveChangesAsync(ct);
        return BaseResponse<bool>.Success(true, "Updated");
    }

    public async Task<BaseResponse<bool>> DeleteAsync(Guid id, CancellationToken ct = default)
    {
        var entity = await _db.WorkOrderStatusHistories.FirstOrDefaultAsync(e => e.Id == id, ct);
        if (entity is null) return BaseResponse<bool>.Failure("NotFound");
        entity.IsDeleted = true;
        entity.DeletedAt = DateTime.UtcNow;
        await _db.SaveChangesAsync(ct);
        return BaseResponse<bool>.Success(true, "Deleted");
    }
}
