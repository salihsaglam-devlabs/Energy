using Energy.Shared.Models.V1.Common.Responses;
using Energy.Shared.Models.V1.BusinessPartners.BusinessPartnerBankAccount.Requests;
using Energy.Shared.Models.V1.BusinessPartners.BusinessPartnerBankAccount.Responses;
using Energy.Web.Clients.Infrastructure;

namespace Energy.Web.Clients.BusinessPartners.BusinessPartnerBankAccount;

/// <summary>BusinessPartnerBankAccount API istemci sözleşmesi.</summary>
public interface IBusinessPartnerBankAccountApiClient
{
    Task<BaseResponse<PaginatedResponse<BusinessPartnerBankAccountListResponse>>> GetListAsync(int pageNumber, int pageSize, string? search, CancellationToken ct = default);
    Task<BaseResponse<BusinessPartnerBankAccountDetailResponse>> GetByIdAsync(Guid id, CancellationToken ct = default);
    Task<BaseResponse<IReadOnlyList<BusinessPartnerBankAccountLookupResponse>>> GetLookupAsync(string? search, bool activeOnly, CancellationToken ct = default);
    Task<BaseResponse<Guid>> CreateAsync(CreateBusinessPartnerBankAccountRequest request, CancellationToken ct = default);
    Task<BaseResponse<bool>> UpdateAsync(Guid id, UpdateBusinessPartnerBankAccountRequest request, CancellationToken ct = default);
    Task<BaseResponse<bool>> DeleteAsync(Guid id, CancellationToken ct = default);
}

/// <summary>BusinessPartnerBankAccount API istemcisi (HttpClientFactory + BaseResponse).</summary>
public sealed class BusinessPartnerBankAccountApiClient : ApiClientBase, IBusinessPartnerBankAccountApiClient
{
    private const string Base = "api/v1/business-partners/business-partner-bank-accounts";

    public BusinessPartnerBankAccountApiClient(HttpClient httpClient) : base(httpClient) { }

    public Task<BaseResponse<PaginatedResponse<BusinessPartnerBankAccountListResponse>>> GetListAsync(int pageNumber, int pageSize, string? search, CancellationToken ct = default)
        => GetAsync<BaseResponse<PaginatedResponse<BusinessPartnerBankAccountListResponse>>>($"{Base}?pageNumber={pageNumber}&pageSize={pageSize}&search={Uri.EscapeDataString(search ?? string.Empty)}", ct);

    public Task<BaseResponse<BusinessPartnerBankAccountDetailResponse>> GetByIdAsync(Guid id, CancellationToken ct = default)
        => GetAsync<BaseResponse<BusinessPartnerBankAccountDetailResponse>>($"{Base}/{id}", ct);

    public Task<BaseResponse<IReadOnlyList<BusinessPartnerBankAccountLookupResponse>>> GetLookupAsync(string? search, bool activeOnly, CancellationToken ct = default)
        => GetAsync<BaseResponse<IReadOnlyList<BusinessPartnerBankAccountLookupResponse>>>($"{Base}/lookup?search={Uri.EscapeDataString(search ?? string.Empty)}&activeOnly={activeOnly}", ct);

    public Task<BaseResponse<Guid>> CreateAsync(CreateBusinessPartnerBankAccountRequest request, CancellationToken ct = default)
        => PostAsync<CreateBusinessPartnerBankAccountRequest, BaseResponse<Guid>>(Base, request, ct);

    public Task<BaseResponse<bool>> UpdateAsync(Guid id, UpdateBusinessPartnerBankAccountRequest request, CancellationToken ct = default)
        => PutAsync<UpdateBusinessPartnerBankAccountRequest, BaseResponse<bool>>($"{Base}/{id}", request, ct);

    public Task<BaseResponse<bool>> DeleteAsync(Guid id, CancellationToken ct = default)
        => DeleteAsync<BaseResponse<bool>>($"{Base}/{id}", ct);
}
