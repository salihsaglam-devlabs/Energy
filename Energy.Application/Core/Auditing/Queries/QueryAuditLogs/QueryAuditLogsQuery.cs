using Energy.Shared.Models.V1.Common.Responses;
using Energy.Shared.Models.V1.Common.Requests;
using Energy.Shared.Models.V1.Logger.Requests;
using Energy.Shared.Models.V1.Logger.Responses;
using Energy.Shared.Identity;
using MediatR;

namespace Energy.Application.Core.Auditing.Queries.QueryAuditLogs;

/// <summary>QueryAuditLogs</summary>
public sealed record QueryAuditLogsQuery(AuditLogQueryRequest Query, PaginatedRequest Paging)
    : IRequest<BaseResponse<PaginatedResponse<AuditLogResponse>>>;
