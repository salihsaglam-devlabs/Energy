using Microsoft.EntityFrameworkCore;
using Energy.Infrastructure.Persistence;
using Energy.Shared.Models.V1.Common.Responses;
using Energy.Application.Modules.Operations.WorkOrderType.Services;
using Energy.Shared.Models.V1.Operations.WorkOrderType.Requests;
using Energy.Shared.Models.V1.Operations.WorkOrderType.Responses;

namespace Energy.Infrastructure.Modules.Operations.WorkOrderType.Services;

/// <summary>WorkOrderType CRUD servisi (projection, pagination, soft-delete).</summary>
public class WorkOrderTypeService : IWorkOrderTypeService
{
    private readonly EnergyDbContext _db;

    public WorkOrderTypeService(EnergyDbContext db) => _db = db;

    public async Task<BaseResponse<PaginatedResponse<WorkOrderTypeListResponse>>> GetListAsync(GetWorkOrderTypeListRequest request, CancellationToken ct = default)
    {
        var query = _db.WorkOrderTypes.AsNoTracking();
        var total = await query.CountAsync(ct);
        var items = await query
            .OrderByDescending(e => e.Id)
            .Skip((request.PageNumber - 1) * request.PageSize)
            .Take(request.PageSize)
            .Select(e => new WorkOrderTypeListResponse
            {
                Id = e.Id,
                Code = e.Code,
                Name = e.Name,
                IsActive = e.IsActive,
                CreatedAt = e.CreatedAt
            })
            .ToListAsync(ct);
        var page = PaginatedResponse<WorkOrderTypeListResponse>.Create(items, request.PageNumber, request.PageSize, total);
        return BaseResponse<PaginatedResponse<WorkOrderTypeListResponse>>.Success(page);
    }

    public async Task<BaseResponse<WorkOrderTypeDetailResponse>> GetByIdAsync(Guid id, CancellationToken ct = default)
    {
        var dto = await _db.WorkOrderTypes.AsNoTracking().Where(e => e.Id == id)
            .Select(e => new WorkOrderTypeDetailResponse
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
                IsActive = e.IsActive
            }).FirstOrDefaultAsync(ct);
        return dto is null
            ? BaseResponse<WorkOrderTypeDetailResponse>.Failure("NotFound")
            : BaseResponse<WorkOrderTypeDetailResponse>.Success(dto);
    }

    public async Task<BaseResponse<Guid>> CreateAsync(CreateWorkOrderTypeRequest request, CancellationToken ct = default)
    {
        var entity = new global::Energy.Domain.Modules.Operations.WorkOrderType
        {
            Id = Guid.NewGuid(),
            Code = request.Code,
            Name = request.Name,
            IsActive = request.IsActive,
            CreatedAt = DateTime.UtcNow,
        };
        _db.WorkOrderTypes.Add(entity);
        await _db.SaveChangesAsync(ct);
        return BaseResponse<Guid>.Success(entity.Id, "Created");
    }

    public async Task<BaseResponse<bool>> UpdateAsync(Guid id, UpdateWorkOrderTypeRequest request, CancellationToken ct = default)
    {
        var entity = await _db.WorkOrderTypes.FirstOrDefaultAsync(e => e.Id == id, ct);
        if (entity is null) return BaseResponse<bool>.Failure("NotFound");
            entity.Code = request.Code;
            entity.Name = request.Name;
            entity.IsActive = request.IsActive;
        entity.UpdatedAt = DateTime.UtcNow;
        await _db.SaveChangesAsync(ct);
        return BaseResponse<bool>.Success(true, "Updated");
    }

    public async Task<BaseResponse<bool>> DeleteAsync(Guid id, CancellationToken ct = default)
    {
        var entity = await _db.WorkOrderTypes.FirstOrDefaultAsync(e => e.Id == id, ct);
        if (entity is null) return BaseResponse<bool>.Failure("NotFound");
        entity.IsDeleted = true;
        entity.DeletedAt = DateTime.UtcNow;
        await _db.SaveChangesAsync(ct);
        return BaseResponse<bool>.Success(true, "Deleted");
    }
}
