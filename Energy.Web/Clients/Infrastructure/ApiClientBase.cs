using System.Net.Http.Json;
using Energy.Localization;

namespace Energy.Web.Clients.Infrastructure;

/// <summary>
/// Thin base class that exposes JSON HTTP helpers. Client identity, machine
/// name and authentication headers are added by dedicated DelegatingHandlers,
/// so this type intentionally does not touch request headers.
/// </summary>
public abstract class ApiClientBase
{
    private readonly HttpClient _httpClient;

    protected ApiClientBase(HttpClient httpClient)
    {
        _httpClient = httpClient;
    }

    protected async Task<TResponse> GetAsync<TResponse>(
        string requestUri,
        CancellationToken cancellationToken = default)
    {
        using var response = await _httpClient.GetAsync(requestUri, cancellationToken);
        return await ReadAsync<TResponse>(response, requestUri, cancellationToken);
    }

    protected async Task<TResponse> PostAsync<TRequest, TResponse>(
        string requestUri,
        TRequest request,
        CancellationToken cancellationToken = default)
    {
        using var response = await _httpClient.PostAsJsonAsync(requestUri, request, cancellationToken);
        return await ReadAsync<TResponse>(response, requestUri, cancellationToken);
    }

    protected async Task<TResponse> PostAsync<TResponse>(
        string requestUri,
        CancellationToken cancellationToken = default)
    {
        using var response = await _httpClient.PostAsync(requestUri, content: null, cancellationToken);
        return await ReadAsync<TResponse>(response, requestUri, cancellationToken);
    }

    protected async Task<TResponse> PutAsync<TRequest, TResponse>(
        string requestUri,
        TRequest request,
        CancellationToken cancellationToken = default)
    {
        using var response = await _httpClient.PutAsJsonAsync(requestUri, request, cancellationToken);
        return await ReadAsync<TResponse>(response, requestUri, cancellationToken);
    }

    protected async Task<TResponse> DeleteAsync<TResponse>(
        string requestUri,
        CancellationToken cancellationToken = default)
    {
        using var response = await _httpClient.DeleteAsync(requestUri, cancellationToken);
        return await ReadAsync<TResponse>(response, requestUri, cancellationToken);
    }

    /// <summary>
    /// Fire-style POST that does not parse the response envelope. Returns whether
    /// the call succeeded (2xx). Used for best-effort calls such as audit-log
    /// ingestion where the response body is irrelevant and must never surface a
    /// deserialization error to the caller.
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
    /// Performs a raw GET request and returns the response body as bytes along with
    /// the resolved <c>Content-Type</c> header. Used for binary payloads such as
    /// avatar images that bypass the JSON envelope.
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

    private static async Task<TResponse> ReadAsync<TResponse>(
        HttpResponseMessage response,
        string requestUri,
        CancellationToken cancellationToken)
    {
        // The API always returns a BaseResponse<T> envelope – both on success
        // and on failure – so we deserialize regardless of the HTTP status.
        // The caller decides how to react to IsSuccess / Errors.
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

