using Asp.Versioning;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Energy.Application.Workflow.ApprovalAction.Commands.CreateApprovalAction;
using Energy.Application.Workflow.ApprovalAction.Commands.DeleteApprovalAction;
using Energy.Application.Workflow.ApprovalAction.Commands.UpdateApprovalAction;
using Energy.Application.Workflow.ApprovalAction.Queries.GetApprovalActionById;
using Energy.Application.Workflow.ApprovalAction.Queries.GetApprovalActionList;
using Energy.Application.Workflow.ApprovalAction.Queries.GetApprovalActionLookup;
using Energy.Shared.Models.V1.Common.Responses;
using Energy.Shared.Models.V1.Workflow.ApprovalAction.Requests;
using Energy.Shared.Models.V1.Workflow.ApprovalAction.Responses;

namespace Energy.Api.Controllers.Workflow;

/// <summary>
/// ApprovalAction uç noktaları (liste, detay, lookup, create, update, delete).
/// Controller iş mantığı içermez; her istek ilgili Command/Query'ye map edilip
/// <see cref="IMediator"/> üzerinden Application use-case'ine yönlendirilir.
/// </summary>
[ApiController]
[ApiVersion("1.0")]
[Route("api/v{version:apiVersion}/workflow/approval-actions")]
public sealed class ApprovalActionController : ControllerBase
{
    private readonly IMediator _mediator;

    public ApprovalActionController(IMediator mediator)
        => _mediator = mediator;

    /// <summary>Sayfalanmış liste.</summary>
    [HttpGet]
    public async Task<ActionResult<BaseResponse<PaginatedResponse<ApprovalActionListResponse>>>> GetList([FromQuery] GetApprovalActionListRequest request, CancellationToken ct)
        => Ok(await _mediator.Send(new GetApprovalActionListQuery(request), ct));

    /// <summary>Kimliğe göre detay.</summary>
    [HttpGet("{id:guid}")]
    public async Task<ActionResult<BaseResponse<ApprovalActionDetailResponse>>> GetById(Guid id, CancellationToken ct)
        => Ok(await _mediator.Send(new GetApprovalActionByIdQuery(id), ct));

    /// <summary>Lookup listesi.</summary>
    [HttpGet("lookup")]
    public async Task<ActionResult<BaseResponse<IReadOnlyList<ApprovalActionLookupResponse>>>> Lookup([FromQuery] string? search, [FromQuery] bool activeOnly, CancellationToken ct)
        => Ok(await _mediator.Send(new GetApprovalActionLookupQuery(search, activeOnly), ct));

    /// <summary>Yeni kayıt oluşturur.</summary>
    [HttpPost]
    public async Task<ActionResult<BaseResponse<Guid>>> Create(CreateApprovalActionRequest request, CancellationToken ct)
        => Ok(await _mediator.Send(new CreateApprovalActionCommand(request), ct));

    /// <summary>Var olan kaydı günceller.</summary>
    [HttpPut("{id:guid}")]
    public async Task<ActionResult<BaseResponse<bool>>> Update(Guid id, UpdateApprovalActionRequest request, CancellationToken ct)
        => Ok(await _mediator.Send(new UpdateApprovalActionCommand(id, request), ct));

    /// <summary>Kaydı (soft-delete) siler.</summary>
    [HttpDelete("{id:guid}")]
    public async Task<ActionResult<BaseResponse<bool>>> Delete(Guid id, CancellationToken ct)
        => Ok(await _mediator.Send(new DeleteApprovalActionCommand(id), ct));
}
