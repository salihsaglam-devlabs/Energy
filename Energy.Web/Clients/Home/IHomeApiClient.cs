using Energy.Shared.Models.V1.Common.Responses;
using Energy.Shared.Models.V1.Home.Requests;
using Energy.Shared.Models.V1.Home.Responses;

namespace Energy.Web.Clients.Home;

public interface IHomeApiClient
{
    Task<BaseResponse<HomeDashboardResponse>> GetDashboardAsync(GetHomeDashboardRequest request, CancellationToken cancellationToken = default);
}
