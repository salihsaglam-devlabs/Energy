using Energy.Shared.Models.V1.Common.Responses;
using Energy.Shared.Models.V1.Home.Requests;
using Energy.Shared.Models.V1.Home.Responses;
using MediatR;

namespace Energy.Application.Home.Dashboard.Queries.GetHomeDashboard;

/// <summary>GetHomeDashboard</summary>
public sealed record GetHomeDashboardQuery(GetHomeDashboardRequest Request)
    : IRequest<BaseResponse<HomeDashboardResponse>>;
