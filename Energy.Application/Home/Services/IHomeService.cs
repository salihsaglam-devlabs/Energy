using Energy.Shared.Models.V1.Home.Requests;
using Energy.Shared.Models.V1.Home.Responses;

namespace Energy.Application.Home.Services;

public interface IHomeService
{
    Task<HomeDashboardResponse> GetDashboardAsync(GetHomeDashboardRequest request, CancellationToken cancellationToken = default);
}

