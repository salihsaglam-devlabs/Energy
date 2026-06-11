using Energy.Shared.Models.V1.Common.Requests;
using Energy.Shared.Models.V1.Common.Responses;
using Energy.Shared.Models.V1.Logger.Responses;
using Energy.Web.Clients.Infrastructure;

namespace Energy.Web.Clients.Logger;

public interface IAuditLogQueryClient
{
    Task<BaseResponse<PaginatedResponse<AuditLogResponse>>> GetAllAsync(PaginatedRequest request, CancellationToken ct = default);
    Task<BaseResponse<AuditLogResponse>> GetByIdAsync(long id, CancellationToken ct = default);
}

/// <summary>
/// Reads audit log entries from the API through the authenticated pipeline so
/// the Logs screen (like every other admin screen) is proxied server-side and
/// carries the signed-in user's bearer token — never called directly from the
/// browser, which has no API credentials.
/// </summary>
public sealed class AuditLogQueryClient : ApiClientBase, IAuditLogQueryClient
{
    public AuditLogQueryClient(HttpClient httpClient) : base(httpClient) { }

    public Task<BaseResponse<PaginatedResponse<AuditLogResponse>>> GetAllAsync(PaginatedRequest request, CancellationToken ct = default)
        => GetAsync<BaseResponse<PaginatedResponse<AuditLogResponse>>>(
            $"{ApiRoutes.Logs.Base}?pageNumber={request.PageNumber}&pageSize={request.PageSize}", ct);

    public Task<BaseResponse<AuditLogResponse>> GetByIdAsync(long id, CancellationToken ct = default)
        => GetAsync<BaseResponse<AuditLogResponse>>(ApiRoutes.Logs.ById(id), ct);
}

