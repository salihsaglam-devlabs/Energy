using Energy.Shared.Models.V1.Common.Responses;
using Energy.Shared.Models.V1.Workflow.Processes.Approval.Requests;
using Energy.Shared.Models.V1.Workflow.Processes.Approval.Responses;
using Energy.Web.Clients.Infrastructure;

namespace Energy.Web.Clients.Workflow.Processes.Approval;

/// <summary>Onay süreci API istemci sözleşmesi.</summary>
public interface IApprovalProcessApiClient
{
    Task<BaseResponse<IReadOnlyList<ApprovalRequestListItemResponse>>> GetMyPendingAsync(CancellationToken ct = default);
    Task<BaseResponse<ApprovalRequestListItemResponse>> ApproveAsync(Guid id, ApprovalActionRequest request, CancellationToken ct = default);
    Task<BaseResponse<ApprovalRequestListItemResponse>> RejectAsync(Guid id, ApprovalActionRequest request, CancellationToken ct = default);
    Task<BaseResponse<ApprovalRequestListItemResponse>> CancelAsync(Guid id, ApprovalActionRequest request, CancellationToken ct = default);
}

/// <summary>Onay süreci API istemcisi (HttpClientFactory + BaseResponse).</summary>
public sealed class ApprovalProcessApiClient : ApiClientBase, IApprovalProcessApiClient
{
    private const string Base = "api/v1/workflow/processes/approval";

    public ApprovalProcessApiClient(HttpClient httpClient) : base(httpClient) { }

    public Task<BaseResponse<IReadOnlyList<ApprovalRequestListItemResponse>>> GetMyPendingAsync(CancellationToken ct = default)
        => GetAsync<BaseResponse<IReadOnlyList<ApprovalRequestListItemResponse>>>($"{Base}/my-pending", ct);

    public Task<BaseResponse<ApprovalRequestListItemResponse>> ApproveAsync(Guid id, ApprovalActionRequest request, CancellationToken ct = default)
        => PostAsync<ApprovalActionRequest, BaseResponse<ApprovalRequestListItemResponse>>($"{Base}/{id}/approve", request, ct);

    public Task<BaseResponse<ApprovalRequestListItemResponse>> RejectAsync(Guid id, ApprovalActionRequest request, CancellationToken ct = default)
        => PostAsync<ApprovalActionRequest, BaseResponse<ApprovalRequestListItemResponse>>($"{Base}/{id}/reject", request, ct);

    public Task<BaseResponse<ApprovalRequestListItemResponse>> CancelAsync(Guid id, ApprovalActionRequest request, CancellationToken ct = default)
        => PostAsync<ApprovalActionRequest, BaseResponse<ApprovalRequestListItemResponse>>($"{Base}/{id}/cancel", request, ct);
}

