using Energy.Shared.Models.V1.Common.Responses;
using Energy.Shared.Models.V1.Home.Responses;
using Energy.Web.Clients.Infrastructure;

namespace Energy.Web.Clients.Home;

public sealed class HomeApiClient : ApiClientBase, IHomeApiClient
{
    public HomeApiClient(HttpClient httpClient) : base(httpClient) { }

    public Task<BaseResponse<HomeDashboardResponse>> GetDashboardAsync(CancellationToken ct = default)
        => GetAsync<BaseResponse<HomeDashboardResponse>>(ApiRoutes.Home.Dashboard, ct);
}
