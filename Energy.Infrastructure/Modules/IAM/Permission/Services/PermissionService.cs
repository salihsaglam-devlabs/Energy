using Microsoft.EntityFrameworkCore;
using Energy.Infrastructure.Persistence;
using Energy.Shared.Models.V1.Common.Responses;
using Energy.Application.Modules.IAM.Permission.Services;
using Energy.Shared.Models.V1.IAM.Permission.Requests;
using Energy.Shared.Models.V1.IAM.Permission.Responses;

namespace Energy.Infrastructure.Modules.IAM.Permission.Services;

/// <summary>Permission CRUD servisi (projection, pagination, soft-delete).</summary>
public class PermissionService : IPermissionService
{
    private readonly EnergyDbContext _db;

    public PermissionService(EnergyDbContext db) => _db = db;

    public async Task<BaseResponse<PaginatedResponse<PermissionListResponse>>> GetListAsync(GetPermissionListRequest request, CancellationToken ct = default)
    {
        var query = _db.Permissions.AsNoTracking();
        var total = await query.CountAsync(ct);
        var items = await query
            .OrderByDescending(e => e.Id)
            .Skip((request.PageNumber - 1) * request.PageSize)
            .Take(request.PageSize)
            .Select(e => new PermissionListResponse
            {
                Id = e.Id,
                Code = e.Code,
                Module = e.Module,
                Action = e.Action,
                DisplayNameKey = e.DisplayNameKey,
                CreatedAt = e.CreatedAt
            })
            .ToListAsync(ct);
        var page = PaginatedResponse<PermissionListResponse>.Create(items, request.PageNumber, request.PageSize, total);
        return BaseResponse<PaginatedResponse<PermissionListResponse>>.Success(page);
    }

    public async Task<BaseResponse<PermissionDetailResponse>> GetByIdAsync(Guid id, CancellationToken ct = default)
    {
        var dto = await _db.Permissions.AsNoTracking().Where(e => e.Id == id)
            .Select(e => new PermissionDetailResponse
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
                Module = e.Module,
                Action = e.Action,
                DisplayNameKey = e.DisplayNameKey
            }).FirstOrDefaultAsync(ct);
        return dto is null
            ? BaseResponse<PermissionDetailResponse>.Failure("NotFound")
            : BaseResponse<PermissionDetailResponse>.Success(dto);
    }

    public async Task<BaseResponse<Guid>> CreateAsync(CreatePermissionRequest request, CancellationToken ct = default)
    {
        var entity = new global::Energy.Domain.Modules.IAM.Permission
        {
            Id = Guid.NewGuid(),
            Code = request.Code,
            Module = request.Module,
            Action = request.Action,
            DisplayNameKey = request.DisplayNameKey,
            CreatedAt = DateTime.UtcNow,
        };
        _db.Permissions.Add(entity);
        await _db.SaveChangesAsync(ct);
        return BaseResponse<Guid>.Success(entity.Id, "Created");
    }

    public async Task<BaseResponse<bool>> UpdateAsync(Guid id, UpdatePermissionRequest request, CancellationToken ct = default)
    {
        var entity = await _db.Permissions.FirstOrDefaultAsync(e => e.Id == id, ct);
        if (entity is null) return BaseResponse<bool>.Failure("NotFound");
            entity.Code = request.Code;
            entity.Module = request.Module;
            entity.Action = request.Action;
            entity.DisplayNameKey = request.DisplayNameKey;
        entity.UpdatedAt = DateTime.UtcNow;
        await _db.SaveChangesAsync(ct);
        return BaseResponse<bool>.Success(true, "Updated");
    }

    public async Task<BaseResponse<bool>> DeleteAsync(Guid id, CancellationToken ct = default)
    {
        var entity = await _db.Permissions.FirstOrDefaultAsync(e => e.Id == id, ct);
        if (entity is null) return BaseResponse<bool>.Failure("NotFound");
        entity.IsDeleted = true;
        entity.DeletedAt = DateTime.UtcNow;
        await _db.SaveChangesAsync(ct);
        return BaseResponse<bool>.Success(true, "Deleted");
    }
}
