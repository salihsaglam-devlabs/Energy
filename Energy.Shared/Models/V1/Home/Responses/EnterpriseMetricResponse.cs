namespace Energy.Shared.Models.V1.Home.Responses;

/// <summary>
/// Gösterge panosundaki kurumsal (iş) modül widget'ı için canlı hesaplanmış metrik.
/// Değer, widget koduna göre veritabanından gerçek zamanlı üretilir; yalnızca çağıran
/// kullanıcının görmeye yetkili olduğu (widget'ın gerektirdiği yetki) metrikler döner.
/// </summary>
public sealed class EnterpriseMetricResponse
{
    /// <summary>Widget kodu (ör. <c>LowStock</c>, <c>PendingApprovals</c>).</summary>
    public string Code { get; init; } = string.Empty;

    /// <summary>İlgili iş modülü (ör. <c>Inventory</c>, <c>Workflow</c>).</summary>
    public string Module { get; init; } = string.Empty;

    /// <summary>Widget başlığı için yerelleştirme anahtarı (ör. <c>DashboardWidgets.LowStock.Name</c>).</summary>
    public string NameKey { get; init; } = string.Empty;

    /// <summary>Widget açıklaması için yerelleştirme anahtarı.</summary>
    public string DescriptionKey { get; init; } = string.Empty;

    /// <summary>Widget türü (Counter, Chart, Grid, Gauge ...).</summary>
    public string WidgetType { get; init; } = "Counter";

    /// <summary>Canlı hesaplanmış sayısal değer.</summary>
    public decimal Value { get; init; }

    /// <summary>Görüntüleme sırası.</summary>
    public int DisplayOrder { get; init; }
}

