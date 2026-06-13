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
/// Tek ekranlı kullanıcı erişim yönetimi. Bir yönetici, tek bir sayfadan bir kullanıcının
/// rollerini ve bireysel yetkilerini tamamen DevExtreme onay kutuları aracılığıyla
/// verebilir veya geri alabilir. Her aksiyon API'ye vekillenir ve
/// <see cref="PermissionCatalog.UserUpdate"/> yetkisiyle korunur.
/// </summary>
[Authorize]
[PagePermission(PermissionCatalog.UserUpdate)]
[Route("user-access")]
public sealed class UserAccessController : Controller
{
    private readonly IUserApiClient _users;
    private readonly IRoleApiClient _roles;
    private readonly IPermissionApiClient _permissions;

    public UserAccessController(IUserApiClient users, IRoleApiClient roles, IPermissionApiClient permissions)
    {
        _users = users;
        _roles = roles;
        _permissions = permissions;
    }

    [HttpGet("")]
    [HttpGet("index")]
    public IActionResult Index() => View();

    /// <summary>Kullanıcı seçici grid'i için DevExtreme CustomStore yükleme uç noktası.</summary>
    [HttpGet("users-list")]
    public async Task<IActionResult> UsersList(int skip = 0, int take = 20, string? searchValue = null, CancellationToken ct = default)
    {
        var pageNumber = (take <= 0 ? 1 : skip / take) + 1;
        var envelope = await _users.GetAllAsync(new PaginatedRequest
        {
            PageNumber = pageNumber,
            PageSize = take <= 0 ? 20 : take,
            Search = string.IsNullOrWhiteSpace(searchValue) ? null : searchValue
        }, ct);

        var page = envelope.Data;
        var items = (page?.Items ?? Array.Empty<Shared.Models.V1.Identity.Responses.UserSummaryResponse>())
            .Select(u => new
            {
                id = u.Id,
                userName = u.UserName,
                email = u.Email,
                fullName = u.FullName,
                isActive = u.IsActive
            })
            .ToArray();

        return Json(new { data = items, totalCount = page?.TotalCount ?? 0 });
    }

    /// <summary>Rol onay kutusu listesi için tüm roller.</summary>
    [HttpGet("roles-lookup")]
    public async Task<IActionResult> RolesLookup(CancellationToken ct)
    {
        var envelope = await _roles.GetAllAsync(new PaginatedRequest { PageNumber = 1, PageSize = 500 }, ct);
        var items = (envelope.Data?.Items ?? Array.Empty<Shared.Models.V1.Identity.Responses.RoleSummaryResponse>())
            .OrderBy(r => r.Name)
            .Select(r => new { id = r.Id, name = r.Name, description = r.Description, isSystem = r.IsSystem })
            .ToArray();
        return Json(items);
    }

    /// <summary>Yetki ağacı için modüllere göre gruplanmış tam yetki kataloğu.</summary>
    [HttpGet("permissions-catalog")]
    public async Task<IActionResult> PermissionsCatalog(CancellationToken ct)
    {
        var envelope = await _permissions.GetAllAsync(ct);
        var items = (envelope.Data ?? Array.Empty<Shared.Models.V1.Identity.Responses.PermissionResponse>())
            .OrderBy(p => p.Module).ThenBy(p => p.Action)
            .Select(p => new { code = p.Code, name = p.DisplayName, module = p.Module, action = p.Action })
            .ToArray();
        return Json(items);
    }

    /// <summary>Tek bir kullanıcının geçerli erişim anlık görüntüsü.</summary>
    [HttpGet("{id:guid}/access")]
    public async Task<IActionResult> GetAccess(Guid id, CancellationToken ct)
    {
        var envelope = await _users.GetAccessAsync(id, ct);
        if (envelope.Data is null) return NotFound();
        var a = envelope.Data;
        return Json(new
        {
            userName = a.UserName,
            fullName = a.FullName,
            isActive = a.IsActive,
            roleIds = a.RoleIds,
            rolePermissionCodes = a.RolePermissionCodes,
            directPermissionCodes = a.DirectPermissionCodes
        });
    }

    public sealed class UserAccessInput
    {
        public List<Guid> RoleIds { get; set; } = new();
        public List<string> DirectPermissionCodes { get; set; } = new();
    }

    [HttpPut("{id:guid}/access")]
    [IgnoreAntiforgeryToken]
    public async Task<IActionResult> SetAccess(Guid id, [FromBody] UserAccessInput input, CancellationToken ct)
    {
        var envelope = await _users.SetAccessAsync(id, new SetUserAccessRequest
        {
            RoleIds = input.RoleIds,
            DirectPermissionCodes = input.DirectPermissionCodes
        }, ct);
        return Json(envelope);
    }
}

