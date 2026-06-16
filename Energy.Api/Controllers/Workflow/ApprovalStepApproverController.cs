using Asp.Versioning;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Energy.Application.Modules.Workflow.ApprovalStepApprover.Commands.CreateApprovalStepApprover;
using Energy.Application.Modules.Workflow.ApprovalStepApprover.Commands.DeleteApprovalStepApprover;
using Energy.Application.Modules.Workflow.ApprovalStepApprover.Commands.UpdateApprovalStepApprover;
using Energy.Application.Modules.Workflow.ApprovalStepApprover.Queries.GetApprovalStepApproverById;
using Energy.Application.Modules.Workflow.ApprovalStepApprover.Queries.GetApprovalStepApproverList;
using Energy.Application.Modules.Workflow.ApprovalStepApprover.Queries.GetApprovalStepApproverLookup;
using Energy.Shared.Models.V1.Common.Responses;
using Energy.Shared.Models.V1.Workflow.ApprovalStepApprover.Requests;
using Energy.Shared.Models.V1.Workflow.ApprovalStepApprover.Responses;

namespace Energy.Api.Controllers.Workflow;

/// <summary>
/// ApprovalStepApprover uç noktaları (liste, detay, lookup, create, update, delete).
/// Controller iş mantığı içermez; her istek ilgili Command/Query'ye map edilip
/// <see cref="IMediator"/> üzerinden Application use-case'ine yönlendirilir.
/// </summary>
[ApiController]
[ApiVersion("1.0")]
[Route("api/v{version:apiVersion}/workflow/approval-step-approvers")]
public sealed class ApprovalStepApproverController : ControllerBase
{
    private readonly IMediator _mediator;

    public ApprovalStepApproverController(IMediator mediator)
        => _mediator = mediator;

    /// <summary>Sayfalanmış liste.</summary>
    [HttpGet]
    public async Task<ActionResult<BaseResponse<PaginatedResponse<ApprovalStepApproverListResponse>>>> GetList([FromQuery] GetApprovalStepApproverListRequest request, CancellationToken ct)
        => Ok(await _mediator.Send(new GetApprovalStepApproverListQuery(request), ct));

    /// <summary>Kimliğe göre detay.</summary>
    [HttpGet("{id:guid}")]
    public async Task<ActionResult<BaseResponse<ApprovalStepApproverDetailResponse>>> GetById(Guid id, CancellationToken ct)
        => Ok(await _mediator.Send(new GetApprovalStepApproverByIdQuery(id), ct));

    /// <summary>Lookup listesi.</summary>
    [HttpGet("lookup")]
    public async Task<ActionResult<BaseResponse<IReadOnlyList<ApprovalStepApproverLookupResponse>>>> Lookup([FromQuery] string? search, [FromQuery] bool activeOnly, CancellationToken ct)
        => Ok(await _mediator.Send(new GetApprovalStepApproverLookupQuery(search, activeOnly), ct));

    /// <summary>Yeni kayıt oluşturur.</summary>
    [HttpPost]
    public async Task<ActionResult<BaseResponse<Guid>>> Create(CreateApprovalStepApproverRequest request, CancellationToken ct)
        => Ok(await _mediator.Send(new CreateApprovalStepApproverCommand(request), ct));

    /// <summary>Var olan kaydı günceller.</summary>
    [HttpPut("{id:guid}")]
    public async Task<ActionResult<BaseResponse<bool>>> Update(Guid id, UpdateApprovalStepApproverRequest request, CancellationToken ct)
        => Ok(await _mediator.Send(new UpdateApprovalStepApproverCommand(id, request), ct));

    /// <summary>Kaydı (soft-delete) siler.</summary>
    [HttpDelete("{id:guid}")]
    public async Task<ActionResult<BaseResponse<bool>>> Delete(Guid id, CancellationToken ct)
        => Ok(await _mediator.Send(new DeleteApprovalStepApproverCommand(id), ct));
}
