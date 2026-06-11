using Asp.Versioning;
using Energy.Application.Common.Exceptions;
using Energy.Application.Identity.Services;
using Energy.Application.Logger.Services;
using Energy.Localization;
using Energy.Shared.Models.V1.Common.Requests;
using Energy.Shared.Models.V1.Common.Responses;
using Energy.Shared.Models.V1.Logger.Requests;
using Energy.Shared.Models.V1.Logger.Responses;
using Microsoft.AspNetCore.Mvc;

namespace Energy.Api.Controllers;

[ApiController]
[ApiVersion("1.0")]
[Route("api/v{version:apiVersion}/audit-logs")]
public sealed class AuditLogsController : ControllerBase
{
    private readonly IAuditLogService _logs;
    private readonly ICurrentUser _currentUser;
    public AuditLogsController(IAuditLogService logs, ICurrentUser currentUser)
    {
        _logs = logs;
        _currentUser = currentUser;
    }

    [HttpGet]
    public async Task<ActionResult<BaseResponse<PaginatedResponse<AuditLogResponse>>>> Query(
        [FromQuery] AuditLogQueryRequest query, [FromQuery] PaginatedRequest paging, CancellationToken ct)
        => Ok(BaseResponse<PaginatedResponse<AuditLogResponse>>.Success(await _logs.QueryAsync(query, paging, ct)));

    [HttpGet("{id:long}")]
    public async Task<ActionResult<BaseResponse<AuditLogResponse>>> GetById(long id, CancellationToken ct)
    {
        var item = await _logs.GetByIdAsync(id, ct)
                   ?? throw new NotFoundException(LocalizationKeys.Messages.LogEntryNotFound, id);
        return Ok(BaseResponse<AuditLogResponse>.Success(item));
    }

    /// <summary>
    /// Ingests an audit entry from an upper layer (Web). Identity and source are
    /// stamped server-side from the authenticated principal — the body is only
    /// trusted for the request/response payload, never for the user identity.
    /// </summary>
    [HttpPost]
    public async Task<ActionResult<BaseResponse<bool>>> Ingest(CreateAuditLogRequest request, CancellationToken ct)
    {
        await _logs.IngestAsync(
            request,
            _currentUser.UserId,
            _currentUser.UserName,
            HttpContext.Connection.RemoteIpAddress?.ToString(),
            ct);
        return Ok(BaseResponse<bool>.Success(true));
    }
}
