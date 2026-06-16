namespace Energy.Shared.Models.V1.IAM.ApiEndpoint.Responses;

/// <summary>ApiEndpoint detay görünümü.</summary>
public class ApiEndpointDetailResponse
{
    /// <summary>Kimlik.</summary>
    public Guid Id { get; set; }

    /// <summary>Oluşturma zamanı</summary>
    public DateTime CreatedAt { get; set; }

    /// <summary>Oluşturan kullanıcı</summary>
    public Guid? CreatedBy { get; set; }

    /// <summary>Son güncelleme zamanı</summary>
    public DateTime? UpdatedAt { get; set; }

    /// <summary>Güncelleyen kullanıcı</summary>
    public Guid? UpdatedBy { get; set; }

    /// <summary>Soft delete bayrağı</summary>
    public bool IsDeleted { get; set; }

    /// <summary>Silinme zamanı</summary>
    public DateTime? DeletedAt { get; set; }

    /// <summary>Silen kullanıcı</summary>
    public Guid? DeletedBy { get; set; }

    /// <summary>Endpoint yolu</summary>
    public string Path { get; set; } = string.Empty;

    /// <summary>HTTP metodu</summary>
    public string HttpMethod { get; set; } = string.Empty;

    /// <summary>Gerekli permission</summary>
    public string? RequiredPermissionCode { get; set; }
}
