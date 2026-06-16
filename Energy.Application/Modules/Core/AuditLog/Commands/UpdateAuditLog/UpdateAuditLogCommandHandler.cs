using Energy.Application.Modules.Core.AuditLog.Services;
using Energy.Shared.Models.V1.Common.Responses;
using MediatR;

namespace Energy.Application.Modules.Core.AuditLog.Commands.UpdateAuditLog;

/// <summary>
/// <see cref="UpdateAuditLogCommand"/> handler'ı. <see cref="IAuditLogService"/>'i orkestre eder.
/// </summary>
public sealed class UpdateAuditLogCommandHandler
    : IRequestHandler<UpdateAuditLogCommand, BaseResponse<bool>>
{
    private readonly IAuditLogService _service;

    public UpdateAuditLogCommandHandler(IAuditLogService service)
        => _service = service;

    public Task<BaseResponse<bool>> Handle(
        UpdateAuditLogCommand request,
        CancellationToken cancellationToken)
        => _service.UpdateAsync(request.Id, request.Request, cancellationToken);
}
