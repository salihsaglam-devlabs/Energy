using Energy.Shared.Models.V1.Common.Responses;
using Energy.Shared.Models.V1.Organization.Employee.Requests;
using Energy.Shared.Models.V1.Organization.Employee.Responses;
using Energy.Web.Clients.Infrastructure;

namespace Energy.Web.Clients.Organization.Employee;

/// <summary>Employee API istemci sözleşmesi.</summary>
public interface IEmployeeApiClient
{
    Task<BaseResponse<PaginatedResponse<EmployeeListResponse>>> GetListAsync(int pageNumber, int pageSize, string? search, CancellationToken ct = default);
    Task<BaseResponse<EmployeeDetailResponse>> GetByIdAsync(Guid id, CancellationToken ct = default);
    Task<BaseResponse<IReadOnlyList<EmployeeLookupResponse>>> GetLookupAsync(string? search, bool activeOnly, CancellationToken ct = default);
    Task<BaseResponse<Guid>> CreateAsync(CreateEmployeeRequest request, CancellationToken ct = default);
    Task<BaseResponse<bool>> UpdateAsync(Guid id, UpdateEmployeeRequest request, CancellationToken ct = default);
    Task<BaseResponse<bool>> DeleteAsync(Guid id, CancellationToken ct = default);
}

/// <summary>Employee API istemcisi (HttpClientFactory + BaseResponse).</summary>
public sealed class EmployeeApiClient : ApiClientBase, IEmployeeApiClient
{
    private const string Base = "api/v1/organization/employees";

    public EmployeeApiClient(HttpClient httpClient) : base(httpClient) { }

    public Task<BaseResponse<PaginatedResponse<EmployeeListResponse>>> GetListAsync(int pageNumber, int pageSize, string? search, CancellationToken ct = default)
        => GetAsync<BaseResponse<PaginatedResponse<EmployeeListResponse>>>($"{Base}?pageNumber={pageNumber}&pageSize={pageSize}&search={Uri.EscapeDataString(search ?? string.Empty)}", ct);

    public Task<BaseResponse<EmployeeDetailResponse>> GetByIdAsync(Guid id, CancellationToken ct = default)
        => GetAsync<BaseResponse<EmployeeDetailResponse>>($"{Base}/{id}", ct);

    public Task<BaseResponse<IReadOnlyList<EmployeeLookupResponse>>> GetLookupAsync(string? search, bool activeOnly, CancellationToken ct = default)
        => GetAsync<BaseResponse<IReadOnlyList<EmployeeLookupResponse>>>($"{Base}/lookup?search={Uri.EscapeDataString(search ?? string.Empty)}&activeOnly={activeOnly}", ct);

    public Task<BaseResponse<Guid>> CreateAsync(CreateEmployeeRequest request, CancellationToken ct = default)
        => PostAsync<CreateEmployeeRequest, BaseResponse<Guid>>(Base, request, ct);

    public Task<BaseResponse<bool>> UpdateAsync(Guid id, UpdateEmployeeRequest request, CancellationToken ct = default)
        => PutAsync<UpdateEmployeeRequest, BaseResponse<bool>>($"{Base}/{id}", request, ct);

    public Task<BaseResponse<bool>> DeleteAsync(Guid id, CancellationToken ct = default)
        => DeleteAsync<BaseResponse<bool>>($"{Base}/{id}", ct);
}
