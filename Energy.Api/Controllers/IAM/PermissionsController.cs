using Asp.Versioning;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Energy.Shared.Models.V1.Common.Responses;
using Energy.Shared.Models.V1.Identity.Responses;
using Energy.Application.Modules.IAM.Permission.Queries.GetPermissionByCode;
using Energy.Application.Modules.IAM.Permission.Queries.GetPermissionList;

namespace Energy.Api.Controllers.IAM;

/// <summary>Permission kataloğu uç noktaları (IAM).</summary>
[ApiController]
[ApiVersion("1.0")]
[Route("api/v{version:apiVersion}/permissions")]
public sealed class PermissionsController : ControllerBase
{
    private readonly IMediator _mediator;

    public PermissionsController(IMediator mediator)
        => _mediator = mediator;

    [HttpGet]
    public async Task<ActionResult<BaseResponse<IReadOnlyList<PermissionResponse>>>> GetAll(CancellationToken ct)
        => Ok(await _mediator.Send(new GetPermissionListQuery(), ct));

    [HttpGet("{code}")]
    public async Task<ActionResult<BaseResponse<PermissionResponse>>> GetByCode(string code, CancellationToken ct)
        => Ok(await _mediator.Send(new GetPermissionByCodeQuery(code), ct));
}
