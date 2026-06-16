using Microsoft.EntityFrameworkCore;
using Energy.Infrastructure.Persistence;
using Energy.Shared.Models.V1.Common.Responses;
using Energy.Application.Modules.Inventory.StockDocumentType.Services;
using Energy.Shared.Models.V1.Inventory.StockDocumentType.Requests;
using Energy.Shared.Models.V1.Inventory.StockDocumentType.Responses;

namespace Energy.Infrastructure.Modules.Inventory.StockDocumentType.Services;

/// <summary>StockDocumentType CRUD servisi (projection, pagination, soft-delete).</summary>
public class StockDocumentTypeService : IStockDocumentTypeService
{
    private readonly EnergyDbContext _db;

    public StockDocumentTypeService(EnergyDbContext db) => _db = db;

    public async Task<BaseResponse<PaginatedResponse<StockDocumentTypeListResponse>>> GetListAsync(GetStockDocumentTypeListRequest request, CancellationToken ct = default)
    {
        var query = _db.StockDocumentTypes.AsNoTracking();
        var total = await query.CountAsync(ct);
        var items = await query
            .OrderByDescending(e => e.Id)
            .Skip((request.PageNumber - 1) * request.PageSize)
            .Take(request.PageSize)
            .Select(e => new StockDocumentTypeListResponse
            {
                Id = e.Id,
                Code = e.Code,
                Name = e.Name,
                Direction = e.Direction,
                IsActive = e.IsActive,
                CreatedAt = e.CreatedAt
            })
            .ToListAsync(ct);
        var page = PaginatedResponse<StockDocumentTypeListResponse>.Create(items, request.PageNumber, request.PageSize, total);
        return BaseResponse<PaginatedResponse<StockDocumentTypeListResponse>>.Success(page);
    }

    public async Task<BaseResponse<StockDocumentTypeDetailResponse>> GetByIdAsync(Guid id, CancellationToken ct = default)
    {
        var dto = await _db.StockDocumentTypes.AsNoTracking().Where(e => e.Id == id)
            .Select(e => new StockDocumentTypeDetailResponse
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
                Direction = e.Direction,
                IsActive = e.IsActive
            }).FirstOrDefaultAsync(ct);
        return dto is null
            ? BaseResponse<StockDocumentTypeDetailResponse>.Failure("NotFound")
            : BaseResponse<StockDocumentTypeDetailResponse>.Success(dto);
    }

    public async Task<BaseResponse<Guid>> CreateAsync(CreateStockDocumentTypeRequest request, CancellationToken ct = default)
    {
        var entity = new global::Energy.Domain.Modules.Inventory.StockDocumentType
        {
            Id = Guid.NewGuid(),
            Code = request.Code,
            Name = request.Name,
            Direction = request.Direction,
            IsActive = request.IsActive,
            CreatedAt = DateTime.UtcNow,
        };
        _db.StockDocumentTypes.Add(entity);
        await _db.SaveChangesAsync(ct);
        return BaseResponse<Guid>.Success(entity.Id, "Created");
    }

    public async Task<BaseResponse<bool>> UpdateAsync(Guid id, UpdateStockDocumentTypeRequest request, CancellationToken ct = default)
    {
        var entity = await _db.StockDocumentTypes.FirstOrDefaultAsync(e => e.Id == id, ct);
        if (entity is null) return BaseResponse<bool>.Failure("NotFound");
            entity.Code = request.Code;
            entity.Name = request.Name;
            entity.Direction = request.Direction;
            entity.IsActive = request.IsActive;
        entity.UpdatedAt = DateTime.UtcNow;
        await _db.SaveChangesAsync(ct);
        return BaseResponse<bool>.Success(true, "Updated");
    }

    public async Task<BaseResponse<bool>> DeleteAsync(Guid id, CancellationToken ct = default)
    {
        var entity = await _db.StockDocumentTypes.FirstOrDefaultAsync(e => e.Id == id, ct);
        if (entity is null) return BaseResponse<bool>.Failure("NotFound");
        entity.IsDeleted = true;
        entity.DeletedAt = DateTime.UtcNow;
        await _db.SaveChangesAsync(ct);
        return BaseResponse<bool>.Success(true, "Deleted");
    }
}
