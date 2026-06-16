using Energy.Shared.Models.V1.Common.Responses;
using Energy.Shared.Models.V1.BusinessPartners.BusinessPartner.Requests;
using Energy.Shared.Models.V1.BusinessPartners.BusinessPartner.Responses;
using Energy.Web.Clients.Infrastructure;

namespace Energy.Web.Clients.Modules.BusinessPartners.BusinessPartner;

/// <summary>BusinessPartner API istemci sözleşmesi.</summary>
public interface IBusinessPartnerApiClient
{
    Task<BaseResponse<PaginatedResponse<BusinessPartnerListResponse>>> GetListAsync(int pageNumber, int pageSize, string? search, CancellationToken ct = default);
    Task<BaseResponse<BusinessPartnerDetailResponse>> GetByIdAsync(Guid id, CancellationToken ct = default);
    Task<BaseResponse<IReadOnlyList<BusinessPartnerLookupResponse>>> GetLookupAsync(string? search, bool activeOnly, CancellationToken ct = default);
    Task<BaseResponse<Guid>> CreateAsync(CreateBusinessPartnerRequest request, CancellationToken ct = default);
    Task<BaseResponse<bool>> UpdateAsync(Guid id, UpdateBusinessPartnerRequest request, CancellationToken ct = default);
    Task<BaseResponse<bool>> DeleteAsync(Guid id, CancellationToken ct = default);
}

/// <summary>BusinessPartner API istemcisi (HttpClientFactory + BaseResponse).</summary>
public sealed class BusinessPartnerApiClient : ApiClientBase, IBusinessPartnerApiClient
{
    private const string Base = "api/v1/business-partners/business-partners";

    public BusinessPartnerApiClient(HttpClient httpClient) : base(httpClient) { }

    public Task<BaseResponse<PaginatedResponse<BusinessPartnerListResponse>>> GetListAsync(int pageNumber, int pageSize, string? search, CancellationToken ct = default)
        => GetAsync<BaseResponse<PaginatedResponse<BusinessPartnerListResponse>>>($"{Base}?pageNumber={pageNumber}&pageSize={pageSize}&search={Uri.EscapeDataString(search ?? string.Empty)}", ct);

    public Task<BaseResponse<BusinessPartnerDetailResponse>> GetByIdAsync(Guid id, CancellationToken ct = default)
        => GetAsync<BaseResponse<BusinessPartnerDetailResponse>>($"{Base}/{id}", ct);

    public Task<BaseResponse<IReadOnlyList<BusinessPartnerLookupResponse>>> GetLookupAsync(string? search, bool activeOnly, CancellationToken ct = default)
        => GetAsync<BaseResponse<IReadOnlyList<BusinessPartnerLookupResponse>>>($"{Base}/lookup?search={Uri.EscapeDataString(search ?? string.Empty)}&activeOnly={activeOnly}", ct);

    public Task<BaseResponse<Guid>> CreateAsync(CreateBusinessPartnerRequest request, CancellationToken ct = default)
        => PostAsync<CreateBusinessPartnerRequest, BaseResponse<Guid>>(Base, request, ct);

    public Task<BaseResponse<bool>> UpdateAsync(Guid id, UpdateBusinessPartnerRequest request, CancellationToken ct = default)
        => PutAsync<UpdateBusinessPartnerRequest, BaseResponse<bool>>($"{Base}/{id}", request, ct);

    public Task<BaseResponse<bool>> DeleteAsync(Guid id, CancellationToken ct = default)
        => DeleteAsync<BaseResponse<bool>>($"{Base}/{id}", ct);
}
