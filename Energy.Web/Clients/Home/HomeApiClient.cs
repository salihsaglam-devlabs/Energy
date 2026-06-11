using Energy.Shared.Models.V1.Common.Responses;
using Energy.Shared.Models.V1.Home.Requests;
using Energy.Shared.Models.V1.Home.Responses;
using Energy.Web.Clients.Infrastructure;

namespace Energy.Web.Clients.Home;

public sealed class HomeApiClient : ApiClientBase, IHomeApiClient
{
    public HomeApiClient(HttpClient httpClient) : base(httpClient)
    {
    }

    public Task<BaseResponse<HomeDashboardResponse>> GetDashboardAsync(
        GetHomeDashboardRequest request,
        CancellationToken cancellationToken = default)
    {
        var requestUri = ApiQueryString.Append(
            ApiRoutes.Home.Dashboard,
            ("includeQuickLinks", request.IncludeQuickLinks),
            ("quickLinkCount", request.QuickLinkCount));

        return GetAsync<BaseResponse<HomeDashboardResponse>>(requestUri, cancellationToken);
    }
}
