using Energy.Shared.Models.V1.Common.Responses;
using Energy.Shared.Models.V1.Organization.EmployeeSkill.Requests;
using Energy.Shared.Models.V1.Organization.EmployeeSkill.Responses;
using Energy.Web.Clients.Infrastructure;

namespace Energy.Web.Clients.Organization.EmployeeSkill;

/// <summary>EmployeeSkill API istemci sözleşmesi.</summary>
public interface IEmployeeSkillApiClient
{
    Task<BaseResponse<PaginatedResponse<EmployeeSkillListResponse>>> GetListAsync(int pageNumber, int pageSize, string? search, CancellationToken ct = default);
    Task<BaseResponse<EmployeeSkillDetailResponse>> GetByIdAsync(Guid id, CancellationToken ct = default);
    Task<BaseResponse<IReadOnlyList<EmployeeSkillLookupResponse>>> GetLookupAsync(string? search, bool activeOnly, CancellationToken ct = default);
    Task<BaseResponse<Guid>> CreateAsync(CreateEmployeeSkillRequest request, CancellationToken ct = default);
    Task<BaseResponse<bool>> UpdateAsync(Guid id, UpdateEmployeeSkillRequest request, CancellationToken ct = default);
    Task<BaseResponse<bool>> DeleteAsync(Guid id, CancellationToken ct = default);
}

/// <summary>EmployeeSkill API istemcisi (HttpClientFactory + BaseResponse).</summary>
public sealed class EmployeeSkillApiClient : ApiClientBase, IEmployeeSkillApiClient
{
    private const string Base = "api/v1/organization/employee-skills";

    public EmployeeSkillApiClient(HttpClient httpClient) : base(httpClient) { }

    public Task<BaseResponse<PaginatedResponse<EmployeeSkillListResponse>>> GetListAsync(int pageNumber, int pageSize, string? search, CancellationToken ct = default)
        => GetAsync<BaseResponse<PaginatedResponse<EmployeeSkillListResponse>>>($"{Base}?pageNumber={pageNumber}&pageSize={pageSize}&search={Uri.EscapeDataString(search ?? string.Empty)}", ct);

    public Task<BaseResponse<EmployeeSkillDetailResponse>> GetByIdAsync(Guid id, CancellationToken ct = default)
        => GetAsync<BaseResponse<EmployeeSkillDetailResponse>>($"{Base}/{id}", ct);

    public Task<BaseResponse<IReadOnlyList<EmployeeSkillLookupResponse>>> GetLookupAsync(string? search, bool activeOnly, CancellationToken ct = default)
        => GetAsync<BaseResponse<IReadOnlyList<EmployeeSkillLookupResponse>>>($"{Base}/lookup?search={Uri.EscapeDataString(search ?? string.Empty)}&activeOnly={activeOnly}", ct);

    public Task<BaseResponse<Guid>> CreateAsync(CreateEmployeeSkillRequest request, CancellationToken ct = default)
        => PostAsync<CreateEmployeeSkillRequest, BaseResponse<Guid>>(Base, request, ct);

    public Task<BaseResponse<bool>> UpdateAsync(Guid id, UpdateEmployeeSkillRequest request, CancellationToken ct = default)
        => PutAsync<UpdateEmployeeSkillRequest, BaseResponse<bool>>($"{Base}/{id}", request, ct);

    public Task<BaseResponse<bool>> DeleteAsync(Guid id, CancellationToken ct = default)
        => DeleteAsync<BaseResponse<bool>>($"{Base}/{id}", ct);
}
