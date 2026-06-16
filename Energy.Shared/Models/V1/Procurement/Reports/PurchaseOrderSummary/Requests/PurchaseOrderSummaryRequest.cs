namespace Energy.Shared.Models.V1.Procurement.Reports.PurchaseOrderSummary.Requests;

/// <summary>PurchaseOrderSummary raporu filtre/sayfalama isteği (salt-okunur).</summary>
public sealed class PurchaseOrderSummaryRequest
{
    /// <summary>Sayfa numarası (1 tabanlı).</summary>
    public int PageNumber { get; set; } = 1;

    /// <summary>Sayfa boyutu.</summary>
    public int PageSize { get; set; } = 50;

    /// <summary>Başlangıç tarihi filtresi (dahil).</summary>
    public DateTime? StartDate { get; set; }

    /// <summary>Bitiş tarihi filtresi (dahil).</summary>
    public DateTime? EndDate { get; set; }

    /// <summary>Durum filtresi.</summary>
    public string? Status { get; set; }
}
