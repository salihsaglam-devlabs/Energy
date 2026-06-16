using Energy.Application.Modules.Core.AuditLog.Services;
using Energy.Shared.Models.V1.Common.Responses;
using MediatR;

namespace Energy.Application.Modules.Core.AuditLog.Commands.DeleteAuditLog;

/// <summary>
/// <see cref="DeleteAuditLogCommand"/> handler'ı. <see cref="IAuditLogService"/>'i orkestre eder.
/// </summary>
public sealed class DeleteAuditLogCommandHandler
    : IRequestHandler<DeleteAuditLogCommand, BaseResponse<bool>>
{
    private readonly IAuditLogService _service;

    public DeleteAuditLogCommandHandler(IAuditLogService service)
        => _service = service;

    public Task<BaseResponse<bool>> Handle(
        DeleteAuditLogCommand request,
        CancellationToken cancellationToken)
        => _service.DeleteAsync(request.Id, cancellationToken);
}
