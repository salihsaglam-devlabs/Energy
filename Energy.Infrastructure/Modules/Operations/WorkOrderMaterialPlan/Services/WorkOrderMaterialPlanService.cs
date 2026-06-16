using Microsoft.EntityFrameworkCore;
using Energy.Infrastructure.Persistence;
using Energy.Shared.Models.V1.Common.Responses;
using Energy.Application.Modules.Operations.WorkOrderMaterialPlan.Services;
using Energy.Shared.Models.V1.Operations.WorkOrderMaterialPlan.Requests;
using Energy.Shared.Models.V1.Operations.WorkOrderMaterialPlan.Responses;

namespace Energy.Infrastructure.Modules.Operations.WorkOrderMaterialPlan.Services;

/// <summary>WorkOrderMaterialPlan CRUD servisi (projection, pagination, soft-delete).</summary>
public class WorkOrderMaterialPlanService : IWorkOrderMaterialPlanService
{
    private readonly EnergyDbContext _db;

    public WorkOrderMaterialPlanService(EnergyDbContext db) => _db = db;

    public async Task<BaseResponse<PaginatedResponse<WorkOrderMaterialPlanListResponse>>> GetListAsync(GetWorkOrderMaterialPlanListRequest request, CancellationToken ct = default)
    {
        var query = _db.WorkOrderMaterialPlans.AsNoTracking();
        var total = await query.CountAsync(ct);
        var items = await query
            .OrderByDescending(e => e.Id)
            .Skip((request.PageNumber - 1) * request.PageSize)
            .Take(request.PageSize)
            .Select(e => new WorkOrderMaterialPlanListResponse
            {
                Id = e.Id,
                WorkOrderId = e.WorkOrderId,
                MaterialId = e.MaterialId,
                PlannedQuantity = e.PlannedQuantity,
                CreatedAt = e.CreatedAt
            })
            .ToListAsync(ct);
        var page = PaginatedResponse<WorkOrderMaterialPlanListResponse>.Create(items, request.PageNumber, request.PageSize, total);
        return BaseResponse<PaginatedResponse<WorkOrderMaterialPlanListResponse>>.Success(page);
    }

    public async Task<BaseResponse<WorkOrderMaterialPlanDetailResponse>> GetByIdAsync(Guid id, CancellationToken ct = default)
    {
        var dto = await _db.WorkOrderMaterialPlans.AsNoTracking().Where(e => e.Id == id)
            .Select(e => new WorkOrderMaterialPlanDetailResponse
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
                MaterialId = e.MaterialId,
                PlannedQuantity = e.PlannedQuantity
            }).FirstOrDefaultAsync(ct);
        return dto is null
            ? BaseResponse<WorkOrderMaterialPlanDetailResponse>.Failure("NotFound")
            : BaseResponse<WorkOrderMaterialPlanDetailResponse>.Success(dto);
    }

    public async Task<BaseResponse<Guid>> CreateAsync(CreateWorkOrderMaterialPlanRequest request, CancellationToken ct = default)
    {
        var entity = new global::Energy.Domain.Modules.Operations.WorkOrderMaterialPlan
        {
            Id = Guid.NewGuid(),
            WorkOrderId = request.WorkOrderId,
            MaterialId = request.MaterialId,
            PlannedQuantity = request.PlannedQuantity,
            CreatedAt = DateTime.UtcNow,
        };
        _db.WorkOrderMaterialPlans.Add(entity);
        await _db.SaveChangesAsync(ct);
        return BaseResponse<Guid>.Success(entity.Id, "Created");
    }

    public async Task<BaseResponse<bool>> UpdateAsync(Guid id, UpdateWorkOrderMaterialPlanRequest request, CancellationToken ct = default)
    {
        var entity = await _db.WorkOrderMaterialPlans.FirstOrDefaultAsync(e => e.Id == id, ct);
        if (entity is null) return BaseResponse<bool>.Failure("NotFound");
            entity.WorkOrderId = request.WorkOrderId;
            entity.MaterialId = request.MaterialId;
            entity.PlannedQuantity = request.PlannedQuantity;
        entity.UpdatedAt = DateTime.UtcNow;
        await _db.SaveChangesAsync(ct);
        return BaseResponse<bool>.Success(true, "Updated");
    }

    public async Task<BaseResponse<bool>> DeleteAsync(Guid id, CancellationToken ct = default)
    {
        var entity = await _db.WorkOrderMaterialPlans.FirstOrDefaultAsync(e => e.Id == id, ct);
        if (entity is null) return BaseResponse<bool>.Failure("NotFound");
        entity.IsDeleted = true;
        entity.DeletedAt = DateTime.UtcNow;
        await _db.SaveChangesAsync(ct);
        return BaseResponse<bool>.Success(true, "Deleted");
    }
}
