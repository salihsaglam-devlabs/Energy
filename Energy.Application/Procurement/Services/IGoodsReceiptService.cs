namespace Energy.Application.Procurement.Services;

/// <summary>
/// Mal kabul iş kuralı: onaylanan bir mal kabul belgesini Inventory modülünde stok
/// girişine dönüştürür, sipariş satırlarının teslim alınan miktarını ve sipariş durumunu
/// (PartiallyReceived / Received) günceller.
/// </summary>
public interface IGoodsReceiptService
{
    /// <summary>Mal kabulü stok girişine dönüştürür; sipariş teslim durumunu günceller.</summary>
    Task ReceiveAsync(Guid purchaseReceiptId, CancellationToken ct = default);
}

