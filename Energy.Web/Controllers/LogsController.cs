using Energy.Shared.Identity.Permissions;
using Energy.Shared.Models.V1.Common.Requests;
using Energy.Web.Clients.Logger;
using Energy.Web.Common.Filters;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Energy.Web.Controllers;

[Authorize]
[PagePermission(PermissionCatalog.LogReadAll)]
public sealed class LogsController : Controller
{
    private readonly IAuditLogQueryClient _logs;

    public LogsController(IAuditLogQueryClient logs)
    {
        _logs = logs;
    }

    [HttpGet]
    public IActionResult Index() => View();

    /// <summary>DevExtreme CustomStore load endpoint, proxied to the API.</summary>
    [HttpGet("/logs/list")]
    public async Task<IActionResult> List(int skip = 0, int take = 25, CancellationToken ct = default)
    {
        var pageNumber = (take <= 0 ? 1 : skip / take) + 1;
        var envelope = await _logs.GetAllAsync(new PaginatedRequest
        {
            PageNumber = pageNumber,
            PageSize = take <= 0 ? 25 : take
        }, ct);

        var page = envelope.Data;
        var items = (page?.Items ?? Array.Empty<Shared.Models.V1.Logger.Responses.AuditLogResponse>())
            .Select(l => new
            {
                id = l.Id,
                occurredAt = l.OccurredAt,
                source = l.Source,
                userName = l.UserName,
                ipAddress = l.IpAddress,
                httpMethod = l.HttpMethod,
                path = l.Path,
                statusCode = l.StatusCode,
                isSuccess = l.IsSuccess,
                hasException = l.HasException,
                durationMs = l.DurationMs
            })
            .ToArray();

        return Json(new { data = items, totalCount = page?.TotalCount ?? 0 });
    }

    [HttpGet("/logs/{id:long}")]
    public IActionResult Details(long id)
    {
        ViewBag.LogId = id;
        return View();
    }

    /// <summary>Single audit entry as JSON, proxied to the API.</summary>
    [HttpGet("/logs/{id:long}/detail")]
    public async Task<IActionResult> Detail(long id, CancellationToken ct)
    {
        var envelope = await _logs.GetByIdAsync(id, ct);
        if (envelope.Data is null) return NotFound();
        return Json(new { data = envelope.Data });
    }
}
