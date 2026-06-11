using System.Linq;
using Energy.Shared.Identity.Permissions;
using Energy.Shared.Models.V1.Common.Requests;
using Energy.Shared.Models.V1.Identity.Requests;
using Energy.Web.Clients.Identity;
using Energy.Web.Common.Filters;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Energy.Web.Controllers;

/// <summary>
/// Roles management DevExtreme grid + JSON adapter. Permission "ids" exposed
/// to the front-end are actually permission codes — the JS only needs a
/// stable key/value pair for the tag-box, and the new API consumes codes
/// directly via <c>SetPermissionsAsync</c>. Role↔menu visibility is implicit:
/// a role sees a menu when it owns that menu's <c>RequiredPermissionCode</c>,
/// so there is no separate role-menu mapping to manage here.
/// </summary>
[Authorize]
[PagePermission(PermissionCatalog.RoleReadAll)]
[Route("roles")]
public sealed class RolesController : Controller
{
    private readonly IRoleApiClient _roles;
    private readonly IPermissionApiClient _permissions;

    public RolesController(IRoleApiClient roles, IPermissionApiClient permissions)
    {
        _roles = roles;
        _permissions = permissions;
    }

    [HttpGet("")]
    [HttpGet("index")]
    public IActionResult Index() => View();

    [HttpGet("list")]
    public async Task<IActionResult> List(int skip = 0, int take = 20, string? searchValue = null, CancellationToken ct = default)
    {
        var pageNumber = (take <= 0 ? 1 : skip / take) + 1;
        var envelope = await _roles.GetAllAsync(new PaginatedRequest
        {
            PageNumber = pageNumber,
            PageSize = take <= 0 ? 20 : take,
            Search = string.IsNullOrWhiteSpace(searchValue) ? null : searchValue
        }, ct);

        var page = envelope.Data;
        var items = (page?.Items ?? Array.Empty<Shared.Models.V1.Identity.Responses.RoleSummaryResponse>())
            .Select(r => new
            {
                id = r.Id,
                name = r.Name,
                description = r.Description,
                isSystem = r.IsSystem,
                permissionCount = r.PermissionCount,
                userCount = r.UserCount
            })
            .ToArray();

        return Json(new { data = items, totalCount = page?.TotalCount ?? 0 });
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
        var envelope = await _roles.GetByIdAsync(id, ct);
        if (envelope.Data is null) return NotFound();
        return Json(new { selected = envelope.Data.PermissionCodes });
    }

    public sealed class SetPermissionsInput { public List<string> PermissionIds { get; set; } = new(); }

    [HttpPut("{id:guid}/permissions")]
    [IgnoreAntiforgeryToken]
    public async Task<IActionResult> SetPermissions(Guid id, [FromBody] SetPermissionsInput input, CancellationToken ct)
    {
        var envelope = await _roles.SetPermissionsAsync(id, new SetRolePermissionsRequest
        {
            PermissionCodes = input.PermissionIds
        }, ct);
        return Json(envelope);
    }


    public sealed class RoleInput
    {
        public string Name { get; set; } = string.Empty;
        public string? Description { get; set; }
    }

    [HttpPost("")]
    [IgnoreAntiforgeryToken]
    public async Task<IActionResult> Create([FromBody] RoleInput input, CancellationToken ct)
        => Json(await _roles.CreateAsync(new CreateRoleRequest { Name = input.Name, Description = input.Description }, ct));

    [HttpPut("{id:guid}")]
    [IgnoreAntiforgeryToken]
    public async Task<IActionResult> Update(Guid id, [FromBody] RoleInput input, CancellationToken ct)
        => Json(await _roles.UpdateAsync(id, new UpdateRoleRequest { Name = input.Name, Description = input.Description }, ct));

    [HttpDelete("{id:guid}")]
    [IgnoreAntiforgeryToken]
    public async Task<IActionResult> Delete(Guid id, CancellationToken ct)
        => Json(await _roles.DeleteAsync(id, ct));
}
