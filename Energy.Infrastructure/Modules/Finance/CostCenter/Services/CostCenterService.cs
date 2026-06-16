using Microsoft.EntityFrameworkCore;
using Energy.Infrastructure.Persistence;
using Energy.Shared.Models.V1.Common.Responses;
using Energy.Application.Modules.Finance.CostCenter.Services;
using Energy.Shared.Models.V1.Finance.CostCenter.Requests;
using Energy.Shared.Models.V1.Finance.CostCenter.Responses;

namespace Energy.Infrastructure.Modules.Finance.CostCenter.Services;

/// <summary>CostCenter CRUD servisi (projection, pagination, soft-delete).</summary>
public class CostCenterService : ICostCenterService
{
    private readonly EnergyDbContext _db;

    public CostCenterService(EnergyDbContext db) => _db = db;

    public async Task<BaseResponse<PaginatedResponse<CostCenterListResponse>>> GetListAsync(GetCostCenterListRequest request, CancellationToken ct = default)
    {
        var query = _db.CostCenters.AsNoTracking();
        var total = await query.CountAsync(ct);
        var items = await query
            .OrderByDescending(e => e.Id)
            .Skip((request.PageNumber - 1) * request.PageSize)
            .Take(request.PageSize)
            .Select(e => new CostCenterListResponse
            {
                Id = e.Id,
                ParentCostCenterId = e.ParentCostCenterId,
                Code = e.Code,
                Name = e.Name,
                IsActive = e.IsActive,
                CreatedAt = e.CreatedAt
            })
            .ToListAsync(ct);
        var page = PaginatedResponse<CostCenterListResponse>.Create(items, request.PageNumber, request.PageSize, total);
        return BaseResponse<PaginatedResponse<CostCenterListResponse>>.Success(page);
    }

    public async Task<BaseResponse<CostCenterDetailResponse>> GetByIdAsync(Guid id, CancellationToken ct = default)
    {
        var dto = await _db.CostCenters.AsNoTracking().Where(e => e.Id == id)
            .Select(e => new CostCenterDetailResponse
            {
                Id = e.Id,
                CreatedAt = e.CreatedAt,
                CreatedBy = e.CreatedBy,
                UpdatedAt = e.UpdatedAt,
                UpdatedBy = e.UpdatedBy,
                IsDeleted = e.IsDeleted,
                DeletedAt = e.DeletedAt,
                DeletedBy = e.DeletedBy,
                ParentCostCenterId = e.ParentCostCenterId,
                Code = e.Code,
                Name = e.Name,
                IsActive = e.IsActive
            }).FirstOrDefaultAsync(ct);
        return dto is null
            ? BaseResponse<CostCenterDetailResponse>.Failure("NotFound")
            : BaseResponse<CostCenterDetailResponse>.Success(dto);
    }

    public async Task<BaseResponse<Guid>> CreateAsync(CreateCostCenterRequest request, CancellationToken ct = default)
    {
        var entity = new global::Energy.Domain.Modules.Finance.CostCenter
        {
            Id = Guid.NewGuid(),
            ParentCostCenterId = request.ParentCostCenterId,
            Code = request.Code,
            Name = request.Name,
            IsActive = request.IsActive,
            CreatedAt = DateTime.UtcNow,
        };
        _db.CostCenters.Add(entity);
        await _db.SaveChangesAsync(ct);
        return BaseResponse<Guid>.Success(entity.Id, "Created");
    }

    public async Task<BaseResponse<bool>> UpdateAsync(Guid id, UpdateCostCenterRequest request, CancellationToken ct = default)
    {
        var entity = await _db.CostCenters.FirstOrDefaultAsync(e => e.Id == id, ct);
        if (entity is null) return BaseResponse<bool>.Failure("NotFound");
            entity.ParentCostCenterId = request.ParentCostCenterId;
            entity.Code = request.Code;
            entity.Name = request.Name;
            entity.IsActive = request.IsActive;
        entity.UpdatedAt = DateTime.UtcNow;
        await _db.SaveChangesAsync(ct);
        return BaseResponse<bool>.Success(true, "Updated");
    }

    public async Task<BaseResponse<bool>> DeleteAsync(Guid id, CancellationToken ct = default)
    {
        var entity = await _db.CostCenters.FirstOrDefaultAsync(e => e.Id == id, ct);
        if (entity is null) return BaseResponse<bool>.Failure("NotFound");
        entity.IsDeleted = true;
        entity.DeletedAt = DateTime.UtcNow;
        await _db.SaveChangesAsync(ct);
        return BaseResponse<bool>.Success(true, "Deleted");
    }
}
