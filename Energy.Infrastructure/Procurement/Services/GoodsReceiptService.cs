using Energy.Application.Inventory.Services;
using Energy.Application.Procurement.Services;
using Energy.Domain.Common;
using Energy.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace Energy.Infrastructure.Procurement.Services;

/// <summary>
/// <see cref="IGoodsReceiptService"/> uygulaması. Mal kabul satırlarını Inventory
/// modülünde stok girişine dönüştürür (her satır ayrı lot) ve sipariş teslim
/// durumunu günceller.
/// </summary>
public sealed class GoodsReceiptService : IGoodsReceiptService
{
    private readonly AppDbContext _db;
    private readonly IInventoryService _inventory;
    private readonly ILogger<GoodsReceiptService> _logger;

    public GoodsReceiptService(AppDbContext db, IInventoryService inventory, ILogger<GoodsReceiptService> logger)
    {
        _db = db;
        _inventory = inventory;
        _logger = logger;
    }

    public async Task ReceiveAsync(Guid purchaseReceiptId, CancellationToken ct = default)
    {
        var receipt = await _db.PurchaseReceipts.FirstOrDefaultAsync(r => r.Id == purchaseReceiptId, ct)
            ?? throw new InvalidOperationException($"Purchase receipt {purchaseReceiptId} not found.");

        if (receipt.Status == DocumentStatus.Approved)
        {
            return; // Idempotent: zaten işlenmiş.
        }

        var lines = await _db.PurchaseReceiptLines
            .Where(l => l.PurchaseReceiptId == purchaseReceiptId)
            .ToListAsync(ct);

        foreach (var line in lines)
        {
            var baseUnitId = await _db.Materials
                .Where(m => m.Id == line.MaterialId)
                .Select(m => m.BaseUnitOfMeasureId)
                .FirstAsync(ct);

            // Her mal kabul satırı ayrı bir lot olarak depoya girer (kendi transaction'ında atomik).
            await _inventory.PostStockInAsync(new StockInRequest(
                WarehouseId: receipt.WarehouseId,
                MaterialId: line.MaterialId,
                UnitOfMeasureId: baseUnitId,
                Quantity: line.Quantity,
                UnitCost: line.UnitPrice,
                Note: $"GoodsReceipt {receipt.ReceiptNo}"), ct);

            // Sipariş satırının teslim alınan miktarını güncelle.
            if (line.PurchaseOrderLineId is { } poLineId)
            {
                var poLine = await _db.PurchaseOrderLines.FirstOrDefaultAsync(p => p.Id == poLineId, ct);
                if (poLine is not null)
                {
                    poLine.ReceivedQuantity += line.Quantity;
                }
            }
        }

        receipt.Status = DocumentStatus.Approved;
        await _db.SaveChangesAsync(ct);

        // Sipariş teslim durumunu yeniden hesapla.
        if (receipt.PurchaseOrderId is { } orderId)
        {
            await RecalculateOrderStatusAsync(orderId, ct);
        }
    }

    private async Task RecalculateOrderStatusAsync(Guid orderId, CancellationToken ct)
    {
        var order = await _db.PurchaseOrders.FirstOrDefaultAsync(o => o.Id == orderId, ct);
        if (order is null || order.Status == PurchaseOrderStatus.Cancelled)
        {
            return;
        }

        var lines = await _db.PurchaseOrderLines
            .Where(l => l.PurchaseOrderId == orderId)
            .Select(l => new { l.Quantity, l.ReceivedQuantity })
            .ToListAsync(ct);

        if (lines.Count == 0)
        {
            return;
        }

        var fullyReceived = lines.All(l => l.ReceivedQuantity >= l.Quantity);
        var anyReceived = lines.Any(l => l.ReceivedQuantity > 0);

        order.Status = fullyReceived
            ? PurchaseOrderStatus.Received
            : anyReceived
                ? PurchaseOrderStatus.PartiallyReceived
                : order.Status;

        await _db.SaveChangesAsync(ct);
    }
}

