using Microsoft.EntityFrameworkCore;
using Energy.Infrastructure.Persistence;
using Energy.Shared.Models.V1.Common.Responses;
using Energy.Application.Modules.Operations.WorkOrder.Services;
using Energy.Shared.Models.V1.Operations.WorkOrder.Requests;
using Energy.Shared.Models.V1.Operations.WorkOrder.Responses;

namespace Energy.Infrastructure.Modules.Operations.WorkOrder.Services;

/// <summary>WorkOrder CRUD servisi (projection, pagination, soft-delete).</summary>
public class WorkOrderService : IWorkOrderService
{
    private readonly AppDbContext _db;

    public WorkOrderService(AppDbContext db) => _db = db;

    public async Task<BaseResponse<PaginatedResponse<WorkOrderListResponse>>> GetListAsync(GetWorkOrderListRequest request, CancellationToken ct = default)
    {
        var query = _db.WorkOrders.AsNoTracking();
        var total = await query.CountAsync(ct);
        var items = await query
            .OrderByDescending(e => e.Id)
            .Skip((request.PageNumber - 1) * request.PageSize)
            .Take(request.PageSize)
            .Select(e => new WorkOrderListResponse
            {
                Id = e.Id,
                WorkOrderTypeId = e.WorkOrderTypeId,
                ProjectId = e.ProjectId,
                ProjectPhaseId = e.ProjectPhaseId,
                ProjectLocationId = e.ProjectLocationId,
                Status = e.Status,
                WorkOrderNo = e.WorkOrderNo,
                Title = e.Title,
                Description = e.Description,
                PlannedStart = e.PlannedStart,
                PlannedEnd = e.PlannedEnd,
                CreatedAt = e.CreatedAt
            })
            .ToListAsync(ct);
        var page = PaginatedResponse<WorkOrderListResponse>.Create(items, request.PageNumber, request.PageSize, total);
        return BaseResponse<PaginatedResponse<WorkOrderListResponse>>.Success(page);
    }

    public async Task<BaseResponse<WorkOrderDetailResponse>> GetByIdAsync(Guid id, CancellationToken ct = default)
    {
        var dto = await _db.WorkOrders.AsNoTracking().Where(e => e.Id == id)
            .Select(e => new WorkOrderDetailResponse
            {
                Id = e.Id,
                CreatedAt = e.CreatedAt,
                CreatedBy = e.CreatedBy,
                UpdatedAt = e.UpdatedAt,
                UpdatedBy = e.UpdatedBy,
                IsDeleted = e.IsDeleted,
                DeletedAt = e.DeletedAt,
                DeletedBy = e.DeletedBy,
                WorkOrderTypeId = e.WorkOrderTypeId,
                ProjectId = e.ProjectId,
                ProjectPhaseId = e.ProjectPhaseId,
                ProjectLocationId = e.ProjectLocationId,
                Status = e.Status,
                WorkOrderNo = e.WorkOrderNo,
                Title = e.Title,
                Description = e.Description,
                PlannedStart = e.PlannedStart,
                PlannedEnd = e.PlannedEnd
            }).FirstOrDefaultAsync(ct);
        return dto is null
            ? BaseResponse<WorkOrderDetailResponse>.Failure("NotFound")
            : BaseResponse<WorkOrderDetailResponse>.Success(dto);
    }

    public async Task<BaseResponse<Guid>> CreateAsync(CreateWorkOrderRequest request, CancellationToken ct = default)
    {
        var entity = new global::Energy.Domain.Modules.Operations.WorkOrder
        {
            Id = Guid.NewGuid(),
            WorkOrderTypeId = request.WorkOrderTypeId,
            ProjectId = request.ProjectId,
            ProjectPhaseId = request.ProjectPhaseId,
            ProjectLocationId = request.ProjectLocationId,
            Status = request.Status,
            WorkOrderNo = request.WorkOrderNo,
            Title = request.Title,
            Description = request.Description,
            PlannedStart = request.PlannedStart,
            PlannedEnd = request.PlannedEnd,
            CreatedAt = DateTime.UtcNow,
        };
        _db.WorkOrders.Add(entity);
        await _db.SaveChangesAsync(ct);
        return BaseResponse<Guid>.Success(entity.Id, "Created");
    }

    public async Task<BaseResponse<bool>> UpdateAsync(Guid id, UpdateWorkOrderRequest request, CancellationToken ct = default)
    {
        var entity = await _db.WorkOrders.FirstOrDefaultAsync(e => e.Id == id, ct);
        if (entity is null) return BaseResponse<bool>.Failure("NotFound");
            entity.WorkOrderTypeId = request.WorkOrderTypeId;
            entity.ProjectId = request.ProjectId;
            entity.ProjectPhaseId = request.ProjectPhaseId;
            entity.ProjectLocationId = request.ProjectLocationId;
            entity.Status = request.Status;
            entity.WorkOrderNo = request.WorkOrderNo;
            entity.Title = request.Title;
            entity.Description = request.Description;
            entity.PlannedStart = request.PlannedStart;
            entity.PlannedEnd = request.PlannedEnd;
        entity.UpdatedAt = DateTime.UtcNow;
        await _db.SaveChangesAsync(ct);
        return BaseResponse<bool>.Success(true, "Updated");
    }

    public async Task<BaseResponse<bool>> DeleteAsync(Guid id, CancellationToken ct = default)
    {
        var entity = await _db.WorkOrders.FirstOrDefaultAsync(e => e.Id == id, ct);
        if (entity is null) return BaseResponse<bool>.Failure("NotFound");
        entity.IsDeleted = true;
        entity.DeletedAt = DateTime.UtcNow;
        await _db.SaveChangesAsync(ct);
        return BaseResponse<bool>.Success(true, "Deleted");
    }
}
