using Asp.Versioning;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Energy.Application.Core.AuditLog.Commands.CreateAuditLog;
using Energy.Application.Core.AuditLog.Commands.DeleteAuditLog;
using Energy.Application.Core.AuditLog.Commands.UpdateAuditLog;
using Energy.Application.Core.AuditLog.Queries.GetAuditLogById;
using Energy.Application.Core.AuditLog.Queries.GetAuditLogList;
using Energy.Application.Core.AuditLog.Queries.GetAuditLogLookup;
using Energy.Shared.Models.V1.Common.Responses;
using Energy.Shared.Models.V1.Core.AuditLog.Requests;
using Energy.Shared.Models.V1.Core.AuditLog.Responses;

namespace Energy.Api.Controllers.Core;

/// <summary>
/// AuditLog uç noktaları (liste, detay, lookup, create, update, delete).
/// Controller iş mantığı içermez; her istek ilgili Command/Query'ye map edilip
/// <see cref="IMediator"/> üzerinden Application use-case'ine yönlendirilir.
/// </summary>
[ApiController]
[ApiVersion("1.0")]
[Route("api/v{version:apiVersion}/core/audit-logs")]
public sealed class AuditLogController : ControllerBase
{
    private readonly IMediator _mediator;

    public AuditLogController(IMediator mediator)
        => _mediator = mediator;

    /// <summary>Sayfalanmış liste.</summary>
    [HttpGet]
    public async Task<ActionResult<BaseResponse<PaginatedResponse<AuditLogListResponse>>>> GetList([FromQuery] GetAuditLogListRequest request, CancellationToken ct)
        => Ok(await _mediator.Send(new GetAuditLogListQuery(request), ct));

    /// <summary>Kimliğe göre detay.</summary>
    [HttpGet("{id:guid}")]
    public async Task<ActionResult<BaseResponse<AuditLogDetailResponse>>> GetById(Guid id, CancellationToken ct)
        => Ok(await _mediator.Send(new GetAuditLogByIdQuery(id), ct));

    /// <summary>Lookup listesi.</summary>
    [HttpGet("lookup")]
    public async Task<ActionResult<BaseResponse<IReadOnlyList<AuditLogLookupResponse>>>> Lookup([FromQuery] string? search, [FromQuery] bool activeOnly, CancellationToken ct)
        => Ok(await _mediator.Send(new GetAuditLogLookupQuery(search, activeOnly), ct));

    /// <summary>Yeni kayıt oluşturur.</summary>
    [HttpPost]
    public async Task<ActionResult<BaseResponse<Guid>>> Create(CreateAuditLogRequest request, CancellationToken ct)
        => Ok(await _mediator.Send(new CreateAuditLogCommand(request), ct));

    /// <summary>Var olan kaydı günceller.</summary>
    [HttpPut("{id:guid}")]
    public async Task<ActionResult<BaseResponse<bool>>> Update(Guid id, UpdateAuditLogRequest request, CancellationToken ct)
        => Ok(await _mediator.Send(new UpdateAuditLogCommand(id, request), ct));

    /// <summary>Kaydı (soft-delete) siler.</summary>
    [HttpDelete("{id:guid}")]
    public async Task<ActionResult<BaseResponse<bool>>> Delete(Guid id, CancellationToken ct)
        => Ok(await _mediator.Send(new DeleteAuditLogCommand(id), ct));
}
