using Energy.Shared.Models.V1.Common.Responses;
using Energy.Shared.Models.V1.Organization.EmployeeSkillAssignment.Requests;
using Energy.Shared.Models.V1.Organization.EmployeeSkillAssignment.Responses;
using Energy.Web.Clients.Infrastructure;

namespace Energy.Web.Clients.Modules.Organization.EmployeeSkillAssignment;

/// <summary>EmployeeSkillAssignment API istemci sözleşmesi.</summary>
public interface IEmployeeSkillAssignmentApiClient
{
    Task<BaseResponse<PaginatedResponse<EmployeeSkillAssignmentListResponse>>> GetListAsync(int pageNumber, int pageSize, string? search, CancellationToken ct = default);
    Task<BaseResponse<EmployeeSkillAssignmentDetailResponse>> GetByIdAsync(Guid id, CancellationToken ct = default);
    Task<BaseResponse<IReadOnlyList<EmployeeSkillAssignmentLookupResponse>>> GetLookupAsync(string? search, bool activeOnly, CancellationToken ct = default);
    Task<BaseResponse<Guid>> CreateAsync(CreateEmployeeSkillAssignmentRequest request, CancellationToken ct = default);
    Task<BaseResponse<bool>> UpdateAsync(Guid id, UpdateEmployeeSkillAssignmentRequest request, CancellationToken ct = default);
    Task<BaseResponse<bool>> DeleteAsync(Guid id, CancellationToken ct = default);
}

/// <summary>EmployeeSkillAssignment API istemcisi (HttpClientFactory + BaseResponse).</summary>
public sealed class EmployeeSkillAssignmentApiClient : ApiClientBase, IEmployeeSkillAssignmentApiClient
{
    private const string Base = "api/v1/organization/employee-skill-assignments";

    public EmployeeSkillAssignmentApiClient(HttpClient httpClient) : base(httpClient) { }

    public Task<BaseResponse<PaginatedResponse<EmployeeSkillAssignmentListResponse>>> GetListAsync(int pageNumber, int pageSize, string? search, CancellationToken ct = default)
        => GetAsync<BaseResponse<PaginatedResponse<EmployeeSkillAssignmentListResponse>>>($"{Base}?pageNumber={pageNumber}&pageSize={pageSize}&search={Uri.EscapeDataString(search ?? string.Empty)}", ct);

    public Task<BaseResponse<EmployeeSkillAssignmentDetailResponse>> GetByIdAsync(Guid id, CancellationToken ct = default)
        => GetAsync<BaseResponse<EmployeeSkillAssignmentDetailResponse>>($"{Base}/{id}", ct);

    public Task<BaseResponse<IReadOnlyList<EmployeeSkillAssignmentLookupResponse>>> GetLookupAsync(string? search, bool activeOnly, CancellationToken ct = default)
        => GetAsync<BaseResponse<IReadOnlyList<EmployeeSkillAssignmentLookupResponse>>>($"{Base}/lookup?search={Uri.EscapeDataString(search ?? string.Empty)}&activeOnly={activeOnly}", ct);

    public Task<BaseResponse<Guid>> CreateAsync(CreateEmployeeSkillAssignmentRequest request, CancellationToken ct = default)
        => PostAsync<CreateEmployeeSkillAssignmentRequest, BaseResponse<Guid>>(Base, request, ct);

    public Task<BaseResponse<bool>> UpdateAsync(Guid id, UpdateEmployeeSkillAssignmentRequest request, CancellationToken ct = default)
        => PutAsync<UpdateEmployeeSkillAssignmentRequest, BaseResponse<bool>>($"{Base}/{id}", request, ct);

    public Task<BaseResponse<bool>> DeleteAsync(Guid id, CancellationToken ct = default)
        => DeleteAsync<BaseResponse<bool>>($"{Base}/{id}", ct);
}
