using Asp.Versioning;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Energy.Application.Modules.Workflow.ApprovalDelegation.Commands.CreateApprovalDelegation;
using Energy.Application.Modules.Workflow.ApprovalDelegation.Commands.DeleteApprovalDelegation;
using Energy.Application.Modules.Workflow.ApprovalDelegation.Commands.UpdateApprovalDelegation;
using Energy.Application.Modules.Workflow.ApprovalDelegation.Queries.GetApprovalDelegationById;
using Energy.Application.Modules.Workflow.ApprovalDelegation.Queries.GetApprovalDelegationList;
using Energy.Application.Modules.Workflow.ApprovalDelegation.Queries.GetApprovalDelegationLookup;
using Energy.Shared.Models.V1.Common.Responses;
using Energy.Shared.Models.V1.Workflow.ApprovalDelegation.Requests;
using Energy.Shared.Models.V1.Workflow.ApprovalDelegation.Responses;

namespace Energy.Api.Controllers.Workflow;

/// <summary>
/// ApprovalDelegation uç noktaları (liste, detay, lookup, create, update, delete).
/// Controller iş mantığı içermez; her istek ilgili Command/Query'ye map edilip
/// <see cref="IMediator"/> üzerinden Application use-case'ine yönlendirilir.
/// </summary>
[ApiController]
[ApiVersion("1.0")]
[Route("api/v{version:apiVersion}/workflow/approval-delegations")]
public sealed class ApprovalDelegationController : ControllerBase
{
    private readonly IMediator _mediator;

    public ApprovalDelegationController(IMediator mediator)
        => _mediator = mediator;

    /// <summary>Sayfalanmış liste.</summary>
    [HttpGet]
    public async Task<ActionResult<BaseResponse<PaginatedResponse<ApprovalDelegationListResponse>>>> GetList([FromQuery] GetApprovalDelegationListRequest request, CancellationToken ct)
        => Ok(await _mediator.Send(new GetApprovalDelegationListQuery(request), ct));

    /// <summary>Kimliğe göre detay.</summary>
    [HttpGet("{id:guid}")]
    public async Task<ActionResult<BaseResponse<ApprovalDelegationDetailResponse>>> GetById(Guid id, CancellationToken ct)
        => Ok(await _mediator.Send(new GetApprovalDelegationByIdQuery(id), ct));

    /// <summary>Lookup listesi.</summary>
    [HttpGet("lookup")]
    public async Task<ActionResult<BaseResponse<IReadOnlyList<ApprovalDelegationLookupResponse>>>> Lookup([FromQuery] string? search, [FromQuery] bool activeOnly, CancellationToken ct)
        => Ok(await _mediator.Send(new GetApprovalDelegationLookupQuery(search, activeOnly), ct));

    /// <summary>Yeni kayıt oluşturur.</summary>
    [HttpPost]
    public async Task<ActionResult<BaseResponse<Guid>>> Create(CreateApprovalDelegationRequest request, CancellationToken ct)
        => Ok(await _mediator.Send(new CreateApprovalDelegationCommand(request), ct));

    /// <summary>Var olan kaydı günceller.</summary>
    [HttpPut("{id:guid}")]
    public async Task<ActionResult<BaseResponse<bool>>> Update(Guid id, UpdateApprovalDelegationRequest request, CancellationToken ct)
        => Ok(await _mediator.Send(new UpdateApprovalDelegationCommand(id, request), ct));

    /// <summary>Kaydı (soft-delete) siler.</summary>
    [HttpDelete("{id:guid}")]
    public async Task<ActionResult<BaseResponse<bool>>> Delete(Guid id, CancellationToken ct)
        => Ok(await _mediator.Send(new DeleteApprovalDelegationCommand(id), ct));
}
