using Energy.Shared.Models.V1.Common.Responses;
using Energy.Shared.Models.V1.FieldOperations.ProgressEntry.Requests;
using Energy.Shared.Models.V1.FieldOperations.ProgressEntry.Responses;
using Energy.Web.Clients.Infrastructure;

namespace Energy.Web.Clients.FieldOperations.ProgressEntry;

/// <summary>ProgressEntry API istemci sözleşmesi.</summary>
public interface IProgressEntryApiClient
{
    Task<BaseResponse<PaginatedResponse<ProgressEntryListResponse>>> GetListAsync(int pageNumber, int pageSize, string? search, CancellationToken ct = default);
    Task<BaseResponse<ProgressEntryDetailResponse>> GetByIdAsync(Guid id, CancellationToken ct = default);
    Task<BaseResponse<IReadOnlyList<ProgressEntryLookupResponse>>> GetLookupAsync(string? search, bool activeOnly, CancellationToken ct = default);
    Task<BaseResponse<Guid>> CreateAsync(CreateProgressEntryRequest request, CancellationToken ct = default);
    Task<BaseResponse<bool>> UpdateAsync(Guid id, UpdateProgressEntryRequest request, CancellationToken ct = default);
    Task<BaseResponse<bool>> DeleteAsync(Guid id, CancellationToken ct = default);
}

/// <summary>ProgressEntry API istemcisi (HttpClientFactory + BaseResponse).</summary>
public sealed class ProgressEntryApiClient : ApiClientBase, IProgressEntryApiClient
{
    private const string Base = "api/v1/field-operations/progress-entries";

    public ProgressEntryApiClient(HttpClient httpClient) : base(httpClient) { }

    public Task<BaseResponse<PaginatedResponse<ProgressEntryListResponse>>> GetListAsync(int pageNumber, int pageSize, string? search, CancellationToken ct = default)
        => GetAsync<BaseResponse<PaginatedResponse<ProgressEntryListResponse>>>($"{Base}?pageNumber={pageNumber}&pageSize={pageSize}&search={Uri.EscapeDataString(search ?? string.Empty)}", ct);

    public Task<BaseResponse<ProgressEntryDetailResponse>> GetByIdAsync(Guid id, CancellationToken ct = default)
        => GetAsync<BaseResponse<ProgressEntryDetailResponse>>($"{Base}/{id}", ct);

    public Task<BaseResponse<IReadOnlyList<ProgressEntryLookupResponse>>> GetLookupAsync(string? search, bool activeOnly, CancellationToken ct = default)
        => GetAsync<BaseResponse<IReadOnlyList<ProgressEntryLookupResponse>>>($"{Base}/lookup?search={Uri.EscapeDataString(search ?? string.Empty)}&activeOnly={activeOnly}", ct);

    public Task<BaseResponse<Guid>> CreateAsync(CreateProgressEntryRequest request, CancellationToken ct = default)
        => PostAsync<CreateProgressEntryRequest, BaseResponse<Guid>>(Base, request, ct);

    public Task<BaseResponse<bool>> UpdateAsync(Guid id, UpdateProgressEntryRequest request, CancellationToken ct = default)
        => PutAsync<UpdateProgressEntryRequest, BaseResponse<bool>>($"{Base}/{id}", request, ct);

    public Task<BaseResponse<bool>> DeleteAsync(Guid id, CancellationToken ct = default)
        => DeleteAsync<BaseResponse<bool>>($"{Base}/{id}", ct);
}
