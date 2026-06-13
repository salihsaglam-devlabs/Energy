namespace Energy.Shared.Models.V1.System.Responses;

/// <summary>Hiyerarşik menü ağacındaki tek bir düğüm (alt menüleri ile birlikte).</summary>
public sealed class MenuTreeNodeResponse
{
    /// <summary>Menü düğümünün kimliği.</summary>
    public Guid Id { get; init; }

    /// <summary>Görüntülenecek menü adı (çözümlenmiş).</summary>
    public string Name { get; init; } = string.Empty;

    /// <summary>Menünün bağlantı adresi (URL).</summary>
    public string? Url { get; init; }

    /// <summary>Menü ikonu.</summary>
    public string? Icon { get; init; }

    /// <summary>Kardeş öğeler arasında görüntülenme sırası.</summary>
    public int DisplayOrder { get; init; }

    /// <summary>Alt menü düğümleri.</summary>
    public IReadOnlyList<MenuTreeNodeResponse> Children { get; init; } = Array.Empty<MenuTreeNodeResponse>();
}
