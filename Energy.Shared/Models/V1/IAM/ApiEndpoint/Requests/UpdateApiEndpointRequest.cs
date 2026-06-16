namespace Energy.Shared.Models.V1.IAM.ApiEndpoint.Requests;

/// <summary>ApiEndpoint güncelleme isteği.</summary>
public class UpdateApiEndpointRequest
{
    /// <summary>Güncellenecek kaydın kimliği.</summary>
    public Guid Id { get; set; }

    /// <summary>Endpoint yolu</summary>
    public string Path { get; set; } = string.Empty;

    /// <summary>HTTP metodu</summary>
    public string HttpMethod { get; set; } = string.Empty;

    /// <summary>Gerekli permission</summary>
    public string? RequiredPermissionCode { get; set; }
}
