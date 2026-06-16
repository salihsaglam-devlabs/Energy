using Asp.Versioning;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Energy.Shared.Models.V1.Common.Responses;
using Energy.Shared.Models.V1.Finance.Processes.ProgressPaymentPosting.Requests;
using Energy.Shared.Models.V1.Finance.Processes.ProgressPaymentPosting.Responses;
using Energy.Application.Modules.Finance.Processes.ProgressPaymentPosting.Commands.ExecuteProgressPaymentPosting;

namespace Energy.Api.Controllers.Finance.Processes;

/// <summary>Hakediş muhasebeleştirme süreci (alacak/borç finansal hareketi).</summary>
[ApiController]
[ApiVersion("1.0")]
[Route("api/v{version:apiVersion}/finance/processes/progress-payment-posting")]
public sealed class ProgressPaymentPostingProcessController : ControllerBase
{
    private readonly IMediator _mediator;

    public ProgressPaymentPostingProcessController(IMediator mediator)
        => _mediator = mediator;

    [HttpPost]
    public async Task<ActionResult<BaseResponse<ProgressPaymentPostingProcessResponse>>> Execute([FromBody] ProgressPaymentPostingProcessRequest request, CancellationToken ct)
        => Ok(await _mediator.Send(new ExecuteProgressPaymentPostingCommand(request), ct));
}
