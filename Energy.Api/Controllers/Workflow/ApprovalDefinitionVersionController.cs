using Asp.Versioning;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Energy.Application.Workflow.ApprovalDefinitionVersion.Commands.CreateApprovalDefinitionVersion;
using Energy.Application.Workflow.ApprovalDefinitionVersion.Commands.DeleteApprovalDefinitionVersion;
using Energy.Application.Workflow.ApprovalDefinitionVersion.Commands.UpdateApprovalDefinitionVersion;
using Energy.Application.Workflow.ApprovalDefinitionVersion.Queries.GetApprovalDefinitionVersionById;
using Energy.Application.Workflow.ApprovalDefinitionVersion.Queries.GetApprovalDefinitionVersionList;
using Energy.Application.Workflow.ApprovalDefinitionVersion.Queries.GetApprovalDefinitionVersionLookup;
using Energy.Shared.Models.V1.Common.Responses;
using Energy.Shared.Models.V1.Workflow.ApprovalDefinitionVersion.Requests;
using Energy.Shared.Models.V1.Workflow.ApprovalDefinitionVersion.Responses;

namespace Energy.Api.Controllers.Workflow;

/// <summary>
/// ApprovalDefinitionVersion uç noktaları (liste, detay, lookup, create, update, delete).
/// Controller iş mantığı içermez; her istek ilgili Command/Query'ye map edilip
/// <see cref="IMediator"/> üzerinden Application use-case'ine yönlendirilir.
/// </summary>
[ApiController]
[ApiVersion("1.0")]
[Route("api/v{version:apiVersion}/workflow/approval-definition-versions")]
public sealed class ApprovalDefinitionVersionController : ControllerBase
{
    private readonly IMediator _mediator;

    public ApprovalDefinitionVersionController(IMediator mediator)
        => _mediator = mediator;

    /// <summary>Sayfalanmış liste.</summary>
    [HttpGet]
    public async Task<ActionResult<BaseResponse<PaginatedResponse<ApprovalDefinitionVersionListResponse>>>> GetList([FromQuery] GetApprovalDefinitionVersionListRequest request, CancellationToken ct)
        => Ok(await _mediator.Send(new GetApprovalDefinitionVersionListQuery(request), ct));

    /// <summary>Kimliğe göre detay.</summary>
    [HttpGet("{id:guid}")]
    public async Task<ActionResult<BaseResponse<ApprovalDefinitionVersionDetailResponse>>> GetById(Guid id, CancellationToken ct)
        => Ok(await _mediator.Send(new GetApprovalDefinitionVersionByIdQuery(id), ct));

    /// <summary>Lookup listesi.</summary>
    [HttpGet("lookup")]
    public async Task<ActionResult<BaseResponse<IReadOnlyList<ApprovalDefinitionVersionLookupResponse>>>> Lookup([FromQuery] string? search, [FromQuery] bool activeOnly, CancellationToken ct)
        => Ok(await _mediator.Send(new GetApprovalDefinitionVersionLookupQuery(search, activeOnly), ct));

    /// <summary>Yeni kayıt oluşturur.</summary>
    [HttpPost]
    public async Task<ActionResult<BaseResponse<Guid>>> Create(CreateApprovalDefinitionVersionRequest request, CancellationToken ct)
        => Ok(await _mediator.Send(new CreateApprovalDefinitionVersionCommand(request), ct));

    /// <summary>Var olan kaydı günceller.</summary>
    [HttpPut("{id:guid}")]
    public async Task<ActionResult<BaseResponse<bool>>> Update(Guid id, UpdateApprovalDefinitionVersionRequest request, CancellationToken ct)
        => Ok(await _mediator.Send(new UpdateApprovalDefinitionVersionCommand(id, request), ct));

    /// <summary>Kaydı (soft-delete) siler.</summary>
    [HttpDelete("{id:guid}")]
    public async Task<ActionResult<BaseResponse<bool>>> Delete(Guid id, CancellationToken ct)
        => Ok(await _mediator.Send(new DeleteApprovalDefinitionVersionCommand(id), ct));
}
