using Energy.Shared.Models.V1.Common.Responses;
using Energy.Shared.Models.V1.Contracts.ContractAmendment.Requests;
using Energy.Shared.Models.V1.Contracts.ContractAmendment.Responses;
using Energy.Web.Clients.Infrastructure;

namespace Energy.Web.Clients.Contracts.ContractAmendment;

/// <summary>ContractAmendment API istemci sözleşmesi.</summary>
public interface IContractAmendmentApiClient
{
    Task<BaseResponse<PaginatedResponse<ContractAmendmentListResponse>>> GetListAsync(int pageNumber, int pageSize, string? search, CancellationToken ct = default);
    Task<BaseResponse<ContractAmendmentDetailResponse>> GetByIdAsync(Guid id, CancellationToken ct = default);
    Task<BaseResponse<IReadOnlyList<ContractAmendmentLookupResponse>>> GetLookupAsync(string? search, bool activeOnly, CancellationToken ct = default);
    Task<BaseResponse<Guid>> CreateAsync(CreateContractAmendmentRequest request, CancellationToken ct = default);
    Task<BaseResponse<bool>> UpdateAsync(Guid id, UpdateContractAmendmentRequest request, CancellationToken ct = default);
    Task<BaseResponse<bool>> DeleteAsync(Guid id, CancellationToken ct = default);
}

/// <summary>ContractAmendment API istemcisi (HttpClientFactory + BaseResponse).</summary>
public sealed class ContractAmendmentApiClient : ApiClientBase, IContractAmendmentApiClient
{
    private const string Base = "api/v1/contracts/contract-amendments";

    public ContractAmendmentApiClient(HttpClient httpClient) : base(httpClient) { }

    public Task<BaseResponse<PaginatedResponse<ContractAmendmentListResponse>>> GetListAsync(int pageNumber, int pageSize, string? search, CancellationToken ct = default)
        => GetAsync<BaseResponse<PaginatedResponse<ContractAmendmentListResponse>>>($"{Base}?pageNumber={pageNumber}&pageSize={pageSize}&search={Uri.EscapeDataString(search ?? string.Empty)}", ct);

    public Task<BaseResponse<ContractAmendmentDetailResponse>> GetByIdAsync(Guid id, CancellationToken ct = default)
        => GetAsync<BaseResponse<ContractAmendmentDetailResponse>>($"{Base}/{id}", ct);

    public Task<BaseResponse<IReadOnlyList<ContractAmendmentLookupResponse>>> GetLookupAsync(string? search, bool activeOnly, CancellationToken ct = default)
        => GetAsync<BaseResponse<IReadOnlyList<ContractAmendmentLookupResponse>>>($"{Base}/lookup?search={Uri.EscapeDataString(search ?? string.Empty)}&activeOnly={activeOnly}", ct);

    public Task<BaseResponse<Guid>> CreateAsync(CreateContractAmendmentRequest request, CancellationToken ct = default)
        => PostAsync<CreateContractAmendmentRequest, BaseResponse<Guid>>(Base, request, ct);

    public Task<BaseResponse<bool>> UpdateAsync(Guid id, UpdateContractAmendmentRequest request, CancellationToken ct = default)
        => PutAsync<UpdateContractAmendmentRequest, BaseResponse<bool>>($"{Base}/{id}", request, ct);

    public Task<BaseResponse<bool>> DeleteAsync(Guid id, CancellationToken ct = default)
        => DeleteAsync<BaseResponse<bool>>($"{Base}/{id}", ct);
}
