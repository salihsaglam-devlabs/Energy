using Asp.Versioning;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Energy.Shared.Models.V1.Common.Responses;
using Energy.Shared.Models.V1.Common.Requests;
using Energy.Shared.Models.V1.Logger.Requests;
using Energy.Shared.Models.V1.Logger.Responses;
using Energy.Application.Core.Auditing.Commands.IngestAuditLog;
using Energy.Application.Core.Auditing.Queries.GetAuditLogById;
using Energy.Application.Core.Auditing.Queries.QueryAuditLogs;

namespace Energy.Api.Controllers.Core;

/// <summary>Denetim kaydı (audit log) sorgu/ingest uç noktaları (Core).</summary>
[ApiController]
[ApiVersion("1.0")]
[Route("api/v{version:apiVersion}/audit-logs")]
public sealed class AuditLogsController : ControllerBase
{
    private readonly IMediator _mediator;

    public AuditLogsController(IMediator mediator)
        => _mediator = mediator;

    [HttpGet]
    public async Task<ActionResult<BaseResponse<PaginatedResponse<AuditLogResponse>>>> Query([FromQuery] AuditLogQueryRequest query, [FromQuery] PaginatedRequest paging, CancellationToken ct)
        => Ok(await _mediator.Send(new QueryAuditLogsQuery(query, paging), ct));

    [HttpGet("{id:long}")]
    public async Task<ActionResult<BaseResponse<AuditLogResponse>>> GetById(long id, CancellationToken ct)
        => Ok(await _mediator.Send(new GetAuditLogByIdQuery(id), ct));

    [HttpPost]
    public async Task<ActionResult<BaseResponse<bool>>> Ingest(CreateAuditLogRequest request, CancellationToken ct)
        => Ok(await _mediator.Send(new IngestAuditLogCommand(request, HttpContext.Connection.RemoteIpAddress?.ToString()), ct));
}
