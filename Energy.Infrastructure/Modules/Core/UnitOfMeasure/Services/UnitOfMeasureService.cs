using Microsoft.EntityFrameworkCore;
using Energy.Infrastructure.Persistence;
using Energy.Shared.Models.V1.Common.Responses;
using Energy.Application.Modules.Core.UnitOfMeasure.Services;
using Energy.Shared.Models.V1.Core.UnitOfMeasure.Requests;
using Energy.Shared.Models.V1.Core.UnitOfMeasure.Responses;

namespace Energy.Infrastructure.Modules.Core.UnitOfMeasure.Services;

/// <summary>UnitOfMeasure CRUD servisi (projection, pagination, soft-delete).</summary>
public class UnitOfMeasureService : IUnitOfMeasureService
{
    private readonly EnergyDbContext _db;

    public UnitOfMeasureService(EnergyDbContext db) => _db = db;

    public async Task<BaseResponse<PaginatedResponse<UnitOfMeasureListResponse>>> GetListAsync(GetUnitOfMeasureListRequest request, CancellationToken ct = default)
    {
        var query = _db.UnitsOfMeasure.AsNoTracking();
        var total = await query.CountAsync(ct);
        var items = await query
            .OrderByDescending(e => e.Id)
            .Skip((request.PageNumber - 1) * request.PageSize)
            .Take(request.PageSize)
            .Select(e => new UnitOfMeasureListResponse
            {
                Id = e.Id,
                Code = e.Code,
                Name = e.Name,
                Symbol = e.Symbol,
                IsActive = e.IsActive,
                CreatedAt = e.CreatedAt
            })
            .ToListAsync(ct);
        var page = PaginatedResponse<UnitOfMeasureListResponse>.Create(items, request.PageNumber, request.PageSize, total);
        return BaseResponse<PaginatedResponse<UnitOfMeasureListResponse>>.Success(page);
    }

    public async Task<BaseResponse<UnitOfMeasureDetailResponse>> GetByIdAsync(Guid id, CancellationToken ct = default)
    {
        var dto = await _db.UnitsOfMeasure.AsNoTracking().Where(e => e.Id == id)
            .Select(e => new UnitOfMeasureDetailResponse
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
                Symbol = e.Symbol,
                IsActive = e.IsActive
            }).FirstOrDefaultAsync(ct);
        return dto is null
            ? BaseResponse<UnitOfMeasureDetailResponse>.Failure("NotFound")
            : BaseResponse<UnitOfMeasureDetailResponse>.Success(dto);
    }

    public async Task<BaseResponse<Guid>> CreateAsync(CreateUnitOfMeasureRequest request, CancellationToken ct = default)
    {
        var entity = new global::Energy.Domain.Modules.Core.UnitOfMeasure
        {
            Id = Guid.NewGuid(),
            Code = request.Code,
            Name = request.Name,
            Symbol = request.Symbol,
            IsActive = request.IsActive,
            CreatedAt = DateTime.UtcNow,
        };
        _db.UnitsOfMeasure.Add(entity);
        await _db.SaveChangesAsync(ct);
        return BaseResponse<Guid>.Success(entity.Id, "Created");
    }

    public async Task<BaseResponse<bool>> UpdateAsync(Guid id, UpdateUnitOfMeasureRequest request, CancellationToken ct = default)
    {
        var entity = await _db.UnitsOfMeasure.FirstOrDefaultAsync(e => e.Id == id, ct);
        if (entity is null) return BaseResponse<bool>.Failure("NotFound");
            entity.Code = request.Code;
            entity.Name = request.Name;
            entity.Symbol = request.Symbol;
            entity.IsActive = request.IsActive;
        entity.UpdatedAt = DateTime.UtcNow;
        await _db.SaveChangesAsync(ct);
        return BaseResponse<bool>.Success(true, "Updated");
    }

    public async Task<BaseResponse<bool>> DeleteAsync(Guid id, CancellationToken ct = default)
    {
        var entity = await _db.UnitsOfMeasure.FirstOrDefaultAsync(e => e.Id == id, ct);
        if (entity is null) return BaseResponse<bool>.Failure("NotFound");
        entity.IsDeleted = true;
        entity.DeletedAt = DateTime.UtcNow;
        await _db.SaveChangesAsync(ct);
        return BaseResponse<bool>.Success(true, "Deleted");
    }
}
