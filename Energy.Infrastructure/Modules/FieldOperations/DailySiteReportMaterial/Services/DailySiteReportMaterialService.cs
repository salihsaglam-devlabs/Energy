using Microsoft.EntityFrameworkCore;
using Energy.Infrastructure.Persistence;
using Energy.Shared.Models.V1.Common.Responses;
using Energy.Application.Modules.FieldOperations.DailySiteReportMaterial.Services;
using Energy.Shared.Models.V1.FieldOperations.DailySiteReportMaterial.Requests;
using Energy.Shared.Models.V1.FieldOperations.DailySiteReportMaterial.Responses;

namespace Energy.Infrastructure.Modules.FieldOperations.DailySiteReportMaterial.Services;

/// <summary>DailySiteReportMaterial CRUD servisi (projection, pagination, soft-delete).</summary>
public class DailySiteReportMaterialService : IDailySiteReportMaterialService
{
    private readonly EnergyDbContext _db;

    public DailySiteReportMaterialService(EnergyDbContext db) => _db = db;

    public async Task<BaseResponse<PaginatedResponse<DailySiteReportMaterialListResponse>>> GetListAsync(GetDailySiteReportMaterialListRequest request, CancellationToken ct = default)
    {
        var query = _db.DailySiteReportMaterials.AsNoTracking();
        var total = await query.CountAsync(ct);
        var items = await query
            .OrderByDescending(e => e.Id)
            .Skip((request.PageNumber - 1) * request.PageSize)
            .Take(request.PageSize)
            .Select(e => new DailySiteReportMaterialListResponse
            {
                Id = e.Id,
                DailySiteReportId = e.DailySiteReportId,
                MaterialId = e.MaterialId,
                Quantity = e.Quantity,
                CreatedAt = e.CreatedAt
            })
            .ToListAsync(ct);
        var page = PaginatedResponse<DailySiteReportMaterialListResponse>.Create(items, request.PageNumber, request.PageSize, total);
        return BaseResponse<PaginatedResponse<DailySiteReportMaterialListResponse>>.Success(page);
    }

    public async Task<BaseResponse<DailySiteReportMaterialDetailResponse>> GetByIdAsync(Guid id, CancellationToken ct = default)
    {
        var dto = await _db.DailySiteReportMaterials.AsNoTracking().Where(e => e.Id == id)
            .Select(e => new DailySiteReportMaterialDetailResponse
            {
                Id = e.Id,
                CreatedAt = e.CreatedAt,
                CreatedBy = e.CreatedBy,
                UpdatedAt = e.UpdatedAt,
                UpdatedBy = e.UpdatedBy,
                IsDeleted = e.IsDeleted,
                DeletedAt = e.DeletedAt,
                DeletedBy = e.DeletedBy,
                DailySiteReportId = e.DailySiteReportId,
                MaterialId = e.MaterialId,
                Quantity = e.Quantity
            }).FirstOrDefaultAsync(ct);
        return dto is null
            ? BaseResponse<DailySiteReportMaterialDetailResponse>.Failure("NotFound")
            : BaseResponse<DailySiteReportMaterialDetailResponse>.Success(dto);
    }

    public async Task<BaseResponse<Guid>> CreateAsync(CreateDailySiteReportMaterialRequest request, CancellationToken ct = default)
    {
        var entity = new global::Energy.Domain.Modules.FieldOperations.DailySiteReportMaterial
        {
            Id = Guid.NewGuid(),
            DailySiteReportId = request.DailySiteReportId,
            MaterialId = request.MaterialId,
            Quantity = request.Quantity,
            CreatedAt = DateTime.UtcNow,
        };
        _db.DailySiteReportMaterials.Add(entity);
        await _db.SaveChangesAsync(ct);
        return BaseResponse<Guid>.Success(entity.Id, "Created");
    }

    public async Task<BaseResponse<bool>> UpdateAsync(Guid id, UpdateDailySiteReportMaterialRequest request, CancellationToken ct = default)
    {
        var entity = await _db.DailySiteReportMaterials.FirstOrDefaultAsync(e => e.Id == id, ct);
        if (entity is null) return BaseResponse<bool>.Failure("NotFound");
            entity.DailySiteReportId = request.DailySiteReportId;
            entity.MaterialId = request.MaterialId;
            entity.Quantity = request.Quantity;
        entity.UpdatedAt = DateTime.UtcNow;
        await _db.SaveChangesAsync(ct);
        return BaseResponse<bool>.Success(true, "Updated");
    }

    public async Task<BaseResponse<bool>> DeleteAsync(Guid id, CancellationToken ct = default)
    {
        var entity = await _db.DailySiteReportMaterials.FirstOrDefaultAsync(e => e.Id == id, ct);
        if (entity is null) return BaseResponse<bool>.Failure("NotFound");
        entity.IsDeleted = true;
        entity.DeletedAt = DateTime.UtcNow;
        await _db.SaveChangesAsync(ct);
        return BaseResponse<bool>.Success(true, "Deleted");
    }
}
