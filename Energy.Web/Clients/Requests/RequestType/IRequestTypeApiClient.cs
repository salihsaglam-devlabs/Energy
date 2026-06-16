using Energy.Shared.Models.V1.Common.Responses;
using Energy.Shared.Models.V1.Requests.RequestType.Requests;
using Energy.Shared.Models.V1.Requests.RequestType.Responses;
using Energy.Web.Clients.Infrastructure;

namespace Energy.Web.Clients.Requests.RequestType;

/// <summary>RequestType API istemci sözleşmesi.</summary>
public interface IRequestTypeApiClient
{
    Task<BaseResponse<PaginatedResponse<RequestTypeListResponse>>> GetListAsync(int pageNumber, int pageSize, string? search, CancellationToken ct = default);
    Task<BaseResponse<RequestTypeDetailResponse>> GetByIdAsync(Guid id, CancellationToken ct = default);
    Task<BaseResponse<IReadOnlyList<RequestTypeLookupResponse>>> GetLookupAsync(string? search, bool activeOnly, CancellationToken ct = default);
    Task<BaseResponse<Guid>> CreateAsync(CreateRequestTypeRequest request, CancellationToken ct = default);
    Task<BaseResponse<bool>> UpdateAsync(Guid id, UpdateRequestTypeRequest request, CancellationToken ct = default);
    Task<BaseResponse<bool>> DeleteAsync(Guid id, CancellationToken ct = default);
}

/// <summary>RequestType API istemcisi (HttpClientFactory + BaseResponse).</summary>
public sealed class RequestTypeApiClient : ApiClientBase, IRequestTypeApiClient
{
    private const string Base = "api/v1/requests/request-types";

    public RequestTypeApiClient(HttpClient httpClient) : base(httpClient) { }

    public Task<BaseResponse<PaginatedResponse<RequestTypeListResponse>>> GetListAsync(int pageNumber, int pageSize, string? search, CancellationToken ct = default)
        => GetAsync<BaseResponse<PaginatedResponse<RequestTypeListResponse>>>($"{Base}?pageNumber={pageNumber}&pageSize={pageSize}&search={Uri.EscapeDataString(search ?? string.Empty)}", ct);

    public Task<BaseResponse<RequestTypeDetailResponse>> GetByIdAsync(Guid id, CancellationToken ct = default)
        => GetAsync<BaseResponse<RequestTypeDetailResponse>>($"{Base}/{id}", ct);

    public Task<BaseResponse<IReadOnlyList<RequestTypeLookupResponse>>> GetLookupAsync(string? search, bool activeOnly, CancellationToken ct = default)
        => GetAsync<BaseResponse<IReadOnlyList<RequestTypeLookupResponse>>>($"{Base}/lookup?search={Uri.EscapeDataString(search ?? string.Empty)}&activeOnly={activeOnly}", ct);

    public Task<BaseResponse<Guid>> CreateAsync(CreateRequestTypeRequest request, CancellationToken ct = default)
        => PostAsync<CreateRequestTypeRequest, BaseResponse<Guid>>(Base, request, ct);

    public Task<BaseResponse<bool>> UpdateAsync(Guid id, UpdateRequestTypeRequest request, CancellationToken ct = default)
        => PutAsync<UpdateRequestTypeRequest, BaseResponse<bool>>($"{Base}/{id}", request, ct);

    public Task<BaseResponse<bool>> DeleteAsync(Guid id, CancellationToken ct = default)
        => DeleteAsync<BaseResponse<bool>>($"{Base}/{id}", ct);
}
