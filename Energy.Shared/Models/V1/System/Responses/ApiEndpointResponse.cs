namespace Energy.Shared.Models.V1.System.Responses;

/// <summary>Tek bir API uç noktası kaydının görünümü.</summary>
public sealed class ApiEndpointResponse
{
    /// <summary>Uç noktanın kimliği.</summary>
    public Guid Id { get; init; }

    /// <summary>Uç noktanın adı.</summary>
    public string Name { get; init; } = string.Empty;

    /// <summary>İsteğe bağlı açıklama.</summary>
    public string? Description { get; init; }

    /// <summary>Uç noktanın yol (path) şablonu.</summary>
    public string Path { get; init; } = string.Empty;

    /// <summary>HTTP metodu (GET, POST vb.).</summary>
    public string HttpMethod { get; init; } = string.Empty;

    /// <summary>Uç noktanın etkin olup olmadığı.</summary>
    public bool IsActive { get; init; }

    /// <summary>Erişim için gereken yetki kodu (varsa).</summary>
    public string? RequiredPermissionCode { get; init; }
}
