using Microsoft.EntityFrameworkCore;
using Energy.Infrastructure.Persistence;
using Energy.Shared.Models.V1.Common.Responses;
using Energy.Application.Modules.FieldOperations.MeasurementSheetLine.Services;
using Energy.Shared.Models.V1.FieldOperations.MeasurementSheetLine.Requests;
using Energy.Shared.Models.V1.FieldOperations.MeasurementSheetLine.Responses;

namespace Energy.Infrastructure.Modules.FieldOperations.MeasurementSheetLine.Services;

/// <summary>MeasurementSheetLine CRUD servisi (projection, pagination, soft-delete).</summary>
public class MeasurementSheetLineService : IMeasurementSheetLineService
{
    private readonly AppDbContext _db;

    public MeasurementSheetLineService(AppDbContext db) => _db = db;

    public async Task<BaseResponse<PaginatedResponse<MeasurementSheetLineListResponse>>> GetListAsync(GetMeasurementSheetLineListRequest request, CancellationToken ct = default)
    {
        var query = _db.MeasurementSheetLines.AsNoTracking();
        var total = await query.CountAsync(ct);
        var items = await query
            .OrderByDescending(e => e.Id)
            .Skip((request.PageNumber - 1) * request.PageSize)
            .Take(request.PageSize)
            .Select(e => new MeasurementSheetLineListResponse
            {
                Id = e.Id,
                MeasurementSheetId = e.MeasurementSheetId,
                Description = e.Description,
                Quantity = e.Quantity,
                UnitPrice = e.UnitPrice,
                CreatedAt = e.CreatedAt
            })
            .ToListAsync(ct);
        var page = PaginatedResponse<MeasurementSheetLineListResponse>.Create(items, request.PageNumber, request.PageSize, total);
        return BaseResponse<PaginatedResponse<MeasurementSheetLineListResponse>>.Success(page);
    }

    public async Task<BaseResponse<MeasurementSheetLineDetailResponse>> GetByIdAsync(Guid id, CancellationToken ct = default)
    {
        var dto = await _db.MeasurementSheetLines.AsNoTracking().Where(e => e.Id == id)
            .Select(e => new MeasurementSheetLineDetailResponse
            {
                Id = e.Id,
                CreatedAt = e.CreatedAt,
                CreatedBy = e.CreatedBy,
                UpdatedAt = e.UpdatedAt,
                UpdatedBy = e.UpdatedBy,
                IsDeleted = e.IsDeleted,
                DeletedAt = e.DeletedAt,
                DeletedBy = e.DeletedBy,
                MeasurementSheetId = e.MeasurementSheetId,
                Description = e.Description,
                Quantity = e.Quantity,
                UnitPrice = e.UnitPrice
            }).FirstOrDefaultAsync(ct);
        return dto is null
            ? BaseResponse<MeasurementSheetLineDetailResponse>.Failure("NotFound")
            : BaseResponse<MeasurementSheetLineDetailResponse>.Success(dto);
    }

    public async Task<BaseResponse<Guid>> CreateAsync(CreateMeasurementSheetLineRequest request, CancellationToken ct = default)
    {
        var entity = new global::Energy.Domain.Modules.FieldOperations.MeasurementSheetLine
        {
            Id = Guid.NewGuid(),
            MeasurementSheetId = request.MeasurementSheetId,
            Description = request.Description,
            Quantity = request.Quantity,
            UnitPrice = request.UnitPrice,
            CreatedAt = DateTime.UtcNow,
        };
        _db.MeasurementSheetLines.Add(entity);
        await _db.SaveChangesAsync(ct);
        return BaseResponse<Guid>.Success(entity.Id, "Created");
    }

    public async Task<BaseResponse<bool>> UpdateAsync(Guid id, UpdateMeasurementSheetLineRequest request, CancellationToken ct = default)
    {
        var entity = await _db.MeasurementSheetLines.FirstOrDefaultAsync(e => e.Id == id, ct);
        if (entity is null) return BaseResponse<bool>.Failure("NotFound");
            entity.MeasurementSheetId = request.MeasurementSheetId;
            entity.Description = request.Description;
            entity.Quantity = request.Quantity;
            entity.UnitPrice = request.UnitPrice;
        entity.UpdatedAt = DateTime.UtcNow;
        await _db.SaveChangesAsync(ct);
        return BaseResponse<bool>.Success(true, "Updated");
    }

    public async Task<BaseResponse<bool>> DeleteAsync(Guid id, CancellationToken ct = default)
    {
        var entity = await _db.MeasurementSheetLines.FirstOrDefaultAsync(e => e.Id == id, ct);
        if (entity is null) return BaseResponse<bool>.Failure("NotFound");
        entity.IsDeleted = true;
        entity.DeletedAt = DateTime.UtcNow;
        await _db.SaveChangesAsync(ct);
        return BaseResponse<bool>.Success(true, "Deleted");
    }
}
