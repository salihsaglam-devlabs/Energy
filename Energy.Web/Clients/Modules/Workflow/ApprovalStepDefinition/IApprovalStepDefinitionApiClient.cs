using Energy.Shared.Models.V1.Common.Responses;
using Energy.Shared.Models.V1.Workflow.ApprovalStepDefinition.Requests;
using Energy.Shared.Models.V1.Workflow.ApprovalStepDefinition.Responses;
using Energy.Web.Clients.Infrastructure;

namespace Energy.Web.Clients.Modules.Workflow.ApprovalStepDefinition;

/// <summary>ApprovalStepDefinition API istemci sözleşmesi.</summary>
public interface IApprovalStepDefinitionApiClient
{
    Task<BaseResponse<PaginatedResponse<ApprovalStepDefinitionListResponse>>> GetListAsync(int pageNumber, int pageSize, string? search, CancellationToken ct = default);
    Task<BaseResponse<ApprovalStepDefinitionDetailResponse>> GetByIdAsync(Guid id, CancellationToken ct = default);
    Task<BaseResponse<IReadOnlyList<ApprovalStepDefinitionLookupResponse>>> GetLookupAsync(string? search, bool activeOnly, CancellationToken ct = default);
    Task<BaseResponse<Guid>> CreateAsync(CreateApprovalStepDefinitionRequest request, CancellationToken ct = default);
    Task<BaseResponse<bool>> UpdateAsync(Guid id, UpdateApprovalStepDefinitionRequest request, CancellationToken ct = default);
    Task<BaseResponse<bool>> DeleteAsync(Guid id, CancellationToken ct = default);
}

/// <summary>ApprovalStepDefinition API istemcisi (HttpClientFactory + BaseResponse).</summary>
public sealed class ApprovalStepDefinitionApiClient : ApiClientBase, IApprovalStepDefinitionApiClient
{
    private const string Base = "api/v1/workflow/approval-step-definitions";

    public ApprovalStepDefinitionApiClient(HttpClient httpClient) : base(httpClient) { }

    public Task<BaseResponse<PaginatedResponse<ApprovalStepDefinitionListResponse>>> GetListAsync(int pageNumber, int pageSize, string? search, CancellationToken ct = default)
        => GetAsync<BaseResponse<PaginatedResponse<ApprovalStepDefinitionListResponse>>>($"{Base}?pageNumber={pageNumber}&pageSize={pageSize}&search={Uri.EscapeDataString(search ?? string.Empty)}", ct);

    public Task<BaseResponse<ApprovalStepDefinitionDetailResponse>> GetByIdAsync(Guid id, CancellationToken ct = default)
        => GetAsync<BaseResponse<ApprovalStepDefinitionDetailResponse>>($"{Base}/{id}", ct);

    public Task<BaseResponse<IReadOnlyList<ApprovalStepDefinitionLookupResponse>>> GetLookupAsync(string? search, bool activeOnly, CancellationToken ct = default)
        => GetAsync<BaseResponse<IReadOnlyList<ApprovalStepDefinitionLookupResponse>>>($"{Base}/lookup?search={Uri.EscapeDataString(search ?? string.Empty)}&activeOnly={activeOnly}", ct);

    public Task<BaseResponse<Guid>> CreateAsync(CreateApprovalStepDefinitionRequest request, CancellationToken ct = default)
        => PostAsync<CreateApprovalStepDefinitionRequest, BaseResponse<Guid>>(Base, request, ct);

    public Task<BaseResponse<bool>> UpdateAsync(Guid id, UpdateApprovalStepDefinitionRequest request, CancellationToken ct = default)
        => PutAsync<UpdateApprovalStepDefinitionRequest, BaseResponse<bool>>($"{Base}/{id}", request, ct);

    public Task<BaseResponse<bool>> DeleteAsync(Guid id, CancellationToken ct = default)
        => DeleteAsync<BaseResponse<bool>>($"{Base}/{id}", ct);
}
