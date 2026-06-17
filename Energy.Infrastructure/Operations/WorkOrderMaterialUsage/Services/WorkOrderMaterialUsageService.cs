using Microsoft.EntityFrameworkCore;
using Energy.Infrastructure.Persistence;
using Energy.Shared.Models.V1.Common.Responses;
using Energy.Application.Operations.WorkOrderMaterialUsage.Services;
using Energy.Shared.Models.V1.Operations.WorkOrderMaterialUsage.Requests;
using Energy.Shared.Models.V1.Operations.WorkOrderMaterialUsage.Responses;

namespace Energy.Infrastructure.Operations.WorkOrderMaterialUsage.Services;

/// <summary>WorkOrderMaterialUsage CRUD servisi (projection, pagination, soft-delete).</summary>
public class WorkOrderMaterialUsageService : IWorkOrderMaterialUsageService
{
    private readonly AppDbContext _db;

    public WorkOrderMaterialUsageService(AppDbContext db) => _db = db;

    public async Task<BaseResponse<PaginatedResponse<WorkOrderMaterialUsageListResponse>>> GetListAsync(GetWorkOrderMaterialUsageListRequest request, CancellationToken ct = default)
    {
        var query = _db.WorkOrderMaterialUsages.AsNoTracking();
        var total = await query.CountAsync(ct);
        var items = await query
            .OrderByDescending(e => e.Id)
            .Skip((request.PageNumber - 1) * request.PageSize)
            .Take(request.PageSize)
            .Select(e => new WorkOrderMaterialUsageListResponse
            {
                Id = e.Id,
                WorkOrderId = e.WorkOrderId,
                StockDocumentLineId = e.StockDocumentLineId,
                MaterialId = e.MaterialId,
                UsedQuantity = e.UsedQuantity,
                CreatedAt = e.CreatedAt
            })
            .ToListAsync(ct);
        var page = PaginatedResponse<WorkOrderMaterialUsageListResponse>.Create(items, request.PageNumber, request.PageSize, total);
        return BaseResponse<PaginatedResponse<WorkOrderMaterialUsageListResponse>>.Success(page);
    }

    public async Task<BaseResponse<WorkOrderMaterialUsageDetailResponse>> GetByIdAsync(Guid id, CancellationToken ct = default)
    {
        var dto = await _db.WorkOrderMaterialUsages.AsNoTracking().Where(e => e.Id == id)
            .Select(e => new WorkOrderMaterialUsageDetailResponse
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
                StockDocumentLineId = e.StockDocumentLineId,
                MaterialId = e.MaterialId,
                UsedQuantity = e.UsedQuantity
            }).FirstOrDefaultAsync(ct);
        return dto is null
            ? BaseResponse<WorkOrderMaterialUsageDetailResponse>.Failure("NotFound")
            : BaseResponse<WorkOrderMaterialUsageDetailResponse>.Success(dto);
    }

    public async Task<BaseResponse<Guid>> CreateAsync(CreateWorkOrderMaterialUsageRequest request, CancellationToken ct = default)
    {
        var entity = new global::Energy.Domain.Operations.WorkOrderMaterialUsage
        {
            Id = Guid.NewGuid(),
            WorkOrderId = request.WorkOrderId,
            StockDocumentLineId = request.StockDocumentLineId,
            MaterialId = request.MaterialId,
            UsedQuantity = request.UsedQuantity,
            CreatedAt = DateTime.UtcNow,
        };
        _db.WorkOrderMaterialUsages.Add(entity);
        await _db.SaveChangesAsync(ct);
        return BaseResponse<Guid>.Success(entity.Id, "Created");
    }

    public async Task<BaseResponse<bool>> UpdateAsync(Guid id, UpdateWorkOrderMaterialUsageRequest request, CancellationToken ct = default)
    {
        var entity = await _db.WorkOrderMaterialUsages.FirstOrDefaultAsync(e => e.Id == id, ct);
        if (entity is null) return BaseResponse<bool>.Failure("NotFound");
            entity.WorkOrderId = request.WorkOrderId;
            entity.StockDocumentLineId = request.StockDocumentLineId;
            entity.MaterialId = request.MaterialId;
            entity.UsedQuantity = request.UsedQuantity;
        entity.UpdatedAt = DateTime.UtcNow;
        await _db.SaveChangesAsync(ct);
        return BaseResponse<bool>.Success(true, "Updated");
    }

    public async Task<BaseResponse<bool>> DeleteAsync(Guid id, CancellationToken ct = default)
    {
        var entity = await _db.WorkOrderMaterialUsages.FirstOrDefaultAsync(e => e.Id == id, ct);
        if (entity is null) return BaseResponse<bool>.Failure("NotFound");
        entity.IsDeleted = true;
        entity.DeletedAt = DateTime.UtcNow;
        await _db.SaveChangesAsync(ct);
        return BaseResponse<bool>.Success(true, "Deleted");
    }
}
