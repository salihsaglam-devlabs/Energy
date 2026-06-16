using Asp.Versioning;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Energy.Application.Modules.Organization.EmployeePosition.Commands.CreateEmployeePosition;
using Energy.Application.Modules.Organization.EmployeePosition.Commands.DeleteEmployeePosition;
using Energy.Application.Modules.Organization.EmployeePosition.Commands.UpdateEmployeePosition;
using Energy.Application.Modules.Organization.EmployeePosition.Queries.GetEmployeePositionById;
using Energy.Application.Modules.Organization.EmployeePosition.Queries.GetEmployeePositionList;
using Energy.Application.Modules.Organization.EmployeePosition.Queries.GetEmployeePositionLookup;
using Energy.Shared.Models.V1.Common.Responses;
using Energy.Shared.Models.V1.Organization.EmployeePosition.Requests;
using Energy.Shared.Models.V1.Organization.EmployeePosition.Responses;

namespace Energy.Api.Controllers.Organization;

/// <summary>
/// EmployeePosition uç noktaları (liste, detay, lookup, create, update, delete).
/// Controller iş mantığı içermez; her istek ilgili Command/Query'ye map edilip
/// <see cref="IMediator"/> üzerinden Application use-case'ine yönlendirilir.
/// </summary>
[ApiController]
[ApiVersion("1.0")]
[Route("api/v{version:apiVersion}/organization/employee-positions")]
public sealed class EmployeePositionController : ControllerBase
{
    private readonly IMediator _mediator;

    public EmployeePositionController(IMediator mediator)
        => _mediator = mediator;

    /// <summary>Sayfalanmış liste.</summary>
    [HttpGet]
    public async Task<ActionResult<BaseResponse<PaginatedResponse<EmployeePositionListResponse>>>> GetList([FromQuery] GetEmployeePositionListRequest request, CancellationToken ct)
        => Ok(await _mediator.Send(new GetEmployeePositionListQuery(request), ct));

    /// <summary>Kimliğe göre detay.</summary>
    [HttpGet("{id:guid}")]
    public async Task<ActionResult<BaseResponse<EmployeePositionDetailResponse>>> GetById(Guid id, CancellationToken ct)
        => Ok(await _mediator.Send(new GetEmployeePositionByIdQuery(id), ct));

    /// <summary>Lookup listesi.</summary>
    [HttpGet("lookup")]
    public async Task<ActionResult<BaseResponse<IReadOnlyList<EmployeePositionLookupResponse>>>> Lookup([FromQuery] string? search, [FromQuery] bool activeOnly, CancellationToken ct)
        => Ok(await _mediator.Send(new GetEmployeePositionLookupQuery(search, activeOnly), ct));

    /// <summary>Yeni kayıt oluşturur.</summary>
    [HttpPost]
    public async Task<ActionResult<BaseResponse<Guid>>> Create(CreateEmployeePositionRequest request, CancellationToken ct)
        => Ok(await _mediator.Send(new CreateEmployeePositionCommand(request), ct));

    /// <summary>Var olan kaydı günceller.</summary>
    [HttpPut("{id:guid}")]
    public async Task<ActionResult<BaseResponse<bool>>> Update(Guid id, UpdateEmployeePositionRequest request, CancellationToken ct)
        => Ok(await _mediator.Send(new UpdateEmployeePositionCommand(id, request), ct));

    /// <summary>Kaydı (soft-delete) siler.</summary>
    [HttpDelete("{id:guid}")]
    public async Task<ActionResult<BaseResponse<bool>>> Delete(Guid id, CancellationToken ct)
        => Ok(await _mediator.Send(new DeleteEmployeePositionCommand(id), ct));
}
