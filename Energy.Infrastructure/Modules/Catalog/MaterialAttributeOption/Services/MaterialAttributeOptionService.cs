using Microsoft.EntityFrameworkCore;
using Energy.Infrastructure.Persistence;
using Energy.Shared.Models.V1.Common.Responses;
using Energy.Application.Modules.Catalog.MaterialAttributeOption.Services;
using Energy.Shared.Models.V1.Catalog.MaterialAttributeOption.Requests;
using Energy.Shared.Models.V1.Catalog.MaterialAttributeOption.Responses;

namespace Energy.Infrastructure.Modules.Catalog.MaterialAttributeOption.Services;

/// <summary>MaterialAttributeOption CRUD servisi (projection, pagination, soft-delete).</summary>
public class MaterialAttributeOptionService : IMaterialAttributeOptionService
{
    private readonly EnergyDbContext _db;

    public MaterialAttributeOptionService(EnergyDbContext db) => _db = db;

    public async Task<BaseResponse<PaginatedResponse<MaterialAttributeOptionListResponse>>> GetListAsync(GetMaterialAttributeOptionListRequest request, CancellationToken ct = default)
    {
        var query = _db.MaterialAttributeOptions.AsNoTracking();
        var total = await query.CountAsync(ct);
        var items = await query
            .OrderByDescending(e => e.Id)
            .Skip((request.PageNumber - 1) * request.PageSize)
            .Take(request.PageSize)
            .Select(e => new MaterialAttributeOptionListResponse
            {
                Id = e.Id,
                MaterialAttributeDefinitionId = e.MaterialAttributeDefinitionId,
                Value = e.Value,
                DisplayOrder = e.DisplayOrder,
                CreatedAt = e.CreatedAt
            })
            .ToListAsync(ct);
        var page = PaginatedResponse<MaterialAttributeOptionListResponse>.Create(items, request.PageNumber, request.PageSize, total);
        return BaseResponse<PaginatedResponse<MaterialAttributeOptionListResponse>>.Success(page);
    }

    public async Task<BaseResponse<MaterialAttributeOptionDetailResponse>> GetByIdAsync(Guid id, CancellationToken ct = default)
    {
        var dto = await _db.MaterialAttributeOptions.AsNoTracking().Where(e => e.Id == id)
            .Select(e => new MaterialAttributeOptionDetailResponse
            {
                Id = e.Id,
                CreatedAt = e.CreatedAt,
                CreatedBy = e.CreatedBy,
                UpdatedAt = e.UpdatedAt,
                UpdatedBy = e.UpdatedBy,
                IsDeleted = e.IsDeleted,
                DeletedAt = e.DeletedAt,
                DeletedBy = e.DeletedBy,
                MaterialAttributeDefinitionId = e.MaterialAttributeDefinitionId,
                Value = e.Value,
                DisplayOrder = e.DisplayOrder
            }).FirstOrDefaultAsync(ct);
        return dto is null
            ? BaseResponse<MaterialAttributeOptionDetailResponse>.Failure("NotFound")
            : BaseResponse<MaterialAttributeOptionDetailResponse>.Success(dto);
    }

    public async Task<BaseResponse<Guid>> CreateAsync(CreateMaterialAttributeOptionRequest request, CancellationToken ct = default)
    {
        var entity = new global::Energy.Domain.Modules.Catalog.MaterialAttributeOption
        {
            Id = Guid.NewGuid(),
            MaterialAttributeDefinitionId = request.MaterialAttributeDefinitionId,
            Value = request.Value,
            DisplayOrder = request.DisplayOrder,
            CreatedAt = DateTime.UtcNow,
        };
        _db.MaterialAttributeOptions.Add(entity);
        await _db.SaveChangesAsync(ct);
        return BaseResponse<Guid>.Success(entity.Id, "Created");
    }

    public async Task<BaseResponse<bool>> UpdateAsync(Guid id, UpdateMaterialAttributeOptionRequest request, CancellationToken ct = default)
    {
        var entity = await _db.MaterialAttributeOptions.FirstOrDefaultAsync(e => e.Id == id, ct);
        if (entity is null) return BaseResponse<bool>.Failure("NotFound");
            entity.MaterialAttributeDefinitionId = request.MaterialAttributeDefinitionId;
            entity.Value = request.Value;
            entity.DisplayOrder = request.DisplayOrder;
        entity.UpdatedAt = DateTime.UtcNow;
        await _db.SaveChangesAsync(ct);
        return BaseResponse<bool>.Success(true, "Updated");
    }

    public async Task<BaseResponse<bool>> DeleteAsync(Guid id, CancellationToken ct = default)
    {
        var entity = await _db.MaterialAttributeOptions.FirstOrDefaultAsync(e => e.Id == id, ct);
        if (entity is null) return BaseResponse<bool>.Failure("NotFound");
        entity.IsDeleted = true;
        entity.DeletedAt = DateTime.UtcNow;
        await _db.SaveChangesAsync(ct);
        return BaseResponse<bool>.Success(true, "Deleted");
    }
}
