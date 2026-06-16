using Microsoft.EntityFrameworkCore;
using Energy.Infrastructure.Persistence;
using Energy.Shared.Models.V1.Common.Responses;
using Energy.Application.Modules.IAM.UserRole.Services;
using Energy.Shared.Models.V1.IAM.UserRole.Requests;
using Energy.Shared.Models.V1.IAM.UserRole.Responses;

namespace Energy.Infrastructure.Modules.IAM.UserRole.Services;

/// <summary>UserRole CRUD servisi (projection, pagination, soft-delete).</summary>
public class UserRoleService : IUserRoleService
{
    private readonly EnergyDbContext _db;

    public UserRoleService(EnergyDbContext db) => _db = db;

    public async Task<BaseResponse<PaginatedResponse<UserRoleListResponse>>> GetListAsync(GetUserRoleListRequest request, CancellationToken ct = default)
    {
        var query = _db.UserRoles.AsNoTracking();
        var total = await query.CountAsync(ct);
        var items = await query
            .OrderByDescending(e => e.Id)
            .Skip((request.PageNumber - 1) * request.PageSize)
            .Take(request.PageSize)
            .Select(e => new UserRoleListResponse
            {
                Id = e.Id,
                UserId = e.UserId,
                RoleId = e.RoleId,
                CreatedAt = e.CreatedAt
            })
            .ToListAsync(ct);
        var page = PaginatedResponse<UserRoleListResponse>.Create(items, request.PageNumber, request.PageSize, total);
        return BaseResponse<PaginatedResponse<UserRoleListResponse>>.Success(page);
    }

    public async Task<BaseResponse<UserRoleDetailResponse>> GetByIdAsync(Guid id, CancellationToken ct = default)
    {
        var dto = await _db.UserRoles.AsNoTracking().Where(e => e.Id == id)
            .Select(e => new UserRoleDetailResponse
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
                RoleId = e.RoleId
            }).FirstOrDefaultAsync(ct);
        return dto is null
            ? BaseResponse<UserRoleDetailResponse>.Failure("NotFound")
            : BaseResponse<UserRoleDetailResponse>.Success(dto);
    }

    public async Task<BaseResponse<Guid>> CreateAsync(CreateUserRoleRequest request, CancellationToken ct = default)
    {
        var entity = new global::Energy.Domain.Modules.IAM.UserRole
        {
            Id = Guid.NewGuid(),
            UserId = request.UserId,
            RoleId = request.RoleId,
            CreatedAt = DateTime.UtcNow,
        };
        _db.UserRoles.Add(entity);
        await _db.SaveChangesAsync(ct);
        return BaseResponse<Guid>.Success(entity.Id, "Created");
    }

    public async Task<BaseResponse<bool>> UpdateAsync(Guid id, UpdateUserRoleRequest request, CancellationToken ct = default)
    {
        var entity = await _db.UserRoles.FirstOrDefaultAsync(e => e.Id == id, ct);
        if (entity is null) return BaseResponse<bool>.Failure("NotFound");
            entity.UserId = request.UserId;
            entity.RoleId = request.RoleId;
        entity.UpdatedAt = DateTime.UtcNow;
        await _db.SaveChangesAsync(ct);
        return BaseResponse<bool>.Success(true, "Updated");
    }

    public async Task<BaseResponse<bool>> DeleteAsync(Guid id, CancellationToken ct = default)
    {
        var entity = await _db.UserRoles.FirstOrDefaultAsync(e => e.Id == id, ct);
        if (entity is null) return BaseResponse<bool>.Failure("NotFound");
        entity.IsDeleted = true;
        entity.DeletedAt = DateTime.UtcNow;
        await _db.SaveChangesAsync(ct);
        return BaseResponse<bool>.Success(true, "Deleted");
    }
}
