using Energy.Shared.Models.V1.Common.Responses;
using Energy.Shared.Models.V1.Workflow.ApprovalStepApprover.Requests;
using Energy.Shared.Models.V1.Workflow.ApprovalStepApprover.Responses;
using Energy.Web.Clients.Infrastructure;

namespace Energy.Web.Clients.Modules.Workflow.ApprovalStepApprover;

/// <summary>ApprovalStepApprover API istemci sözleşmesi.</summary>
public interface IApprovalStepApproverApiClient
{
    Task<BaseResponse<PaginatedResponse<ApprovalStepApproverListResponse>>> GetListAsync(int pageNumber, int pageSize, string? search, CancellationToken ct = default);
    Task<BaseResponse<ApprovalStepApproverDetailResponse>> GetByIdAsync(Guid id, CancellationToken ct = default);
    Task<BaseResponse<IReadOnlyList<ApprovalStepApproverLookupResponse>>> GetLookupAsync(string? search, bool activeOnly, CancellationToken ct = default);
    Task<BaseResponse<Guid>> CreateAsync(CreateApprovalStepApproverRequest request, CancellationToken ct = default);
    Task<BaseResponse<bool>> UpdateAsync(Guid id, UpdateApprovalStepApproverRequest request, CancellationToken ct = default);
    Task<BaseResponse<bool>> DeleteAsync(Guid id, CancellationToken ct = default);
}

/// <summary>ApprovalStepApprover API istemcisi (HttpClientFactory + BaseResponse).</summary>
public sealed class ApprovalStepApproverApiClient : ApiClientBase, IApprovalStepApproverApiClient
{
    private const string Base = "api/v1/workflow/approval-step-approvers";

    public ApprovalStepApproverApiClient(HttpClient httpClient) : base(httpClient) { }

    public Task<BaseResponse<PaginatedResponse<ApprovalStepApproverListResponse>>> GetListAsync(int pageNumber, int pageSize, string? search, CancellationToken ct = default)
        => GetAsync<BaseResponse<PaginatedResponse<ApprovalStepApproverListResponse>>>($"{Base}?pageNumber={pageNumber}&pageSize={pageSize}&search={Uri.EscapeDataString(search ?? string.Empty)}", ct);

    public Task<BaseResponse<ApprovalStepApproverDetailResponse>> GetByIdAsync(Guid id, CancellationToken ct = default)
        => GetAsync<BaseResponse<ApprovalStepApproverDetailResponse>>($"{Base}/{id}", ct);

    public Task<BaseResponse<IReadOnlyList<ApprovalStepApproverLookupResponse>>> GetLookupAsync(string? search, bool activeOnly, CancellationToken ct = default)
        => GetAsync<BaseResponse<IReadOnlyList<ApprovalStepApproverLookupResponse>>>($"{Base}/lookup?search={Uri.EscapeDataString(search ?? string.Empty)}&activeOnly={activeOnly}", ct);

    public Task<BaseResponse<Guid>> CreateAsync(CreateApprovalStepApproverRequest request, CancellationToken ct = default)
        => PostAsync<CreateApprovalStepApproverRequest, BaseResponse<Guid>>(Base, request, ct);

    public Task<BaseResponse<bool>> UpdateAsync(Guid id, UpdateApprovalStepApproverRequest request, CancellationToken ct = default)
        => PutAsync<UpdateApprovalStepApproverRequest, BaseResponse<bool>>($"{Base}/{id}", request, ct);

    public Task<BaseResponse<bool>> DeleteAsync(Guid id, CancellationToken ct = default)
        => DeleteAsync<BaseResponse<bool>>($"{Base}/{id}", ct);
}
