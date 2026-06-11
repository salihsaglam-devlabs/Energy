using Asp.Versioning;
using Energy.Application.Identity.Services;
using Energy.Application.System.Services;
using Energy.Shared.Models.V1.Common.Requests;
using Energy.Shared.Models.V1.Common.Responses;
using Energy.Shared.Models.V1.System.Requests;
using Energy.Shared.Models.V1.System.Responses;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Energy.Api.Controllers;

[ApiController]
[ApiVersion("1.0")]
[Route("api/v{version:apiVersion}/menus")]
public sealed class MenusController : ControllerBase
{
    private readonly IMenuService _menus;
    private readonly ICurrentUser _currentUser;

    public MenusController(IMenuService menus, ICurrentUser currentUser)
    {
        _menus = menus;
        _currentUser = currentUser;
    }

    [HttpGet]
    public async Task<ActionResult<BaseResponse<PaginatedResponse<MenuResponse>>>> GetAll(
        [FromQuery] PaginatedRequest request, CancellationToken ct)
        => Ok(BaseResponse<PaginatedResponse<MenuResponse>>.Success(await _menus.GetAllAsync(request, ct)));

    [HttpGet("{id:guid}")]
    public async Task<ActionResult<BaseResponse<MenuResponse>>> GetById(Guid id, CancellationToken ct)
    {
        var item = await _menus.GetByIdAsync(id, ct)
                   ?? throw new Application.Common.Exceptions.NotFoundException(Energy.Localization.LocalizationKeys.Messages.MenuNotFound, id);
        return Ok(BaseResponse<MenuResponse>.Success(item));
    }

    [HttpGet("me")]
    public async Task<ActionResult<BaseResponse<IReadOnlyList<MenuTreeNodeResponse>>>> GetMyMenu(CancellationToken ct)
        => Ok(BaseResponse<IReadOnlyList<MenuTreeNodeResponse>>.Success(
            await _menus.GetTreeForUserAsync(_currentUser.UserId, ct)));

    [HttpPost]
    public async Task<ActionResult<BaseResponse<MenuResponse>>> Create(CreateMenuRequest request, CancellationToken ct)
        => Ok(BaseResponse<MenuResponse>.Success(await _menus.CreateAsync(request, ct)));

    [HttpPut("{id:guid}")]
    public async Task<ActionResult<BaseResponse<MenuResponse>>> Update(Guid id, UpdateMenuRequest request, CancellationToken ct)
        => Ok(BaseResponse<MenuResponse>.Success(await _menus.UpdateAsync(id, request, ct)));

    [HttpDelete("{id:guid}")]
    public async Task<ActionResult<BaseResponse<bool>>> Delete(Guid id, CancellationToken ct)
        => Ok(BaseResponse<bool>.Success(await _menus.DeleteAsync(id, ct)));
}
