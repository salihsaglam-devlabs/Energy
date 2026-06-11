using Asp.Versioning;
using Energy.Application.System.AccessRules.Commands.CreateAccessRule;
using Energy.Application.System.AccessRules.Commands.DeleteAccessRule;
using Energy.Application.System.AccessRules.Commands.SetAccessRulePermissions;
using Energy.Application.System.AccessRules.Commands.UpdateAccessRule;
using Energy.Application.System.AccessRules.Queries.GetAccessRuleById;
using Energy.Application.System.AccessRules.Queries.GetAccessRulePermissions;
using Energy.Application.System.AccessRules.Queries.GetAccessRules;
using Energy.Application.System.AccessRules.Queries.GetRequiredPermissionsForRequest;
using Energy.Shared.Identity.Permissions;
using Energy.Shared.Models.V1.System.Requests;
using Energy.Shared.Versioning;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Energy.Api.Controllers;

[ApiController]
[ApiVersion(ApiVersions.V1)]
[Route("api/v{version:apiVersion}/access-rules")]
[Authorize]
public sealed class AccessRulesController : ControllerBase
{
    private readonly ISender _sender;

    public AccessRulesController(ISender sender)
    {
        _sender = sender;
    }

    [HttpGet]
    [Authorize(Policy = AccessRulePermissions.GetAccessRules)]
    public async Task<IActionResult> GetAccessRules([FromQuery] GetAccessRulesQuery query, CancellationToken cancellationToken)
    {
        var response = await _sender.Send(query, cancellationToken);
        return Ok(response);
    }

    [HttpGet("{id:guid}")]
    [Authorize(Policy = AccessRulePermissions.GetAccessRule)]
    public async Task<IActionResult> GetAccessRule(Guid id, CancellationToken cancellationToken)
    {
        var response = await _sender.Send(new GetAccessRuleByIdQuery(id), cancellationToken);
        return Ok(response);
    }

    [HttpPost]
    [Authorize(Policy = AccessRulePermissions.CreateAccessRule)]
    public async Task<IActionResult> CreateAccessRule([FromBody] CreateAccessRuleRequest request, CancellationToken cancellationToken)
    {
        var response = await _sender.Send(new CreateAccessRuleCommand(request), cancellationToken);
        return Ok(response);
    }

    [HttpPut("{id:guid}")]
    [Authorize(Policy = AccessRulePermissions.UpdateAccessRule)]
    public async Task<IActionResult> UpdateAccessRule(Guid id, [FromBody] UpdateAccessRuleRequest request, CancellationToken cancellationToken)
    {
        var response = await _sender.Send(new UpdateAccessRuleCommand(id, request), cancellationToken);
        return Ok(response);
    }

    [HttpDelete("{id:guid}")]
    [Authorize(Policy = AccessRulePermissions.DeleteAccessRule)]
    public async Task<IActionResult> DeleteAccessRule(Guid id, CancellationToken cancellationToken)
    {
        var response = await _sender.Send(new DeleteAccessRuleCommand(id), cancellationToken);
        return Ok(response);
    }

    [HttpGet("{id:guid}/permissions")]
    [Authorize(Policy = AccessRulePermissions.GetAccessRulePermissions)]
    public async Task<IActionResult> GetAccessRulePermissions(
        Guid id,
        [FromQuery] GetAccessRulePermissionsQuery query,
        CancellationToken cancellationToken)
    {
        var effectiveQuery = new GetAccessRulePermissionsQuery(id)
        {
            PageNumber = query.PageNumber,
            PageSize = query.PageSize,
            Search = query.Search,
            SortBy = query.SortBy,
            IsDescending = query.IsDescending,
            Filters = query.Filters
        };

        var response = await _sender.Send(effectiveQuery, cancellationToken);
        return Ok(response);
    }

    [HttpPut("{id:guid}/permissions")]
    [Authorize(Policy = AccessRulePermissions.SetAccessRulePermissions)]
    public async Task<IActionResult> SetAccessRulePermissions(
        Guid id,
        [FromBody] SetAccessRulePermissionsRequest request,
        CancellationToken cancellationToken)
    {
        var response = await _sender.Send(new SetAccessRulePermissionsCommand(id, request.PermissionIds), cancellationToken);
        return Ok(response);
    }

    [HttpGet("required-permissions")]
    [Authorize(Policy = AccessRulePermissions.GetRequiredPermissions)]
    public async Task<IActionResult> GetRequiredPermissions(
        [FromQuery] string scope,
        [FromQuery] string path,
        [FromQuery] string? httpMethod,
        CancellationToken cancellationToken)
    {
        var response = await _sender.Send(new GetRequiredPermissionsForRequestQuery(scope, path, httpMethod), cancellationToken);
        return Ok(response);
    }
}

