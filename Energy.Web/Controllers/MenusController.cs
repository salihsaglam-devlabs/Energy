using System.Linq;
using Energy.Shared.Identity.Permissions;
using Energy.Shared.Models.V1.Common.Requests;
using Energy.Shared.Models.V1.System.Requests;
using Energy.Web.Clients.Identity;
using Energy.Web.Clients.System;
using Energy.Web.Common.Filters;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Energy.Web.Controllers;

/// <summary>
/// Menus DevExtreme grid + JSON adapter. The new API allows a single
/// <c>RequiredPermissionCode</c> per menu, so the "menu permissions" popup
/// exposes a degenerate multi-select that only persists the first code.
/// </summary>
[Authorize]
[PagePermission(PermissionCatalog.MenuReadAll)]
[Route("menus")]
public sealed class MenusController : Controller
{
    private readonly IMenuApiClient _menus;
    private readonly IPermissionApiClient _permissions;

    public MenusController(IMenuApiClient menus, IPermissionApiClient permissions)
    {
        _menus = menus;
        _permissions = permissions;
    }

    [HttpGet("")]
    [HttpGet("index")]
    public IActionResult Index() => View();

    [HttpGet("list")]
    public async Task<IActionResult> List(int skip = 0, int take = 20, string? searchValue = null, CancellationToken ct = default)
    {
        var pageNumber = (take <= 0 ? 1 : skip / take) + 1;
        var envelope = await _menus.GetAllAsync(new PaginatedRequest
        {
            PageNumber = pageNumber,
            PageSize = take <= 0 ? 20 : take,
            Search = string.IsNullOrWhiteSpace(searchValue) ? null : searchValue
        }, ct);

        var page = envelope.Data;
        var items = (page?.Items ?? Array.Empty<Shared.Models.V1.System.Responses.MenuResponse>())
            .Select(Project).ToArray();

        return Json(new { data = items, totalCount = page?.TotalCount ?? 0 });
    }

    [HttpGet("lookup")]
    public async Task<IActionResult> Lookup(CancellationToken ct)
    {
        var envelope = await _menus.GetAllAsync(new PaginatedRequest { PageNumber = 1, PageSize = 500 }, ct);
        var items = (envelope.Data?.Items ?? Array.Empty<Shared.Models.V1.System.Responses.MenuResponse>())
            .Select(m => new { id = m.Id, name = m.NameKey })
            .ToArray();
        return Json(items);
    }

    [HttpGet("permissions-lookup")]
    public async Task<IActionResult> PermissionsLookup(CancellationToken ct)
    {
        var envelope = await _permissions.GetAllAsync(ct);
        var items = (envelope.Data ?? Array.Empty<Shared.Models.V1.Identity.Responses.PermissionResponse>())
            .OrderBy(p => p.Module).ThenBy(p => p.Action)
            .Select(p => new { id = p.Code, code = p.Code, name = p.DisplayName, module = p.Module })
            .ToArray();
        return Json(items);
    }

    [HttpGet("{id:guid}/permissions")]
    public async Task<IActionResult> GetPermissions(Guid id, CancellationToken ct)
    {
        var envelope = await _menus.GetByIdAsync(id, ct);
        if (envelope.Data is null) return NotFound();
        var selected = string.IsNullOrWhiteSpace(envelope.Data.RequiredPermissionCode)
            ? Array.Empty<string>()
            : new[] { envelope.Data.RequiredPermissionCode! };
        return Json(new { selected });
    }

    public sealed class SetPermissionsInput { public List<string> PermissionIds { get; set; } = new(); }

    [HttpPut("{id:guid}/permissions")]
    [IgnoreAntiforgeryToken]
    public async Task<IActionResult> SetPermissions(Guid id, [FromBody] SetPermissionsInput input, CancellationToken ct)
    {
        var existing = (await _menus.GetByIdAsync(id, ct)).Data;
        if (existing is null) return NotFound();

        var code = input.PermissionIds.FirstOrDefault();
        var envelope = await _menus.UpdateAsync(id, new UpdateMenuRequest
        {
            ParentId = existing.ParentId,
            NameKey = existing.NameKey,
            Url = existing.Url,
            Icon = existing.Icon,
            DisplayOrder = existing.DisplayOrder,
            IsVisible = existing.IsVisible,
            IsActive = existing.IsActive,
            RequiredPermissionCode = string.IsNullOrWhiteSpace(code) ? null : code
        }, ct);
        return Json(envelope);
    }

    public sealed class MenuInput
    {
        public string Name { get; set; } = string.Empty;
        public string? Url { get; set; }
        public string? Icon { get; set; }
        public int Order { get; set; }
        public Guid? ParentId { get; set; }
    }

    [HttpPost("")]
    [IgnoreAntiforgeryToken]
    public async Task<IActionResult> Create([FromBody] MenuInput input, CancellationToken ct)
        => Json(await _menus.CreateAsync(new CreateMenuRequest
        {
            NameKey = input.Name,
            Url = input.Url,
            Icon = input.Icon,
            DisplayOrder = input.Order,
            ParentId = input.ParentId,
            IsVisible = true,
            IsActive = true
        }, ct));

    [HttpPut("{id:guid}")]
    [IgnoreAntiforgeryToken]
    public async Task<IActionResult> Update(Guid id, [FromBody] MenuInput input, CancellationToken ct)
    {
        var existing = (await _menus.GetByIdAsync(id, ct)).Data;
        return Json(await _menus.UpdateAsync(id, new UpdateMenuRequest
        {
            NameKey = input.Name,
            Url = input.Url,
            Icon = input.Icon,
            DisplayOrder = input.Order,
            ParentId = input.ParentId,
            IsVisible = existing?.IsVisible ?? true,
            IsActive = existing?.IsActive ?? true,
            RequiredPermissionCode = existing?.RequiredPermissionCode
        }, ct));
    }

    [HttpDelete("{id:guid}")]
    [IgnoreAntiforgeryToken]
    public async Task<IActionResult> Delete(Guid id, CancellationToken ct)
        => Json(await _menus.DeleteAsync(id, ct));

    private static object Project(Shared.Models.V1.System.Responses.MenuResponse m) => new
    {
        id = m.Id,
        parentId = m.ParentId,
        name = m.NameKey,
        nameKey = m.NameKey,
        url = m.Url,
        icon = m.Icon,
        order = m.DisplayOrder,
        isVisible = m.IsVisible,
        isActive = m.IsActive,
        requiredPermissionCode = m.RequiredPermissionCode
    };
}
