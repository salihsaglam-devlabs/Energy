using Microsoft.EntityFrameworkCore;
using Energy.Infrastructure.Persistence;
using Energy.Shared.Models.V1.Common.Responses;
using Energy.Application.Modules.IAM.UserPermission.Services;
using Energy.Shared.Models.V1.IAM.UserPermission.Requests;
using Energy.Shared.Models.V1.IAM.UserPermission.Responses;

namespace Energy.Infrastructure.Modules.IAM.UserPermission.Services;

/// <summary>UserPermission CRUD servisi (projection, pagination, soft-delete).</summary>
public class UserPermissionService : IUserPermissionService
{
    private readonly EnergyDbContext _db;

    public UserPermissionService(EnergyDbContext db) => _db = db;

    public async Task<BaseResponse<PaginatedResponse<UserPermissionListResponse>>> GetListAsync(GetUserPermissionListRequest request, CancellationToken ct = default)
    {
        var query = _db.UserPermissions.AsNoTracking();
        var total = await query.CountAsync(ct);
        var items = await query
            .OrderByDescending(e => e.Id)
            .Skip((request.PageNumber - 1) * request.PageSize)
            .Take(request.PageSize)
            .Select(e => new UserPermissionListResponse
            {
                Id = e.Id,
                UserId = e.UserId,
                PermissionCode = e.PermissionCode,
                CreatedAt = e.CreatedAt
            })
            .ToListAsync(ct);
        var page = PaginatedResponse<UserPermissionListResponse>.Create(items, request.PageNumber, request.PageSize, total);
        return BaseResponse<PaginatedResponse<UserPermissionListResponse>>.Success(page);
    }

    public async Task<BaseResponse<UserPermissionDetailResponse>> GetByIdAsync(Guid id, CancellationToken ct = default)
    {
        var dto = await _db.UserPermissions.AsNoTracking().Where(e => e.Id == id)
            .Select(e => new UserPermissionDetailResponse
            {
                Id = e.Id,
                CreatedAt = e.CreatedAt,
                CreatedBy = e.CreatedBy,
                UpdatedAt = e.UpdatedAt,
                UpdatedBy = e.UpdatedBy,
                IsDeleted = e.IsDeleted,
                DeletedAt = e.DeletedAt,
                DeletedBy = e.DeletedBy,
                UserId = e.UserId,
                PermissionCode = e.PermissionCode
            }).FirstOrDefaultAsync(ct);
        return dto is null
            ? BaseResponse<UserPermissionDetailResponse>.Failure("NotFound")
            : BaseResponse<UserPermissionDetailResponse>.Success(dto);
    }

    public async Task<BaseResponse<Guid>> CreateAsync(CreateUserPermissionRequest request, CancellationToken ct = default)
    {
        var entity = new global::Energy.Domain.Modules.IAM.UserPermission
        {
            Id = Guid.NewGuid(),
            UserId = request.UserId,
            PermissionCode = request.PermissionCode,
            CreatedAt = DateTime.UtcNow,
        };
        _db.UserPermissions.Add(entity);
        await _db.SaveChangesAsync(ct);
        return BaseResponse<Guid>.Success(entity.Id, "Created");
    }

    public async Task<BaseResponse<bool>> UpdateAsync(Guid id, UpdateUserPermissionRequest request, CancellationToken ct = default)
    {
        var entity = await _db.UserPermissions.FirstOrDefaultAsync(e => e.Id == id, ct);
        if (entity is null) return BaseResponse<bool>.Failure("NotFound");
            entity.UserId = request.UserId;
            entity.PermissionCode = request.PermissionCode;
        entity.UpdatedAt = DateTime.UtcNow;
        await _db.SaveChangesAsync(ct);
        return BaseResponse<bool>.Success(true, "Updated");
    }

    public async Task<BaseResponse<bool>> DeleteAsync(Guid id, CancellationToken ct = default)
    {
        var entity = await _db.UserPermissions.FirstOrDefaultAsync(e => e.Id == id, ct);
        if (entity is null) return BaseResponse<bool>.Failure("NotFound");
        entity.IsDeleted = true;
        entity.DeletedAt = DateTime.UtcNow;
        await _db.SaveChangesAsync(ct);
        return BaseResponse<bool>.Success(true, "Deleted");
    }
}
