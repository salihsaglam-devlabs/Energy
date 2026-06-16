using Microsoft.EntityFrameworkCore;
using Energy.Infrastructure.Persistence;
using Energy.Shared.Models.V1.Common.Responses;
using Energy.Application.Assets.EquipmentAssignment.Services;
using Energy.Shared.Models.V1.Assets.EquipmentAssignment.Requests;
using Energy.Shared.Models.V1.Assets.EquipmentAssignment.Responses;

namespace Energy.Infrastructure.Assets.EquipmentAssignment.Services;

/// <summary>EquipmentAssignment CRUD servisi (projection, pagination, soft-delete).</summary>
public class EquipmentAssignmentService : IEquipmentAssignmentService
{
    private readonly AppDbContext _db;

    public EquipmentAssignmentService(AppDbContext db) => _db = db;

    public async Task<BaseResponse<PaginatedResponse<EquipmentAssignmentListResponse>>> GetListAsync(GetEquipmentAssignmentListRequest request, CancellationToken ct = default)
    {
        var query = _db.EquipmentAssignments.AsNoTracking();
        var total = await query.CountAsync(ct);
        var items = await query
            .OrderByDescending(e => e.Id)
            .Skip((request.PageNumber - 1) * request.PageSize)
            .Take(request.PageSize)
            .Select(e => new EquipmentAssignmentListResponse
            {
                Id = e.Id,
                EquipmentAssetId = e.EquipmentAssetId,
                ProjectId = e.ProjectId,
                EmployeeId = e.EmployeeId,
                WarehouseId = e.WarehouseId,
                StartDate = e.StartDate,
                EndDate = e.EndDate,
                IsActive = e.IsActive,
                CreatedAt = e.CreatedAt
            })
            .ToListAsync(ct);
        var page = PaginatedResponse<EquipmentAssignmentListResponse>.Create(items, request.PageNumber, request.PageSize, total);
        return BaseResponse<PaginatedResponse<EquipmentAssignmentListResponse>>.Success(page);
    }

    public async Task<BaseResponse<EquipmentAssignmentDetailResponse>> GetByIdAsync(Guid id, CancellationToken ct = default)
    {
        var dto = await _db.EquipmentAssignments.AsNoTracking().Where(e => e.Id == id)
            .Select(e => new EquipmentAssignmentDetailResponse
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
                ProjectId = e.ProjectId,
                EmployeeId = e.EmployeeId,
                WarehouseId = e.WarehouseId,
                StartDate = e.StartDate,
                EndDate = e.EndDate,
                IsActive = e.IsActive
            }).FirstOrDefaultAsync(ct);
        return dto is null
            ? BaseResponse<EquipmentAssignmentDetailResponse>.Failure("NotFound")
            : BaseResponse<EquipmentAssignmentDetailResponse>.Success(dto);
    }

    public async Task<BaseResponse<Guid>> CreateAsync(CreateEquipmentAssignmentRequest request, CancellationToken ct = default)
    {
        var entity = new global::Energy.Domain.Assets.EquipmentAssignment
        {
            Id = Guid.NewGuid(),
            EquipmentAssetId = request.EquipmentAssetId,
            ProjectId = request.ProjectId,
            EmployeeId = request.EmployeeId,
            WarehouseId = request.WarehouseId,
            StartDate = request.StartDate,
            EndDate = request.EndDate,
            IsActive = request.IsActive,
            CreatedAt = DateTime.UtcNow,
        };
        _db.EquipmentAssignments.Add(entity);
        await _db.SaveChangesAsync(ct);
        return BaseResponse<Guid>.Success(entity.Id, "Created");
    }

    public async Task<BaseResponse<bool>> UpdateAsync(Guid id, UpdateEquipmentAssignmentRequest request, CancellationToken ct = default)
    {
        var entity = await _db.EquipmentAssignments.FirstOrDefaultAsync(e => e.Id == id, ct);
        if (entity is null) return BaseResponse<bool>.Failure("NotFound");
            entity.EquipmentAssetId = request.EquipmentAssetId;
            entity.ProjectId = request.ProjectId;
            entity.EmployeeId = request.EmployeeId;
            entity.WarehouseId = request.WarehouseId;
            entity.StartDate = request.StartDate;
            entity.EndDate = request.EndDate;
            entity.IsActive = request.IsActive;
        entity.UpdatedAt = DateTime.UtcNow;
        await _db.SaveChangesAsync(ct);
        return BaseResponse<bool>.Success(true, "Updated");
    }

    public async Task<BaseResponse<bool>> DeleteAsync(Guid id, CancellationToken ct = default)
    {
        var entity = await _db.EquipmentAssignments.FirstOrDefaultAsync(e => e.Id == id, ct);
        if (entity is null) return BaseResponse<bool>.Failure("NotFound");
        entity.IsDeleted = true;
        entity.DeletedAt = DateTime.UtcNow;
        await _db.SaveChangesAsync(ct);
        return BaseResponse<bool>.Success(true, "Deleted");
    }
}
