using Microsoft.EntityFrameworkCore;
using Energy.Infrastructure.Persistence;
using Energy.Shared.Models.V1.Common.Responses;
using Energy.Application.Modules.IAM.Menu.Services;
using Energy.Shared.Models.V1.IAM.Menu.Requests;
using Energy.Shared.Models.V1.IAM.Menu.Responses;

namespace Energy.Infrastructure.Modules.IAM.Menu.Services;

/// <summary>Menu CRUD servisi (projection, pagination, soft-delete).</summary>
public class MenuService : IMenuService
{
    private readonly AppDbContext _db;

    public MenuService(AppDbContext db) => _db = db;

    public async Task<BaseResponse<PaginatedResponse<MenuListResponse>>> GetListAsync(GetMenuListRequest request, CancellationToken ct = default)
    {
        var query = _db.Menus.AsNoTracking();
        var total = await query.CountAsync(ct);
        var items = await query
            .OrderByDescending(e => e.Id)
            .Skip((request.PageNumber - 1) * request.PageSize)
            .Take(request.PageSize)
            .Select(e => new MenuListResponse
            {
                Id = e.Id,
                ParentId = e.ParentId,
                NameKey = e.NameKey,
                Url = e.Url,
                RequiredPermissionCode = e.RequiredPermissionCode,
                CreatedAt = e.CreatedAt
            })
            .ToListAsync(ct);
        var page = PaginatedResponse<MenuListResponse>.Create(items, request.PageNumber, request.PageSize, total);
        return BaseResponse<PaginatedResponse<MenuListResponse>>.Success(page);
    }

    public async Task<BaseResponse<MenuDetailResponse>> GetByIdAsync(Guid id, CancellationToken ct = default)
    {
        var dto = await _db.Menus.AsNoTracking().Where(e => e.Id == id)
            .Select(e => new MenuDetailResponse
            {
                Id = e.Id,
                CreatedAt = e.CreatedAt,
                CreatedBy = e.CreatedBy,
                UpdatedAt = e.UpdatedAt,
                UpdatedBy = e.UpdatedBy,
                IsDeleted = e.IsDeleted,
                DeletedAt = e.DeletedAt,
                DeletedBy = e.DeletedBy,
                ParentId = e.ParentId,
                NameKey = e.NameKey,
                Url = e.Url,
                RequiredPermissionCode = e.RequiredPermissionCode
            }).FirstOrDefaultAsync(ct);
        return dto is null
            ? BaseResponse<MenuDetailResponse>.Failure("NotFound")
            : BaseResponse<MenuDetailResponse>.Success(dto);
    }

    public async Task<BaseResponse<Guid>> CreateAsync(CreateMenuRequest request, CancellationToken ct = default)
    {
        var entity = new global::Energy.Domain.Modules.IAM.Menu
        {
            Id = Guid.NewGuid(),
            ParentId = request.ParentId,
            NameKey = request.NameKey,
            Url = request.Url,
            RequiredPermissionCode = request.RequiredPermissionCode,
            CreatedAt = DateTime.UtcNow,
        };
        _db.Menus.Add(entity);
        await _db.SaveChangesAsync(ct);
        return BaseResponse<Guid>.Success(entity.Id, "Created");
    }

    public async Task<BaseResponse<bool>> UpdateAsync(Guid id, UpdateMenuRequest request, CancellationToken ct = default)
    {
        var entity = await _db.Menus.FirstOrDefaultAsync(e => e.Id == id, ct);
        if (entity is null) return BaseResponse<bool>.Failure("NotFound");
            entity.ParentId = request.ParentId;
            entity.NameKey = request.NameKey;
            entity.Url = request.Url;
            entity.RequiredPermissionCode = request.RequiredPermissionCode;
        entity.UpdatedAt = DateTime.UtcNow;
        await _db.SaveChangesAsync(ct);
        return BaseResponse<bool>.Success(true, "Updated");
    }

    public async Task<BaseResponse<bool>> DeleteAsync(Guid id, CancellationToken ct = default)
    {
        var entity = await _db.Menus.FirstOrDefaultAsync(e => e.Id == id, ct);
        if (entity is null) return BaseResponse<bool>.Failure("NotFound");
        entity.IsDeleted = true;
        entity.DeletedAt = DateTime.UtcNow;
        await _db.SaveChangesAsync(ct);
        return BaseResponse<bool>.Success(true, "Deleted");
    }
}
