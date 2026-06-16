using Asp.Versioning;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Energy.Shared.Models.V1.Common.Responses;
using Energy.Shared.Models.V1.Common.Requests;
using Energy.Shared.Models.V1.Identity.Requests;
using Energy.Shared.Models.V1.Identity.Responses;
using Energy.Application.Modules.IAM.Role.Commands.CreateRole;
using Energy.Application.Modules.IAM.Role.Commands.DeleteRole;
using Energy.Application.Modules.IAM.Role.Commands.SetRolePermissions;
using Energy.Application.Modules.IAM.Role.Commands.UpdateRole;
using Energy.Application.Modules.IAM.Role.Queries.GetRoleById;
using Energy.Application.Modules.IAM.Role.Queries.GetRoleList;

namespace Energy.Api.Controllers.IAM;

/// <summary>Rol yönetimi uç noktaları (IAM).</summary>
[ApiController]
[ApiVersion("1.0")]
[Route("api/v{version:apiVersion}/roles")]
public sealed class RolesController : ControllerBase
{
    private readonly IMediator _mediator;

    public RolesController(IMediator mediator)
        => _mediator = mediator;

    [HttpGet]
    public async Task<ActionResult<BaseResponse<PaginatedResponse<RoleSummaryResponse>>>> GetAll([FromQuery] PaginatedRequest request, CancellationToken ct)
        => Ok(await _mediator.Send(new GetRoleListQuery(request), ct));

    [HttpGet("{id:guid}")]
    public async Task<ActionResult<BaseResponse<RoleDetailResponse>>> GetById(Guid id, CancellationToken ct)
        => Ok(await _mediator.Send(new GetRoleByIdQuery(id), ct));

    [HttpPost]
    public async Task<ActionResult<BaseResponse<RoleDetailResponse>>> Create(CreateRoleRequest request, CancellationToken ct)
        => Ok(await _mediator.Send(new CreateRoleCommand(request), ct));

    [HttpPut("{id:guid}")]
    public async Task<ActionResult<BaseResponse<RoleDetailResponse>>> Update(Guid id, UpdateRoleRequest request, CancellationToken ct)
        => Ok(await _mediator.Send(new UpdateRoleCommand(id, request), ct));

    [HttpDelete("{id:guid}")]
    public async Task<ActionResult<BaseResponse<bool>>> Delete(Guid id, CancellationToken ct)
        => Ok(await _mediator.Send(new DeleteRoleCommand(id), ct));

    [HttpPut("{id:guid}/permissions")]
    public async Task<ActionResult<BaseResponse<RoleDetailResponse>>> SetPermissions(Guid id, SetRolePermissionsRequest request, CancellationToken ct)
        => Ok(await _mediator.Send(new SetRolePermissionsCommand(id, request), ct));
}
