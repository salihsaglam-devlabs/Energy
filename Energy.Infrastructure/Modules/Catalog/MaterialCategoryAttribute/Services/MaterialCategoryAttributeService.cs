using Microsoft.EntityFrameworkCore;
using Energy.Infrastructure.Persistence;
using Energy.Shared.Models.V1.Common.Responses;
using Energy.Application.Modules.Catalog.MaterialCategoryAttribute.Services;
using Energy.Shared.Models.V1.Catalog.MaterialCategoryAttribute.Requests;
using Energy.Shared.Models.V1.Catalog.MaterialCategoryAttribute.Responses;

namespace Energy.Infrastructure.Modules.Catalog.MaterialCategoryAttribute.Services;

/// <summary>MaterialCategoryAttribute CRUD servisi (projection, pagination, soft-delete).</summary>
public class MaterialCategoryAttributeService : IMaterialCategoryAttributeService
{
    private readonly EnergyDbContext _db;

    public MaterialCategoryAttributeService(EnergyDbContext db) => _db = db;

    public async Task<BaseResponse<PaginatedResponse<MaterialCategoryAttributeListResponse>>> GetListAsync(GetMaterialCategoryAttributeListRequest request, CancellationToken ct = default)
    {
        var query = _db.MaterialCategoryAttributes.AsNoTracking();
        var total = await query.CountAsync(ct);
        var items = await query
            .OrderByDescending(e => e.Id)
            .Skip((request.PageNumber - 1) * request.PageSize)
            .Take(request.PageSize)
            .Select(e => new MaterialCategoryAttributeListResponse
            {
                Id = e.Id,
                MaterialCategoryId = e.MaterialCategoryId,
                MaterialAttributeDefinitionId = e.MaterialAttributeDefinitionId,
                IsRequired = e.IsRequired,
                DisplayOrder = e.DisplayOrder,
                CreatedAt = e.CreatedAt
            })
            .ToListAsync(ct);
        var page = PaginatedResponse<MaterialCategoryAttributeListResponse>.Create(items, request.PageNumber, request.PageSize, total);
        return BaseResponse<PaginatedResponse<MaterialCategoryAttributeListResponse>>.Success(page);
    }

    public async Task<BaseResponse<MaterialCategoryAttributeDetailResponse>> GetByIdAsync(Guid id, CancellationToken ct = default)
    {
        var dto = await _db.MaterialCategoryAttributes.AsNoTracking().Where(e => e.Id == id)
            .Select(e => new MaterialCategoryAttributeDetailResponse
            {
                Id = e.Id,
                CreatedAt = e.CreatedAt,
                CreatedBy = e.CreatedBy,
                UpdatedAt = e.UpdatedAt,
                UpdatedBy = e.UpdatedBy,
                IsDeleted = e.IsDeleted,
                DeletedAt = e.DeletedAt,
                DeletedBy = e.DeletedBy,
                MaterialCategoryId = e.MaterialCategoryId,
                MaterialAttributeDefinitionId = e.MaterialAttributeDefinitionId,
                IsRequired = e.IsRequired,
                DisplayOrder = e.DisplayOrder
            }).FirstOrDefaultAsync(ct);
        return dto is null
            ? BaseResponse<MaterialCategoryAttributeDetailResponse>.Failure("NotFound")
            : BaseResponse<MaterialCategoryAttributeDetailResponse>.Success(dto);
    }

    public async Task<BaseResponse<Guid>> CreateAsync(CreateMaterialCategoryAttributeRequest request, CancellationToken ct = default)
    {
        var entity = new global::Energy.Domain.Modules.Catalog.MaterialCategoryAttribute
        {
            Id = Guid.NewGuid(),
            MaterialCategoryId = request.MaterialCategoryId,
            MaterialAttributeDefinitionId = request.MaterialAttributeDefinitionId,
            IsRequired = request.IsRequired,
            DisplayOrder = request.DisplayOrder,
            CreatedAt = DateTime.UtcNow,
        };
        _db.MaterialCategoryAttributes.Add(entity);
        await _db.SaveChangesAsync(ct);
        return BaseResponse<Guid>.Success(entity.Id, "Created");
    }

    public async Task<BaseResponse<bool>> UpdateAsync(Guid id, UpdateMaterialCategoryAttributeRequest request, CancellationToken ct = default)
    {
        var entity = await _db.MaterialCategoryAttributes.FirstOrDefaultAsync(e => e.Id == id, ct);
        if (entity is null) return BaseResponse<bool>.Failure("NotFound");
            entity.MaterialCategoryId = request.MaterialCategoryId;
            entity.MaterialAttributeDefinitionId = request.MaterialAttributeDefinitionId;
            entity.IsRequired = request.IsRequired;
            entity.DisplayOrder = request.DisplayOrder;
        entity.UpdatedAt = DateTime.UtcNow;
        await _db.SaveChangesAsync(ct);
        return BaseResponse<bool>.Success(true, "Updated");
    }

    public async Task<BaseResponse<bool>> DeleteAsync(Guid id, CancellationToken ct = default)
    {
        var entity = await _db.MaterialCategoryAttributes.FirstOrDefaultAsync(e => e.Id == id, ct);
        if (entity is null) return BaseResponse<bool>.Failure("NotFound");
        entity.IsDeleted = true;
        entity.DeletedAt = DateTime.UtcNow;
        await _db.SaveChangesAsync(ct);
        return BaseResponse<bool>.Success(true, "Deleted");
    }
}
