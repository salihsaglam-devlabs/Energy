namespace Energy.Shared.Models.V1.IAM.ApiEndpoint.Responses;

/// <summary>ApiEndpoint liste satırı.</summary>
public class ApiEndpointListResponse
{
    /// <summary>Kimlik.</summary>
    public Guid Id { get; set; }

    /// <summary>Endpoint yolu</summary>
    public string Path { get; set; } = string.Empty;

    /// <summary>HTTP metodu</summary>
    public string HttpMethod { get; set; } = string.Empty;

    /// <summary>Gerekli permission</summary>
    public string? RequiredPermissionCode { get; set; }

    /// <summary>Oluşturma zamanı.</summary>
    public DateTime CreatedAt { get; set; }
}
