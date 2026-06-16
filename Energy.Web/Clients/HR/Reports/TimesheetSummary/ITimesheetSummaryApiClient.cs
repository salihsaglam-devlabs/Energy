using Energy.Shared.Models.V1.Common.Responses;
using Energy.Shared.Models.V1.HR.Reports.TimesheetSummary.Responses;
using Energy.Web.Clients.Infrastructure;

namespace Energy.Web.Clients.HR.Reports.TimesheetSummary;

/// <summary>TimesheetSummary raporu API istemci sözleşmesi.</summary>
public interface ITimesheetSummaryApiClient
{
    Task<BaseResponse<PaginatedResponse<TimesheetSummaryRowResponse>>> GetDataAsync(string query, CancellationToken ct = default);
}

/// <summary>TimesheetSummary raporu API istemcisi (HttpClientFactory + BaseResponse).</summary>
public sealed class TimesheetSummaryApiClient : ApiClientBase, ITimesheetSummaryApiClient
{
    private const string Base = "api/v1/h-r/reports/timesheet-summary";

    public TimesheetSummaryApiClient(HttpClient httpClient) : base(httpClient) { }

    public Task<BaseResponse<PaginatedResponse<TimesheetSummaryRowResponse>>> GetDataAsync(string query, CancellationToken ct = default)
        => GetAsync<BaseResponse<PaginatedResponse<TimesheetSummaryRowResponse>>>(string.IsNullOrEmpty(query) ? Base : $"{Base}?{query}", ct);
}
