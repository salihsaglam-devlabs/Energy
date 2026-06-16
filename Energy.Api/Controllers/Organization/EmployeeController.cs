using Asp.Versioning;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Energy.Application.Modules.Organization.Employee.Commands.CreateEmployee;
using Energy.Application.Modules.Organization.Employee.Commands.DeleteEmployee;
using Energy.Application.Modules.Organization.Employee.Commands.UpdateEmployee;
using Energy.Application.Modules.Organization.Employee.Queries.GetEmployeeById;
using Energy.Application.Modules.Organization.Employee.Queries.GetEmployeeList;
using Energy.Application.Modules.Organization.Employee.Queries.GetEmployeeLookup;
using Energy.Shared.Models.V1.Common.Responses;
using Energy.Shared.Models.V1.Organization.Employee.Requests;
using Energy.Shared.Models.V1.Organization.Employee.Responses;

namespace Energy.Api.Controllers.Organization;

/// <summary>
/// Employee uç noktaları (liste, detay, lookup, create, update, delete).
/// Controller iş mantığı içermez; her istek ilgili Command/Query'ye map edilip
/// <see cref="IMediator"/> üzerinden Application use-case'ine yönlendirilir.
/// </summary>
[ApiController]
[ApiVersion("1.0")]
[Route("api/v{version:apiVersion}/organization/employees")]
public sealed class EmployeeController : ControllerBase
{
    private readonly IMediator _mediator;

    public EmployeeController(IMediator mediator)
        => _mediator = mediator;

    /// <summary>Sayfalanmış liste.</summary>
    [HttpGet]
    public async Task<ActionResult<BaseResponse<PaginatedResponse<EmployeeListResponse>>>> GetList([FromQuery] GetEmployeeListRequest request, CancellationToken ct)
        => Ok(await _mediator.Send(new GetEmployeeListQuery(request), ct));

    /// <summary>Kimliğe göre detay.</summary>
    [HttpGet("{id:guid}")]
    public async Task<ActionResult<BaseResponse<EmployeeDetailResponse>>> GetById(Guid id, CancellationToken ct)
        => Ok(await _mediator.Send(new GetEmployeeByIdQuery(id), ct));

    /// <summary>Lookup listesi.</summary>
    [HttpGet("lookup")]
    public async Task<ActionResult<BaseResponse<IReadOnlyList<EmployeeLookupResponse>>>> Lookup([FromQuery] string? search, [FromQuery] bool activeOnly, CancellationToken ct)
        => Ok(await _mediator.Send(new GetEmployeeLookupQuery(search, activeOnly), ct));

    /// <summary>Yeni kayıt oluşturur.</summary>
    [HttpPost]
    public async Task<ActionResult<BaseResponse<Guid>>> Create(CreateEmployeeRequest request, CancellationToken ct)
        => Ok(await _mediator.Send(new CreateEmployeeCommand(request), ct));

    /// <summary>Var olan kaydı günceller.</summary>
    [HttpPut("{id:guid}")]
    public async Task<ActionResult<BaseResponse<bool>>> Update(Guid id, UpdateEmployeeRequest request, CancellationToken ct)
        => Ok(await _mediator.Send(new UpdateEmployeeCommand(id, request), ct));

    /// <summary>Kaydı (soft-delete) siler.</summary>
    [HttpDelete("{id:guid}")]
    public async Task<ActionResult<BaseResponse<bool>>> Delete(Guid id, CancellationToken ct)
        => Ok(await _mediator.Send(new DeleteEmployeeCommand(id), ct));
}
