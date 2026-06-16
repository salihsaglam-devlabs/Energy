using Energy.Shared.Common;
using Energy.Application.Operations.Services;
using Energy.Domain.Common;
using Energy.Domain.Modules.Operations;
using Energy.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace Energy.Infrastructure.Operations.Services;

/// <summary>
/// <see cref="IWorkOrderService"/> uygulaması. Durum geçişlerini geçmiş kaydıyla
/// yönetir; zorunlu checklist tamamlanmadan kapatmayı engeller.
/// </summary>
public sealed class WorkOrderService : IWorkOrderService
{
    private readonly AppDbContext _db;
    private readonly ILogger<WorkOrderService> _logger;

    public WorkOrderService(AppDbContext db, ILogger<WorkOrderService> logger)
    {
        _db = db;
        _logger = logger;
    }

    public async Task ChangeStatusAsync(Guid workOrderId, WorkOrderStatus newStatus, string? note = null, CancellationToken ct = default)
    {
        await using var tx = await _db.Database.BeginTransactionAsync(ct);
        var workOrder = await LoadAsync(workOrderId, ct);
        await TransitionAsync(workOrder, newStatus, note, ct);
        await _db.SaveChangesAsync(ct);
        await tx.CommitAsync(ct);
    }

    public async Task CloseAsync(Guid workOrderId, string? note = null, CancellationToken ct = default)
    {
        await using var tx = await _db.Database.BeginTransactionAsync(ct);
        var workOrder = await LoadAsync(workOrderId, ct);

        if (workOrder.Status == WorkOrderStatus.Closed)
        {
            return;
        }

        // Zorunlu checklist kalemleri tamamlanmadan kapatma engellenir.
        var checklistIds = await _db.WorkOrderChecklists
            .Where(c => c.WorkOrderId == workOrderId)
            .Select(c => c.Id)
            .ToListAsync(ct);

        var hasIncompleteRequired = await _db.WorkOrderChecklistItems
            .AnyAsync(i => checklistIds.Contains(i.WorkOrderChecklistId) && i.IsRequired && !i.IsCompleted, ct);

        if (hasIncompleteRequired)
        {
            throw new InvalidOperationException("Cannot close work order: required checklist items are not completed.");
        }

        await TransitionAsync(workOrder, WorkOrderStatus.Closed, note, ct);
        await _db.SaveChangesAsync(ct);
        await tx.CommitAsync(ct);
    }

    public async Task ReopenAsync(Guid workOrderId, string? note = null, CancellationToken ct = default)
    {
        await using var tx = await _db.Database.BeginTransactionAsync(ct);
        var workOrder = await LoadAsync(workOrderId, ct);

        if (workOrder.Status != WorkOrderStatus.Closed)
        {
            throw new InvalidOperationException("Only a closed work order can be reopened.");
        }

        await TransitionAsync(workOrder, WorkOrderStatus.InProgress, note, ct);
        await _db.SaveChangesAsync(ct);
        await tx.CommitAsync(ct);
    }

    private async Task<WorkOrder> LoadAsync(Guid id, CancellationToken ct)
        => await _db.WorkOrders.FirstOrDefaultAsync(w => w.Id == id, ct)
           ?? throw new InvalidOperationException($"Work order {id} not found.");

    private Task TransitionAsync(WorkOrder workOrder, WorkOrderStatus newStatus, string? note, CancellationToken ct)
    {
        var from = workOrder.Status;
        workOrder.Status = newStatus;

        _db.WorkOrderStatusHistories.Add(new WorkOrderStatusHistory
        {
            Id = Guid.NewGuid(),
            WorkOrderId = workOrder.Id,
            FromStatus = from,
            ToStatus = newStatus,
            ChangedAt = DateTime.UtcNow,
            Note = note,
        });

        return Task.CompletedTask;
    }
}

