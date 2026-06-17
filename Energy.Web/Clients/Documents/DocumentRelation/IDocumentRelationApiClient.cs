using Energy.Shared.Models.V1.Common.Responses;
using Energy.Shared.Models.V1.Documents.DocumentRelation.Requests;
using Energy.Shared.Models.V1.Documents.DocumentRelation.Responses;
using Energy.Web.Clients.Infrastructure;

namespace Energy.Web.Clients.Documents.DocumentRelation;

/// <summary>DocumentRelation API istemci sözleşmesi.</summary>
public interface IDocumentRelationApiClient
{
    Task<BaseResponse<PaginatedResponse<DocumentRelationListResponse>>> GetListAsync(int pageNumber, int pageSize, string? search, CancellationToken ct = default);
    Task<BaseResponse<DocumentRelationDetailResponse>> GetByIdAsync(Guid id, CancellationToken ct = default);
    Task<BaseResponse<IReadOnlyList<DocumentRelationLookupResponse>>> GetLookupAsync(string? search, bool activeOnly, CancellationToken ct = default);
    Task<BaseResponse<Guid>> CreateAsync(CreateDocumentRelationRequest request, CancellationToken ct = default);
    Task<BaseResponse<bool>> UpdateAsync(Guid id, UpdateDocumentRelationRequest request, CancellationToken ct = default);
    Task<BaseResponse<bool>> DeleteAsync(Guid id, CancellationToken ct = default);
}

/// <summary>DocumentRelation API istemcisi (HttpClientFactory + BaseResponse).</summary>
public sealed class DocumentRelationApiClient : ApiClientBase, IDocumentRelationApiClient
{
    private const string Base = "api/v1/documents/document-relations";

    public DocumentRelationApiClient(HttpClient httpClient) : base(httpClient) { }

    public Task<BaseResponse<PaginatedResponse<DocumentRelationListResponse>>> GetListAsync(int pageNumber, int pageSize, string? search, CancellationToken ct = default)
        => GetAsync<BaseResponse<PaginatedResponse<DocumentRelationListResponse>>>($"{Base}?pageNumber={pageNumber}&pageSize={pageSize}&search={Uri.EscapeDataString(search ?? string.Empty)}", ct);

    public Task<BaseResponse<DocumentRelationDetailResponse>> GetByIdAsync(Guid id, CancellationToken ct = default)
        => GetAsync<BaseResponse<DocumentRelationDetailResponse>>($"{Base}/{id}", ct);

    public Task<BaseResponse<IReadOnlyList<DocumentRelationLookupResponse>>> GetLookupAsync(string? search, bool activeOnly, CancellationToken ct = default)
        => GetAsync<BaseResponse<IReadOnlyList<DocumentRelationLookupResponse>>>($"{Base}/lookup?search={Uri.EscapeDataString(search ?? string.Empty)}&activeOnly={activeOnly}", ct);

    public Task<BaseResponse<Guid>> CreateAsync(CreateDocumentRelationRequest request, CancellationToken ct = default)
        => PostAsync<CreateDocumentRelationRequest, BaseResponse<Guid>>(Base, request, ct);

    public Task<BaseResponse<bool>> UpdateAsync(Guid id, UpdateDocumentRelationRequest request, CancellationToken ct = default)
        => PutAsync<UpdateDocumentRelationRequest, BaseResponse<bool>>($"{Base}/{id}", request, ct);

    public Task<BaseResponse<bool>> DeleteAsync(Guid id, CancellationToken ct = default)
        => DeleteAsync<BaseResponse<bool>>($"{Base}/{id}", ct);
}
