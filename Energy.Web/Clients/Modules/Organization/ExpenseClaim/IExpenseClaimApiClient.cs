using Energy.Shared.Models.V1.Common.Responses;
using Energy.Shared.Models.V1.Organization.ExpenseClaim.Requests;
using Energy.Shared.Models.V1.Organization.ExpenseClaim.Responses;
using Energy.Web.Clients.Infrastructure;

namespace Energy.Web.Clients.Modules.Organization.ExpenseClaim;

/// <summary>ExpenseClaim API istemci sözleşmesi.</summary>
public interface IExpenseClaimApiClient
{
    Task<BaseResponse<PaginatedResponse<ExpenseClaimListResponse>>> GetListAsync(int pageNumber, int pageSize, string? search, CancellationToken ct = default);
    Task<BaseResponse<ExpenseClaimDetailResponse>> GetByIdAsync(Guid id, CancellationToken ct = default);
    Task<BaseResponse<IReadOnlyList<ExpenseClaimLookupResponse>>> GetLookupAsync(string? search, bool activeOnly, CancellationToken ct = default);
    Task<BaseResponse<Guid>> CreateAsync(CreateExpenseClaimRequest request, CancellationToken ct = default);
    Task<BaseResponse<bool>> UpdateAsync(Guid id, UpdateExpenseClaimRequest request, CancellationToken ct = default);
    Task<BaseResponse<bool>> DeleteAsync(Guid id, CancellationToken ct = default);
}

/// <summary>ExpenseClaim API istemcisi (HttpClientFactory + BaseResponse).</summary>
public sealed class ExpenseClaimApiClient : ApiClientBase, IExpenseClaimApiClient
{
    private const string Base = "api/v1/organization/expense-claims";

    public ExpenseClaimApiClient(HttpClient httpClient) : base(httpClient) { }

    public Task<BaseResponse<PaginatedResponse<ExpenseClaimListResponse>>> GetListAsync(int pageNumber, int pageSize, string? search, CancellationToken ct = default)
        => GetAsync<BaseResponse<PaginatedResponse<ExpenseClaimListResponse>>>($"{Base}?pageNumber={pageNumber}&pageSize={pageSize}&search={Uri.EscapeDataString(search ?? string.Empty)}", ct);

    public Task<BaseResponse<ExpenseClaimDetailResponse>> GetByIdAsync(Guid id, CancellationToken ct = default)
        => GetAsync<BaseResponse<ExpenseClaimDetailResponse>>($"{Base}/{id}", ct);

    public Task<BaseResponse<IReadOnlyList<ExpenseClaimLookupResponse>>> GetLookupAsync(string? search, bool activeOnly, CancellationToken ct = default)
        => GetAsync<BaseResponse<IReadOnlyList<ExpenseClaimLookupResponse>>>($"{Base}/lookup?search={Uri.EscapeDataString(search ?? string.Empty)}&activeOnly={activeOnly}", ct);

    public Task<BaseResponse<Guid>> CreateAsync(CreateExpenseClaimRequest request, CancellationToken ct = default)
        => PostAsync<CreateExpenseClaimRequest, BaseResponse<Guid>>(Base, request, ct);

    public Task<BaseResponse<bool>> UpdateAsync(Guid id, UpdateExpenseClaimRequest request, CancellationToken ct = default)
        => PutAsync<UpdateExpenseClaimRequest, BaseResponse<bool>>($"{Base}/{id}", request, ct);

    public Task<BaseResponse<bool>> DeleteAsync(Guid id, CancellationToken ct = default)
        => DeleteAsync<BaseResponse<bool>>($"{Base}/{id}", ct);
}
