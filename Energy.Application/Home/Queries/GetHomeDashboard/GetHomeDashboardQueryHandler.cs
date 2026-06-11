using Energy.Application.Home.Services;
using Energy.Shared.Models.V1.Common.Responses;
using Energy.Shared.Models.V1.Home.Requests;
using Energy.Shared.Models.V1.Home.Responses;
using MediatR;

namespace Energy.Application.Home.Queries.GetHomeDashboard;

public sealed class GetHomeDashboardQueryHandler
    : IRequestHandler<GetHomeDashboardQuery, BaseResponse<HomeDashboardResponse>>
{
    private readonly IHomeService _homeService;

    public GetHomeDashboardQueryHandler(IHomeService homeService)
    {
        _homeService = homeService;
    }

    public async Task<BaseResponse<HomeDashboardResponse>> Handle(
        GetHomeDashboardQuery request,
        CancellationToken cancellationToken)
    {
        var apiRequest = new GetHomeDashboardRequest
        {
            IncludeQuickLinks = request.IncludeQuickLinks,
            QuickLinkCount = request.QuickLinkCount
        };

        var result = await _homeService.GetDashboardAsync(apiRequest, cancellationToken);
        return BaseResponse<HomeDashboardResponse>.Success(result);
    }
}
