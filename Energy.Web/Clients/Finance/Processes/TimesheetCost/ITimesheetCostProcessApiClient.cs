using Energy.Shared.Models.V1.Common.Responses;
using Energy.Shared.Models.V1.Finance.Processes.TimesheetCost.Requests;
using Energy.Shared.Models.V1.Finance.Processes.TimesheetCost.Responses;
using Energy.Web.Clients.Infrastructure;

namespace Energy.Web.Clients.Finance.Processes.TimesheetCost;

/// <summary>Puantaj maliyet süreci API istemci sözleşmesi.</summary>
public interface ITimesheetCostProcessApiClient
{
    Task<BaseResponse<TimesheetCostProcessResponse>> ExecuteAsync(TimesheetCostProcessRequest request, CancellationToken ct = default);
}

/// <summary>Puantaj maliyet süreci API istemcisi (HttpClientFactory + BaseResponse).</summary>
public sealed class TimesheetCostProcessApiClient : ApiClientBase, ITimesheetCostProcessApiClient
{
    private const string Base = "api/v1/finance/processes/timesheet-cost";

    public TimesheetCostProcessApiClient(HttpClient httpClient) : base(httpClient) { }

    public Task<BaseResponse<TimesheetCostProcessResponse>> ExecuteAsync(TimesheetCostProcessRequest request, CancellationToken ct = default)
        => PostAsync<TimesheetCostProcessRequest, BaseResponse<TimesheetCostProcessResponse>>(Base, request, ct);
}

