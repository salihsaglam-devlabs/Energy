using Asp.Versioning;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Energy.Application.Workflow.ApprovalDefinition.Commands.CreateApprovalDefinition;
using Energy.Application.Workflow.ApprovalDefinition.Commands.DeleteApprovalDefinition;
using Energy.Application.Workflow.ApprovalDefinition.Commands.UpdateApprovalDefinition;
using Energy.Application.Workflow.ApprovalDefinition.Queries.GetApprovalDefinitionById;
using Energy.Application.Workflow.ApprovalDefinition.Queries.GetApprovalDefinitionList;
using Energy.Application.Workflow.ApprovalDefinition.Queries.GetApprovalDefinitionLookup;
using Energy.Shared.Models.V1.Common.Responses;
using Energy.Shared.Models.V1.Workflow.ApprovalDefinition.Requests;
using Energy.Shared.Models.V1.Workflow.ApprovalDefinition.Responses;

namespace Energy.Api.Controllers.Workflow;

/// <summary>
/// ApprovalDefinition uç noktaları (liste, detay, lookup, create, update, delete).
/// Controller iş mantığı içermez; her istek ilgili Command/Query'ye map edilip
/// <see cref="IMediator"/> üzerinden Application use-case'ine yönlendirilir.
/// </summary>
[ApiController]
[ApiVersion("1.0")]
[Route("api/v{version:apiVersion}/workflow/approval-definitions")]
public sealed class ApprovalDefinitionController : ControllerBase
{
    private readonly IMediator _mediator;

    public ApprovalDefinitionController(IMediator mediator)
        => _mediator = mediator;

    /// <summary>Sayfalanmış liste.</summary>
    [HttpGet]
    public async Task<ActionResult<BaseResponse<PaginatedResponse<ApprovalDefinitionListResponse>>>> GetList([FromQuery] GetApprovalDefinitionListRequest request, CancellationToken ct)
        => Ok(await _mediator.Send(new GetApprovalDefinitionListQuery(request), ct));

    /// <summary>Kimliğe göre detay.</summary>
    [HttpGet("{id:guid}")]
    public async Task<ActionResult<BaseResponse<ApprovalDefinitionDetailResponse>>> GetById(Guid id, CancellationToken ct)
        => Ok(await _mediator.Send(new GetApprovalDefinitionByIdQuery(id), ct));

    /// <summary>Lookup listesi.</summary>
    [HttpGet("lookup")]
    public async Task<ActionResult<BaseResponse<IReadOnlyList<ApprovalDefinitionLookupResponse>>>> Lookup([FromQuery] string? search, [FromQuery] bool activeOnly, CancellationToken ct)
        => Ok(await _mediator.Send(new GetApprovalDefinitionLookupQuery(search, activeOnly), ct));

    /// <summary>Yeni kayıt oluşturur.</summary>
    [HttpPost]
    public async Task<ActionResult<BaseResponse<Guid>>> Create(CreateApprovalDefinitionRequest request, CancellationToken ct)
        => Ok(await _mediator.Send(new CreateApprovalDefinitionCommand(request), ct));

    /// <summary>Var olan kaydı günceller.</summary>
    [HttpPut("{id:guid}")]
    public async Task<ActionResult<BaseResponse<bool>>> Update(Guid id, UpdateApprovalDefinitionRequest request, CancellationToken ct)
        => Ok(await _mediator.Send(new UpdateApprovalDefinitionCommand(id, request), ct));

    /// <summary>Kaydı (soft-delete) siler.</summary>
    [HttpDelete("{id:guid}")]
    public async Task<ActionResult<BaseResponse<bool>>> Delete(Guid id, CancellationToken ct)
        => Ok(await _mediator.Send(new DeleteApprovalDefinitionCommand(id), ct));
}
