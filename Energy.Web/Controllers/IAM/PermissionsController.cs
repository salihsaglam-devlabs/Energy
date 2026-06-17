using System.Linq;
using Energy.Shared.Identity.Permissions;
using Energy.Web.Clients.Identity;
using Energy.Web.Common.Filters;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Energy.Web.Controllers.IAM;

/// <summary>
/// Yetki kataloğu salt okunurdur — API'nin başlangıçta veritabanına senkronladığı
/// derleme zamanı <c>PermissionCatalog</c>'unu yüzeye çıkarır. Görünüm, aşağıdaki JSON
/// liste uç noktasını tüketen bir dxDataGrid oluşturur.
/// </summary>
[Authorize]
[PagePermission(PermissionCatalog.PermissionReadAll)]
[Route("permissions")]
public sealed class PermissionsController : Controller
{
    private readonly IPermissionApiClient _permissions;

    public PermissionsController(IPermissionApiClient permissions)
    {
        _permissions = permissions;
    }

    [HttpGet("")]
    [HttpGet("index")]
    public IActionResult Index() => View();

    [HttpGet("list")]
    public async Task<IActionResult> List(CancellationToken ct)
    {
        var envelope = await _permissions.GetAllAsync(ct);
        var items = (envelope.Data ?? Array.Empty<Shared.Models.V1.Identity.Responses.PermissionResponse>())
            .OrderBy(p => p.Module).ThenBy(p => p.Action)
            .Select(p => new
            {
                code = p.Code,
                module = p.Module,
                action = p.Action,
                name = p.DisplayName,
                description = p.Description,
                roleCount = p.RoleCount,
                menuCount = p.MenuCount,
                endpointCount = p.EndpointCount
            })
            .ToArray();
        return Json(items);
    }
}
