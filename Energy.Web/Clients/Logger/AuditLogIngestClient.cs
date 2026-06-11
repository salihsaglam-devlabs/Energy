using Energy.Shared.Models.V1.Logger.Requests;
using Energy.Web.Clients.Infrastructure;

namespace Energy.Web.Clients.Logger;

public interface IAuditLogIngestClient
{
    Task<bool> IngestAsync(CreateAuditLogRequest request, CancellationToken ct = default);
}

/// <summary>
/// Posts Web-layer request audit entries to the API's single audit sink so that
/// requests handled by the Web tier are recorded alongside API requests. The
/// response envelope is intentionally ignored so a transient API failure can
/// never surface as a deserialization error in the request pipeline.
/// </summary>
public sealed class AuditLogIngestClient : ApiClientBase, IAuditLogIngestClient
{
    public AuditLogIngestClient(HttpClient httpClient) : base(httpClient) { }

    public Task<bool> IngestAsync(CreateAuditLogRequest request, CancellationToken ct = default)
        => PostIgnoreResultAsync(ApiRoutes.Logs.Base, request, ct);
}

