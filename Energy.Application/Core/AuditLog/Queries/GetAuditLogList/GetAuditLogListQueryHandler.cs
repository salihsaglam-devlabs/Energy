using Energy.Application.Core.AuditLog.Services;
using Energy.Shared.Models.V1.Common.Responses;
using Energy.Shared.Models.V1.Core.AuditLog.Responses;
using MediatR;

namespace Energy.Application.Core.AuditLog.Queries.GetAuditLogList;

/// <summary>
/// <see cref="GetAuditLogListQuery"/> handler'ı. <see cref="IAuditLogService"/>'i orkestre eder.
/// </summary>
public sealed class GetAuditLogListQueryHandler
    : IRequestHandler<GetAuditLogListQuery, BaseResponse<PaginatedResponse<AuditLogListResponse>>>
{
    private readonly IAuditLogService _service;

    public GetAuditLogListQueryHandler(IAuditLogService service)
        => _service = service;

    public Task<BaseResponse<PaginatedResponse<AuditLogListResponse>>> Handle(
        GetAuditLogListQuery request,
        CancellationToken cancellationToken)
        => _service.GetListAsync(request.Request, cancellationToken);
}
