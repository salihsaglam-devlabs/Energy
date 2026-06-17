using Energy.Shared.Models.V1.Common.Responses;
using Energy.Shared.Models.V1.BusinessPartners.BusinessPartnerContact.Requests;
using Energy.Shared.Models.V1.BusinessPartners.BusinessPartnerContact.Responses;
using Energy.Web.Clients.Infrastructure;

namespace Energy.Web.Clients.BusinessPartners.BusinessPartnerContact;

/// <summary>BusinessPartnerContact API istemci sözleşmesi.</summary>
public interface IBusinessPartnerContactApiClient
{
    Task<BaseResponse<PaginatedResponse<BusinessPartnerContactListResponse>>> GetListAsync(int pageNumber, int pageSize, string? search, CancellationToken ct = default);
    Task<BaseResponse<BusinessPartnerContactDetailResponse>> GetByIdAsync(Guid id, CancellationToken ct = default);
    Task<BaseResponse<IReadOnlyList<BusinessPartnerContactLookupResponse>>> GetLookupAsync(string? search, bool activeOnly, CancellationToken ct = default);
    Task<BaseResponse<Guid>> CreateAsync(CreateBusinessPartnerContactRequest request, CancellationToken ct = default);
    Task<BaseResponse<bool>> UpdateAsync(Guid id, UpdateBusinessPartnerContactRequest request, CancellationToken ct = default);
    Task<BaseResponse<bool>> DeleteAsync(Guid id, CancellationToken ct = default);
}

/// <summary>BusinessPartnerContact API istemcisi (HttpClientFactory + BaseResponse).</summary>
public sealed class BusinessPartnerContactApiClient : ApiClientBase, IBusinessPartnerContactApiClient
{
    private const string Base = "api/v1/business-partners/business-partner-contacts";

    public BusinessPartnerContactApiClient(HttpClient httpClient) : base(httpClient) { }

    public Task<BaseResponse<PaginatedResponse<BusinessPartnerContactListResponse>>> GetListAsync(int pageNumber, int pageSize, string? search, CancellationToken ct = default)
        => GetAsync<BaseResponse<PaginatedResponse<BusinessPartnerContactListResponse>>>($"{Base}?pageNumber={pageNumber}&pageSize={pageSize}&search={Uri.EscapeDataString(search ?? string.Empty)}", ct);

    public Task<BaseResponse<BusinessPartnerContactDetailResponse>> GetByIdAsync(Guid id, CancellationToken ct = default)
        => GetAsync<BaseResponse<BusinessPartnerContactDetailResponse>>($"{Base}/{id}", ct);

    public Task<BaseResponse<IReadOnlyList<BusinessPartnerContactLookupResponse>>> GetLookupAsync(string? search, bool activeOnly, CancellationToken ct = default)
        => GetAsync<BaseResponse<IReadOnlyList<BusinessPartnerContactLookupResponse>>>($"{Base}/lookup?search={Uri.EscapeDataString(search ?? string.Empty)}&activeOnly={activeOnly}", ct);

    public Task<BaseResponse<Guid>> CreateAsync(CreateBusinessPartnerContactRequest request, CancellationToken ct = default)
        => PostAsync<CreateBusinessPartnerContactRequest, BaseResponse<Guid>>(Base, request, ct);

    public Task<BaseResponse<bool>> UpdateAsync(Guid id, UpdateBusinessPartnerContactRequest request, CancellationToken ct = default)
        => PutAsync<UpdateBusinessPartnerContactRequest, BaseResponse<bool>>($"{Base}/{id}", request, ct);

    public Task<BaseResponse<bool>> DeleteAsync(Guid id, CancellationToken ct = default)
        => DeleteAsync<BaseResponse<bool>>($"{Base}/{id}", ct);
}
