using Microsoft.EntityFrameworkCore;
using Energy.Infrastructure.Persistence;
using Energy.Shared.Models.V1.Common.Responses;
using Energy.Application.Modules.Core.UnitConversion.Services;
using Energy.Shared.Models.V1.Core.UnitConversion.Requests;
using Energy.Shared.Models.V1.Core.UnitConversion.Responses;

namespace Energy.Infrastructure.Modules.Core.UnitConversion.Services;

/// <summary>UnitConversion CRUD servisi (projection, pagination, soft-delete).</summary>
public class UnitConversionService : IUnitConversionService
{
    private readonly EnergyDbContext _db;

    public UnitConversionService(EnergyDbContext db) => _db = db;

    public async Task<BaseResponse<PaginatedResponse<UnitConversionListResponse>>> GetListAsync(GetUnitConversionListRequest request, CancellationToken ct = default)
    {
        var query = _db.UnitConversions.AsNoTracking();
        var total = await query.CountAsync(ct);
        var items = await query
            .OrderByDescending(e => e.Id)
            .Skip((request.PageNumber - 1) * request.PageSize)
            .Take(request.PageSize)
            .Select(e => new UnitConversionListResponse
            {
                Id = e.Id,
                FromUnitOfMeasureId = e.FromUnitOfMeasureId,
                ToUnitOfMeasureId = e.ToUnitOfMeasureId,
                Factor = e.Factor,
                CreatedAt = e.CreatedAt
            })
            .ToListAsync(ct);
        var page = PaginatedResponse<UnitConversionListResponse>.Create(items, request.PageNumber, request.PageSize, total);
        return BaseResponse<PaginatedResponse<UnitConversionListResponse>>.Success(page);
    }

    public async Task<BaseResponse<UnitConversionDetailResponse>> GetByIdAsync(Guid id, CancellationToken ct = default)
    {
        var dto = await _db.UnitConversions.AsNoTracking().Where(e => e.Id == id)
            .Select(e => new UnitConversionDetailResponse
            {
                Id = e.Id,
                CreatedAt = e.CreatedAt,
                CreatedBy = e.CreatedBy,
                UpdatedAt = e.UpdatedAt,
                UpdatedBy = e.UpdatedBy,
                IsDeleted = e.IsDeleted,
                DeletedAt = e.DeletedAt,
                DeletedBy = e.DeletedBy,
                FromUnitOfMeasureId = e.FromUnitOfMeasureId,
                ToUnitOfMeasureId = e.ToUnitOfMeasureId,
                Factor = e.Factor
            }).FirstOrDefaultAsync(ct);
        return dto is null
            ? BaseResponse<UnitConversionDetailResponse>.Failure("NotFound")
            : BaseResponse<UnitConversionDetailResponse>.Success(dto);
    }

    public async Task<BaseResponse<Guid>> CreateAsync(CreateUnitConversionRequest request, CancellationToken ct = default)
    {
        var entity = new global::Energy.Domain.Modules.Core.UnitConversion
        {
            Id = Guid.NewGuid(),
            FromUnitOfMeasureId = request.FromUnitOfMeasureId,
            ToUnitOfMeasureId = request.ToUnitOfMeasureId,
            Factor = request.Factor,
            CreatedAt = DateTime.UtcNow,
        };
        _db.UnitConversions.Add(entity);
        await _db.SaveChangesAsync(ct);
        return BaseResponse<Guid>.Success(entity.Id, "Created");
    }

    public async Task<BaseResponse<bool>> UpdateAsync(Guid id, UpdateUnitConversionRequest request, CancellationToken ct = default)
    {
        var entity = await _db.UnitConversions.FirstOrDefaultAsync(e => e.Id == id, ct);
        if (entity is null) return BaseResponse<bool>.Failure("NotFound");
            entity.FromUnitOfMeasureId = request.FromUnitOfMeasureId;
            entity.ToUnitOfMeasureId = request.ToUnitOfMeasureId;
            entity.Factor = request.Factor;
        entity.UpdatedAt = DateTime.UtcNow;
        await _db.SaveChangesAsync(ct);
        return BaseResponse<bool>.Success(true, "Updated");
    }

    public async Task<BaseResponse<bool>> DeleteAsync(Guid id, CancellationToken ct = default)
    {
        var entity = await _db.UnitConversions.FirstOrDefaultAsync(e => e.Id == id, ct);
        if (entity is null) return BaseResponse<bool>.Failure("NotFound");
        entity.IsDeleted = true;
        entity.DeletedAt = DateTime.UtcNow;
        await _db.SaveChangesAsync(ct);
        return BaseResponse<bool>.Success(true, "Deleted");
    }
}
