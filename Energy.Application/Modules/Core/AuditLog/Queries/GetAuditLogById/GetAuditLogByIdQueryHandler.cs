using Energy.Application.Modules.Core.AuditLog.Services;
using Energy.Shared.Models.V1.Common.Responses;
using Energy.Shared.Models.V1.Core.AuditLog.Responses;
using MediatR;

namespace Energy.Application.Modules.Core.AuditLog.Queries.GetAuditLogById;

/// <summary>
/// <see cref="GetAuditLogByIdQuery"/> handler'ı. <see cref="IAuditLogService"/>'i orkestre eder.
/// </summary>
public sealed class GetAuditLogByIdQueryHandler
    : IRequestHandler<GetAuditLogByIdQuery, BaseResponse<AuditLogDetailResponse>>
{
    private readonly IAuditLogService _service;

    public GetAuditLogByIdQueryHandler(IAuditLogService service)
        => _service = service;

    public Task<BaseResponse<AuditLogDetailResponse>> Handle(
        GetAuditLogByIdQuery request,
        CancellationToken cancellationToken)
        => _service.GetByIdAsync(request.Id, cancellationToken);
}
