using Microsoft.EntityFrameworkCore;
using Energy.Infrastructure.Persistence;
using Energy.Shared.Models.V1.Common.Responses;
using Energy.Application.Assets.EquipmentAsset.Services;
using Energy.Shared.Models.V1.Assets.EquipmentAsset.Requests;
using Energy.Shared.Models.V1.Assets.EquipmentAsset.Responses;

namespace Energy.Infrastructure.Assets.EquipmentAsset.Services;

/// <summary>EquipmentAsset CRUD servisi (projection, pagination, soft-delete).</summary>
public class EquipmentAssetService : IEquipmentAssetService
{
    private readonly AppDbContext _db;

    public EquipmentAssetService(AppDbContext db) => _db = db;

    public async Task<BaseResponse<PaginatedResponse<EquipmentAssetListResponse>>> GetListAsync(GetEquipmentAssetListRequest request, CancellationToken ct = default)
    {
        var query = _db.EquipmentAssets.AsNoTracking();
        var total = await query.CountAsync(ct);
        var items = await query
            .OrderByDescending(e => e.Id)
            .Skip((request.PageNumber - 1) * request.PageSize)
            .Take(request.PageSize)
            .Select(e => new EquipmentAssetListResponse
            {
                Id = e.Id,
                CompanyId = e.CompanyId,
                Code = e.Code,
                Name = e.Name,
                AssetType = e.AssetType,
                SerialNo = e.SerialNo,
                PurchaseDate = e.PurchaseDate,
                IsActive = e.IsActive,
                CreatedAt = e.CreatedAt
            })
            .ToListAsync(ct);
        var page = PaginatedResponse<EquipmentAssetListResponse>.Create(items, request.PageNumber, request.PageSize, total);
        return BaseResponse<PaginatedResponse<EquipmentAssetListResponse>>.Success(page);
    }

    public async Task<BaseResponse<EquipmentAssetDetailResponse>> GetByIdAsync(Guid id, CancellationToken ct = default)
    {
        var dto = await _db.EquipmentAssets.AsNoTracking().Where(e => e.Id == id)
            .Select(e => new EquipmentAssetDetailResponse
            {
                Id = e.Id,
                CreatedAt = e.CreatedAt,
                CreatedBy = e.CreatedBy,
                UpdatedAt = e.UpdatedAt,
                UpdatedBy = e.UpdatedBy,
                IsDeleted = e.IsDeleted,
                DeletedAt = e.DeletedAt,
                DeletedBy = e.DeletedBy,
                CompanyId = e.CompanyId,
                Code = e.Code,
                Name = e.Name,
                AssetType = e.AssetType,
                SerialNo = e.SerialNo,
                PurchaseDate = e.PurchaseDate,
                IsActive = e.IsActive
            }).FirstOrDefaultAsync(ct);
        return dto is null
            ? BaseResponse<EquipmentAssetDetailResponse>.Failure("NotFound")
            : BaseResponse<EquipmentAssetDetailResponse>.Success(dto);
    }

    public async Task<BaseResponse<Guid>> CreateAsync(CreateEquipmentAssetRequest request, CancellationToken ct = default)
    {
        var entity = new global::Energy.Domain.Assets.EquipmentAsset
        {
            Id = Guid.NewGuid(),
            CompanyId = request.CompanyId,
            Code = request.Code,
            Name = request.Name,
            AssetType = request.AssetType,
            SerialNo = request.SerialNo,
            PurchaseDate = request.PurchaseDate,
            IsActive = request.IsActive,
            CreatedAt = DateTime.UtcNow,
        };
        _db.EquipmentAssets.Add(entity);
        await _db.SaveChangesAsync(ct);
        return BaseResponse<Guid>.Success(entity.Id, "Created");
    }

    public async Task<BaseResponse<bool>> UpdateAsync(Guid id, UpdateEquipmentAssetRequest request, CancellationToken ct = default)
    {
        var entity = await _db.EquipmentAssets.FirstOrDefaultAsync(e => e.Id == id, ct);
        if (entity is null) return BaseResponse<bool>.Failure("NotFound");
            entity.CompanyId = request.CompanyId;
            entity.Code = request.Code;
            entity.Name = request.Name;
            entity.AssetType = request.AssetType;
            entity.SerialNo = request.SerialNo;
            entity.PurchaseDate = request.PurchaseDate;
            entity.IsActive = request.IsActive;
        entity.UpdatedAt = DateTime.UtcNow;
        await _db.SaveChangesAsync(ct);
        return BaseResponse<bool>.Success(true, "Updated");
    }

    public async Task<BaseResponse<bool>> DeleteAsync(Guid id, CancellationToken ct = default)
    {
        var entity = await _db.EquipmentAssets.FirstOrDefaultAsync(e => e.Id == id, ct);
        if (entity is null) return BaseResponse<bool>.Failure("NotFound");
        entity.IsDeleted = true;
        entity.DeletedAt = DateTime.UtcNow;
        await _db.SaveChangesAsync(ct);
        return BaseResponse<bool>.Success(true, "Deleted");
    }
}
