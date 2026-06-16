using Energy.Shared.Models.V1.Common.Responses;
using Energy.Shared.Models.V1.Core.AuditLog.Requests;
using Energy.Shared.Models.V1.Core.AuditLog.Responses;
using Energy.Web.Clients.Infrastructure;

namespace Energy.Web.Clients.Core.AuditLog;

/// <summary>AuditLog API istemci sözleşmesi.</summary>
public interface IAuditLogApiClient
{
    Task<BaseResponse<PaginatedResponse<AuditLogListResponse>>> GetListAsync(int pageNumber, int pageSize, string? search, CancellationToken ct = default);
    Task<BaseResponse<AuditLogDetailResponse>> GetByIdAsync(Guid id, CancellationToken ct = default);
    Task<BaseResponse<IReadOnlyList<AuditLogLookupResponse>>> GetLookupAsync(string? search, bool activeOnly, CancellationToken ct = default);
    Task<BaseResponse<Guid>> CreateAsync(CreateAuditLogRequest request, CancellationToken ct = default);
    Task<BaseResponse<bool>> UpdateAsync(Guid id, UpdateAuditLogRequest request, CancellationToken ct = default);
    Task<BaseResponse<bool>> DeleteAsync(Guid id, CancellationToken ct = default);
}

/// <summary>AuditLog API istemcisi (HttpClientFactory + BaseResponse).</summary>
public sealed class AuditLogApiClient : ApiClientBase, IAuditLogApiClient
{
    private const string Base = "api/v1/core/audit-logs";

    public AuditLogApiClient(HttpClient httpClient) : base(httpClient) { }

    public Task<BaseResponse<PaginatedResponse<AuditLogListResponse>>> GetListAsync(int pageNumber, int pageSize, string? search, CancellationToken ct = default)
        => GetAsync<BaseResponse<PaginatedResponse<AuditLogListResponse>>>($"{Base}?pageNumber={pageNumber}&pageSize={pageSize}&search={Uri.EscapeDataString(search ?? string.Empty)}", ct);

    public Task<BaseResponse<AuditLogDetailResponse>> GetByIdAsync(Guid id, CancellationToken ct = default)
        => GetAsync<BaseResponse<AuditLogDetailResponse>>($"{Base}/{id}", ct);

    public Task<BaseResponse<IReadOnlyList<AuditLogLookupResponse>>> GetLookupAsync(string? search, bool activeOnly, CancellationToken ct = default)
        => GetAsync<BaseResponse<IReadOnlyList<AuditLogLookupResponse>>>($"{Base}/lookup?search={Uri.EscapeDataString(search ?? string.Empty)}&activeOnly={activeOnly}", ct);

    public Task<BaseResponse<Guid>> CreateAsync(CreateAuditLogRequest request, CancellationToken ct = default)
        => PostAsync<CreateAuditLogRequest, BaseResponse<Guid>>(Base, request, ct);

    public Task<BaseResponse<bool>> UpdateAsync(Guid id, UpdateAuditLogRequest request, CancellationToken ct = default)
        => PutAsync<UpdateAuditLogRequest, BaseResponse<bool>>($"{Base}/{id}", request, ct);

    public Task<BaseResponse<bool>> DeleteAsync(Guid id, CancellationToken ct = default)
        => DeleteAsync<BaseResponse<bool>>($"{Base}/{id}", ct);
}
