using Energy.Shared.Models.V1.Common.Responses;
using Energy.Shared.Models.V1.Home.Requests;
using Energy.Shared.Models.V1.Home.Responses;
using MediatR;

namespace Energy.Application.Home.Dashboard.Queries.GetEnterpriseMetrics;

/// <summary>GetEnterpriseMetrics</summary>
public sealed record GetEnterpriseMetricsQuery()
    : IRequest<BaseResponse<IReadOnlyList<EnterpriseMetricResponse>>>;
