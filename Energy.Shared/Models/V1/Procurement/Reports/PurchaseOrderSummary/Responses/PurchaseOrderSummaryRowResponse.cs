namespace Energy.Shared.Models.V1.Procurement.Reports.PurchaseOrderSummary.Responses;

/// <summary>PurchaseOrderSummary raporu satırı (salt-okunur projeksiyon).</summary>
public sealed class PurchaseOrderSummaryRowResponse
{
    /// <summary>Kaynak kayıt kimliği.</summary>
    public Guid Id { get; set; }

    /// <summary>OrderNo</summary>
    public string? OrderNo { get; set; }

    /// <summary>OrderDate</summary>
    public DateTime OrderDate { get; set; }

    /// <summary>SupplierId</summary>
    public Guid SupplierId { get; set; }

    /// <summary>ProjectId</summary>
    public Guid? ProjectId { get; set; }

    /// <summary>CurrencyId</summary>
    public Guid CurrencyId { get; set; }

    /// <summary>Status</summary>
    public string? Status { get; set; }
}
