namespace Energy.Web.Models.Dashboard;

/// <summary>Gösterge panosundaki kurumsal (iş) modül metrik kartı.</summary>
public sealed class EnterpriseMetricViewModel
{
    /// <summary>Widget başlığı yerelleştirme anahtarı.</summary>
    public string NameKey { get; init; } = string.Empty;

    /// <summary>Widget açıklaması yerelleştirme anahtarı.</summary>
    public string DescriptionKey { get; init; } = string.Empty;

    /// <summary>İlgili iş modülü.</summary>
    public string Module { get; init; } = string.Empty;

    /// <summary>Canlı hesaplanmış değer (görüntülenmeye hazır).</summary>
    public string Value { get; init; } = string.Empty;

    /// <summary>Modül CRUD ekranına hızlı geçiş bağlantısı (varsa).</summary>
    public string? Url { get; init; }
}

