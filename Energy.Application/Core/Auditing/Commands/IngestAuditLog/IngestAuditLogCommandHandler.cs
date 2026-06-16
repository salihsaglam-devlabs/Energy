using Energy.Application.Common.Exceptions;
using Energy.Localization;
using Energy.Shared.Models.V1.Common.Responses;
using Energy.Shared.Models.V1.Common.Requests;
using Energy.Shared.Models.V1.Logger.Requests;
using Energy.Shared.Models.V1.Logger.Responses;
using Energy.Shared.Identity;
using Energy.Application.Identity.Services;
using Energy.Application.Logger.Services;
using MediatR;

namespace Energy.Application.Core.Auditing.Commands.IngestAuditLog;

/// <summary><see cref="IngestAuditLogCommand"/> handler'ı (orkestrasyon).</summary>
public sealed class IngestAuditLogCommandHandler
    : IRequestHandler<IngestAuditLogCommand, BaseResponse<bool>>
{
    private readonly IAuditLogService _logs;
    private readonly ICurrentUser _currentUser;

    public IngestAuditLogCommandHandler(IAuditLogService logs, ICurrentUser currentUser)
    {
        _logs = logs;
        _currentUser = currentUser;
    }

    public async Task<BaseResponse<bool>> Handle(IngestAuditLogCommand request, CancellationToken ct)
    {
        var currentUserId = _currentUser.UserId ?? Guid.Empty;
        var isSystemService = string.Equals(
            _currentUser.UserName, ServiceAccount.UserName, StringComparison.OrdinalIgnoreCase);
        var userId = isSystemService ? request.Request.UserId : _currentUser.UserId;
        var userName = isSystemService ? request.Request.UserName : _currentUser.UserName;
        await _logs.IngestAsync(request.Request, userId, userName, request.IpAddress, ct);
        return BaseResponse<bool>.Success(true);
    }
}
