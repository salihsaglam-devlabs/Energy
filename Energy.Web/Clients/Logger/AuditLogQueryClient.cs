using Energy.Shared.Models.V1.Common.Requests;
using Energy.Shared.Models.V1.Common.Responses;
using Energy.Shared.Models.V1.Logger.Responses;
using Energy.Web.Clients.Infrastructure;

namespace Energy.Web.Clients.Logger;

/// <summary>Denetim günlüklerini API'den okuyan istemci sözleşmesi.</summary>
public interface IAuditLogQueryClient
{
    /// <summary>Denetim günlüklerini sayfalanmış olarak getirir.</summary>
    Task<BaseResponse<PaginatedResponse<AuditLogResponse>>> GetAllAsync(PaginatedRequest request, CancellationToken ct = default);
    /// <summary>Tek bir denetim günlüğünü kimliğine göre getirir.</summary>
    Task<BaseResponse<AuditLogResponse>> GetByIdAsync(long id, CancellationToken ct = default);
}

/// <summary>
/// Denetim günlüğü kayıtlarını kimliği doğrulanmış hat üzerinden API'den okur; böylece
/// Günlükler ekranı (diğer her yönetim ekranı gibi) sunucu tarafında vekillenir (proxy)
/// ve oturum açmış kullanıcının bearer jetonunu taşır — API kimlik bilgisi olmayan
/// tarayıcıdan asla doğrudan çağrılmaz.
/// </summary>
public sealed class AuditLogQueryClient : ApiClientBase, IAuditLogQueryClient
{
    /// <summary>HTTP istemcisi ile istemciyi başlatır.</summary>
    public AuditLogQueryClient(HttpClient httpClient) : base(httpClient) { }

    /// <inheritdoc />
    public Task<BaseResponse<PaginatedResponse<AuditLogResponse>>> GetAllAsync(PaginatedRequest request, CancellationToken ct = default)
        => GetAsync<BaseResponse<PaginatedResponse<AuditLogResponse>>>(
            $"{ApiRoutes.Logs.Base}?pageNumber={request.PageNumber}&pageSize={request.PageSize}", ct);

    /// <inheritdoc />
    public Task<BaseResponse<AuditLogResponse>> GetByIdAsync(long id, CancellationToken ct = default)
        => GetAsync<BaseResponse<AuditLogResponse>>(ApiRoutes.Logs.ById(id), ct);
}

