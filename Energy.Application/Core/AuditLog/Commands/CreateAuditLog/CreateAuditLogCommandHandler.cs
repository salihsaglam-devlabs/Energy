using Energy.Application.Core.AuditLog.Services;
using Energy.Shared.Models.V1.Common.Responses;
using MediatR;

namespace Energy.Application.Core.AuditLog.Commands.CreateAuditLog;

/// <summary>
/// <see cref="CreateAuditLogCommand"/> handler'ı. İş mantığı içermez; yalnızca
/// <see cref="IAuditLogService"/>'i orkestre eder.
/// </summary>
public sealed class CreateAuditLogCommandHandler
    : IRequestHandler<CreateAuditLogCommand, BaseResponse<Guid>>
{
    private readonly IAuditLogService _service;

    public CreateAuditLogCommandHandler(IAuditLogService service)
        => _service = service;

    public Task<BaseResponse<Guid>> Handle(
        CreateAuditLogCommand request,
        CancellationToken cancellationToken)
        => _service.CreateAsync(request.Request, cancellationToken);
}
