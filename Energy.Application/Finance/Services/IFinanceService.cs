namespace Energy.Application.Finance.Services;

/// <summary>Bir ödeme/tahsilatın tek bir borç/alacağa dağıtılan tutarı.</summary>
public sealed record FinanceAllocationLine(Guid TargetId, decimal Amount);

/// <summary>
/// Ön muhasebe iş kuralları: borç/alacak oluşturma, ödeme/tahsilat parçalı kapama
/// (allocation), puantaj maliyet hareketi, hakediş alacak/borç üretimi ve bütçe aşımı
/// bildirimi. Tüm para hareketleri transaction içinde atomiktir.
/// </summary>
public interface IFinanceService
{
    /// <summary>Kaynak belgeye bağlı bir borç (payable) kaydı oluşturur.</summary>
    Task<Guid> CreatePayableAsync(Guid partnerId, Guid currencyId, decimal amount, DateTime dueDate,
        string? relatedModule, string? relatedEntityType, Guid? relatedEntityId, CancellationToken ct = default);

    /// <summary>Kaynak belgeye bağlı bir alacak (receivable) kaydı oluşturur.</summary>
    Task<Guid> CreateReceivableAsync(Guid partnerId, Guid currencyId, decimal amount, DateTime dueDate,
        string? relatedModule, string? relatedEntityType, Guid? relatedEntityId, CancellationToken ct = default);

    /// <summary>Bir ödemeyi birden fazla borca dağıtır (parçalı kapama).</summary>
    Task AllocatePaymentAsync(Guid paymentId, IReadOnlyList<FinanceAllocationLine> allocations, CancellationToken ct = default);

    /// <summary>Bir tahsilatı birden fazla alacağa dağıtır (parçalı kapama).</summary>
    Task AllocateCollectionAsync(Guid collectionId, IReadOnlyList<FinanceAllocationLine> allocations, CancellationToken ct = default);

    /// <summary>Onaylanan puantajdan proje bazlı işçilik maliyet hareketi üretir.</summary>
    Task<Guid> PostTimesheetCostAsync(Guid timesheetId, Guid currencyId, CancellationToken ct = default);

    /// <summary>Onaylanan hakedişten sözleşme türüne göre alacak/borç üretir.</summary>
    Task<Guid> PostProgressPaymentAsync(Guid progressPaymentId, CancellationToken ct = default);

    /// <summary>Bütçe planlanan/gerçekleşen karşılaştırması; aşım varsa bildirim üretir.</summary>
    Task<bool> CheckBudgetOverrunAsync(Guid budgetId, CancellationToken ct = default);
}

