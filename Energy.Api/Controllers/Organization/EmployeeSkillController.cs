using Asp.Versioning;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Energy.Application.Modules.Organization.EmployeeSkill.Commands.CreateEmployeeSkill;
using Energy.Application.Modules.Organization.EmployeeSkill.Commands.DeleteEmployeeSkill;
using Energy.Application.Modules.Organization.EmployeeSkill.Commands.UpdateEmployeeSkill;
using Energy.Application.Modules.Organization.EmployeeSkill.Queries.GetEmployeeSkillById;
using Energy.Application.Modules.Organization.EmployeeSkill.Queries.GetEmployeeSkillList;
using Energy.Application.Modules.Organization.EmployeeSkill.Queries.GetEmployeeSkillLookup;
using Energy.Shared.Models.V1.Common.Responses;
using Energy.Shared.Models.V1.Organization.EmployeeSkill.Requests;
using Energy.Shared.Models.V1.Organization.EmployeeSkill.Responses;

namespace Energy.Api.Controllers.Organization;

/// <summary>
/// EmployeeSkill uç noktaları (liste, detay, lookup, create, update, delete).
/// Controller iş mantığı içermez; her istek ilgili Command/Query'ye map edilip
/// <see cref="IMediator"/> üzerinden Application use-case'ine yönlendirilir.
/// </summary>
[ApiController]
[ApiVersion("1.0")]
[Route("api/v{version:apiVersion}/organization/employee-skills")]
public sealed class EmployeeSkillController : ControllerBase
{
    private readonly IMediator _mediator;

    public EmployeeSkillController(IMediator mediator)
        => _mediator = mediator;

    /// <summary>Sayfalanmış liste.</summary>
    [HttpGet]
    public async Task<ActionResult<BaseResponse<PaginatedResponse<EmployeeSkillListResponse>>>> GetList([FromQuery] GetEmployeeSkillListRequest request, CancellationToken ct)
        => Ok(await _mediator.Send(new GetEmployeeSkillListQuery(request), ct));

    /// <summary>Kimliğe göre detay.</summary>
    [HttpGet("{id:guid}")]
    public async Task<ActionResult<BaseResponse<EmployeeSkillDetailResponse>>> GetById(Guid id, CancellationToken ct)
        => Ok(await _mediator.Send(new GetEmployeeSkillByIdQuery(id), ct));

    /// <summary>Lookup listesi.</summary>
    [HttpGet("lookup")]
    public async Task<ActionResult<BaseResponse<IReadOnlyList<EmployeeSkillLookupResponse>>>> Lookup([FromQuery] string? search, [FromQuery] bool activeOnly, CancellationToken ct)
        => Ok(await _mediator.Send(new GetEmployeeSkillLookupQuery(search, activeOnly), ct));

    /// <summary>Yeni kayıt oluşturur.</summary>
    [HttpPost]
    public async Task<ActionResult<BaseResponse<Guid>>> Create(CreateEmployeeSkillRequest request, CancellationToken ct)
        => Ok(await _mediator.Send(new CreateEmployeeSkillCommand(request), ct));

    /// <summary>Var olan kaydı günceller.</summary>
    [HttpPut("{id:guid}")]
    public async Task<ActionResult<BaseResponse<bool>>> Update(Guid id, UpdateEmployeeSkillRequest request, CancellationToken ct)
        => Ok(await _mediator.Send(new UpdateEmployeeSkillCommand(id, request), ct));

    /// <summary>Kaydı (soft-delete) siler.</summary>
    [HttpDelete("{id:guid}")]
    public async Task<ActionResult<BaseResponse<bool>>> Delete(Guid id, CancellationToken ct)
        => Ok(await _mediator.Send(new DeleteEmployeeSkillCommand(id), ct));
}
