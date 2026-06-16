using Microsoft.EntityFrameworkCore;
using Energy.Infrastructure.Persistence;
using Energy.Shared.Models.V1.Common.Responses;
using Energy.Application.IAM.Role.Services;
using Energy.Shared.Models.V1.IAM.Role.Requests;
using Energy.Shared.Models.V1.IAM.Role.Responses;

namespace Energy.Infrastructure.IAM.Role.Services;

/// <summary>Role CRUD servisi (projection, pagination, soft-delete).</summary>
public class RoleService : IRoleService
{
    private readonly AppDbContext _db;

    public RoleService(AppDbContext db) => _db = db;

    public async Task<BaseResponse<PaginatedResponse<RoleListResponse>>> GetListAsync(GetRoleListRequest request, CancellationToken ct = default)
    {
        var query = _db.Roles.AsNoTracking();
        var total = await query.CountAsync(ct);
        var items = await query
            .OrderByDescending(e => e.Id)
            .Skip((request.PageNumber - 1) * request.PageSize)
            .Take(request.PageSize)
            .Select(e => new RoleListResponse
            {
                Id = e.Id,
                Name = e.Name,
                Description = e.Description,
                IsSystem = e.IsSystem,
                CreatedAt = e.CreatedAt
            })
            .ToListAsync(ct);
        var page = PaginatedResponse<RoleListResponse>.Create(items, request.PageNumber, request.PageSize, total);
        return BaseResponse<PaginatedResponse<RoleListResponse>>.Success(page);
    }

    public async Task<BaseResponse<RoleDetailResponse>> GetByIdAsync(Guid id, CancellationToken ct = default)
    {
        var dto = await _db.Roles.AsNoTracking().Where(e => e.Id == id)
            .Select(e => new RoleDetailResponse
            {
                Id = e.Id,
                CreatedAt = e.CreatedAt,
                CreatedBy = e.CreatedBy,
                UpdatedAt = e.UpdatedAt,
                UpdatedBy = e.UpdatedBy,
                IsDeleted = e.IsDeleted,
                DeletedAt = e.DeletedAt,
                DeletedBy = e.DeletedBy,
                Name = e.Name,
                Description = e.Description,
                IsSystem = e.IsSystem
            }).FirstOrDefaultAsync(ct);
        return dto is null
            ? BaseResponse<RoleDetailResponse>.Failure("NotFound")
            : BaseResponse<RoleDetailResponse>.Success(dto);
    }

    public async Task<BaseResponse<Guid>> CreateAsync(CreateRoleRequest request, CancellationToken ct = default)
    {
        var entity = new global::Energy.Domain.IAM.Role
        {
            Id = Guid.NewGuid(),
            Name = request.Name,
            Description = request.Description,
            IsSystem = request.IsSystem,
            CreatedAt = DateTime.UtcNow,
        };
        _db.Roles.Add(entity);
        await _db.SaveChangesAsync(ct);
        return BaseResponse<Guid>.Success(entity.Id, "Created");
    }

    public async Task<BaseResponse<bool>> UpdateAsync(Guid id, UpdateRoleRequest request, CancellationToken ct = default)
    {
        var entity = await _db.Roles.FirstOrDefaultAsync(e => e.Id == id, ct);
        if (entity is null) return BaseResponse<bool>.Failure("NotFound");
            entity.Name = request.Name;
            entity.Description = request.Description;
            entity.IsSystem = request.IsSystem;
        entity.UpdatedAt = DateTime.UtcNow;
        await _db.SaveChangesAsync(ct);
        return BaseResponse<bool>.Success(true, "Updated");
    }

    public async Task<BaseResponse<bool>> DeleteAsync(Guid id, CancellationToken ct = default)
    {
        var entity = await _db.Roles.FirstOrDefaultAsync(e => e.Id == id, ct);
        if (entity is null) return BaseResponse<bool>.Failure("NotFound");
        entity.IsDeleted = true;
        entity.DeletedAt = DateTime.UtcNow;
        await _db.SaveChangesAsync(ct);
        return BaseResponse<bool>.Success(true, "Deleted");
    }
}
