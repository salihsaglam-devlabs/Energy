using Energy.Shared.Models.V1.Common.Requests;
using Energy.Shared.Models.V1.System.Requests;
using Energy.Web.Clients.Identity;
using Energy.Web.Clients.System;
using Energy.Web.Common;
using Energy.Web.Common.Filters;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Energy.Web.Controllers;

[Authorize]
[Route("access-rules")]
[Route("system/access-rules")]
[ServiceFilter(typeof(ApiExceptionFilter))]
public sealed class AccessRulesController : Controller
{
    private readonly IAccessRuleApiClient _accessRuleApiClient;
    private readonly IPermissionApiClient _permissionApiClient;

    public AccessRulesController(
        IAccessRuleApiClient accessRuleApiClient,
        IPermissionApiClient permissionApiClient)
    {
        _accessRuleApiClient = accessRuleApiClient;
        _permissionApiClient = permissionApiClient;
    }

    [HttpGet("")]
    [HttpGet("Index")]
    public IActionResult Index()
    {
        ViewData["Title"] = "Access Rules";
        return View();
    }

    [HttpGet("list")]
    public async Task<IActionResult> List([FromQuery] GridLoadOptions options, CancellationToken cancellationToken)
    {
        var envelope = await _accessRuleApiClient.GetAccessRulesAsync(options.ToPaginatedRequest(), cancellationToken);
        return envelope.ToGridResult();
    }

    [HttpGet("{id:guid}")]
    public async Task<IActionResult> Get(Guid id, CancellationToken cancellationToken)
    {
        var envelope = await _accessRuleApiClient.GetAccessRuleAsync(id, cancellationToken);
        return envelope.ToJsonResult();
    }

    [HttpPost("")]
    public async Task<IActionResult> Create([FromBody] CreateAccessRuleRequest request, CancellationToken cancellationToken)
    {
        var envelope = await _accessRuleApiClient.CreateAccessRuleAsync(request, cancellationToken);
        return envelope.ToJsonResult();
    }

    [HttpPut("{id:guid}")]
    public async Task<IActionResult> Update(Guid id, [FromBody] UpdateAccessRuleRequest request, CancellationToken cancellationToken)
    {
        var envelope = await _accessRuleApiClient.UpdateAccessRuleAsync(id, request, cancellationToken);
        return envelope.ToJsonResult();
    }

    [HttpDelete("{id:guid}")]
    public async Task<IActionResult> Delete(Guid id, CancellationToken cancellationToken)
    {
        var envelope = await _accessRuleApiClient.DeleteAccessRuleAsync(id, cancellationToken);
        return envelope.ToJsonResult();
    }

    [HttpGet("permissions-lookup")]
    public async Task<IActionResult> PermissionsLookup(CancellationToken cancellationToken)
    {
        var envelope = await _permissionApiClient.GetPermissionsAsync(
            new PaginatedRequest { PageNumber = 1, PageSize = 200 },
            cancellationToken);

        if (!envelope.IsSuccess || envelope.Data is null)
        {
            return Ok(Array.Empty<object>());
        }

        return Ok(envelope.Data.Items.Select(permission => new
        {
            id = permission.Id,
            code = permission.Code,
            name = permission.Name
        }));
    }

    [HttpGet("{id:guid}/permissions")]
    public async Task<IActionResult> GetPermissions(Guid id, CancellationToken cancellationToken)
    {
        var envelope = await _accessRuleApiClient.GetAccessRulePermissionsAsync(
            id,
            new PaginatedRequest { PageNumber = 1, PageSize = 200 },
            cancellationToken);

        if (!envelope.IsSuccess || envelope.Data is null)
        {
            return Ok(new { selected = Array.Empty<Guid>() });
        }

        return Ok(new { selected = envelope.Data.Items.Select(permission => permission.Id).ToArray() });
    }

    [HttpPut("{id:guid}/permissions")]
    public async Task<IActionResult> SetPermissions(
        Guid id,
        [FromBody] SetAccessRulePermissionsRequest request,
        CancellationToken cancellationToken)
    {
        var envelope = await _accessRuleApiClient.SetAccessRulePermissionsAsync(id, request, cancellationToken);
        return envelope.ToJsonResult();
    }
}

