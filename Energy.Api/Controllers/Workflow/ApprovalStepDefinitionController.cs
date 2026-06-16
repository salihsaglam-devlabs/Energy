using Asp.Versioning;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Energy.Application.Workflow.ApprovalStepDefinition.Commands.CreateApprovalStepDefinition;
using Energy.Application.Workflow.ApprovalStepDefinition.Commands.DeleteApprovalStepDefinition;
using Energy.Application.Workflow.ApprovalStepDefinition.Commands.UpdateApprovalStepDefinition;
using Energy.Application.Workflow.ApprovalStepDefinition.Queries.GetApprovalStepDefinitionById;
using Energy.Application.Workflow.ApprovalStepDefinition.Queries.GetApprovalStepDefinitionList;
using Energy.Application.Workflow.ApprovalStepDefinition.Queries.GetApprovalStepDefinitionLookup;
using Energy.Shared.Models.V1.Common.Responses;
using Energy.Shared.Models.V1.Workflow.ApprovalStepDefinition.Requests;
using Energy.Shared.Models.V1.Workflow.ApprovalStepDefinition.Responses;

namespace Energy.Api.Controllers.Workflow;

/// <summary>
/// ApprovalStepDefinition uç noktaları (liste, detay, lookup, create, update, delete).
/// Controller iş mantığı içermez; her istek ilgili Command/Query'ye map edilip
/// <see cref="IMediator"/> üzerinden Application use-case'ine yönlendirilir.
/// </summary>
[ApiController]
[ApiVersion("1.0")]
[Route("api/v{version:apiVersion}/workflow/approval-step-definitions")]
public sealed class ApprovalStepDefinitionController : ControllerBase
{
    private readonly IMediator _mediator;

    public ApprovalStepDefinitionController(IMediator mediator)
        => _mediator = mediator;

    /// <summary>Sayfalanmış liste.</summary>
    [HttpGet]
    public async Task<ActionResult<BaseResponse<PaginatedResponse<ApprovalStepDefinitionListResponse>>>> GetList([FromQuery] GetApprovalStepDefinitionListRequest request, CancellationToken ct)
        => Ok(await _mediator.Send(new GetApprovalStepDefinitionListQuery(request), ct));

    /// <summary>Kimliğe göre detay.</summary>
    [HttpGet("{id:guid}")]
    public async Task<ActionResult<BaseResponse<ApprovalStepDefinitionDetailResponse>>> GetById(Guid id, CancellationToken ct)
        => Ok(await _mediator.Send(new GetApprovalStepDefinitionByIdQuery(id), ct));

    /// <summary>Lookup listesi.</summary>
    [HttpGet("lookup")]
    public async Task<ActionResult<BaseResponse<IReadOnlyList<ApprovalStepDefinitionLookupResponse>>>> Lookup([FromQuery] string? search, [FromQuery] bool activeOnly, CancellationToken ct)
        => Ok(await _mediator.Send(new GetApprovalStepDefinitionLookupQuery(search, activeOnly), ct));

    /// <summary>Yeni kayıt oluşturur.</summary>
    [HttpPost]
    public async Task<ActionResult<BaseResponse<Guid>>> Create(CreateApprovalStepDefinitionRequest request, CancellationToken ct)
        => Ok(await _mediator.Send(new CreateApprovalStepDefinitionCommand(request), ct));

    /// <summary>Var olan kaydı günceller.</summary>
    [HttpPut("{id:guid}")]
    public async Task<ActionResult<BaseResponse<bool>>> Update(Guid id, UpdateApprovalStepDefinitionRequest request, CancellationToken ct)
        => Ok(await _mediator.Send(new UpdateApprovalStepDefinitionCommand(id, request), ct));

    /// <summary>Kaydı (soft-delete) siler.</summary>
    [HttpDelete("{id:guid}")]
    public async Task<ActionResult<BaseResponse<bool>>> Delete(Guid id, CancellationToken ct)
        => Ok(await _mediator.Send(new DeleteApprovalStepDefinitionCommand(id), ct));
}
