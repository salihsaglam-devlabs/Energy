using Microsoft.EntityFrameworkCore;
using Energy.Infrastructure.Persistence;
using Energy.Shared.Models.V1.Common.Responses;
using Energy.Application.Modules.FieldOperations.DailySiteReportEquipment.Services;
using Energy.Shared.Models.V1.FieldOperations.DailySiteReportEquipment.Requests;
using Energy.Shared.Models.V1.FieldOperations.DailySiteReportEquipment.Responses;

namespace Energy.Infrastructure.Modules.FieldOperations.DailySiteReportEquipment.Services;

/// <summary>DailySiteReportEquipment CRUD servisi (projection, pagination, soft-delete).</summary>
public class DailySiteReportEquipmentService : IDailySiteReportEquipmentService
{
    private readonly EnergyDbContext _db;

    public DailySiteReportEquipmentService(EnergyDbContext db) => _db = db;

    public async Task<BaseResponse<PaginatedResponse<DailySiteReportEquipmentListResponse>>> GetListAsync(GetDailySiteReportEquipmentListRequest request, CancellationToken ct = default)
    {
        var query = _db.DailySiteReportEquipments.AsNoTracking();
        var total = await query.CountAsync(ct);
        var items = await query
            .OrderByDescending(e => e.Id)
            .Skip((request.PageNumber - 1) * request.PageSize)
            .Take(request.PageSize)
            .Select(e => new DailySiteReportEquipmentListResponse
            {
                Id = e.Id,
                DailySiteReportId = e.DailySiteReportId,
                EquipmentAssetId = e.EquipmentAssetId,
                EquipmentText = e.EquipmentText,
                Hours = e.Hours,
                CreatedAt = e.CreatedAt
            })
            .ToListAsync(ct);
        var page = PaginatedResponse<DailySiteReportEquipmentListResponse>.Create(items, request.PageNumber, request.PageSize, total);
        return BaseResponse<PaginatedResponse<DailySiteReportEquipmentListResponse>>.Success(page);
    }

    public async Task<BaseResponse<DailySiteReportEquipmentDetailResponse>> GetByIdAsync(Guid id, CancellationToken ct = default)
    {
        var dto = await _db.DailySiteReportEquipments.AsNoTracking().Where(e => e.Id == id)
            .Select(e => new DailySiteReportEquipmentDetailResponse
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
                EquipmentAssetId = e.EquipmentAssetId,
                EquipmentText = e.EquipmentText,
                Hours = e.Hours
            }).FirstOrDefaultAsync(ct);
        return dto is null
            ? BaseResponse<DailySiteReportEquipmentDetailResponse>.Failure("NotFound")
            : BaseResponse<DailySiteReportEquipmentDetailResponse>.Success(dto);
    }

    public async Task<BaseResponse<Guid>> CreateAsync(CreateDailySiteReportEquipmentRequest request, CancellationToken ct = default)
    {
        var entity = new global::Energy.Domain.Modules.FieldOperations.DailySiteReportEquipment
        {
            Id = Guid.NewGuid(),
            DailySiteReportId = request.DailySiteReportId,
            EquipmentAssetId = request.EquipmentAssetId,
            EquipmentText = request.EquipmentText,
            Hours = request.Hours,
            CreatedAt = DateTime.UtcNow,
        };
        _db.DailySiteReportEquipments.Add(entity);
        await _db.SaveChangesAsync(ct);
        return BaseResponse<Guid>.Success(entity.Id, "Created");
    }

    public async Task<BaseResponse<bool>> UpdateAsync(Guid id, UpdateDailySiteReportEquipmentRequest request, CancellationToken ct = default)
    {
        var entity = await _db.DailySiteReportEquipments.FirstOrDefaultAsync(e => e.Id == id, ct);
        if (entity is null) return BaseResponse<bool>.Failure("NotFound");
            entity.DailySiteReportId = request.DailySiteReportId;
            entity.EquipmentAssetId = request.EquipmentAssetId;
            entity.EquipmentText = request.EquipmentText;
            entity.Hours = request.Hours;
        entity.UpdatedAt = DateTime.UtcNow;
        await _db.SaveChangesAsync(ct);
        return BaseResponse<bool>.Success(true, "Updated");
    }

    public async Task<BaseResponse<bool>> DeleteAsync(Guid id, CancellationToken ct = default)
    {
        var entity = await _db.DailySiteReportEquipments.FirstOrDefaultAsync(e => e.Id == id, ct);
        if (entity is null) return BaseResponse<bool>.Failure("NotFound");
        entity.IsDeleted = true;
        entity.DeletedAt = DateTime.UtcNow;
        await _db.SaveChangesAsync(ct);
        return BaseResponse<bool>.Success(true, "Deleted");
    }
}
