using Energy.Shared.Models.V1.Common.Responses;
using Energy.Shared.Models.V1.Workflow.ApprovalDefinitionVersion.Requests;
using Energy.Shared.Models.V1.Workflow.ApprovalDefinitionVersion.Responses;
using Energy.Web.Clients.Infrastructure;

namespace Energy.Web.Clients.Modules.Workflow.ApprovalDefinitionVersion;

/// <summary>ApprovalDefinitionVersion API istemci sözleşmesi.</summary>
public interface IApprovalDefinitionVersionApiClient
{
    Task<BaseResponse<PaginatedResponse<ApprovalDefinitionVersionListResponse>>> GetListAsync(int pageNumber, int pageSize, string? search, CancellationToken ct = default);
    Task<BaseResponse<ApprovalDefinitionVersionDetailResponse>> GetByIdAsync(Guid id, CancellationToken ct = default);
    Task<BaseResponse<IReadOnlyList<ApprovalDefinitionVersionLookupResponse>>> GetLookupAsync(string? search, bool activeOnly, CancellationToken ct = default);
    Task<BaseResponse<Guid>> CreateAsync(CreateApprovalDefinitionVersionRequest request, CancellationToken ct = default);
    Task<BaseResponse<bool>> UpdateAsync(Guid id, UpdateApprovalDefinitionVersionRequest request, CancellationToken ct = default);
    Task<BaseResponse<bool>> DeleteAsync(Guid id, CancellationToken ct = default);
}

/// <summary>ApprovalDefinitionVersion API istemcisi (HttpClientFactory + BaseResponse).</summary>
public sealed class ApprovalDefinitionVersionApiClient : ApiClientBase, IApprovalDefinitionVersionApiClient
{
    private const string Base = "api/v1/workflow/approval-definition-versions";

    public ApprovalDefinitionVersionApiClient(HttpClient httpClient) : base(httpClient) { }

    public Task<BaseResponse<PaginatedResponse<ApprovalDefinitionVersionListResponse>>> GetListAsync(int pageNumber, int pageSize, string? search, CancellationToken ct = default)
        => GetAsync<BaseResponse<PaginatedResponse<ApprovalDefinitionVersionListResponse>>>($"{Base}?pageNumber={pageNumber}&pageSize={pageSize}&search={Uri.EscapeDataString(search ?? string.Empty)}", ct);

    public Task<BaseResponse<ApprovalDefinitionVersionDetailResponse>> GetByIdAsync(Guid id, CancellationToken ct = default)
        => GetAsync<BaseResponse<ApprovalDefinitionVersionDetailResponse>>($"{Base}/{id}", ct);

    public Task<BaseResponse<IReadOnlyList<ApprovalDefinitionVersionLookupResponse>>> GetLookupAsync(string? search, bool activeOnly, CancellationToken ct = default)
        => GetAsync<BaseResponse<IReadOnlyList<ApprovalDefinitionVersionLookupResponse>>>($"{Base}/lookup?search={Uri.EscapeDataString(search ?? string.Empty)}&activeOnly={activeOnly}", ct);

    public Task<BaseResponse<Guid>> CreateAsync(CreateApprovalDefinitionVersionRequest request, CancellationToken ct = default)
        => PostAsync<CreateApprovalDefinitionVersionRequest, BaseResponse<Guid>>(Base, request, ct);

    public Task<BaseResponse<bool>> UpdateAsync(Guid id, UpdateApprovalDefinitionVersionRequest request, CancellationToken ct = default)
        => PutAsync<UpdateApprovalDefinitionVersionRequest, BaseResponse<bool>>($"{Base}/{id}", request, ct);

    public Task<BaseResponse<bool>> DeleteAsync(Guid id, CancellationToken ct = default)
        => DeleteAsync<BaseResponse<bool>>($"{Base}/{id}", ct);
}
