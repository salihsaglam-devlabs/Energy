using Microsoft.EntityFrameworkCore;
using Energy.Infrastructure.Persistence;
using Energy.Shared.Models.V1.Common.Responses;
using Energy.Application.Modules.Assets.EquipmentMaintenance.Services;
using Energy.Shared.Models.V1.Assets.EquipmentMaintenance.Requests;
using Energy.Shared.Models.V1.Assets.EquipmentMaintenance.Responses;

namespace Energy.Infrastructure.Modules.Assets.EquipmentMaintenance.Services;

/// <summary>EquipmentMaintenance CRUD servisi (projection, pagination, soft-delete).</summary>
public class EquipmentMaintenanceService : IEquipmentMaintenanceService
{
    private readonly AppDbContext _db;

    public EquipmentMaintenanceService(AppDbContext db) => _db = db;

    public async Task<BaseResponse<PaginatedResponse<EquipmentMaintenanceListResponse>>> GetListAsync(GetEquipmentMaintenanceListRequest request, CancellationToken ct = default)
    {
        var query = _db.EquipmentMaintenances.AsNoTracking();
        var total = await query.CountAsync(ct);
        var items = await query
            .OrderByDescending(e => e.Id)
            .Skip((request.PageNumber - 1) * request.PageSize)
            .Take(request.PageSize)
            .Select(e => new EquipmentMaintenanceListResponse
            {
                Id = e.Id,
                EquipmentAssetId = e.EquipmentAssetId,
                MaintenanceType = e.MaintenanceType,
                ScheduledDate = e.ScheduledDate,
                CompletedDate = e.CompletedDate,
                Cost = e.Cost,
                Note = e.Note,
                CreatedAt = e.CreatedAt
            })
            .ToListAsync(ct);
        var page = PaginatedResponse<EquipmentMaintenanceListResponse>.Create(items, request.PageNumber, request.PageSize, total);
        return BaseResponse<PaginatedResponse<EquipmentMaintenanceListResponse>>.Success(page);
    }

    public async Task<BaseResponse<EquipmentMaintenanceDetailResponse>> GetByIdAsync(Guid id, CancellationToken ct = default)
    {
        var dto = await _db.EquipmentMaintenances.AsNoTracking().Where(e => e.Id == id)
            .Select(e => new EquipmentMaintenanceDetailResponse
            {
                Id = e.Id,
                CreatedAt = e.CreatedAt,
                CreatedBy = e.CreatedBy,
                UpdatedAt = e.UpdatedAt,
                UpdatedBy = e.UpdatedBy,
                IsDeleted = e.IsDeleted,
                DeletedAt = e.DeletedAt,
                DeletedBy = e.DeletedBy,
                EquipmentAssetId = e.EquipmentAssetId,
                MaintenanceType = e.MaintenanceType,
                ScheduledDate = e.ScheduledDate,
                CompletedDate = e.CompletedDate,
                Cost = e.Cost,
                Note = e.Note
            }).FirstOrDefaultAsync(ct);
        return dto is null
            ? BaseResponse<EquipmentMaintenanceDetailResponse>.Failure("NotFound")
            : BaseResponse<EquipmentMaintenanceDetailResponse>.Success(dto);
    }

    public async Task<BaseResponse<Guid>> CreateAsync(CreateEquipmentMaintenanceRequest request, CancellationToken ct = default)
    {
        var entity = new global::Energy.Domain.Modules.Assets.EquipmentMaintenance
        {
            Id = Guid.NewGuid(),
            EquipmentAssetId = request.EquipmentAssetId,
            MaintenanceType = request.MaintenanceType,
            ScheduledDate = request.ScheduledDate,
            CompletedDate = request.CompletedDate,
            Cost = request.Cost,
            Note = request.Note,
            CreatedAt = DateTime.UtcNow,
        };
        _db.EquipmentMaintenances.Add(entity);
        await _db.SaveChangesAsync(ct);
        return BaseResponse<Guid>.Success(entity.Id, "Created");
    }

    public async Task<BaseResponse<bool>> UpdateAsync(Guid id, UpdateEquipmentMaintenanceRequest request, CancellationToken ct = default)
    {
        var entity = await _db.EquipmentMaintenances.FirstOrDefaultAsync(e => e.Id == id, ct);
        if (entity is null) return BaseResponse<bool>.Failure("NotFound");
            entity.EquipmentAssetId = request.EquipmentAssetId;
            entity.MaintenanceType = request.MaintenanceType;
            entity.ScheduledDate = request.ScheduledDate;
            entity.CompletedDate = request.CompletedDate;
            entity.Cost = request.Cost;
            entity.Note = request.Note;
        entity.UpdatedAt = DateTime.UtcNow;
        await _db.SaveChangesAsync(ct);
        return BaseResponse<bool>.Success(true, "Updated");
    }

    public async Task<BaseResponse<bool>> DeleteAsync(Guid id, CancellationToken ct = default)
    {
        var entity = await _db.EquipmentMaintenances.FirstOrDefaultAsync(e => e.Id == id, ct);
        if (entity is null) return BaseResponse<bool>.Failure("NotFound");
        entity.IsDeleted = true;
        entity.DeletedAt = DateTime.UtcNow;
        await _db.SaveChangesAsync(ct);
        return BaseResponse<bool>.Success(true, "Deleted");
    }
}
