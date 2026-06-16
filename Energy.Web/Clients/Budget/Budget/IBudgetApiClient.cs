using Energy.Shared.Models.V1.Common.Responses;
using Energy.Shared.Models.V1.Budget.Budget.Requests;
using Energy.Shared.Models.V1.Budget.Budget.Responses;
using Energy.Web.Clients.Infrastructure;

namespace Energy.Web.Clients.Budget.Budget;

/// <summary>Budget API istemci sözleşmesi.</summary>
public interface IBudgetApiClient
{
    Task<BaseResponse<PaginatedResponse<BudgetListResponse>>> GetListAsync(int pageNumber, int pageSize, string? search, CancellationToken ct = default);
    Task<BaseResponse<BudgetDetailResponse>> GetByIdAsync(Guid id, CancellationToken ct = default);
    Task<BaseResponse<IReadOnlyList<BudgetLookupResponse>>> GetLookupAsync(string? search, bool activeOnly, CancellationToken ct = default);
    Task<BaseResponse<Guid>> CreateAsync(CreateBudgetRequest request, CancellationToken ct = default);
    Task<BaseResponse<bool>> UpdateAsync(Guid id, UpdateBudgetRequest request, CancellationToken ct = default);
    Task<BaseResponse<bool>> DeleteAsync(Guid id, CancellationToken ct = default);
}

/// <summary>Budget API istemcisi (HttpClientFactory + BaseResponse).</summary>
public sealed class BudgetApiClient : ApiClientBase, IBudgetApiClient
{
    private const string Base = "api/v1/budget/budgets";

    public BudgetApiClient(HttpClient httpClient) : base(httpClient) { }

    public Task<BaseResponse<PaginatedResponse<BudgetListResponse>>> GetListAsync(int pageNumber, int pageSize, string? search, CancellationToken ct = default)
        => GetAsync<BaseResponse<PaginatedResponse<BudgetListResponse>>>($"{Base}?pageNumber={pageNumber}&pageSize={pageSize}&search={Uri.EscapeDataString(search ?? string.Empty)}", ct);

    public Task<BaseResponse<BudgetDetailResponse>> GetByIdAsync(Guid id, CancellationToken ct = default)
        => GetAsync<BaseResponse<BudgetDetailResponse>>($"{Base}/{id}", ct);

    public Task<BaseResponse<IReadOnlyList<BudgetLookupResponse>>> GetLookupAsync(string? search, bool activeOnly, CancellationToken ct = default)
        => GetAsync<BaseResponse<IReadOnlyList<BudgetLookupResponse>>>($"{Base}/lookup?search={Uri.EscapeDataString(search ?? string.Empty)}&activeOnly={activeOnly}", ct);

    public Task<BaseResponse<Guid>> CreateAsync(CreateBudgetRequest request, CancellationToken ct = default)
        => PostAsync<CreateBudgetRequest, BaseResponse<Guid>>(Base, request, ct);

    public Task<BaseResponse<bool>> UpdateAsync(Guid id, UpdateBudgetRequest request, CancellationToken ct = default)
        => PutAsync<UpdateBudgetRequest, BaseResponse<bool>>($"{Base}/{id}", request, ct);

    public Task<BaseResponse<bool>> DeleteAsync(Guid id, CancellationToken ct = default)
        => DeleteAsync<BaseResponse<bool>>($"{Base}/{id}", ct);
}
