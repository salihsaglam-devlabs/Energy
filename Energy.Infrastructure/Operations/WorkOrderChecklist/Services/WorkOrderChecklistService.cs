using Microsoft.EntityFrameworkCore;
using Energy.Infrastructure.Persistence;
using Energy.Shared.Models.V1.Common.Responses;
using Energy.Application.Operations.WorkOrderChecklist.Services;
using Energy.Shared.Models.V1.Operations.WorkOrderChecklist.Requests;
using Energy.Shared.Models.V1.Operations.WorkOrderChecklist.Responses;

namespace Energy.Infrastructure.Operations.WorkOrderChecklist.Services;

/// <summary>WorkOrderChecklist CRUD servisi (projection, pagination, soft-delete).</summary>
public class WorkOrderChecklistService : IWorkOrderChecklistService
{
    private readonly AppDbContext _db;

    public WorkOrderChecklistService(AppDbContext db) => _db = db;

    public async Task<BaseResponse<PaginatedResponse<WorkOrderChecklistListResponse>>> GetListAsync(GetWorkOrderChecklistListRequest request, CancellationToken ct = default)
    {
        var query = _db.WorkOrderChecklists.AsNoTracking();
        var total = await query.CountAsync(ct);
        var items = await query
            .OrderByDescending(e => e.Id)
            .Skip((request.PageNumber - 1) * request.PageSize)
            .Take(request.PageSize)
            .Select(e => new WorkOrderChecklistListResponse
            {
                Id = e.Id,
                WorkOrderId = e.WorkOrderId,
                Name = e.Name,
                IsRequired = e.IsRequired,
                CreatedAt = e.CreatedAt
            })
            .ToListAsync(ct);
        var page = PaginatedResponse<WorkOrderChecklistListResponse>.Create(items, request.PageNumber, request.PageSize, total);
        return BaseResponse<PaginatedResponse<WorkOrderChecklistListResponse>>.Success(page);
    }

    public async Task<BaseResponse<WorkOrderChecklistDetailResponse>> GetByIdAsync(Guid id, CancellationToken ct = default)
    {
        var dto = await _db.WorkOrderChecklists.AsNoTracking().Where(e => e.Id == id)
            .Select(e => new WorkOrderChecklistDetailResponse
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
                Name = e.Name,
                IsRequired = e.IsRequired
            }).FirstOrDefaultAsync(ct);
        return dto is null
            ? BaseResponse<WorkOrderChecklistDetailResponse>.Failure("NotFound")
            : BaseResponse<WorkOrderChecklistDetailResponse>.Success(dto);
    }

    public async Task<BaseResponse<Guid>> CreateAsync(CreateWorkOrderChecklistRequest request, CancellationToken ct = default)
    {
        var entity = new global::Energy.Domain.Operations.WorkOrderChecklist
        {
            Id = Guid.NewGuid(),
            WorkOrderId = request.WorkOrderId,
            Name = request.Name,
            IsRequired = request.IsRequired,
            CreatedAt = DateTime.UtcNow,
        };
        _db.WorkOrderChecklists.Add(entity);
        await _db.SaveChangesAsync(ct);
        return BaseResponse<Guid>.Success(entity.Id, "Created");
    }

    public async Task<BaseResponse<bool>> UpdateAsync(Guid id, UpdateWorkOrderChecklistRequest request, CancellationToken ct = default)
    {
        var entity = await _db.WorkOrderChecklists.FirstOrDefaultAsync(e => e.Id == id, ct);
        if (entity is null) return BaseResponse<bool>.Failure("NotFound");
            entity.WorkOrderId = request.WorkOrderId;
            entity.Name = request.Name;
            entity.IsRequired = request.IsRequired;
        entity.UpdatedAt = DateTime.UtcNow;
        await _db.SaveChangesAsync(ct);
        return BaseResponse<bool>.Success(true, "Updated");
    }

    public async Task<BaseResponse<bool>> DeleteAsync(Guid id, CancellationToken ct = default)
    {
        var entity = await _db.WorkOrderChecklists.FirstOrDefaultAsync(e => e.Id == id, ct);
        if (entity is null) return BaseResponse<bool>.Failure("NotFound");
        entity.IsDeleted = true;
        entity.DeletedAt = DateTime.UtcNow;
        await _db.SaveChangesAsync(ct);
        return BaseResponse<bool>.Success(true, "Deleted");
    }
}
