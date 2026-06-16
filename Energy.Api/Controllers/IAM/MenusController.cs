using Asp.Versioning;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Energy.Shared.Models.V1.Common.Responses;
using Energy.Shared.Models.V1.Common.Requests;
using Energy.Shared.Models.V1.System.Requests;
using Energy.Shared.Models.V1.System.Responses;
using Energy.Application.Modules.IAM.Menu.Commands.CreateMenu;
using Energy.Application.Modules.IAM.Menu.Commands.DeleteMenu;
using Energy.Application.Modules.IAM.Menu.Commands.UpdateMenu;
using Energy.Application.Modules.IAM.Menu.Queries.GetMenuById;
using Energy.Application.Modules.IAM.Menu.Queries.GetMenuList;
using Energy.Application.Modules.IAM.Menu.Queries.GetMyMenu;

namespace Energy.Api.Controllers.IAM;

/// <summary>Menü yönetimi uç noktaları (IAM).</summary>
[ApiController]
[ApiVersion("1.0")]
[Route("api/v{version:apiVersion}/menus")]
public sealed class MenusController : ControllerBase
{
    private readonly IMediator _mediator;

    public MenusController(IMediator mediator)
        => _mediator = mediator;

    [HttpGet]
    public async Task<ActionResult<BaseResponse<PaginatedResponse<MenuResponse>>>> GetAll([FromQuery] PaginatedRequest request, CancellationToken ct)
        => Ok(await _mediator.Send(new GetMenuListQuery(request), ct));

    [HttpGet("{id:guid}")]
    public async Task<ActionResult<BaseResponse<MenuResponse>>> GetById(Guid id, CancellationToken ct)
        => Ok(await _mediator.Send(new GetMenuByIdQuery(id), ct));

    [HttpGet("me")]
    public async Task<ActionResult<BaseResponse<IReadOnlyList<MenuTreeNodeResponse>>>> GetMyMenu(CancellationToken ct)
        => Ok(await _mediator.Send(new GetMyMenuQuery(), ct));

    [HttpPost]
    public async Task<ActionResult<BaseResponse<MenuResponse>>> Create(CreateMenuRequest request, CancellationToken ct)
        => Ok(await _mediator.Send(new CreateMenuCommand(request), ct));

    [HttpPut("{id:guid}")]
    public async Task<ActionResult<BaseResponse<MenuResponse>>> Update(Guid id, UpdateMenuRequest request, CancellationToken ct)
        => Ok(await _mediator.Send(new UpdateMenuCommand(id, request), ct));

    [HttpDelete("{id:guid}")]
    public async Task<ActionResult<BaseResponse<bool>>> Delete(Guid id, CancellationToken ct)
        => Ok(await _mediator.Send(new DeleteMenuCommand(id), ct));
}
