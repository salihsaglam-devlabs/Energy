namespace Energy.Shared.Models.V1.System.Responses;

/// <summary>Tek bir menü öğesinin düz (ağaç olmayan) görünümü.</summary>
public sealed class MenuResponse
{
    /// <summary>Menünün kimliği.</summary>
    public Guid Id { get; init; }

    /// <summary>Üst menü kimliği (kök öğeler için null).</summary>
    public Guid? ParentId { get; init; }

    /// <summary>Menü adının yerelleştirme anahtarı.</summary>
    public string NameKey { get; init; } = string.Empty;

    /// <summary>Menünün bağlantı adresi (URL).</summary>
    public string? Url { get; init; }

    /// <summary>Menü ikonu.</summary>
    public string? Icon { get; init; }

    /// <summary>Kardeş öğeler arasında görüntülenme sırası.</summary>
    public int DisplayOrder { get; init; }

    /// <summary>Menünün görünür olup olmadığı.</summary>
    public bool IsVisible { get; init; }

    /// <summary>Menünün etkin olup olmadığı.</summary>
    public bool IsActive { get; init; }

    /// <summary>Menüyü görmek için gereken yetki kodu (varsa).</summary>
    public string? RequiredPermissionCode { get; init; }
}
