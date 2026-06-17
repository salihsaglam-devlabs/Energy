using Energy.Shared.Models.V1.Common.Responses;
using Energy.Shared.Models.V1.Core.Company.Requests;
using Energy.Shared.Models.V1.Core.Company.Responses;
using Energy.Web.Clients.Infrastructure;

namespace Energy.Web.Clients.Core.Company;

/// <summary>Company API istemci sözleşmesi.</summary>
public interface ICompanyApiClient
{
    Task<BaseResponse<PaginatedResponse<CompanyListResponse>>> GetListAsync(int pageNumber, int pageSize, string? search, CancellationToken ct = default);
    Task<BaseResponse<CompanyDetailResponse>> GetByIdAsync(Guid id, CancellationToken ct = default);
    Task<BaseResponse<IReadOnlyList<CompanyLookupResponse>>> GetLookupAsync(string? search, bool activeOnly, CancellationToken ct = default);
    Task<BaseResponse<Guid>> CreateAsync(CreateCompanyRequest request, CancellationToken ct = default);
    Task<BaseResponse<bool>> UpdateAsync(Guid id, UpdateCompanyRequest request, CancellationToken ct = default);
    Task<BaseResponse<bool>> DeleteAsync(Guid id, CancellationToken ct = default);
}

/// <summary>Company API istemcisi (HttpClientFactory + BaseResponse).</summary>
public sealed class CompanyApiClient : ApiClientBase, ICompanyApiClient
{
    private const string Base = "api/v1/core/companies";

    public CompanyApiClient(HttpClient httpClient) : base(httpClient) { }

    public Task<BaseResponse<PaginatedResponse<CompanyListResponse>>> GetListAsync(int pageNumber, int pageSize, string? search, CancellationToken ct = default)
        => GetAsync<BaseResponse<PaginatedResponse<CompanyListResponse>>>($"{Base}?pageNumber={pageNumber}&pageSize={pageSize}&search={Uri.EscapeDataString(search ?? string.Empty)}", ct);

    public Task<BaseResponse<CompanyDetailResponse>> GetByIdAsync(Guid id, CancellationToken ct = default)
        => GetAsync<BaseResponse<CompanyDetailResponse>>($"{Base}/{id}", ct);

    public Task<BaseResponse<IReadOnlyList<CompanyLookupResponse>>> GetLookupAsync(string? search, bool activeOnly, CancellationToken ct = default)
        => GetAsync<BaseResponse<IReadOnlyList<CompanyLookupResponse>>>($"{Base}/lookup?search={Uri.EscapeDataString(search ?? string.Empty)}&activeOnly={activeOnly}", ct);

    public Task<BaseResponse<Guid>> CreateAsync(CreateCompanyRequest request, CancellationToken ct = default)
        => PostAsync<CreateCompanyRequest, BaseResponse<Guid>>(Base, request, ct);

    public Task<BaseResponse<bool>> UpdateAsync(Guid id, UpdateCompanyRequest request, CancellationToken ct = default)
        => PutAsync<UpdateCompanyRequest, BaseResponse<bool>>($"{Base}/{id}", request, ct);

    public Task<BaseResponse<bool>> DeleteAsync(Guid id, CancellationToken ct = default)
        => DeleteAsync<BaseResponse<bool>>($"{Base}/{id}", ct);
}
