namespace Energy.Shared.Models.V1.System.Requests;

/// <summary>Var olan bir menü öğesini güncellemek için kullanılan istek.</summary>
public sealed class UpdateMenuRequest
{
    /// <summary>Üst menü kimliği (kök öğeler için null).</summary>
    public Guid? ParentId { get; set; }

    /// <summary>Menü adının yerelleştirme anahtarı.</summary>
    public string NameKey { get; set; } = string.Empty;

    /// <summary>Menünün bağlantı adresi (URL).</summary>
    public string? Url { get; set; }

    /// <summary>Menü ikonu.</summary>
    public string? Icon { get; set; }

    /// <summary>Kardeş öğeler arasında görüntülenme sırası.</summary>
    public int DisplayOrder { get; set; }

    /// <summary>Menünün görünür olup olmadığı.</summary>
    public bool IsVisible { get; set; } = true;

    /// <summary>Menünün etkin olup olmadığı.</summary>
    public bool IsActive { get; set; } = true;

    /// <summary>Menüyü görmek için gereken yetki kodu (varsa).</summary>
    public string? RequiredPermissionCode { get; set; }
}
