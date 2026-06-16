using Asp.Versioning;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Energy.Application.Modules.Organization.EmployeeSkillAssignment.Commands.CreateEmployeeSkillAssignment;
using Energy.Application.Modules.Organization.EmployeeSkillAssignment.Commands.DeleteEmployeeSkillAssignment;
using Energy.Application.Modules.Organization.EmployeeSkillAssignment.Commands.UpdateEmployeeSkillAssignment;
using Energy.Application.Modules.Organization.EmployeeSkillAssignment.Queries.GetEmployeeSkillAssignmentById;
using Energy.Application.Modules.Organization.EmployeeSkillAssignment.Queries.GetEmployeeSkillAssignmentList;
using Energy.Application.Modules.Organization.EmployeeSkillAssignment.Queries.GetEmployeeSkillAssignmentLookup;
using Energy.Shared.Models.V1.Common.Responses;
using Energy.Shared.Models.V1.Organization.EmployeeSkillAssignment.Requests;
using Energy.Shared.Models.V1.Organization.EmployeeSkillAssignment.Responses;

namespace Energy.Api.Controllers.Organization;

/// <summary>
/// EmployeeSkillAssignment uç noktaları (liste, detay, lookup, create, update, delete).
/// Controller iş mantığı içermez; her istek ilgili Command/Query'ye map edilip
/// <see cref="IMediator"/> üzerinden Application use-case'ine yönlendirilir.
/// </summary>
[ApiController]
[ApiVersion("1.0")]
[Route("api/v{version:apiVersion}/organization/employee-skill-assignments")]
public sealed class EmployeeSkillAssignmentController : ControllerBase
{
    private readonly IMediator _mediator;

    public EmployeeSkillAssignmentController(IMediator mediator)
        => _mediator = mediator;

    /// <summary>Sayfalanmış liste.</summary>
    [HttpGet]
    public async Task<ActionResult<BaseResponse<PaginatedResponse<EmployeeSkillAssignmentListResponse>>>> GetList([FromQuery] GetEmployeeSkillAssignmentListRequest request, CancellationToken ct)
        => Ok(await _mediator.Send(new GetEmployeeSkillAssignmentListQuery(request), ct));

    /// <summary>Kimliğe göre detay.</summary>
    [HttpGet("{id:guid}")]
    public async Task<ActionResult<BaseResponse<EmployeeSkillAssignmentDetailResponse>>> GetById(Guid id, CancellationToken ct)
        => Ok(await _mediator.Send(new GetEmployeeSkillAssignmentByIdQuery(id), ct));

    /// <summary>Lookup listesi.</summary>
    [HttpGet("lookup")]
    public async Task<ActionResult<BaseResponse<IReadOnlyList<EmployeeSkillAssignmentLookupResponse>>>> Lookup([FromQuery] string? search, [FromQuery] bool activeOnly, CancellationToken ct)
        => Ok(await _mediator.Send(new GetEmployeeSkillAssignmentLookupQuery(search, activeOnly), ct));

    /// <summary>Yeni kayıt oluşturur.</summary>
    [HttpPost]
    public async Task<ActionResult<BaseResponse<Guid>>> Create(CreateEmployeeSkillAssignmentRequest request, CancellationToken ct)
        => Ok(await _mediator.Send(new CreateEmployeeSkillAssignmentCommand(request), ct));

    /// <summary>Var olan kaydı günceller.</summary>
    [HttpPut("{id:guid}")]
    public async Task<ActionResult<BaseResponse<bool>>> Update(Guid id, UpdateEmployeeSkillAssignmentRequest request, CancellationToken ct)
        => Ok(await _mediator.Send(new UpdateEmployeeSkillAssignmentCommand(id, request), ct));

    /// <summary>Kaydı (soft-delete) siler.</summary>
    [HttpDelete("{id:guid}")]
    public async Task<ActionResult<BaseResponse<bool>>> Delete(Guid id, CancellationToken ct)
        => Ok(await _mediator.Send(new DeleteEmployeeSkillAssignmentCommand(id), ct));
}
