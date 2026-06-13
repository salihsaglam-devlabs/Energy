using Asp.Versioning;
using Energy.Application.Common.Exceptions;
using Energy.Application.Identity.Services;
using Energy.Application.Logger.Services;
using Energy.Localization;
using Energy.Shared.Identity;
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
    /// Üst bir katmandan (Web) gelen bir denetim kaydını alır. Web katmanı bu çağrıyı
    /// her zaman etkileşimsiz sistem servis hesabı olarak kimlik doğrular; bu yüzden o
    /// güvenilir çağıran bir kayıt ilettiğinde gerçek aktör istek gövdesinden alınır.
    /// Normal (insan) bir çağıran yalnızca kendi kimliği altında kayıt tutabilir —
    /// onlar için gövdedeki kimlik yok sayılır. Kaynak ve IP her zaman sunucu tarafında
    /// damgalanır.
    /// </summary>
    [HttpPost]
    public async Task<ActionResult<BaseResponse<bool>>> Ingest(CreateAuditLogRequest request, CancellationToken ct)
    {
        // Gövdeyle verilen kimliğe YALNIZCA çağıran sistem servis hesabı olduğunda
        // güven; aksi halde kaydı kimliği doğrulanmış asıl kullanıcıya ata; böylece
        // bir kullanıcı başka bir kullanıcının kimliğini sahteleyemez.
        var isSystemService = string.Equals(
            _currentUser.UserName, ServiceAccount.UserName, StringComparison.OrdinalIgnoreCase);
        var userId = isSystemService ? request.UserId : _currentUser.UserId;
        var userName = isSystemService ? request.UserName : _currentUser.UserName;

        await _logs.IngestAsync(
            request,
            userId,
            userName,
            HttpContext.Connection.RemoteIpAddress?.ToString(),
            ct);
        return Ok(BaseResponse<bool>.Success(true));
    }
}
