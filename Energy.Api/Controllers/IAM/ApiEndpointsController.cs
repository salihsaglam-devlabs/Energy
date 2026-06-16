using Asp.Versioning;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Energy.Shared.Models.V1.Common.Responses;
using Energy.Shared.Models.V1.Common.Requests;
using Energy.Shared.Models.V1.System.Requests;
using Energy.Shared.Models.V1.System.Responses;
using Energy.Application.IAM.ApiEndpoint.Commands.CreateApiEndpoint;
using Energy.Application.IAM.ApiEndpoint.Commands.DeleteApiEndpoint;
using Energy.Application.IAM.ApiEndpoint.Commands.UpdateApiEndpoint;
using Energy.Application.IAM.ApiEndpoint.Queries.GetApiEndpointById;
using Energy.Application.IAM.ApiEndpoint.Queries.GetApiEndpointList;

namespace Energy.Api.Controllers.IAM;

/// <summary>API endpoint kataloğu uç noktaları (IAM).</summary>
[ApiController]
[ApiVersion("1.0")]
[Route("api/v{version:apiVersion}/api-endpoints")]
public sealed class ApiEndpointsController : ControllerBase
{
    private readonly IMediator _mediator;

    public ApiEndpointsController(IMediator mediator)
        => _mediator = mediator;

    [HttpGet]
    public async Task<ActionResult<BaseResponse<PaginatedResponse<ApiEndpointResponse>>>> GetAll([FromQuery] PaginatedRequest request, CancellationToken ct)
        => Ok(await _mediator.Send(new GetApiEndpointListQuery(request), ct));

    [HttpGet("{id:guid}")]
    public async Task<ActionResult<BaseResponse<ApiEndpointResponse>>> GetById(Guid id, CancellationToken ct)
        => Ok(await _mediator.Send(new GetApiEndpointByIdQuery(id), ct));

    [HttpPost]
    public async Task<ActionResult<BaseResponse<ApiEndpointResponse>>> Create(CreateApiEndpointRequest request, CancellationToken ct)
        => Ok(await _mediator.Send(new CreateApiEndpointCommand(request), ct));

    [HttpPut("{id:guid}")]
    public async Task<ActionResult<BaseResponse<ApiEndpointResponse>>> Update(Guid id, UpdateApiEndpointRequest request, CancellationToken ct)
        => Ok(await _mediator.Send(new UpdateApiEndpointCommand(id, request), ct));

    [HttpDelete("{id:guid}")]
    public async Task<ActionResult<BaseResponse<bool>>> Delete(Guid id, CancellationToken ct)
        => Ok(await _mediator.Send(new DeleteApiEndpointCommand(id), ct));
}
