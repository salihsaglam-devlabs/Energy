using Microsoft.EntityFrameworkCore;
using Energy.Infrastructure.Persistence;
using Energy.Shared.Models.V1.Common.Responses;
using Energy.Application.Modules.IAM.RolePermission.Services;
using Energy.Shared.Models.V1.IAM.RolePermission.Requests;
using Energy.Shared.Models.V1.IAM.RolePermission.Responses;

namespace Energy.Infrastructure.Modules.IAM.RolePermission.Services;

/// <summary>RolePermission CRUD servisi (projection, pagination, soft-delete).</summary>
public class RolePermissionService : IRolePermissionService
{
    private readonly EnergyDbContext _db;

    public RolePermissionService(EnergyDbContext db) => _db = db;

    public async Task<BaseResponse<PaginatedResponse<RolePermissionListResponse>>> GetListAsync(GetRolePermissionListRequest request, CancellationToken ct = default)
    {
        var query = _db.RolePermissions.AsNoTracking();
        var total = await query.CountAsync(ct);
        var items = await query
            .OrderByDescending(e => e.Id)
            .Skip((request.PageNumber - 1) * request.PageSize)
            .Take(request.PageSize)
            .Select(e => new RolePermissionListResponse
            {
                Id = e.Id,
                RoleId = e.RoleId,
                PermissionCode = e.PermissionCode,
                CreatedAt = e.CreatedAt
            })
            .ToListAsync(ct);
        var page = PaginatedResponse<RolePermissionListResponse>.Create(items, request.PageNumber, request.PageSize, total);
        return BaseResponse<PaginatedResponse<RolePermissionListResponse>>.Success(page);
    }

    public async Task<BaseResponse<RolePermissionDetailResponse>> GetByIdAsync(Guid id, CancellationToken ct = default)
    {
        var dto = await _db.RolePermissions.AsNoTracking().Where(e => e.Id == id)
            .Select(e => new RolePermissionDetailResponse
            {
                Id = e.Id,
                CreatedAt = e.CreatedAt,
                CreatedBy = e.CreatedBy,
                UpdatedAt = e.UpdatedAt,
                UpdatedBy = e.UpdatedBy,
                IsDeleted = e.IsDeleted,
                DeletedAt = e.DeletedAt,
                DeletedBy = e.DeletedBy,
                RoleId = e.RoleId,
                PermissionCode = e.PermissionCode
            }).FirstOrDefaultAsync(ct);
        return dto is null
            ? BaseResponse<RolePermissionDetailResponse>.Failure("NotFound")
            : BaseResponse<RolePermissionDetailResponse>.Success(dto);
    }

    public async Task<BaseResponse<Guid>> CreateAsync(CreateRolePermissionRequest request, CancellationToken ct = default)
    {
        var entity = new global::Energy.Domain.Modules.IAM.RolePermission
        {
            Id = Guid.NewGuid(),
            RoleId = request.RoleId,
            PermissionCode = request.PermissionCode,
            CreatedAt = DateTime.UtcNow,
        };
        _db.RolePermissions.Add(entity);
        await _db.SaveChangesAsync(ct);
        return BaseResponse<Guid>.Success(entity.Id, "Created");
    }

    public async Task<BaseResponse<bool>> UpdateAsync(Guid id, UpdateRolePermissionRequest request, CancellationToken ct = default)
    {
        var entity = await _db.RolePermissions.FirstOrDefaultAsync(e => e.Id == id, ct);
        if (entity is null) return BaseResponse<bool>.Failure("NotFound");
            entity.RoleId = request.RoleId;
            entity.PermissionCode = request.PermissionCode;
        entity.UpdatedAt = DateTime.UtcNow;
        await _db.SaveChangesAsync(ct);
        return BaseResponse<bool>.Success(true, "Updated");
    }

    public async Task<BaseResponse<bool>> DeleteAsync(Guid id, CancellationToken ct = default)
    {
        var entity = await _db.RolePermissions.FirstOrDefaultAsync(e => e.Id == id, ct);
        if (entity is null) return BaseResponse<bool>.Failure("NotFound");
        entity.IsDeleted = true;
        entity.DeletedAt = DateTime.UtcNow;
        await _db.SaveChangesAsync(ct);
        return BaseResponse<bool>.Success(true, "Deleted");
    }
}
