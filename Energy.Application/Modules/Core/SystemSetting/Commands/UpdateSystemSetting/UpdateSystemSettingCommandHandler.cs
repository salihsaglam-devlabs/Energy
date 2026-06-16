using Energy.Application.Modules.Core.SystemSetting.Services;
using Energy.Shared.Models.V1.Common.Responses;
using MediatR;

namespace Energy.Application.Modules.Core.SystemSetting.Commands.UpdateSystemSetting;

/// <summary>
/// <see cref="UpdateSystemSettingCommand"/> handler'ı. <see cref="ISystemSettingService"/>'i orkestre eder.
/// </summary>
public sealed class UpdateSystemSettingCommandHandler
    : IRequestHandler<UpdateSystemSettingCommand, BaseResponse<bool>>
{
    private readonly ISystemSettingService _service;

    public UpdateSystemSettingCommandHandler(ISystemSettingService service)
        => _service = service;

    public Task<BaseResponse<bool>> Handle(
        UpdateSystemSettingCommand request,
        CancellationToken cancellationToken)
        => _service.UpdateAsync(request.Id, request.Request, cancellationToken);
}
