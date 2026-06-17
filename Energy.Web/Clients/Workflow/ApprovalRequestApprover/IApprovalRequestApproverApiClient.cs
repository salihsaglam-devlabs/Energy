using Energy.Shared.Models.V1.Common.Responses;
using Energy.Shared.Models.V1.Workflow.ApprovalRequestApprover.Requests;
using Energy.Shared.Models.V1.Workflow.ApprovalRequestApprover.Responses;
using Energy.Web.Clients.Infrastructure;

namespace Energy.Web.Clients.Workflow.ApprovalRequestApprover;

/// <summary>ApprovalRequestApprover API istemci sözleşmesi.</summary>
public interface IApprovalRequestApproverApiClient
{
    Task<BaseResponse<PaginatedResponse<ApprovalRequestApproverListResponse>>> GetListAsync(int pageNumber, int pageSize, string? search, CancellationToken ct = default);
    Task<BaseResponse<ApprovalRequestApproverDetailResponse>> GetByIdAsync(Guid id, CancellationToken ct = default);
    Task<BaseResponse<IReadOnlyList<ApprovalRequestApproverLookupResponse>>> GetLookupAsync(string? search, bool activeOnly, CancellationToken ct = default);
    Task<BaseResponse<Guid>> CreateAsync(CreateApprovalRequestApproverRequest request, CancellationToken ct = default);
    Task<BaseResponse<bool>> UpdateAsync(Guid id, UpdateApprovalRequestApproverRequest request, CancellationToken ct = default);
    Task<BaseResponse<bool>> DeleteAsync(Guid id, CancellationToken ct = default);
}

/// <summary>ApprovalRequestApprover API istemcisi (HttpClientFactory + BaseResponse).</summary>
public sealed class ApprovalRequestApproverApiClient : ApiClientBase, IApprovalRequestApproverApiClient
{
    private const string Base = "api/v1/workflow/approval-request-approvers";

    public ApprovalRequestApproverApiClient(HttpClient httpClient) : base(httpClient) { }

    public Task<BaseResponse<PaginatedResponse<ApprovalRequestApproverListResponse>>> GetListAsync(int pageNumber, int pageSize, string? search, CancellationToken ct = default)
        => GetAsync<BaseResponse<PaginatedResponse<ApprovalRequestApproverListResponse>>>($"{Base}?pageNumber={pageNumber}&pageSize={pageSize}&search={Uri.EscapeDataString(search ?? string.Empty)}", ct);

    public Task<BaseResponse<ApprovalRequestApproverDetailResponse>> GetByIdAsync(Guid id, CancellationToken ct = default)
        => GetAsync<BaseResponse<ApprovalRequestApproverDetailResponse>>($"{Base}/{id}", ct);

    public Task<BaseResponse<IReadOnlyList<ApprovalRequestApproverLookupResponse>>> GetLookupAsync(string? search, bool activeOnly, CancellationToken ct = default)
        => GetAsync<BaseResponse<IReadOnlyList<ApprovalRequestApproverLookupResponse>>>($"{Base}/lookup?search={Uri.EscapeDataString(search ?? string.Empty)}&activeOnly={activeOnly}", ct);

    public Task<BaseResponse<Guid>> CreateAsync(CreateApprovalRequestApproverRequest request, CancellationToken ct = default)
        => PostAsync<CreateApprovalRequestApproverRequest, BaseResponse<Guid>>(Base, request, ct);

    public Task<BaseResponse<bool>> UpdateAsync(Guid id, UpdateApprovalRequestApproverRequest request, CancellationToken ct = default)
        => PutAsync<UpdateApprovalRequestApproverRequest, BaseResponse<bool>>($"{Base}/{id}", request, ct);

    public Task<BaseResponse<bool>> DeleteAsync(Guid id, CancellationToken ct = default)
        => DeleteAsync<BaseResponse<bool>>($"{Base}/{id}", ct);
}
