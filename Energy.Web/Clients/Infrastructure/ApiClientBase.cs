using System.Net.Http.Json;
using Energy.Localization;

namespace Energy.Web.Clients.Infrastructure;

/// <summary>
/// JSON HTTP yardımcılarını sunan ince temel sınıf. İstemci kimliği, makine adı ve
/// kimlik doğrulama başlıkları özel DelegatingHandler'lar tarafından eklenir; bu yüzden
/// bu tür istek başlıklarına bilinçli olarak dokunmaz.
/// </summary>
public abstract class ApiClientBase
{
    private readonly HttpClient _httpClient;

    /// <summary>Alttaki HttpClient örneğini enjekte eder.</summary>
    protected ApiClientBase(HttpClient httpClient)
    {
        _httpClient = httpClient;
    }

    /// <summary>Bir GET isteği gönderir ve yanıt zarfını çözer.</summary>
    protected async Task<TResponse> GetAsync<TResponse>(
        string requestUri,
        CancellationToken cancellationToken = default)
    {
        using var response = await _httpClient.GetAsync(requestUri, cancellationToken);
        return await ReadAsync<TResponse>(response, requestUri, cancellationToken);
    }

    /// <summary>Gövdeli bir POST isteği gönderir ve yanıt zarfını çözer.</summary>
    protected async Task<TResponse> PostAsync<TRequest, TResponse>(
        string requestUri,
        TRequest request,
        CancellationToken cancellationToken = default)
    {
        using var response = await _httpClient.PostAsJsonAsync(requestUri, request, cancellationToken);
        return await ReadAsync<TResponse>(response, requestUri, cancellationToken);
    }

    /// <summary>Gövdesiz bir POST isteği gönderir ve yanıt zarfını çözer.</summary>
    protected async Task<TResponse> PostAsync<TResponse>(
        string requestUri,
        CancellationToken cancellationToken = default)
    {
        using var response = await _httpClient.PostAsync(requestUri, content: null, cancellationToken);
        return await ReadAsync<TResponse>(response, requestUri, cancellationToken);
    }

    /// <summary>Gövdeli bir PUT isteği gönderir ve yanıt zarfını çözer.</summary>
    protected async Task<TResponse> PutAsync<TRequest, TResponse>(
        string requestUri,
        TRequest request,
        CancellationToken cancellationToken = default)
    {
        using var response = await _httpClient.PutAsJsonAsync(requestUri, request, cancellationToken);
        return await ReadAsync<TResponse>(response, requestUri, cancellationToken);
    }

    /// <summary>Bir DELETE isteği gönderir ve yanıt zarfını çözer.</summary>
    protected async Task<TResponse> DeleteAsync<TResponse>(
        string requestUri,
        CancellationToken cancellationToken = default)
    {
        using var response = await _httpClient.DeleteAsync(requestUri, cancellationToken);
        return await ReadAsync<TResponse>(response, requestUri, cancellationToken);
    }

    /// <summary>
    /// Yanıt zarfını ayrıştırmayan "ateşle ve unut" tarzı bir POST. Çağrının başarılı
    /// olup olmadığını (2xx) döndürür. Yanıt gövdesinin önemsiz olduğu ve çağırana asla
    /// bir seri durumdan çıkarma hatası yansıtmaması gereken denetim günlüğü gönderimi
    /// gibi en iyi çaba çağrıları için kullanılır.
    /// </summary>
    protected async Task<bool> PostIgnoreResultAsync<TRequest>(
        string requestUri,
        TRequest request,
        CancellationToken cancellationToken = default)
    {
        using var response = await _httpClient.PostAsJsonAsync(requestUri, request, cancellationToken);
        return response.IsSuccessStatusCode;
    }

    /// <summary>
    /// Ham bir GET isteği yapar ve yanıt gövdesini, çözülen <c>Content-Type</c> başlığıyla
    /// birlikte bayt olarak döndürür. JSON zarfını atlayan avatar resimleri gibi ikili
    /// yükler için kullanılır.
    /// </summary>
    protected async Task<(byte[] Content, string ContentType, int StatusCode)> GetRawAsync(
        string requestUri,
        CancellationToken cancellationToken = default)
    {
        using var response = await _httpClient.GetAsync(requestUri, cancellationToken);
        var bytes = await response.Content.ReadAsByteArrayAsync(cancellationToken);
        var contentType = response.Content.Headers.ContentType?.MediaType ?? "application/octet-stream";
        return (bytes, contentType, (int)response.StatusCode);
    }

    /// <summary>Yanıt gövdesini BaseResponse zarfı olarak okur ve seri durumdan çıkarır.</summary>
    private static async Task<TResponse> ReadAsync<TResponse>(
        HttpResponseMessage response,
        string requestUri,
        CancellationToken cancellationToken)
    {
        // API hem başarıda hem başarısızlıkta her zaman bir BaseResponse<T> zarfı
        // döndürür; bu yüzden HTTP durumundan bağımsız olarak seri durumdan çıkarırız.
        // IsSuccess / Errors'a nasıl tepki vereceğine çağıran karar verir.
        TResponse? payload;

        try
        {
            payload = await response.Content.ReadFromJsonAsync<TResponse>(cancellationToken);
        }
        catch (Exception ex)
        {
            throw new HttpRequestException(
                string.Format(
                    LocalizationText.Get(LocalizationKeys.Messages.ApiResponseDeserializationFailed, "Failed to deserialize response from {0}."),
                    requestUri),
                ex,
                response.StatusCode);
        }

        return payload
               ?? throw new InvalidOperationException(
                   string.Format(
                       LocalizationText.Get(LocalizationKeys.Messages.ApiResponseBodyEmpty, "Response body was empty for {0}."),
                       requestUri));
    }
}

