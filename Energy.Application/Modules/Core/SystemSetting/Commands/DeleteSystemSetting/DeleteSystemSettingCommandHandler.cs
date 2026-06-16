using Energy.Application.Modules.Core.SystemSetting.Services;
using Energy.Shared.Models.V1.Common.Responses;
using MediatR;

namespace Energy.Application.Modules.Core.SystemSetting.Commands.DeleteSystemSetting;

/// <summary>
/// <see cref="DeleteSystemSettingCommand"/> handler'ı. <see cref="ISystemSettingService"/>'i orkestre eder.
/// </summary>
public sealed class DeleteSystemSettingCommandHandler
    : IRequestHandler<DeleteSystemSettingCommand, BaseResponse<bool>>
{
    private readonly ISystemSettingService _service;

    public DeleteSystemSettingCommandHandler(ISystemSettingService service)
        => _service = service;

    public Task<BaseResponse<bool>> Handle(
        DeleteSystemSettingCommand request,
        CancellationToken cancellationToken)
        => _service.DeleteAsync(request.Id, cancellationToken);
}
