namespace Energy.Application.Inventory.Services;

/// <summary>Depo girişi (stok-in) isteği — yeni bir lot ve maliyet katmanı oluşturur.</summary>
public sealed record StockInRequest(
    Guid WarehouseId,
    Guid MaterialId,
    Guid UnitOfMeasureId,
    decimal Quantity,
    decimal UnitCost,
    Guid? CurrencyId = null,
    Guid? ProjectId = null,
    string? Note = null);

/// <summary>Depo çıkışı (stok-out) isteği — FIFO ile lotlara dağıtılır.</summary>
public sealed record StockOutRequest(
    Guid WarehouseId,
    Guid MaterialId,
    Guid UnitOfMeasureId,
    decimal Quantity,
    Guid? ProjectId = null,
    string? Note = null);

/// <summary>Depolar arası transfer isteği.</summary>
public sealed record StockTransferRequest(
    Guid SourceWarehouseId,
    Guid TargetWarehouseId,
    Guid MaterialId,
    Guid UnitOfMeasureId,
    decimal Quantity,
    string? Note = null);

/// <summary>Bir FIFO çıkış tahsisinin tek satırı.</summary>
public sealed record StockAllocationLine(Guid StockLotId, decimal Quantity, decimal UnitCost);

/// <summary>FIFO çıkışın sonucu: toplam maliyet ve lot bazlı dağılım.</summary>
public sealed record StockIssueResult(decimal TotalCost, IReadOnlyList<StockAllocationLine> Allocations);

/// <summary>Sayım düzeltmesinin sonucu.</summary>
public sealed record StockCountAdjustmentResult(decimal SystemQuantity, decimal CountedQuantity, decimal Difference);

/// <summary>
/// Stok hareket çekirdeği. Tüm fiziksel değişiklikler StockTransaction üretir; çıkış
/// FIFO ile lot maliyet katmanlarına dağıtılır; StockBalances özet tablodur ve
/// hareketlerden yeniden üretilebilir. Negatif stok varsayılan olarak engellenir.
/// Tüm işlemler transaction içinde atomiktir.
/// </summary>
public interface IInventoryService
{
    /// <summary>Depo girişi: yeni lot + giriş hareketi + bakiye güncellemesi.</summary>
    Task<Guid> PostStockInAsync(StockInRequest request, CancellationToken ct = default);

    /// <summary>Depo çıkışı: FIFO lot tahsisi + çıkış hareketleri + bakiye güncellemesi.</summary>
    Task<StockIssueResult> PostStockOutAsync(StockOutRequest request, CancellationToken ct = default);

    /// <summary>Depolar arası transfer: kaynak çıkış + hedef giriş (FIFO maliyetiyle).</summary>
    Task<StockIssueResult> TransferAsync(StockTransferRequest request, CancellationToken ct = default);

    /// <summary>Sayım düzeltmesi: stok kolonunu doğrudan değil, düzeltme hareketiyle düzeltir.</summary>
    Task<StockCountAdjustmentResult> AdjustToCountAsync(Guid warehouseId, Guid materialId, Guid unitOfMeasureId, decimal countedQuantity, CancellationToken ct = default);

    /// <summary>Depodaki bir malzemenin kullanılabilir miktarı (lot kalanları toplamı).</summary>
    Task<decimal> GetAvailableQuantityAsync(Guid warehouseId, Guid materialId, CancellationToken ct = default);

    /// <summary>
    /// Onaylanmış bir stok belgesini fiziksel silmeden ters hareketle iptal eder; lot
    /// kalanlarını ve bakiyeleri geri alır, belgeyi Cancelled durumuna çeker.
    /// </summary>
    Task ReverseDocumentAsync(Guid stockDocumentId, string? note = null, CancellationToken ct = default);

    /// <summary>StockBalances özet tablosunu StockTransactions'tan yeniden üretir.</summary>
    Task<int> RebuildBalancesAsync(Guid? warehouseId = null, Guid? materialId = null, CancellationToken ct = default);
}

