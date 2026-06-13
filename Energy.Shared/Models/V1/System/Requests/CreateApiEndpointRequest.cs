namespace Energy.Shared.Models.V1.System.Requests;

/// <summary>Yeni bir API uç noktası kaydı oluşturmak için kullanılan istek.</summary>
public sealed class CreateApiEndpointRequest
{
    /// <summary>Uç noktanın adı.</summary>
    public string Name { get; set; } = string.Empty;

    /// <summary>İsteğe bağlı açıklama.</summary>
    public string? Description { get; set; }

    /// <summary>Uç noktanın yol (path) şablonu.</summary>
    public string Path { get; set; } = string.Empty;

    /// <summary>HTTP metodu (GET, POST vb.).</summary>
    public string HttpMethod { get; set; } = string.Empty;

    /// <summary>Uç noktanın etkin olup olmadığı.</summary>
    public bool IsActive { get; set; } = true;

    /// <summary>Erişim için gereken yetki kodu (varsa).</summary>
    public string? RequiredPermissionCode { get; set; }
}
