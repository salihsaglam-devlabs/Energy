using Energy.Application.Common.Exceptions;
using Energy.Application.Identity.Services;
using Energy.Application.System.Services;
using Energy.Domain.System;
using Energy.Infrastructure.Persistence;
using Energy.Localization;
using Energy.Shared.Models.V1.Common.Requests;
using Energy.Shared.Models.V1.Common.Responses;
using Energy.Shared.Models.V1.System.Requests;
using Energy.Shared.Models.V1.System.Responses;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Localization;

namespace Energy.Infrastructure.System.Services;

public sealed class MenuService : IMenuService
{
    private readonly AppDbContext _db;
    private readonly IPermissionResolver _permissions;
    private readonly IStringLocalizer<SharedResource> _localizer;

    public MenuService(AppDbContext db, IPermissionResolver permissions, IStringLocalizer<SharedResource> localizer)
    {
        _db = db;
        _permissions = permissions;
        _localizer = localizer;
    }

    public async Task<PaginatedResponse<MenuResponse>> GetAllAsync(PaginatedRequest request, CancellationToken ct = default)
    {
        var query = _db.Menus.AsNoTracking();
        if (!string.IsNullOrWhiteSpace(request.Search))
        {
            var term = request.Search.Trim().ToLower();
            query = query.Where(m => m.NameKey.ToLower().Contains(term) || (m.Url != null && m.Url.ToLower().Contains(term)));
        }
        var total = await query.CountAsync(ct);
        var page = await query
            .OrderBy(m => m.ParentId).ThenBy(m => m.DisplayOrder)
            .Skip((request.PageNumber - 1) * request.PageSize).Take(request.PageSize)
            .Select(m => Project(m))
            .ToListAsync(ct);
        return PaginatedResponse<MenuResponse>.Create(page, request.PageNumber, request.PageSize, total);
    }

    public async Task<MenuResponse?> GetByIdAsync(Guid id, CancellationToken ct = default)
    {
        return await _db.Menus.AsNoTracking().Where(m => m.Id == id).Select(m => Project(m)).FirstOrDefaultAsync(ct);
    }

    public async Task<MenuResponse> CreateAsync(CreateMenuRequest request, CancellationToken ct = default)
    {
        if (request.ParentId.HasValue && !await _db.Menus.AnyAsync(m => m.Id == request.ParentId, ct))
            throw new NotFoundException(LocalizationKeys.Messages.ParentMenuNotFound, request.ParentId!);

        var menu = new Menu
        {
            Id = Guid.NewGuid(),
            ParentId = request.ParentId,
            NameKey = request.NameKey.Trim(),
            Url = string.IsNullOrWhiteSpace(request.Url) ? null : request.Url.Trim(),
            Icon = string.IsNullOrWhiteSpace(request.Icon) ? null : request.Icon.Trim(),
            DisplayOrder = request.DisplayOrder,
            IsVisible = request.IsVisible,
            IsActive = request.IsActive,
            RequiredPermissionCode = string.IsNullOrWhiteSpace(request.RequiredPermissionCode) ? null : request.RequiredPermissionCode.Trim()
        };
        _db.Menus.Add(menu);
        await _db.SaveChangesAsync(ct);
        return Project(menu);
    }

    public async Task<MenuResponse> UpdateAsync(Guid id, UpdateMenuRequest request, CancellationToken ct = default)
    {
        var menu = await _db.Menus.FirstOrDefaultAsync(m => m.Id == id, ct)
                   ?? throw new NotFoundException(LocalizationKeys.Messages.MenuNotFound, id);

        if (request.ParentId == id) throw new ConflictException(LocalizationKeys.Messages.MenuSelfParent);
        if (await CreatesCycle(id, request.ParentId, ct)) throw new ConflictException(LocalizationKeys.Messages.MenuParentCycle);

        menu.ParentId = request.ParentId;
        menu.NameKey = request.NameKey.Trim();
        menu.Url = string.IsNullOrWhiteSpace(request.Url) ? null : request.Url.Trim();
        menu.Icon = string.IsNullOrWhiteSpace(request.Icon) ? null : request.Icon.Trim();
        menu.DisplayOrder = request.DisplayOrder;
        menu.IsVisible = request.IsVisible;
        menu.IsActive = request.IsActive;
        menu.RequiredPermissionCode = string.IsNullOrWhiteSpace(request.RequiredPermissionCode) ? null : request.RequiredPermissionCode.Trim();
        await _db.SaveChangesAsync(ct);
        return Project(menu);
    }

    public async Task<bool> DeleteAsync(Guid id, CancellationToken ct = default)
    {
        var menu = await _db.Menus.FirstOrDefaultAsync(m => m.Id == id, ct);
        if (menu is null) return false;
        if (await _db.Menus.AnyAsync(m => m.ParentId == id, ct))
            throw new ConflictException(LocalizationKeys.Messages.MenuHasChildren);
        _db.Menus.Remove(menu);
        await _db.SaveChangesAsync(ct);
        return true;
    }

    public async Task<IReadOnlyList<MenuTreeNodeResponse>> GetTreeForUserAsync(Guid? userId, CancellationToken ct = default)
    {
        IReadOnlySet<string> userPermissions = userId.HasValue
            ? await _permissions.GetPermissionsAsync(userId.Value, ct)
            : new HashSet<string>();

        var all = await _db.Menus.AsNoTracking()
            .Where(m => m.IsActive && m.IsVisible)
            .OrderBy(m => m.DisplayOrder).ThenBy(m => m.NameKey)
            .ToListAsync(ct);

        bool IsAllowed(Menu m) =>
            m.RequiredPermissionCode is null || userPermissions.Contains(m.RequiredPermissionCode);

        var allowed = all.Where(IsAllowed).ToList();
        // Root menus (null parent) are keyed under Guid.Empty; no real menu Id is empty.
        var byParent = allowed
            .GroupBy(m => m.ParentId ?? Guid.Empty)
            .ToDictionary(g => g.Key, g => g.ToList());

        IReadOnlyList<MenuTreeNodeResponse> Build(Guid? parentId)
        {
            if (!byParent.TryGetValue(parentId ?? Guid.Empty, out var nodes)) return Array.Empty<MenuTreeNodeResponse>();
            var list = new List<MenuTreeNodeResponse>(nodes.Count);
            foreach (var n in nodes)
            {
                var children = Build(n.Id);
                if (n.Url is null && children.Count == 0) continue; // skip empty containers
                list.Add(new MenuTreeNodeResponse
                {
                    Id = n.Id,
                    Name = ResolveLocalized(n.NameKey),
                    Url = n.Url,
                    Icon = n.Icon,
                    DisplayOrder = n.DisplayOrder,
                    Children = children
                });
            }
            return list;
        }

        return Build(null);
    }

    private string ResolveLocalized(string key)
    {
        var value = _localizer[key];
        return value.ResourceNotFound ? key : value.Value;
    }

    private async Task<bool> CreatesCycle(Guid menuId, Guid? newParentId, CancellationToken ct)
    {
        var cursor = newParentId;
        while (cursor.HasValue)
        {
            if (cursor.Value == menuId) return true;
            cursor = await _db.Menus.AsNoTracking().Where(m => m.Id == cursor).Select(m => m.ParentId).FirstOrDefaultAsync(ct);
        }
        return false;
    }

    private static MenuResponse Project(Menu m) => new()
    {
        Id = m.Id,
        ParentId = m.ParentId,
        NameKey = m.NameKey,
        Url = m.Url,
        Icon = m.Icon,
        DisplayOrder = m.DisplayOrder,
        IsVisible = m.IsVisible,
        IsActive = m.IsActive,
        RequiredPermissionCode = m.RequiredPermissionCode
    };
}
