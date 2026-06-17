using Energy.Application.Core.SystemSetting.Services;
using Energy.Shared.Models.V1.Common.Responses;
using MediatR;

namespace Energy.Application.Core.SystemSetting.Commands.CreateSystemSetting;

/// <summary>
/// <see cref="CreateSystemSettingCommand"/> handler'ı. İş mantığı içermez; yalnızca
/// <see cref="ISystemSettingService"/>'i orkestre eder.
/// </summary>
public sealed class CreateSystemSettingCommandHandler
    : IRequestHandler<CreateSystemSettingCommand, BaseResponse<Guid>>
{
    private readonly ISystemSettingService _service;

    public CreateSystemSettingCommandHandler(ISystemSettingService service)
        => _service = service;

    public Task<BaseResponse<Guid>> Handle(
        CreateSystemSettingCommand request,
        CancellationToken cancellationToken)
        => _service.CreateAsync(request.Request, cancellationToken);
}
