using Asp.Versioning;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Energy.Application.Modules.Workflow.ApprovalRequest.Commands.CreateApprovalRequest;
using Energy.Application.Modules.Workflow.ApprovalRequest.Commands.DeleteApprovalRequest;
using Energy.Application.Modules.Workflow.ApprovalRequest.Commands.UpdateApprovalRequest;
using Energy.Application.Modules.Workflow.ApprovalRequest.Queries.GetApprovalRequestById;
using Energy.Application.Modules.Workflow.ApprovalRequest.Queries.GetApprovalRequestList;
using Energy.Application.Modules.Workflow.ApprovalRequest.Queries.GetApprovalRequestLookup;
using Energy.Shared.Models.V1.Common.Responses;
using Energy.Shared.Models.V1.Workflow.ApprovalRequest.Requests;
using Energy.Shared.Models.V1.Workflow.ApprovalRequest.Responses;

namespace Energy.Api.Controllers.Workflow;

/// <summary>
/// ApprovalRequest uç noktaları (liste, detay, lookup, create, update, delete).
/// Controller iş mantığı içermez; her istek ilgili Command/Query'ye map edilip
/// <see cref="IMediator"/> üzerinden Application use-case'ine yönlendirilir.
/// </summary>
[ApiController]
[ApiVersion("1.0")]
[Route("api/v{version:apiVersion}/workflow/approval-requests")]
public sealed class ApprovalRequestController : ControllerBase
{
    private readonly IMediator _mediator;

    public ApprovalRequestController(IMediator mediator)
        => _mediator = mediator;

    /// <summary>Sayfalanmış liste.</summary>
    [HttpGet]
    public async Task<ActionResult<BaseResponse<PaginatedResponse<ApprovalRequestListResponse>>>> GetList([FromQuery] GetApprovalRequestListRequest request, CancellationToken ct)
        => Ok(await _mediator.Send(new GetApprovalRequestListQuery(request), ct));

    /// <summary>Kimliğe göre detay.</summary>
    [HttpGet("{id:guid}")]
    public async Task<ActionResult<BaseResponse<ApprovalRequestDetailResponse>>> GetById(Guid id, CancellationToken ct)
        => Ok(await _mediator.Send(new GetApprovalRequestByIdQuery(id), ct));

    /// <summary>Lookup listesi.</summary>
    [HttpGet("lookup")]
    public async Task<ActionResult<BaseResponse<IReadOnlyList<ApprovalRequestLookupResponse>>>> Lookup([FromQuery] string? search, [FromQuery] bool activeOnly, CancellationToken ct)
        => Ok(await _mediator.Send(new GetApprovalRequestLookupQuery(search, activeOnly), ct));

    /// <summary>Yeni kayıt oluşturur.</summary>
    [HttpPost]
    public async Task<ActionResult<BaseResponse<Guid>>> Create(CreateApprovalRequestRequest request, CancellationToken ct)
        => Ok(await _mediator.Send(new CreateApprovalRequestCommand(request), ct));

    /// <summary>Var olan kaydı günceller.</summary>
    [HttpPut("{id:guid}")]
    public async Task<ActionResult<BaseResponse<bool>>> Update(Guid id, UpdateApprovalRequestRequest request, CancellationToken ct)
        => Ok(await _mediator.Send(new UpdateApprovalRequestCommand(id, request), ct));

    /// <summary>Kaydı (soft-delete) siler.</summary>
    [HttpDelete("{id:guid}")]
    public async Task<ActionResult<BaseResponse<bool>>> Delete(Guid id, CancellationToken ct)
        => Ok(await _mediator.Send(new DeleteApprovalRequestCommand(id), ct));
}
