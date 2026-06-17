using Asp.Versioning;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Energy.Shared.Models.V1.Common.Responses;
using Energy.Shared.Models.V1.Finance.Processes.TimesheetCost.Requests;
using Energy.Shared.Models.V1.Finance.Processes.TimesheetCost.Responses;
using Energy.Application.Finance.Processes.TimesheetCost.Commands.ExecuteTimesheetCost;

namespace Energy.Api.Controllers.Finance.Processes;

/// <summary>Puantaj maliyet süreci (HR maliyet finansal hareketi).</summary>
[ApiController]
[ApiVersion("1.0")]
[Route("api/v{version:apiVersion}/finance/processes/timesheet-cost")]
public sealed class TimesheetCostProcessController : ControllerBase
{
    private readonly IMediator _mediator;

    public TimesheetCostProcessController(IMediator mediator)
        => _mediator = mediator;

    [HttpPost]
    public async Task<ActionResult<BaseResponse<TimesheetCostProcessResponse>>> Execute([FromBody] TimesheetCostProcessRequest request, CancellationToken ct)
        => Ok(await _mediator.Send(new ExecuteTimesheetCostCommand(request), ct));
}
