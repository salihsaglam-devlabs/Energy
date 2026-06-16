using Microsoft.EntityFrameworkCore;
using Energy.Infrastructure.Persistence;
using Energy.Shared.Models.V1.Common.Responses;
using Energy.Application.Catalog.MaterialAttributeValue.Services;
using Energy.Shared.Models.V1.Catalog.MaterialAttributeValue.Requests;
using Energy.Shared.Models.V1.Catalog.MaterialAttributeValue.Responses;

namespace Energy.Infrastructure.Catalog.MaterialAttributeValue.Services;

/// <summary>MaterialAttributeValue CRUD servisi (projection, pagination, soft-delete).</summary>
public class MaterialAttributeValueService : IMaterialAttributeValueService
{
    private readonly AppDbContext _db;

    public MaterialAttributeValueService(AppDbContext db) => _db = db;

    public async Task<BaseResponse<PaginatedResponse<MaterialAttributeValueListResponse>>> GetListAsync(GetMaterialAttributeValueListRequest request, CancellationToken ct = default)
    {
        var query = _db.MaterialAttributeValues.AsNoTracking();
        var total = await query.CountAsync(ct);
        var items = await query
            .OrderByDescending(e => e.Id)
            .Skip((request.PageNumber - 1) * request.PageSize)
            .Take(request.PageSize)
            .Select(e => new MaterialAttributeValueListResponse
            {
                Id = e.Id,
                MaterialId = e.MaterialId,
                MaterialAttributeDefinitionId = e.MaterialAttributeDefinitionId,
                OptionId = e.OptionId,
                ValueText = e.ValueText,
                ValueNumber = e.ValueNumber,
                ValueBoolean = e.ValueBoolean,
                ValueDate = e.ValueDate,
                CreatedAt = e.CreatedAt
            })
            .ToListAsync(ct);
        var page = PaginatedResponse<MaterialAttributeValueListResponse>.Create(items, request.PageNumber, request.PageSize, total);
        return BaseResponse<PaginatedResponse<MaterialAttributeValueListResponse>>.Success(page);
    }

    public async Task<BaseResponse<MaterialAttributeValueDetailResponse>> GetByIdAsync(Guid id, CancellationToken ct = default)
    {
        var dto = await _db.MaterialAttributeValues.AsNoTracking().Where(e => e.Id == id)
            .Select(e => new MaterialAttributeValueDetailResponse
            {
                Id = e.Id,
                CreatedAt = e.CreatedAt,
                CreatedBy = e.CreatedBy,
                UpdatedAt = e.UpdatedAt,
                UpdatedBy = e.UpdatedBy,
                IsDeleted = e.IsDeleted,
                DeletedAt = e.DeletedAt,
                DeletedBy = e.DeletedBy,
                MaterialId = e.MaterialId,
                MaterialAttributeDefinitionId = e.MaterialAttributeDefinitionId,
                OptionId = e.OptionId,
                ValueText = e.ValueText,
                ValueNumber = e.ValueNumber,
                ValueBoolean = e.ValueBoolean,
                ValueDate = e.ValueDate
            }).FirstOrDefaultAsync(ct);
        return dto is null
            ? BaseResponse<MaterialAttributeValueDetailResponse>.Failure("NotFound")
            : BaseResponse<MaterialAttributeValueDetailResponse>.Success(dto);
    }

    public async Task<BaseResponse<Guid>> CreateAsync(CreateMaterialAttributeValueRequest request, CancellationToken ct = default)
    {
        var entity = new global::Energy.Domain.Catalog.MaterialAttributeValue
        {
            Id = Guid.NewGuid(),
            MaterialId = request.MaterialId,
            MaterialAttributeDefinitionId = request.MaterialAttributeDefinitionId,
            OptionId = request.OptionId,
            ValueText = request.ValueText,
            ValueNumber = request.ValueNumber,
            ValueBoolean = request.ValueBoolean,
            ValueDate = request.ValueDate,
            CreatedAt = DateTime.UtcNow,
        };
        _db.MaterialAttributeValues.Add(entity);
        await _db.SaveChangesAsync(ct);
        return BaseResponse<Guid>.Success(entity.Id, "Created");
    }

    public async Task<BaseResponse<bool>> UpdateAsync(Guid id, UpdateMaterialAttributeValueRequest request, CancellationToken ct = default)
    {
        var entity = await _db.MaterialAttributeValues.FirstOrDefaultAsync(e => e.Id == id, ct);
        if (entity is null) return BaseResponse<bool>.Failure("NotFound");
            entity.MaterialId = request.MaterialId;
            entity.MaterialAttributeDefinitionId = request.MaterialAttributeDefinitionId;
            entity.OptionId = request.OptionId;
            entity.ValueText = request.ValueText;
            entity.ValueNumber = request.ValueNumber;
            entity.ValueBoolean = request.ValueBoolean;
            entity.ValueDate = request.ValueDate;
        entity.UpdatedAt = DateTime.UtcNow;
        await _db.SaveChangesAsync(ct);
        return BaseResponse<bool>.Success(true, "Updated");
    }

    public async Task<BaseResponse<bool>> DeleteAsync(Guid id, CancellationToken ct = default)
    {
        var entity = await _db.MaterialAttributeValues.FirstOrDefaultAsync(e => e.Id == id, ct);
        if (entity is null) return BaseResponse<bool>.Failure("NotFound");
        entity.IsDeleted = true;
        entity.DeletedAt = DateTime.UtcNow;
        await _db.SaveChangesAsync(ct);
        return BaseResponse<bool>.Success(true, "Deleted");
    }
}
