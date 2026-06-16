using Energy.Shared.Models.V1.Common.Responses;
using Energy.Shared.Models.V1.Organization.EmployeePosition.Requests;
using Energy.Shared.Models.V1.Organization.EmployeePosition.Responses;
using Energy.Web.Clients.Infrastructure;

namespace Energy.Web.Clients.Organization.EmployeePosition;

/// <summary>EmployeePosition API istemci sözleşmesi.</summary>
public interface IEmployeePositionApiClient
{
    Task<BaseResponse<PaginatedResponse<EmployeePositionListResponse>>> GetListAsync(int pageNumber, int pageSize, string? search, CancellationToken ct = default);
    Task<BaseResponse<EmployeePositionDetailResponse>> GetByIdAsync(Guid id, CancellationToken ct = default);
    Task<BaseResponse<IReadOnlyList<EmployeePositionLookupResponse>>> GetLookupAsync(string? search, bool activeOnly, CancellationToken ct = default);
    Task<BaseResponse<Guid>> CreateAsync(CreateEmployeePositionRequest request, CancellationToken ct = default);
    Task<BaseResponse<bool>> UpdateAsync(Guid id, UpdateEmployeePositionRequest request, CancellationToken ct = default);
    Task<BaseResponse<bool>> DeleteAsync(Guid id, CancellationToken ct = default);
}

/// <summary>EmployeePosition API istemcisi (HttpClientFactory + BaseResponse).</summary>
public sealed class EmployeePositionApiClient : ApiClientBase, IEmployeePositionApiClient
{
    private const string Base = "api/v1/organization/employee-positions";

    public EmployeePositionApiClient(HttpClient httpClient) : base(httpClient) { }

    public Task<BaseResponse<PaginatedResponse<EmployeePositionListResponse>>> GetListAsync(int pageNumber, int pageSize, string? search, CancellationToken ct = default)
        => GetAsync<BaseResponse<PaginatedResponse<EmployeePositionListResponse>>>($"{Base}?pageNumber={pageNumber}&pageSize={pageSize}&search={Uri.EscapeDataString(search ?? string.Empty)}", ct);

    public Task<BaseResponse<EmployeePositionDetailResponse>> GetByIdAsync(Guid id, CancellationToken ct = default)
        => GetAsync<BaseResponse<EmployeePositionDetailResponse>>($"{Base}/{id}", ct);

    public Task<BaseResponse<IReadOnlyList<EmployeePositionLookupResponse>>> GetLookupAsync(string? search, bool activeOnly, CancellationToken ct = default)
        => GetAsync<BaseResponse<IReadOnlyList<EmployeePositionLookupResponse>>>($"{Base}/lookup?search={Uri.EscapeDataString(search ?? string.Empty)}&activeOnly={activeOnly}", ct);

    public Task<BaseResponse<Guid>> CreateAsync(CreateEmployeePositionRequest request, CancellationToken ct = default)
        => PostAsync<CreateEmployeePositionRequest, BaseResponse<Guid>>(Base, request, ct);

    public Task<BaseResponse<bool>> UpdateAsync(Guid id, UpdateEmployeePositionRequest request, CancellationToken ct = default)
        => PutAsync<UpdateEmployeePositionRequest, BaseResponse<bool>>($"{Base}/{id}", request, ct);

    public Task<BaseResponse<bool>> DeleteAsync(Guid id, CancellationToken ct = default)
        => DeleteAsync<BaseResponse<bool>>($"{Base}/{id}", ct);
}
