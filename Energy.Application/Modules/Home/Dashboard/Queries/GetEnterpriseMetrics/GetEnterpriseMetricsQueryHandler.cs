using Energy.Application.Common.Exceptions;
using Energy.Localization;
using Energy.Shared.Models.V1.Common.Responses;
using Energy.Shared.Models.V1.Home.Requests;
using Energy.Shared.Models.V1.Home.Responses;
using Energy.Application.Home.Services;
using MediatR;

namespace Energy.Application.Modules.Home.Dashboard.Queries.GetEnterpriseMetrics;

/// <summary><see cref="GetEnterpriseMetricsQuery"/> handler'ı (orkestrasyon).</summary>
public sealed class GetEnterpriseMetricsQueryHandler
    : IRequestHandler<GetEnterpriseMetricsQuery, BaseResponse<IReadOnlyList<EnterpriseMetricResponse>>>
{
    private readonly IHomeService _home;

    public GetEnterpriseMetricsQueryHandler(IHomeService home)
    {
        _home = home;
    }

    public async Task<BaseResponse<IReadOnlyList<EnterpriseMetricResponse>>> Handle(GetEnterpriseMetricsQuery request, CancellationToken ct)
    {
        var result = await _home.GetEnterpriseMetricsAsync(ct);
        return BaseResponse<IReadOnlyList<EnterpriseMetricResponse>>.Success(result);
    }
}
