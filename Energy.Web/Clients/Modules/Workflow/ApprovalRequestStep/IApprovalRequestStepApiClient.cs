using Energy.Shared.Models.V1.Common.Responses;
using Energy.Shared.Models.V1.Workflow.ApprovalRequestStep.Requests;
using Energy.Shared.Models.V1.Workflow.ApprovalRequestStep.Responses;
using Energy.Web.Clients.Infrastructure;

namespace Energy.Web.Clients.Modules.Workflow.ApprovalRequestStep;

/// <summary>ApprovalRequestStep API istemci sözleşmesi.</summary>
public interface IApprovalRequestStepApiClient
{
    Task<BaseResponse<PaginatedResponse<ApprovalRequestStepListResponse>>> GetListAsync(int pageNumber, int pageSize, string? search, CancellationToken ct = default);
    Task<BaseResponse<ApprovalRequestStepDetailResponse>> GetByIdAsync(Guid id, CancellationToken ct = default);
    Task<BaseResponse<IReadOnlyList<ApprovalRequestStepLookupResponse>>> GetLookupAsync(string? search, bool activeOnly, CancellationToken ct = default);
    Task<BaseResponse<Guid>> CreateAsync(CreateApprovalRequestStepRequest request, CancellationToken ct = default);
    Task<BaseResponse<bool>> UpdateAsync(Guid id, UpdateApprovalRequestStepRequest request, CancellationToken ct = default);
    Task<BaseResponse<bool>> DeleteAsync(Guid id, CancellationToken ct = default);
}

/// <summary>ApprovalRequestStep API istemcisi (HttpClientFactory + BaseResponse).</summary>
public sealed class ApprovalRequestStepApiClient : ApiClientBase, IApprovalRequestStepApiClient
{
    private const string Base = "api/v1/workflow/approval-request-steps";

    public ApprovalRequestStepApiClient(HttpClient httpClient) : base(httpClient) { }

    public Task<BaseResponse<PaginatedResponse<ApprovalRequestStepListResponse>>> GetListAsync(int pageNumber, int pageSize, string? search, CancellationToken ct = default)
        => GetAsync<BaseResponse<PaginatedResponse<ApprovalRequestStepListResponse>>>($"{Base}?pageNumber={pageNumber}&pageSize={pageSize}&search={Uri.EscapeDataString(search ?? string.Empty)}", ct);

    public Task<BaseResponse<ApprovalRequestStepDetailResponse>> GetByIdAsync(Guid id, CancellationToken ct = default)
        => GetAsync<BaseResponse<ApprovalRequestStepDetailResponse>>($"{Base}/{id}", ct);

    public Task<BaseResponse<IReadOnlyList<ApprovalRequestStepLookupResponse>>> GetLookupAsync(string? search, bool activeOnly, CancellationToken ct = default)
        => GetAsync<BaseResponse<IReadOnlyList<ApprovalRequestStepLookupResponse>>>($"{Base}/lookup?search={Uri.EscapeDataString(search ?? string.Empty)}&activeOnly={activeOnly}", ct);

    public Task<BaseResponse<Guid>> CreateAsync(CreateApprovalRequestStepRequest request, CancellationToken ct = default)
        => PostAsync<CreateApprovalRequestStepRequest, BaseResponse<Guid>>(Base, request, ct);

    public Task<BaseResponse<bool>> UpdateAsync(Guid id, UpdateApprovalRequestStepRequest request, CancellationToken ct = default)
        => PutAsync<UpdateApprovalRequestStepRequest, BaseResponse<bool>>($"{Base}/{id}", request, ct);

    public Task<BaseResponse<bool>> DeleteAsync(Guid id, CancellationToken ct = default)
        => DeleteAsync<BaseResponse<bool>>($"{Base}/{id}", ct);
}
