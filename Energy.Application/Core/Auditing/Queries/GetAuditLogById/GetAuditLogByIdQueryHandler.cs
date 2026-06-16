using Energy.Application.Common.Exceptions;
using Energy.Localization;
using Energy.Shared.Models.V1.Common.Responses;
using Energy.Shared.Models.V1.Common.Requests;
using Energy.Shared.Models.V1.Logger.Requests;
using Energy.Shared.Models.V1.Logger.Responses;
using Energy.Shared.Identity;
using Energy.Application.Logger.Services;
using MediatR;

namespace Energy.Application.Core.Auditing.Queries.GetAuditLogById;

/// <summary><see cref="GetAuditLogByIdQuery"/> handler'ı (orkestrasyon).</summary>
public sealed class GetAuditLogByIdQueryHandler
    : IRequestHandler<GetAuditLogByIdQuery, BaseResponse<AuditLogResponse>>
{
    private readonly IAuditLogService _logs;

    public GetAuditLogByIdQueryHandler(IAuditLogService logs)
    {
        _logs = logs;
    }

    public async Task<BaseResponse<AuditLogResponse>> Handle(GetAuditLogByIdQuery request, CancellationToken ct)
    {
        var result = await _logs.GetByIdAsync(request.Id, ct)
            ?? throw new NotFoundException(LocalizationKeys.Messages.LogEntryNotFound, request.Id);
        return BaseResponse<AuditLogResponse>.Success(result);
    }
}
