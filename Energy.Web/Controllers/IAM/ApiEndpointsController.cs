using System.Linq;
using Energy.Shared.Identity.Permissions;
using Energy.Shared.Models.V1.Common.Requests;
using Energy.Shared.Models.V1.System.Requests;
using Energy.Web.Clients.Identity;
using Energy.Web.Clients.System;
using Energy.Web.Common.Filters;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Energy.Web.Controllers.IAM;

/// <summary>
/// API endpoints management grid + JSON adapter (DevExtreme inline editing).
/// </summary>
[Authorize]
[PagePermission(PermissionCatalog.ApiAccessReadAll)]
[Route("api-endpoints")]
public sealed class ApiEndpointsController : Controller
{
    private readonly IApiEndpointApiClient _endpoints;
    private readonly IPermissionApiClient _permissions;

    public ApiEndpointsController(IApiEndpointApiClient endpoints, IPermissionApiClient permissions)
    {
        _endpoints = endpoints;
        _permissions = permissions;
    }

    [HttpGet("")]
    [HttpGet("index")]
    public IActionResult Index() => View();

    [HttpGet("list")]
    public async Task<IActionResult> List(int skip = 0, int take = 20, string? searchValue = null, CancellationToken ct = default)
    {
        var pageNumber = (take <= 0 ? 1 : skip / take) + 1;
        var envelope = await _endpoints.GetAllAsync(new PaginatedRequest
        {
            PageNumber = pageNumber,
            PageSize = take <= 0 ? 20 : take,
            Search = string.IsNullOrWhiteSpace(searchValue) ? null : searchValue
        }, ct);

        var page = envelope.Data;
        var items = (page?.Items ?? Array.Empty<Shared.Models.V1.System.Responses.ApiEndpointResponse>())
            .Select(e => new
            {
                id = e.Id,
                name = e.Name,
                description = e.Description,
                path = e.Path,
                httpMethod = e.HttpMethod,
                isActive = e.IsActive,
                requiredPermissionCode = e.RequiredPermissionCode
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
            .Select(p => new { code = p.Code, name = $"{p.DisplayName} ({p.Code})" })
            .ToArray();
        return Json(items);
    }

    public sealed class EndpointInput
    {
        public string Name { get; set; } = string.Empty;
        public string? Description { get; set; }
        public string Path { get; set; } = string.Empty;
        public string HttpMethod { get; set; } = "GET";
        public bool IsActive { get; set; } = true;
        public string? RequiredPermissionCode { get; set; }
    }

    [HttpPost("")]
    [IgnoreAntiforgeryToken]
    public async Task<IActionResult> Create([FromBody] EndpointInput input, CancellationToken ct)
        => Json(await _endpoints.CreateAsync(new CreateApiEndpointRequest
        {
            Name = input.Name, Description = input.Description, Path = input.Path,
            HttpMethod = input.HttpMethod, IsActive = input.IsActive,
            RequiredPermissionCode = string.IsNullOrWhiteSpace(input.RequiredPermissionCode) ? null : input.RequiredPermissionCode
        }, ct));

    [HttpPut("{id:guid}")]
    [IgnoreAntiforgeryToken]
    public async Task<IActionResult> Update(Guid id, [FromBody] EndpointInput input, CancellationToken ct)
        => Json(await _endpoints.UpdateAsync(id, new UpdateApiEndpointRequest
        {
            Name = input.Name, Description = input.Description, Path = input.Path,
            HttpMethod = input.HttpMethod, IsActive = input.IsActive,
            RequiredPermissionCode = string.IsNullOrWhiteSpace(input.RequiredPermissionCode) ? null : input.RequiredPermissionCode
        }, ct));

    [HttpDelete("{id:guid}")]
    [IgnoreAntiforgeryToken]
    public async Task<IActionResult> Delete(Guid id, CancellationToken ct)
        => Json(await _endpoints.DeleteAsync(id, ct));
}
