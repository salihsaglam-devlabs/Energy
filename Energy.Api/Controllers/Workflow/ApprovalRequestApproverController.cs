using Asp.Versioning;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Energy.Application.Modules.Workflow.ApprovalRequestApprover.Commands.CreateApprovalRequestApprover;
using Energy.Application.Modules.Workflow.ApprovalRequestApprover.Commands.DeleteApprovalRequestApprover;
using Energy.Application.Modules.Workflow.ApprovalRequestApprover.Commands.UpdateApprovalRequestApprover;
using Energy.Application.Modules.Workflow.ApprovalRequestApprover.Queries.GetApprovalRequestApproverById;
using Energy.Application.Modules.Workflow.ApprovalRequestApprover.Queries.GetApprovalRequestApproverList;
using Energy.Application.Modules.Workflow.ApprovalRequestApprover.Queries.GetApprovalRequestApproverLookup;
using Energy.Shared.Models.V1.Common.Responses;
using Energy.Shared.Models.V1.Workflow.ApprovalRequestApprover.Requests;
using Energy.Shared.Models.V1.Workflow.ApprovalRequestApprover.Responses;

namespace Energy.Api.Controllers.Workflow;

/// <summary>
/// ApprovalRequestApprover uç noktaları (liste, detay, lookup, create, update, delete).
/// Controller iş mantığı içermez; her istek ilgili Command/Query'ye map edilip
/// <see cref="IMediator"/> üzerinden Application use-case'ine yönlendirilir.
/// </summary>
[ApiController]
[ApiVersion("1.0")]
[Route("api/v{version:apiVersion}/workflow/approval-request-approvers")]
public sealed class ApprovalRequestApproverController : ControllerBase
{
    private readonly IMediator _mediator;

    public ApprovalRequestApproverController(IMediator mediator)
        => _mediator = mediator;

    /// <summary>Sayfalanmış liste.</summary>
    [HttpGet]
    public async Task<ActionResult<BaseResponse<PaginatedResponse<ApprovalRequestApproverListResponse>>>> GetList([FromQuery] GetApprovalRequestApproverListRequest request, CancellationToken ct)
        => Ok(await _mediator.Send(new GetApprovalRequestApproverListQuery(request), ct));

    /// <summary>Kimliğe göre detay.</summary>
    [HttpGet("{id:guid}")]
    public async Task<ActionResult<BaseResponse<ApprovalRequestApproverDetailResponse>>> GetById(Guid id, CancellationToken ct)
        => Ok(await _mediator.Send(new GetApprovalRequestApproverByIdQuery(id), ct));

    /// <summary>Lookup listesi.</summary>
    [HttpGet("lookup")]
    public async Task<ActionResult<BaseResponse<IReadOnlyList<ApprovalRequestApproverLookupResponse>>>> Lookup([FromQuery] string? search, [FromQuery] bool activeOnly, CancellationToken ct)
        => Ok(await _mediator.Send(new GetApprovalRequestApproverLookupQuery(search, activeOnly), ct));

    /// <summary>Yeni kayıt oluşturur.</summary>
    [HttpPost]
    public async Task<ActionResult<BaseResponse<Guid>>> Create(CreateApprovalRequestApproverRequest request, CancellationToken ct)
        => Ok(await _mediator.Send(new CreateApprovalRequestApproverCommand(request), ct));

    /// <summary>Var olan kaydı günceller.</summary>
    [HttpPut("{id:guid}")]
    public async Task<ActionResult<BaseResponse<bool>>> Update(Guid id, UpdateApprovalRequestApproverRequest request, CancellationToken ct)
        => Ok(await _mediator.Send(new UpdateApprovalRequestApproverCommand(id, request), ct));

    /// <summary>Kaydı (soft-delete) siler.</summary>
    [HttpDelete("{id:guid}")]
    public async Task<ActionResult<BaseResponse<bool>>> Delete(Guid id, CancellationToken ct)
        => Ok(await _mediator.Send(new DeleteApprovalRequestApproverCommand(id), ct));
}
