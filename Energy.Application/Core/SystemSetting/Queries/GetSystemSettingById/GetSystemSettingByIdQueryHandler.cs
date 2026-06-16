using Energy.Application.Core.SystemSetting.Services;
using Energy.Shared.Models.V1.Common.Responses;
using Energy.Shared.Models.V1.Core.SystemSetting.Responses;
using MediatR;

namespace Energy.Application.Core.SystemSetting.Queries.GetSystemSettingById;

/// <summary>
/// <see cref="GetSystemSettingByIdQuery"/> handler'ı. <see cref="ISystemSettingService"/>'i orkestre eder.
/// </summary>
public sealed class GetSystemSettingByIdQueryHandler
    : IRequestHandler<GetSystemSettingByIdQuery, BaseResponse<SystemSettingDetailResponse>>
{
    private readonly ISystemSettingService _service;

    public GetSystemSettingByIdQueryHandler(ISystemSettingService service)
        => _service = service;

    public Task<BaseResponse<SystemSettingDetailResponse>> Handle(
        GetSystemSettingByIdQuery request,
        CancellationToken cancellationToken)
        => _service.GetByIdAsync(request.Id, cancellationToken);
}
