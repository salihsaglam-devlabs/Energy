using Microsoft.EntityFrameworkCore;
using Energy.Infrastructure.Persistence;
using Energy.Shared.Models.V1.Common.Responses;
using Energy.Application.Catalog.MaterialAttributeDefinition.Services;
using Energy.Shared.Models.V1.Catalog.MaterialAttributeDefinition.Requests;
using Energy.Shared.Models.V1.Catalog.MaterialAttributeDefinition.Responses;

namespace Energy.Infrastructure.Catalog.MaterialAttributeDefinition.Services;

/// <summary>MaterialAttributeDefinition CRUD servisi (projection, pagination, soft-delete).</summary>
public class MaterialAttributeDefinitionService : IMaterialAttributeDefinitionService
{
    private readonly AppDbContext _db;

    public MaterialAttributeDefinitionService(AppDbContext db) => _db = db;

    public async Task<BaseResponse<PaginatedResponse<MaterialAttributeDefinitionListResponse>>> GetListAsync(GetMaterialAttributeDefinitionListRequest request, CancellationToken ct = default)
    {
        var query = _db.MaterialAttributeDefinitions.AsNoTracking();
        var total = await query.CountAsync(ct);
        var items = await query
            .OrderByDescending(e => e.Id)
            .Skip((request.PageNumber - 1) * request.PageSize)
            .Take(request.PageSize)
            .Select(e => new MaterialAttributeDefinitionListResponse
            {
                Id = e.Id,
                Code = e.Code,
                Name = e.Name,
                DataType = e.DataType,
                IsActive = e.IsActive,
                CreatedAt = e.CreatedAt
            })
            .ToListAsync(ct);
        var page = PaginatedResponse<MaterialAttributeDefinitionListResponse>.Create(items, request.PageNumber, request.PageSize, total);
        return BaseResponse<PaginatedResponse<MaterialAttributeDefinitionListResponse>>.Success(page);
    }

    public async Task<BaseResponse<MaterialAttributeDefinitionDetailResponse>> GetByIdAsync(Guid id, CancellationToken ct = default)
    {
        var dto = await _db.MaterialAttributeDefinitions.AsNoTracking().Where(e => e.Id == id)
            .Select(e => new MaterialAttributeDefinitionDetailResponse
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
                DataType = e.DataType,
                IsActive = e.IsActive
            }).FirstOrDefaultAsync(ct);
        return dto is null
            ? BaseResponse<MaterialAttributeDefinitionDetailResponse>.Failure("NotFound")
            : BaseResponse<MaterialAttributeDefinitionDetailResponse>.Success(dto);
    }

    public async Task<BaseResponse<Guid>> CreateAsync(CreateMaterialAttributeDefinitionRequest request, CancellationToken ct = default)
    {
        var entity = new global::Energy.Domain.Catalog.MaterialAttributeDefinition
        {
            Id = Guid.NewGuid(),
            Code = request.Code,
            Name = request.Name,
            DataType = request.DataType,
            IsActive = request.IsActive,
            CreatedAt = DateTime.UtcNow,
        };
        _db.MaterialAttributeDefinitions.Add(entity);
        await _db.SaveChangesAsync(ct);
        return BaseResponse<Guid>.Success(entity.Id, "Created");
    }

    public async Task<BaseResponse<bool>> UpdateAsync(Guid id, UpdateMaterialAttributeDefinitionRequest request, CancellationToken ct = default)
    {
        var entity = await _db.MaterialAttributeDefinitions.FirstOrDefaultAsync(e => e.Id == id, ct);
        if (entity is null) return BaseResponse<bool>.Failure("NotFound");
            entity.Code = request.Code;
            entity.Name = request.Name;
            entity.DataType = request.DataType;
            entity.IsActive = request.IsActive;
        entity.UpdatedAt = DateTime.UtcNow;
        await _db.SaveChangesAsync(ct);
        return BaseResponse<bool>.Success(true, "Updated");
    }

    public async Task<BaseResponse<bool>> DeleteAsync(Guid id, CancellationToken ct = default)
    {
        var entity = await _db.MaterialAttributeDefinitions.FirstOrDefaultAsync(e => e.Id == id, ct);
        if (entity is null) return BaseResponse<bool>.Failure("NotFound");
        entity.IsDeleted = true;
        entity.DeletedAt = DateTime.UtcNow;
        await _db.SaveChangesAsync(ct);
        return BaseResponse<bool>.Success(true, "Deleted");
    }
}
