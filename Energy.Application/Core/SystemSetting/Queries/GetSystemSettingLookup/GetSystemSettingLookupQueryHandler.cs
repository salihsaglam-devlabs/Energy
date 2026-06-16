using Energy.Application.Core.SystemSetting.Lookups;
using Energy.Shared.Models.V1.Common.Responses;
using Energy.Shared.Models.V1.Core.SystemSetting.Responses;
using MediatR;

namespace Energy.Application.Core.SystemSetting.Queries.GetSystemSettingLookup;

/// <summary>
/// <see cref="GetSystemSettingLookupQuery"/> handler'ı. <see cref="ISystemSettingLookupService"/>'i orkestre eder.
/// </summary>
public sealed class GetSystemSettingLookupQueryHandler
    : IRequestHandler<GetSystemSettingLookupQuery, BaseResponse<IReadOnlyList<SystemSettingLookupResponse>>>
{
    private readonly ISystemSettingLookupService _lookup;

    public GetSystemSettingLookupQueryHandler(ISystemSettingLookupService lookup)
        => _lookup = lookup;

    public Task<BaseResponse<IReadOnlyList<SystemSettingLookupResponse>>> Handle(
        GetSystemSettingLookupQuery request,
        CancellationToken cancellationToken)
        => _lookup.GetLookupAsync(request.Search, request.ActiveOnly, cancellationToken);
}
