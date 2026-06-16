using Energy.Application.Common.Exceptions;
using Energy.Localization;
using Energy.Shared.Models.V1.Common.Responses;
using Energy.Shared.Models.V1.Common.Requests;
using Energy.Shared.Models.V1.Logger.Requests;
using Energy.Shared.Models.V1.Logger.Responses;
using Energy.Shared.Identity;
using Energy.Application.Logger.Services;
using MediatR;

namespace Energy.Application.Core.Auditing.Queries.QueryAuditLogs;

/// <summary><see cref="QueryAuditLogsQuery"/> handler'ı (orkestrasyon).</summary>
public sealed class QueryAuditLogsQueryHandler
    : IRequestHandler<QueryAuditLogsQuery, BaseResponse<PaginatedResponse<AuditLogResponse>>>
{
    private readonly IAuditLogService _logs;

    public QueryAuditLogsQueryHandler(IAuditLogService logs)
    {
        _logs = logs;
    }

    public async Task<BaseResponse<PaginatedResponse<AuditLogResponse>>> Handle(QueryAuditLogsQuery request, CancellationToken ct)
    {
        var result = await _logs.QueryAsync(request.Query, request.Paging, ct);
        return BaseResponse<PaginatedResponse<AuditLogResponse>>.Success(result);
    }
}
