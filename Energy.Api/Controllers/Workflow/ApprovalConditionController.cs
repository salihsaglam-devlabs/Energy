using Asp.Versioning;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Energy.Application.Workflow.ApprovalCondition.Commands.CreateApprovalCondition;
using Energy.Application.Workflow.ApprovalCondition.Commands.DeleteApprovalCondition;
using Energy.Application.Workflow.ApprovalCondition.Commands.UpdateApprovalCondition;
using Energy.Application.Workflow.ApprovalCondition.Queries.GetApprovalConditionById;
using Energy.Application.Workflow.ApprovalCondition.Queries.GetApprovalConditionList;
using Energy.Application.Workflow.ApprovalCondition.Queries.GetApprovalConditionLookup;
using Energy.Shared.Models.V1.Common.Responses;
using Energy.Shared.Models.V1.Workflow.ApprovalCondition.Requests;
using Energy.Shared.Models.V1.Workflow.ApprovalCondition.Responses;

namespace Energy.Api.Controllers.Workflow;

/// <summary>
/// ApprovalCondition uç noktaları (liste, detay, lookup, create, update, delete).
/// Controller iş mantığı içermez; her istek ilgili Command/Query'ye map edilip
/// <see cref="IMediator"/> üzerinden Application use-case'ine yönlendirilir.
/// </summary>
[ApiController]
[ApiVersion("1.0")]
[Route("api/v{version:apiVersion}/workflow/approval-conditions")]
public sealed class ApprovalConditionController : ControllerBase
{
    private readonly IMediator _mediator;

    public ApprovalConditionController(IMediator mediator)
        => _mediator = mediator;

    /// <summary>Sayfalanmış liste.</summary>
    [HttpGet]
    public async Task<ActionResult<BaseResponse<PaginatedResponse<ApprovalConditionListResponse>>>> GetList([FromQuery] GetApprovalConditionListRequest request, CancellationToken ct)
        => Ok(await _mediator.Send(new GetApprovalConditionListQuery(request), ct));

    /// <summary>Kimliğe göre detay.</summary>
    [HttpGet("{id:guid}")]
    public async Task<ActionResult<BaseResponse<ApprovalConditionDetailResponse>>> GetById(Guid id, CancellationToken ct)
        => Ok(await _mediator.Send(new GetApprovalConditionByIdQuery(id), ct));

    /// <summary>Lookup listesi.</summary>
    [HttpGet("lookup")]
    public async Task<ActionResult<BaseResponse<IReadOnlyList<ApprovalConditionLookupResponse>>>> Lookup([FromQuery] string? search, [FromQuery] bool activeOnly, CancellationToken ct)
        => Ok(await _mediator.Send(new GetApprovalConditionLookupQuery(search, activeOnly), ct));

    /// <summary>Yeni kayıt oluşturur.</summary>
    [HttpPost]
    public async Task<ActionResult<BaseResponse<Guid>>> Create(CreateApprovalConditionRequest request, CancellationToken ct)
        => Ok(await _mediator.Send(new CreateApprovalConditionCommand(request), ct));

    /// <summary>Var olan kaydı günceller.</summary>
    [HttpPut("{id:guid}")]
    public async Task<ActionResult<BaseResponse<bool>>> Update(Guid id, UpdateApprovalConditionRequest request, CancellationToken ct)
        => Ok(await _mediator.Send(new UpdateApprovalConditionCommand(id, request), ct));

    /// <summary>Kaydı (soft-delete) siler.</summary>
    [HttpDelete("{id:guid}")]
    public async Task<ActionResult<BaseResponse<bool>>> Delete(Guid id, CancellationToken ct)
        => Ok(await _mediator.Send(new DeleteApprovalConditionCommand(id), ct));
}
