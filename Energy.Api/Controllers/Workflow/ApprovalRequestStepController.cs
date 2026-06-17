using Asp.Versioning;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Energy.Application.Workflow.ApprovalRequestStep.Commands.CreateApprovalRequestStep;
using Energy.Application.Workflow.ApprovalRequestStep.Commands.DeleteApprovalRequestStep;
using Energy.Application.Workflow.ApprovalRequestStep.Commands.UpdateApprovalRequestStep;
using Energy.Application.Workflow.ApprovalRequestStep.Queries.GetApprovalRequestStepById;
using Energy.Application.Workflow.ApprovalRequestStep.Queries.GetApprovalRequestStepList;
using Energy.Application.Workflow.ApprovalRequestStep.Queries.GetApprovalRequestStepLookup;
using Energy.Shared.Models.V1.Common.Responses;
using Energy.Shared.Models.V1.Workflow.ApprovalRequestStep.Requests;
using Energy.Shared.Models.V1.Workflow.ApprovalRequestStep.Responses;

namespace Energy.Api.Controllers.Workflow;

/// <summary>
/// ApprovalRequestStep uç noktaları (liste, detay, lookup, create, update, delete).
/// Controller iş mantığı içermez; her istek ilgili Command/Query'ye map edilip
/// <see cref="IMediator"/> üzerinden Application use-case'ine yönlendirilir.
/// </summary>
[ApiController]
[ApiVersion("1.0")]
[Route("api/v{version:apiVersion}/workflow/approval-request-steps")]
public sealed class ApprovalRequestStepController : ControllerBase
{
    private readonly IMediator _mediator;

    public ApprovalRequestStepController(IMediator mediator)
        => _mediator = mediator;

    /// <summary>Sayfalanmış liste.</summary>
    [HttpGet]
    public async Task<ActionResult<BaseResponse<PaginatedResponse<ApprovalRequestStepListResponse>>>> GetList([FromQuery] GetApprovalRequestStepListRequest request, CancellationToken ct)
        => Ok(await _mediator.Send(new GetApprovalRequestStepListQuery(request), ct));

    /// <summary>Kimliğe göre detay.</summary>
    [HttpGet("{id:guid}")]
    public async Task<ActionResult<BaseResponse<ApprovalRequestStepDetailResponse>>> GetById(Guid id, CancellationToken ct)
        => Ok(await _mediator.Send(new GetApprovalRequestStepByIdQuery(id), ct));

    /// <summary>Lookup listesi.</summary>
    [HttpGet("lookup")]
    public async Task<ActionResult<BaseResponse<IReadOnlyList<ApprovalRequestStepLookupResponse>>>> Lookup([FromQuery] string? search, [FromQuery] bool activeOnly, CancellationToken ct)
        => Ok(await _mediator.Send(new GetApprovalRequestStepLookupQuery(search, activeOnly), ct));

    /// <summary>Yeni kayıt oluşturur.</summary>
    [HttpPost]
    public async Task<ActionResult<BaseResponse<Guid>>> Create(CreateApprovalRequestStepRequest request, CancellationToken ct)
        => Ok(await _mediator.Send(new CreateApprovalRequestStepCommand(request), ct));

    /// <summary>Var olan kaydı günceller.</summary>
    [HttpPut("{id:guid}")]
    public async Task<ActionResult<BaseResponse<bool>>> Update(Guid id, UpdateApprovalRequestStepRequest request, CancellationToken ct)
        => Ok(await _mediator.Send(new UpdateApprovalRequestStepCommand(id, request), ct));

    /// <summary>Kaydı (soft-delete) siler.</summary>
    [HttpDelete("{id:guid}")]
    public async Task<ActionResult<BaseResponse<bool>>> Delete(Guid id, CancellationToken ct)
        => Ok(await _mediator.Send(new DeleteApprovalRequestStepCommand(id), ct));
}
