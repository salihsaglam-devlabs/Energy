using Energy.Localization;
using Energy.Shared.Models.V1.Identity.Requests;
using Energy.Web.Clients.Identity;
using Energy.Web.Common;
using Energy.Web.Common.Filters;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Localization;

namespace Energy.Web.Controllers;

[Authorize]
[Route("users")]
[Route("system/users")]
[ServiceFilter(typeof(ApiExceptionFilter))]
public sealed class UsersController : Controller
{
    private readonly IUserApiClient _userApiClient;
    private readonly IRoleApiClient _roleApiClient;
    private readonly IStringLocalizer<SharedResource> _localizer;

    public UsersController(
        IUserApiClient userApiClient,
        IRoleApiClient roleApiClient,
        IStringLocalizer<SharedResource> localizer)
    {
        _userApiClient = userApiClient;
        _roleApiClient = roleApiClient;
        _localizer = localizer;
    }

    [HttpGet("")]
    [HttpGet("Index")]
    public IActionResult Index()
    {
        ViewData["Title"] = _localizer.GetText(LocalizationKeys.UsersScreen.Title);
        return View();
    }

    [HttpGet("list")]
    public async Task<IActionResult> List(
        [FromQuery] GridLoadOptions options,
        CancellationToken cancellationToken)
    {
        var envelope = await _userApiClient.GetUsersAsync(options.ToPaginatedRequest(), cancellationToken);
        return envelope.ToGridResult();
    }

    [HttpGet("roles-lookup")]
    public async Task<IActionResult> RolesLookup(CancellationToken cancellationToken)
    {
        var envelope = await _roleApiClient.GetRolesAsync(
            new Energy.Shared.Models.V1.Common.Requests.PaginatedRequest { PageNumber = 1, PageSize = 100 },
            cancellationToken);

        if (!envelope.IsSuccess || envelope.Data is null)
        {
            return Ok(Array.Empty<object>());
        }

        return Ok(envelope.Data.Items.Select(r => new { id = r.Id, name = r.Name, description = r.Description }));
    }

    [HttpGet("{id:guid}")]
    public async Task<IActionResult> Get(Guid id, CancellationToken cancellationToken)
    {
        var envelope = await _userApiClient.GetUserAsync(id, cancellationToken);
        return envelope.ToJsonResult();
    }

    [HttpPost("")]
    public async Task<IActionResult> Create(
        [FromBody] CreateUserRequest request,
        CancellationToken cancellationToken)
    {
        var envelope = await _userApiClient.CreateUserAsync(request, cancellationToken);
        return envelope.ToJsonResult();
    }

    [HttpPut("{id:guid}")]
    public async Task<IActionResult> Update(
        Guid id,
        [FromBody] UpdateUserRequest request,
        CancellationToken cancellationToken)
    {
        var envelope = await _userApiClient.UpdateUserAsync(id, request, cancellationToken);
        return envelope.ToJsonResult();
    }

    [HttpDelete("{id:guid}")]
    public async Task<IActionResult> Delete(Guid id, CancellationToken cancellationToken)
    {
        var envelope = await _userApiClient.DeleteUserAsync(id, cancellationToken);
        return envelope.ToJsonResult();
    }

    [HttpPut("{id:guid}/roles")]
    public async Task<IActionResult> SetRoles(
        Guid id,
        [FromBody] SetUserRolesRequest request,
        CancellationToken cancellationToken)
    {
        var envelope = await _userApiClient.SetRolesAsync(id, request, cancellationToken);
        return envelope.ToJsonResult();
    }

    [HttpPut("{id:guid}/password")]
    public async Task<IActionResult> UpdatePassword(
        Guid id,
        [FromBody] UpdateUserPasswordRequest request,
        CancellationToken cancellationToken)
    {
        var envelope = await _userApiClient.UpdatePasswordAsync(id, request, cancellationToken);
        return envelope.ToJsonResult();
    }
}

