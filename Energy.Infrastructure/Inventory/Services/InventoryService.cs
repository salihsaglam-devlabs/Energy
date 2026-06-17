using StockBalanceEntity = Energy.Domain.Inventory.StockBalance;
using StockDocumentEntity = Energy.Domain.Inventory.StockDocument;
using StockDocumentLineEntity = Energy.Domain.Inventory.StockDocumentLine;
using StockDocumentTypeEntity = Energy.Domain.Inventory.StockDocumentType;
using StockIssueAllocationEntity = Energy.Domain.Inventory.StockIssueAllocation;
using StockLotEntity = Energy.Domain.Inventory.StockLot;
using StockTransactionEntity = Energy.Domain.Inventory.StockTransaction;
using Energy.Shared.Common;
using Energy.Application.Inventory.Services;
using Energy.Domain.Common;
using Energy.Domain.Inventory;
using Energy.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace Energy.Infrastructure.Inventory.Services;

/// <summary>
/// <see cref="IInventoryService"/>'in EF Core uygulaması. FIFO maliyet katmanları,
/// değiştirilemez stok hareketleri ve yeniden üretilebilir özet bakiyeler ile
/// transaction-güvenli stok çekirdeği.
/// </summary>
public sealed class InventoryService : IInventoryService
{
    private readonly AppDbContext _db;
    private readonly ILogger<InventoryService> _logger;

    public InventoryService(AppDbContext db, ILogger<InventoryService> logger)
    {
        _db = db;
        _logger = logger;
    }

    public async Task<Guid> PostStockInAsync(StockInRequest request, CancellationToken ct = default)
    {
        if (request.Quantity <= 0)
        {
            throw new InvalidOperationException("Stock-in quantity must be positive.");
        }

        await using var tx = await _db.Database.BeginTransactionAsync(ct);
        var lotId = await StockInCoreAsync(
            request.WarehouseId, request.MaterialId, request.UnitOfMeasureId,
            request.Quantity, request.UnitCost, request.CurrencyId, request.ProjectId, request.Note, ct);
        await _db.SaveChangesAsync(ct);
        await tx.CommitAsync(ct);
        return lotId;
    }

    public async Task<StockIssueResult> PostStockOutAsync(StockOutRequest request, CancellationToken ct = default)
    {
        if (request.Quantity <= 0)
        {
            throw new InvalidOperationException("Stock-out quantity must be positive.");
        }

        await using var tx = await _db.Database.BeginTransactionAsync(ct);
        var result = await StockOutCoreAsync(
            request.WarehouseId, request.MaterialId, request.UnitOfMeasureId,
            request.Quantity, request.ProjectId, request.Note, ct);
        await _db.SaveChangesAsync(ct);
        await tx.CommitAsync(ct);
        return result;
    }

    public async Task<StockIssueResult> TransferAsync(StockTransferRequest request, CancellationToken ct = default)
    {
        if (request.Quantity <= 0)
        {
            throw new InvalidOperationException("Transfer quantity must be positive.");
        }
        if (request.SourceWarehouseId == request.TargetWarehouseId)
        {
            throw new InvalidOperationException("Source and target warehouses must differ.");
        }

        await using var tx = await _db.Database.BeginTransactionAsync(ct);

        // Kaynaktan FIFO çıkış.
        var issue = await StockOutCoreAsync(
            request.SourceWarehouseId, request.MaterialId, request.UnitOfMeasureId,
            request.Quantity, null, request.Note, ct);

        // Her tahsis katmanını hedefe aynı maliyetle giriş yap (maliyet katmanları korunur).
        foreach (var allocation in issue.Allocations)
        {
            await StockInCoreAsync(
                request.TargetWarehouseId, request.MaterialId, request.UnitOfMeasureId,
                allocation.Quantity, allocation.UnitCost, null, null, request.Note, ct);
        }

        await _db.SaveChangesAsync(ct);
        await tx.CommitAsync(ct);
        return issue;
    }

    public async Task<StockCountAdjustmentResult> AdjustToCountAsync(
        Guid warehouseId, Guid materialId, Guid unitOfMeasureId, decimal countedQuantity, CancellationToken ct = default)
    {
        var system = await GetAvailableQuantityAsync(warehouseId, materialId, ct);
        var diff = countedQuantity - system;

        if (diff == 0)
        {
            return new StockCountAdjustmentResult(system, countedQuantity, 0);
        }

        if (diff > 0)
        {
            // Pozitif fark: ortalama maliyetle düzeltme girişi.
            var balance = await _db.StockBalances
                .FirstOrDefaultAsync(b => b.WarehouseId == warehouseId && b.MaterialId == materialId, ct);
            var avgCost = balance is { Quantity: > 0 } ? balance.TotalCost / balance.Quantity : 0m;

            await PostStockInAsync(new StockInRequest(
                warehouseId, materialId, unitOfMeasureId, diff, avgCost, Note: "Count adjustment (+)"), ct);
        }
        else
        {
            // Negatif fark: FIFO düzeltme çıkışı.
            await PostStockOutAsync(new StockOutRequest(
                warehouseId, materialId, unitOfMeasureId, -diff, Note: "Count adjustment (-)"), ct);
        }

        return new StockCountAdjustmentResult(system, countedQuantity, diff);
    }

    public async Task<decimal> GetAvailableQuantityAsync(Guid warehouseId, Guid materialId, CancellationToken ct = default)
        => await _db.StockLots
            .Where(l => l.WarehouseId == warehouseId && l.MaterialId == materialId && l.RemainingQuantity > 0)
            .SumAsync(l => (decimal?)l.RemainingQuantity, ct) ?? 0m;

    public async Task ReverseDocumentAsync(Guid stockDocumentId, string? note = null, CancellationToken ct = default)
    {
        await using var tx = await _db.Database.BeginTransactionAsync(ct);

        var document = await _db.StockDocuments.FirstOrDefaultAsync(d => d.Id == stockDocumentId, ct)
            ?? throw new InvalidOperationException($"Stock document {stockDocumentId} not found.");
        if (document.Status == DocumentStatus.Cancelled)
        {
            return; // Idempotent.
        }

        var transactions = await _db.StockTransactions
            .Where(t => t.StockDocumentId == stockDocumentId)
            .ToListAsync(ct);
        if (transactions.Count == 0)
        {
            throw new InvalidOperationException("Stock document has no transactions to reverse.");
        }

        var now = DateTime.UtcNow;
        var docType = await EnsureDocumentTypeAsync("Reverse", ct);

        var reversal = new StockDocumentEntity
        {
            Id = Guid.NewGuid(),
            DocumentTypeId = docType.Id,
            SourceWarehouseId = document.SourceWarehouseId,
            TargetWarehouseId = document.TargetWarehouseId,
            ProjectId = document.ProjectId,
            Status = DocumentStatus.Approved,
            DocumentNo = GenerateNo("REV"),
            DocumentDate = now,
            Note = note ?? $"Reversal of {document.DocumentNo}",
        };
        _db.StockDocuments.Add(reversal);

        foreach (var txn in transactions)
        {
            var originalLine = await _db.StockDocumentLines.FirstAsync(l => l.Id == txn.StockDocumentLineId, ct);

            var reversalLine = new StockDocumentLineEntity
            {
                Id = Guid.NewGuid(),
                StockDocumentId = reversal.Id,
                MaterialId = txn.MaterialId,
                UnitOfMeasureId = originalLine.UnitOfMeasureId,
                Quantity = Math.Abs(txn.Quantity),
            };
            _db.StockDocumentLines.Add(reversalLine);

            _db.StockTransactions.Add(new StockTransactionEntity
            {
                Id = Guid.NewGuid(),
                StockDocumentId = reversal.Id,
                StockDocumentLineId = reversalLine.Id,
                StockLotId = txn.StockLotId,
                WarehouseId = txn.WarehouseId,
                MaterialId = txn.MaterialId,
                Quantity = -txn.Quantity,
                UnitCost = txn.UnitCost,
                TransactionDate = now,
            });

            if (txn.StockLotId is { } lotId)
            {
                var lot = await _db.StockLots.FirstAsync(l => l.Id == lotId, ct);
                if (txn.Quantity > 0)
                {
                    // Orijinal giriş → lota geri konan miktar geri alınır (tüketilmemiş olmalı).
                    if (lot.RemainingQuantity < txn.Quantity)
                    {
                        throw new InvalidOperationException(
                            $"Cannot reverse: lot {lot.LotNo} has already been consumed.");
                    }
                    lot.RemainingQuantity -= txn.Quantity;
                }
                else
                {
                    // Orijinal çıkış → lota miktar geri eklenir.
                    lot.RemainingQuantity += -txn.Quantity;
                }
            }

            await ApplyBalanceDeltaAsync(txn.WarehouseId, txn.MaterialId, -txn.Quantity, -txn.Quantity * txn.UnitCost, now, ct);
        }

        document.Status = DocumentStatus.Cancelled;
        await _db.SaveChangesAsync(ct);
        await tx.CommitAsync(ct);
    }

    public async Task<int> RebuildBalancesAsync(Guid? warehouseId = null, Guid? materialId = null, CancellationToken ct = default)
    {
        var query = _db.StockTransactions.AsQueryable();
        if (warehouseId is not null) query = query.Where(t => t.WarehouseId == warehouseId);
        if (materialId is not null) query = query.Where(t => t.MaterialId == materialId);

        var aggregates = await query
            .GroupBy(t => new { t.WarehouseId, t.MaterialId })
            .Select(g => new
            {
                g.Key.WarehouseId,
                g.Key.MaterialId,
                Quantity = g.Sum(x => x.Quantity),
                TotalCost = g.Sum(x => x.Quantity * x.UnitCost),
            })
            .ToListAsync(ct);

        var now = DateTime.UtcNow;
        var touched = 0;
        foreach (var agg in aggregates)
        {
            var balance = await _db.StockBalances
                .FirstOrDefaultAsync(b => b.WarehouseId == agg.WarehouseId && b.MaterialId == agg.MaterialId, ct);
            if (balance is null)
            {
                balance = new StockBalanceEntity
                {
                    Id = Guid.NewGuid(),
                    WarehouseId = agg.WarehouseId,
                    MaterialId = agg.MaterialId,
                };
                _db.StockBalances.Add(balance);
            }
            balance.Quantity = agg.Quantity;
            balance.TotalCost = agg.TotalCost;
            balance.LastRecalculatedAt = now;
            touched++;
        }

        await _db.SaveChangesAsync(ct);
        return touched;
    }

    // ---- İç çekirdek (transaction dışında çağrılır, SaveChanges çağıran sarar) ----

    private async Task<Guid> StockInCoreAsync(
        Guid warehouseId, Guid materialId, Guid unitOfMeasureId, decimal quantity, decimal unitCost,
        Guid? currencyId, Guid? projectId, string? note, CancellationToken ct)
    {
        var now = DateTime.UtcNow;
        var docType = await EnsureDocumentTypeAsync("In", ct);

        var document = new StockDocumentEntity
        {
            Id = Guid.NewGuid(),
            DocumentTypeId = docType.Id,
            TargetWarehouseId = warehouseId,
            ProjectId = projectId,
            Status = DocumentStatus.Approved,
            DocumentNo = GenerateNo("IN"),
            DocumentDate = now,
            Note = note,
        };
        _db.StockDocuments.Add(document);

        var line = new StockDocumentLineEntity
        {
            Id = Guid.NewGuid(),
            StockDocumentId = document.Id,
            MaterialId = materialId,
            UnitOfMeasureId = unitOfMeasureId,
            Quantity = quantity,
            UnitPrice = unitCost,
            CurrencyId = currencyId,
        };
        _db.StockDocumentLines.Add(line);

        var lot = new StockLotEntity
        {
            Id = Guid.NewGuid(),
            WarehouseId = warehouseId,
            MaterialId = materialId,
            SourceStockDocumentLineId = line.Id,
            LotNo = GenerateNo("LOT"),
            InitialQuantity = quantity,
            RemainingQuantity = quantity,
            UnitCost = unitCost,
            ReceivedAt = now,
        };
        _db.StockLots.Add(lot);

        _db.StockTransactions.Add(new StockTransactionEntity
        {
            Id = Guid.NewGuid(),
            StockDocumentId = document.Id,
            StockDocumentLineId = line.Id,
            StockLotId = lot.Id,
            WarehouseId = warehouseId,
            MaterialId = materialId,
            Quantity = quantity,
            UnitCost = unitCost,
            TransactionDate = now,
        });

        await ApplyBalanceDeltaAsync(warehouseId, materialId, quantity, quantity * unitCost, now, ct);
        return lot.Id;
    }

    private async Task<StockIssueResult> StockOutCoreAsync(
        Guid warehouseId, Guid materialId, Guid unitOfMeasureId, decimal quantity,
        Guid? projectId, string? note, CancellationToken ct)
    {
        var lots = await _db.StockLots
            .Where(l => l.WarehouseId == warehouseId && l.MaterialId == materialId && l.RemainingQuantity > 0)
            .OrderBy(l => l.ReceivedAt)
            .ThenBy(l => l.CreatedAt)
            .ToListAsync(ct);

        var available = lots.Sum(l => l.RemainingQuantity);
        if (available < quantity)
        {
            throw new InvalidOperationException(
                $"Insufficient stock: available {available}, requested {quantity}. Negative stock is blocked.");
        }

        var now = DateTime.UtcNow;
        var docType = await EnsureDocumentTypeAsync("Out", ct);

        var document = new StockDocumentEntity
        {
            Id = Guid.NewGuid(),
            DocumentTypeId = docType.Id,
            SourceWarehouseId = warehouseId,
            ProjectId = projectId,
            Status = DocumentStatus.Approved,
            DocumentNo = GenerateNo("OUT"),
            DocumentDate = now,
            Note = note,
        };
        _db.StockDocuments.Add(document);

        var line = new StockDocumentLineEntity
        {
            Id = Guid.NewGuid(),
            StockDocumentId = document.Id,
            MaterialId = materialId,
            UnitOfMeasureId = unitOfMeasureId,
            Quantity = quantity,
        };
        _db.StockDocumentLines.Add(line);

        var remaining = quantity;
        var totalCost = 0m;
        var allocations = new List<StockAllocationLine>();

        foreach (var lot in lots)
        {
            if (remaining <= 0) break;

            var take = Math.Min(remaining, lot.RemainingQuantity);
            lot.RemainingQuantity -= take;

            _db.StockIssueAllocations.Add(new StockIssueAllocationEntity
            {
                Id = Guid.NewGuid(),
                StockDocumentLineId = line.Id,
                StockLotId = lot.Id,
                Quantity = take,
                UnitCost = lot.UnitCost,
            });

            _db.StockTransactions.Add(new StockTransactionEntity
            {
                Id = Guid.NewGuid(),
                StockDocumentId = document.Id,
                StockDocumentLineId = line.Id,
                StockLotId = lot.Id,
                WarehouseId = warehouseId,
                MaterialId = materialId,
                Quantity = -take,
                UnitCost = lot.UnitCost,
                TransactionDate = now,
            });

            totalCost += take * lot.UnitCost;
            remaining -= take;
            allocations.Add(new StockAllocationLine(lot.Id, take, lot.UnitCost));
        }

        await ApplyBalanceDeltaAsync(warehouseId, materialId, -quantity, -totalCost, now, ct);
        return new StockIssueResult(totalCost, allocations);
    }

    private async Task ApplyBalanceDeltaAsync(
        Guid warehouseId, Guid materialId, decimal deltaQuantity, decimal deltaCost, DateTime now, CancellationToken ct)
    {
        var balance = await _db.StockBalances
            .FirstOrDefaultAsync(b => b.WarehouseId == warehouseId && b.MaterialId == materialId, ct);
        if (balance is null)
        {
            balance = new StockBalanceEntity
            {
                Id = Guid.NewGuid(),
                WarehouseId = warehouseId,
                MaterialId = materialId,
            };
            _db.StockBalances.Add(balance);
        }

        balance.Quantity += deltaQuantity;
        balance.TotalCost += deltaCost;
        balance.LastRecalculatedAt = now;
    }

    private async Task<StockDocumentTypeEntity> EnsureDocumentTypeAsync(string direction, CancellationToken ct)
    {
        var code = "SYS-" + direction.ToUpperInvariant();
        var type = await _db.StockDocumentTypes.FirstOrDefaultAsync(t => t.Code == code, ct);
        if (type is null)
        {
            type = new StockDocumentTypeEntity
            {
                Id = Guid.NewGuid(),
                Code = code,
                Name = $"System {direction}",
                Direction = direction,
                IsActive = true,
            };
            _db.StockDocumentTypes.Add(type);
        }
        return type;
    }

    private static string GenerateNo(string prefix)
        => $"{prefix}-{DateTime.UtcNow:yyyyMMddHHmmssfff}-{Guid.NewGuid():N}"[..Math.Min(40, prefix.Length + 24)];
}

