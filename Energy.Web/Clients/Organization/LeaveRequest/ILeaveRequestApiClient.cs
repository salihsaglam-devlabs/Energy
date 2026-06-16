using Energy.Shared.Models.V1.Common.Responses;
using Energy.Shared.Models.V1.Organization.LeaveRequest.Requests;
using Energy.Shared.Models.V1.Organization.LeaveRequest.Responses;
using Energy.Web.Clients.Infrastructure;

namespace Energy.Web.Clients.Organization.LeaveRequest;

/// <summary>LeaveRequest API istemci sözleşmesi.</summary>
public interface ILeaveRequestApiClient
{
    Task<BaseResponse<PaginatedResponse<LeaveRequestListResponse>>> GetListAsync(int pageNumber, int pageSize, string? search, CancellationToken ct = default);
    Task<BaseResponse<LeaveRequestDetailResponse>> GetByIdAsync(Guid id, CancellationToken ct = default);
    Task<BaseResponse<IReadOnlyList<LeaveRequestLookupResponse>>> GetLookupAsync(string? search, bool activeOnly, CancellationToken ct = default);
    Task<BaseResponse<Guid>> CreateAsync(CreateLeaveRequestRequest request, CancellationToken ct = default);
    Task<BaseResponse<bool>> UpdateAsync(Guid id, UpdateLeaveRequestRequest request, CancellationToken ct = default);
    Task<BaseResponse<bool>> DeleteAsync(Guid id, CancellationToken ct = default);
}

/// <summary>LeaveRequest API istemcisi (HttpClientFactory + BaseResponse).</summary>
public sealed class LeaveRequestApiClient : ApiClientBase, ILeaveRequestApiClient
{
    private const string Base = "api/v1/organization/leave-requests";

    public LeaveRequestApiClient(HttpClient httpClient) : base(httpClient) { }

    public Task<BaseResponse<PaginatedResponse<LeaveRequestListResponse>>> GetListAsync(int pageNumber, int pageSize, string? search, CancellationToken ct = default)
        => GetAsync<BaseResponse<PaginatedResponse<LeaveRequestListResponse>>>($"{Base}?pageNumber={pageNumber}&pageSize={pageSize}&search={Uri.EscapeDataString(search ?? string.Empty)}", ct);

    public Task<BaseResponse<LeaveRequestDetailResponse>> GetByIdAsync(Guid id, CancellationToken ct = default)
        => GetAsync<BaseResponse<LeaveRequestDetailResponse>>($"{Base}/{id}", ct);

    public Task<BaseResponse<IReadOnlyList<LeaveRequestLookupResponse>>> GetLookupAsync(string? search, bool activeOnly, CancellationToken ct = default)
        => GetAsync<BaseResponse<IReadOnlyList<LeaveRequestLookupResponse>>>($"{Base}/lookup?search={Uri.EscapeDataString(search ?? string.Empty)}&activeOnly={activeOnly}", ct);

    public Task<BaseResponse<Guid>> CreateAsync(CreateLeaveRequestRequest request, CancellationToken ct = default)
        => PostAsync<CreateLeaveRequestRequest, BaseResponse<Guid>>(Base, request, ct);

    public Task<BaseResponse<bool>> UpdateAsync(Guid id, UpdateLeaveRequestRequest request, CancellationToken ct = default)
        => PutAsync<UpdateLeaveRequestRequest, BaseResponse<bool>>($"{Base}/{id}", request, ct);

    public Task<BaseResponse<bool>> DeleteAsync(Guid id, CancellationToken ct = default)
        => DeleteAsync<BaseResponse<bool>>($"{Base}/{id}", ct);
}
