using Energy.Shared.Models.V1.Common.Responses;
using Energy.Shared.Models.V1.HR.Timesheet.Requests;
using Energy.Shared.Models.V1.HR.Timesheet.Responses;
using Energy.Web.Clients.Infrastructure;

namespace Energy.Web.Clients.HR.Timesheet;

/// <summary>Timesheet API istemci sözleşmesi.</summary>
public interface ITimesheetApiClient
{
    Task<BaseResponse<PaginatedResponse<TimesheetListResponse>>> GetListAsync(int pageNumber, int pageSize, string? search, CancellationToken ct = default);
    Task<BaseResponse<TimesheetDetailResponse>> GetByIdAsync(Guid id, CancellationToken ct = default);
    Task<BaseResponse<IReadOnlyList<TimesheetLookupResponse>>> GetLookupAsync(string? search, bool activeOnly, CancellationToken ct = default);
    Task<BaseResponse<Guid>> CreateAsync(CreateTimesheetRequest request, CancellationToken ct = default);
    Task<BaseResponse<bool>> UpdateAsync(Guid id, UpdateTimesheetRequest request, CancellationToken ct = default);
    Task<BaseResponse<bool>> DeleteAsync(Guid id, CancellationToken ct = default);
}

/// <summary>Timesheet API istemcisi (HttpClientFactory + BaseResponse).</summary>
public sealed class TimesheetApiClient : ApiClientBase, ITimesheetApiClient
{
    private const string Base = "api/v1/h-r/timesheets";

    public TimesheetApiClient(HttpClient httpClient) : base(httpClient) { }

    public Task<BaseResponse<PaginatedResponse<TimesheetListResponse>>> GetListAsync(int pageNumber, int pageSize, string? search, CancellationToken ct = default)
        => GetAsync<BaseResponse<PaginatedResponse<TimesheetListResponse>>>($"{Base}?pageNumber={pageNumber}&pageSize={pageSize}&search={Uri.EscapeDataString(search ?? string.Empty)}", ct);

    public Task<BaseResponse<TimesheetDetailResponse>> GetByIdAsync(Guid id, CancellationToken ct = default)
        => GetAsync<BaseResponse<TimesheetDetailResponse>>($"{Base}/{id}", ct);

    public Task<BaseResponse<IReadOnlyList<TimesheetLookupResponse>>> GetLookupAsync(string? search, bool activeOnly, CancellationToken ct = default)
        => GetAsync<BaseResponse<IReadOnlyList<TimesheetLookupResponse>>>($"{Base}/lookup?search={Uri.EscapeDataString(search ?? string.Empty)}&activeOnly={activeOnly}", ct);

    public Task<BaseResponse<Guid>> CreateAsync(CreateTimesheetRequest request, CancellationToken ct = default)
        => PostAsync<CreateTimesheetRequest, BaseResponse<Guid>>(Base, request, ct);

    public Task<BaseResponse<bool>> UpdateAsync(Guid id, UpdateTimesheetRequest request, CancellationToken ct = default)
        => PutAsync<UpdateTimesheetRequest, BaseResponse<bool>>($"{Base}/{id}", request, ct);

    public Task<BaseResponse<bool>> DeleteAsync(Guid id, CancellationToken ct = default)
        => DeleteAsync<BaseResponse<bool>>($"{Base}/{id}", ct);
}
