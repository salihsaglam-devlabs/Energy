using Energy.Application.Modules.Core.SystemSetting.Services;
using Energy.Shared.Models.V1.Common.Responses;
using Energy.Shared.Models.V1.Core.SystemSetting.Responses;
using MediatR;

namespace Energy.Application.Modules.Core.SystemSetting.Queries.GetSystemSettingList;

/// <summary>
/// <see cref="GetSystemSettingListQuery"/> handler'ı. <see cref="ISystemSettingService"/>'i orkestre eder.
/// </summary>
public sealed class GetSystemSettingListQueryHandler
    : IRequestHandler<GetSystemSettingListQuery, BaseResponse<PaginatedResponse<SystemSettingListResponse>>>
{
    private readonly ISystemSettingService _service;

    public GetSystemSettingListQueryHandler(ISystemSettingService service)
        => _service = service;

    public Task<BaseResponse<PaginatedResponse<SystemSettingListResponse>>> Handle(
        GetSystemSettingListQuery request,
        CancellationToken cancellationToken)
        => _service.GetListAsync(request.Request, cancellationToken);
}
