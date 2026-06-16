using Energy.Shared.Models.V1.Common.Responses;
using Energy.Shared.Models.V1.Core.SequenceDefinition.Requests;
using Energy.Shared.Models.V1.Core.SequenceDefinition.Responses;
using Energy.Web.Clients.Infrastructure;

namespace Energy.Web.Clients.Core.SequenceDefinition;

/// <summary>SequenceDefinition API istemci sözleşmesi.</summary>
public interface ISequenceDefinitionApiClient
{
    Task<BaseResponse<PaginatedResponse<SequenceDefinitionListResponse>>> GetListAsync(int pageNumber, int pageSize, string? search, CancellationToken ct = default);
    Task<BaseResponse<SequenceDefinitionDetailResponse>> GetByIdAsync(Guid id, CancellationToken ct = default);
    Task<BaseResponse<IReadOnlyList<SequenceDefinitionLookupResponse>>> GetLookupAsync(string? search, bool activeOnly, CancellationToken ct = default);
    Task<BaseResponse<Guid>> CreateAsync(CreateSequenceDefinitionRequest request, CancellationToken ct = default);
    Task<BaseResponse<bool>> UpdateAsync(Guid id, UpdateSequenceDefinitionRequest request, CancellationToken ct = default);
    Task<BaseResponse<bool>> DeleteAsync(Guid id, CancellationToken ct = default);
}

/// <summary>SequenceDefinition API istemcisi (HttpClientFactory + BaseResponse).</summary>
public sealed class SequenceDefinitionApiClient : ApiClientBase, ISequenceDefinitionApiClient
{
    private const string Base = "api/v1/core/sequence-definitions";

    public SequenceDefinitionApiClient(HttpClient httpClient) : base(httpClient) { }

    public Task<BaseResponse<PaginatedResponse<SequenceDefinitionListResponse>>> GetListAsync(int pageNumber, int pageSize, string? search, CancellationToken ct = default)
        => GetAsync<BaseResponse<PaginatedResponse<SequenceDefinitionListResponse>>>($"{Base}?pageNumber={pageNumber}&pageSize={pageSize}&search={Uri.EscapeDataString(search ?? string.Empty)}", ct);

    public Task<BaseResponse<SequenceDefinitionDetailResponse>> GetByIdAsync(Guid id, CancellationToken ct = default)
        => GetAsync<BaseResponse<SequenceDefinitionDetailResponse>>($"{Base}/{id}", ct);

    public Task<BaseResponse<IReadOnlyList<SequenceDefinitionLookupResponse>>> GetLookupAsync(string? search, bool activeOnly, CancellationToken ct = default)
        => GetAsync<BaseResponse<IReadOnlyList<SequenceDefinitionLookupResponse>>>($"{Base}/lookup?search={Uri.EscapeDataString(search ?? string.Empty)}&activeOnly={activeOnly}", ct);

    public Task<BaseResponse<Guid>> CreateAsync(CreateSequenceDefinitionRequest request, CancellationToken ct = default)
        => PostAsync<CreateSequenceDefinitionRequest, BaseResponse<Guid>>(Base, request, ct);

    public Task<BaseResponse<bool>> UpdateAsync(Guid id, UpdateSequenceDefinitionRequest request, CancellationToken ct = default)
        => PutAsync<UpdateSequenceDefinitionRequest, BaseResponse<bool>>($"{Base}/{id}", request, ct);

    public Task<BaseResponse<bool>> DeleteAsync(Guid id, CancellationToken ct = default)
        => DeleteAsync<BaseResponse<bool>>($"{Base}/{id}", ct);
}
