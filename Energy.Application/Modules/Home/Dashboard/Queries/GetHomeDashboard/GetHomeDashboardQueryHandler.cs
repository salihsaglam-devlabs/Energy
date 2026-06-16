using Energy.Application.Common.Exceptions;
using Energy.Localization;
using Energy.Shared.Models.V1.Common.Responses;
using Energy.Shared.Models.V1.Home.Requests;
using Energy.Shared.Models.V1.Home.Responses;
using Energy.Application.Home.Services;
using MediatR;

namespace Energy.Application.Modules.Home.Dashboard.Queries.GetHomeDashboard;

/// <summary><see cref="GetHomeDashboardQuery"/> handler'ı (orkestrasyon).</summary>
public sealed class GetHomeDashboardQueryHandler
    : IRequestHandler<GetHomeDashboardQuery, BaseResponse<HomeDashboardResponse>>
{
    private readonly IHomeService _home;

    public GetHomeDashboardQueryHandler(IHomeService home)
    {
        _home = home;
    }

    public async Task<BaseResponse<HomeDashboardResponse>> Handle(GetHomeDashboardQuery request, CancellationToken ct)
    {
        var result = await _home.GetDashboardAsync(request.Request, ct);
        return BaseResponse<HomeDashboardResponse>.Success(result);
    }
}
