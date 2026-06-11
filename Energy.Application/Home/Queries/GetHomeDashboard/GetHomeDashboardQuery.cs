using Energy.Shared.Models.V1.Common.Responses;
using Energy.Shared.Models.V1.Home.Responses;
using MediatR;

namespace Energy.Application.Home.Queries.GetHomeDashboard;

public sealed record GetHomeDashboardQuery(
    bool IncludeQuickLinks = true,
    int QuickLinkCount = 4) : IRequest<BaseResponse<HomeDashboardResponse>>;
