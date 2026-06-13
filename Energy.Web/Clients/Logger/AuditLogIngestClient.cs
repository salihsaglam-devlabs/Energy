using Energy.Shared.Models.V1.Logger.Requests;
using Energy.Web.Clients.Infrastructure;

namespace Energy.Web.Clients.Logger;

/// <summary>Web katmanı denetim kayıtlarını API'nin denetim havuzuna gönderen istemci sözleşmesi.</summary>
public interface IAuditLogIngestClient
{
    /// <summary>Verilen denetim kaydını API'ye gönderir ve çağrının başarılı olup olmadığını döndürür.</summary>
    Task<bool> IngestAsync(CreateAuditLogRequest request, CancellationToken ct = default);
}

/// <summary>
/// Web katmanı istek denetim kayıtlarını API'nin tek denetim havuzuna gönderir; böylece
/// Web katmanının işlediği istekler de API istekleriyle birlikte kaydedilir. Yanıt zarfı
/// kasıtlı olarak yok sayılır; böylece geçici bir API hatası, istek hattında asla bir
/// seri durumdan çıkarma hatası olarak yüzeye çıkmaz.
/// </summary>
public sealed class AuditLogIngestClient : ApiClientBase, IAuditLogIngestClient
{
    /// <summary>HTTP istemcisi ile istemciyi başlatır.</summary>
    public AuditLogIngestClient(HttpClient httpClient) : base(httpClient) { }

    /// <inheritdoc />
    public Task<bool> IngestAsync(CreateAuditLogRequest request, CancellationToken ct = default)
        => PostIgnoreResultAsync(ApiRoutes.Logs.Base, request, ct);
}

