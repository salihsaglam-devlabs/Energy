namespace Energy.Shared.Models.V1.IAM.ApiEndpoint.Requests;

/// <summary>ApiEndpoint oluşturma isteği.</summary>
public class CreateApiEndpointRequest
{
    /// <summary>Endpoint yolu</summary>
    public string Path { get; set; } = string.Empty;

    /// <summary>HTTP metodu</summary>
    public string HttpMethod { get; set; } = string.Empty;

    /// <summary>Gerekli permission</summary>
    public string? RequiredPermissionCode { get; set; }
}
