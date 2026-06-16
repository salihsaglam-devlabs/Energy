using CollectionEntity = Energy.Domain.Finance.Collection;
using CollectionAllocationEntity = Energy.Domain.Finance.CollectionAllocation;
using FinancialTransactionEntity = Energy.Domain.Finance.FinancialTransaction;
using FinancialTransactionLineEntity = Energy.Domain.Finance.FinancialTransactionLine;
using PayableEntity = Energy.Domain.Finance.Payable;
using PaymentEntity = Energy.Domain.Finance.Payment;
using PaymentAllocationEntity = Energy.Domain.Finance.PaymentAllocation;
using ReceivableEntity = Energy.Domain.Finance.Receivable;
using Energy.Shared.Common;
using Energy.Application.Finance.Services;
using Energy.Domain.Common;
using Energy.Domain.Contracts;
using Energy.Domain.Finance;
using Energy.Domain.Notifications;
using Energy.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace Energy.Infrastructure.Finance.Services;

/// <summary>
/// <see cref="IFinanceService"/>'in EF Core uygulaması. Borç/alacak, ödeme/tahsilat
/// parçalı kapama, puantaj maliyet hareketi, hakediş alacak/borç üretimi ve bütçe
/// aşımı bildirimi. Tüm para hareketleri transaction içinde atomiktir.
/// </summary>
public sealed class FinanceService : IFinanceService
{
    private readonly AppDbContext _db;
    private readonly ILogger<FinanceService> _logger;

    public FinanceService(AppDbContext db, ILogger<FinanceService> logger)
    {
        _db = db;
        _logger = logger;
    }

    public async Task<Guid> CreatePayableAsync(Guid partnerId, Guid currencyId, decimal amount, DateTime dueDate,
        string? relatedModule, string? relatedEntityType, Guid? relatedEntityId, CancellationToken ct = default)
    {
        await using var tx = await _db.Database.BeginTransactionAsync(ct);
        var id = CreatePayableCore(partnerId, currencyId, amount, dueDate, relatedModule, relatedEntityType, relatedEntityId);
        await _db.SaveChangesAsync(ct);
        await tx.CommitAsync(ct);
        return id;
    }

    public async Task<Guid> CreateReceivableAsync(Guid partnerId, Guid currencyId, decimal amount, DateTime dueDate,
        string? relatedModule, string? relatedEntityType, Guid? relatedEntityId, CancellationToken ct = default)
    {
        await using var tx = await _db.Database.BeginTransactionAsync(ct);
        var id = CreateReceivableCore(partnerId, currencyId, amount, dueDate, relatedModule, relatedEntityType, relatedEntityId);
        await _db.SaveChangesAsync(ct);
        await tx.CommitAsync(ct);
        return id;
    }

    public async Task AllocatePaymentAsync(Guid paymentId, IReadOnlyList<FinanceAllocationLine> allocations, CancellationToken ct = default)
    {
        await using var tx = await _db.Database.BeginTransactionAsync(ct);

        var payment = await _db.Payments.FirstOrDefaultAsync(p => p.Id == paymentId, ct)
            ?? throw new InvalidOperationException($"PaymentEntity {paymentId} not found.");

        var alreadyAllocated = await _db.PaymentAllocations
            .Where(a => a.PaymentId == paymentId)
            .SumAsync(a => (decimal?)a.Amount, ct) ?? 0m;

        var newTotal = allocations.Sum(a => a.Amount);
        if (alreadyAllocated + newTotal > payment.Amount)
        {
            throw new InvalidOperationException(
                $"Over-allocation: payment amount {payment.Amount}, already {alreadyAllocated}, requested {newTotal}.");
        }

        foreach (var line in allocations)
        {
            if (line.Amount <= 0) continue;

            var payable = await _db.Payables.FirstOrDefaultAsync(p => p.Id == line.TargetId, ct)
                ?? throw new InvalidOperationException($"PayableEntity {line.TargetId} not found.");
            if (line.Amount > payable.RemainingAmount)
            {
                throw new InvalidOperationException(
                    $"Allocation {line.Amount} exceeds payable remaining {payable.RemainingAmount}.");
            }

            _db.PaymentAllocations.Add(new PaymentAllocationEntity
            {
                Id = Guid.NewGuid(),
                PaymentId = paymentId,
                PayableId = payable.Id,
                Amount = line.Amount,
            });

            payable.RemainingAmount -= line.Amount;
            if (payable.RemainingAmount <= 0)
            {
                payable.RemainingAmount = 0;
                payable.IsClosed = true;
            }
        }

        await _db.SaveChangesAsync(ct);
        await tx.CommitAsync(ct);
    }

    public async Task AllocateCollectionAsync(Guid collectionId, IReadOnlyList<FinanceAllocationLine> allocations, CancellationToken ct = default)
    {
        await using var tx = await _db.Database.BeginTransactionAsync(ct);

        var collection = await _db.Collections.FirstOrDefaultAsync(c => c.Id == collectionId, ct)
            ?? throw new InvalidOperationException($"CollectionEntity {collectionId} not found.");

        var alreadyAllocated = await _db.CollectionAllocations
            .Where(a => a.CollectionId == collectionId)
            .SumAsync(a => (decimal?)a.Amount, ct) ?? 0m;

        var newTotal = allocations.Sum(a => a.Amount);
        if (alreadyAllocated + newTotal > collection.Amount)
        {
            throw new InvalidOperationException(
                $"Over-allocation: collection amount {collection.Amount}, already {alreadyAllocated}, requested {newTotal}.");
        }

        foreach (var line in allocations)
        {
            if (line.Amount <= 0) continue;

            var receivable = await _db.Receivables.FirstOrDefaultAsync(r => r.Id == line.TargetId, ct)
                ?? throw new InvalidOperationException($"ReceivableEntity {line.TargetId} not found.");
            if (line.Amount > receivable.RemainingAmount)
            {
                throw new InvalidOperationException(
                    $"Allocation {line.Amount} exceeds receivable remaining {receivable.RemainingAmount}.");
            }

            _db.CollectionAllocations.Add(new CollectionAllocationEntity
            {
                Id = Guid.NewGuid(),
                CollectionId = collectionId,
                ReceivableId = receivable.Id,
                Amount = line.Amount,
            });

            receivable.RemainingAmount -= line.Amount;
            if (receivable.RemainingAmount <= 0)
            {
                receivable.RemainingAmount = 0;
                receivable.IsClosed = true;
            }
        }

        await _db.SaveChangesAsync(ct);
        await tx.CommitAsync(ct);
    }

    public async Task<Guid> PostTimesheetCostAsync(Guid timesheetId, Guid currencyId, CancellationToken ct = default)
    {
        await using var tx = await _db.Database.BeginTransactionAsync(ct);

        var lines = await _db.TimesheetLines
            .Where(l => l.TimesheetId == timesheetId)
            .ToListAsync(ct);
        if (lines.Count == 0)
        {
            throw new InvalidOperationException($"Timesheet {timesheetId} has no lines.");
        }

        var total = lines.Sum(l => (l.NormalHours + l.OvertimeHours) * l.HourlyCost);

        var transaction = new FinancialTransactionEntity
        {
            Id = Guid.NewGuid(),
            TransactionType = FinancialTransactionType.Expense,
            CurrencyId = currencyId,
            Amount = total,
            TransactionDate = DateTime.UtcNow,
            RelatedModule = "HR",
            RelatedEntityType = "Timesheet",
            RelatedEntityId = timesheetId,
            Description = "Labour cost from approved timesheet",
        };
        _db.FinancialTransactions.Add(transaction);

        foreach (var group in lines.GroupBy(l => l.ProjectId))
        {
            _db.FinancialTransactionLines.Add(new FinancialTransactionLineEntity
            {
                Id = Guid.NewGuid(),
                FinancialTransactionId = transaction.Id,
                ProjectId = group.Key,
                Amount = group.Sum(l => (l.NormalHours + l.OvertimeHours) * l.HourlyCost),
                Description = "Labour cost",
            });
        }

        await _db.SaveChangesAsync(ct);
        await tx.CommitAsync(ct);
        return transaction.Id;
    }

    public async Task<Guid> PostProgressPaymentAsync(Guid progressPaymentId, CancellationToken ct = default)
    {
        await using var tx = await _db.Database.BeginTransactionAsync(ct);

        var pp = await _db.ProgressPayments.FirstOrDefaultAsync(p => p.Id == progressPaymentId, ct)
            ?? throw new InvalidOperationException($"Progress payment {progressPaymentId} not found.");
        if (pp.PartnerId is not { } partnerId)
        {
            throw new InvalidOperationException("Progress payment has no partner; cannot post to finance.");
        }

        var contract = await _db.Contracts.FirstOrDefaultAsync(c => c.Id == pp.ContractId, ct)
            ?? throw new InvalidOperationException($"Contract {pp.ContractId} not found.");

        var dueDate = DateTime.UtcNow.AddDays(30);

        // Müşteri sözleşmesi → alacak; taşeron/tedarikçi → borç.
        var id = contract.ContractType == ContractType.Customer
            ? CreateReceivableCore(partnerId, contract.CurrencyId, pp.NetAmount, dueDate, "ProgressPayments", "ProgressPayment", pp.Id)
            : CreatePayableCore(partnerId, contract.CurrencyId, pp.NetAmount, dueDate, "ProgressPayments", "ProgressPayment", pp.Id);

        await _db.SaveChangesAsync(ct);
        await tx.CommitAsync(ct);
        return id;
    }

    public async Task<bool> CheckBudgetOverrunAsync(Guid budgetId, CancellationToken ct = default)
    {
        var budget = await _db.Budgets.FirstOrDefaultAsync(b => b.Id == budgetId, ct)
            ?? throw new InvalidOperationException($"Budget {budgetId} not found.");

        var planned = await _db.BudgetLines
            .Where(l => l.BudgetId == budgetId)
            .SumAsync(l => (decimal?)l.PlannedAmount, ct) ?? 0m;

        var actualQuery = _db.FinancialTransactionLines.AsQueryable();
        if (budget.ProjectId is { } projectId)
        {
            actualQuery = actualQuery.Where(l => l.ProjectId == projectId);
        }
        else if (budget.CostCenterId is { } costCenterId)
        {
            actualQuery = actualQuery.Where(l => l.CostCenterId == costCenterId);
        }
        else
        {
            return false;
        }

        var actual = await actualQuery.SumAsync(l => (decimal?)l.Amount, ct) ?? 0m;
        if (actual <= planned)
        {
            return false;
        }

        var notification = new Notification
        {
            Id = Guid.NewGuid(),
            Title = "Notifications.BudgetOverrun.Title",
            Body = "Notifications.BudgetOverrun.Body",
            NotificationType = "BudgetOverrun",
            RelatedModule = "Budget",
            RelatedEntityType = "Budget",
            RelatedEntityId = budgetId,
        };
        _db.Notifications.Add(notification);

        if (budget.ProjectId is { } pid)
        {
            var managerId = await _db.Projects
                .Where(p => p.Id == pid)
                .Select(p => p.ManagerUserId)
                .FirstOrDefaultAsync(ct);
            if (managerId is not null)
            {
                _db.NotificationRecipients.Add(new NotificationRecipient
                {
                    Id = Guid.NewGuid(),
                    NotificationId = notification.Id,
                    UserId = managerId.Value,
                    IsRead = false,
                });
            }
        }

        await _db.SaveChangesAsync(ct);
        return true;
    }

    // ---- İç çekirdek (transaction dışında çağrılır) ----

    private Guid CreatePayableCore(Guid partnerId, Guid currencyId, decimal amount, DateTime dueDate,
        string? relatedModule, string? relatedEntityType, Guid? relatedEntityId)
    {
        var payable = new PayableEntity
        {
            Id = Guid.NewGuid(),
            PartnerId = partnerId,
            CurrencyId = currencyId,
            Amount = amount,
            RemainingAmount = amount,
            DueDate = dueDate,
            RelatedModule = relatedModule,
            RelatedEntityType = relatedEntityType,
            RelatedEntityId = relatedEntityId,
        };
        _db.Payables.Add(payable);
        return payable.Id;
    }

    private Guid CreateReceivableCore(Guid partnerId, Guid currencyId, decimal amount, DateTime dueDate,
        string? relatedModule, string? relatedEntityType, Guid? relatedEntityId)
    {
        var receivable = new ReceivableEntity
        {
            Id = Guid.NewGuid(),
            PartnerId = partnerId,
            CurrencyId = currencyId,
            Amount = amount,
            RemainingAmount = amount,
            DueDate = dueDate,
            RelatedModule = relatedModule,
            RelatedEntityType = relatedEntityType,
            RelatedEntityId = relatedEntityId,
        };
        _db.Receivables.Add(receivable);
        return receivable.Id;
    }
}

