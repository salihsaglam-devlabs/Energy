using Energy.Shared.Models.V1.Common.Responses;
using Energy.Shared.Models.V1.BusinessPartners.BusinessPartnerAddress.Requests;
using Energy.Shared.Models.V1.BusinessPartners.BusinessPartnerAddress.Responses;
using Energy.Web.Clients.Infrastructure;

namespace Energy.Web.Clients.BusinessPartners.BusinessPartnerAddress;

/// <summary>BusinessPartnerAddress API istemci sözleşmesi.</summary>
public interface IBusinessPartnerAddressApiClient
{
    Task<BaseResponse<PaginatedResponse<BusinessPartnerAddressListResponse>>> GetListAsync(int pageNumber, int pageSize, string? search, CancellationToken ct = default);
    Task<BaseResponse<BusinessPartnerAddressDetailResponse>> GetByIdAsync(Guid id, CancellationToken ct = default);
    Task<BaseResponse<IReadOnlyList<BusinessPartnerAddressLookupResponse>>> GetLookupAsync(string? search, bool activeOnly, CancellationToken ct = default);
    Task<BaseResponse<Guid>> CreateAsync(CreateBusinessPartnerAddressRequest request, CancellationToken ct = default);
    Task<BaseResponse<bool>> UpdateAsync(Guid id, UpdateBusinessPartnerAddressRequest request, CancellationToken ct = default);
    Task<BaseResponse<bool>> DeleteAsync(Guid id, CancellationToken ct = default);
}

/// <summary>BusinessPartnerAddress API istemcisi (HttpClientFactory + BaseResponse).</summary>
public sealed class BusinessPartnerAddressApiClient : ApiClientBase, IBusinessPartnerAddressApiClient
{
    private const string Base = "api/v1/business-partners/business-partner-addresses";

    public BusinessPartnerAddressApiClient(HttpClient httpClient) : base(httpClient) { }

    public Task<BaseResponse<PaginatedResponse<BusinessPartnerAddressListResponse>>> GetListAsync(int pageNumber, int pageSize, string? search, CancellationToken ct = default)
        => GetAsync<BaseResponse<PaginatedResponse<BusinessPartnerAddressListResponse>>>($"{Base}?pageNumber={pageNumber}&pageSize={pageSize}&search={Uri.EscapeDataString(search ?? string.Empty)}", ct);

    public Task<BaseResponse<BusinessPartnerAddressDetailResponse>> GetByIdAsync(Guid id, CancellationToken ct = default)
        => GetAsync<BaseResponse<BusinessPartnerAddressDetailResponse>>($"{Base}/{id}", ct);

    public Task<BaseResponse<IReadOnlyList<BusinessPartnerAddressLookupResponse>>> GetLookupAsync(string? search, bool activeOnly, CancellationToken ct = default)
        => GetAsync<BaseResponse<IReadOnlyList<BusinessPartnerAddressLookupResponse>>>($"{Base}/lookup?search={Uri.EscapeDataString(search ?? string.Empty)}&activeOnly={activeOnly}", ct);

    public Task<BaseResponse<Guid>> CreateAsync(CreateBusinessPartnerAddressRequest request, CancellationToken ct = default)
        => PostAsync<CreateBusinessPartnerAddressRequest, BaseResponse<Guid>>(Base, request, ct);

    public Task<BaseResponse<bool>> UpdateAsync(Guid id, UpdateBusinessPartnerAddressRequest request, CancellationToken ct = default)
        => PutAsync<UpdateBusinessPartnerAddressRequest, BaseResponse<bool>>($"{Base}/{id}", request, ct);

    public Task<BaseResponse<bool>> DeleteAsync(Guid id, CancellationToken ct = default)
        => DeleteAsync<BaseResponse<bool>>($"{Base}/{id}", ct);
}
