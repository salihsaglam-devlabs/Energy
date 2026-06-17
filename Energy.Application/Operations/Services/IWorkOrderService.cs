using Energy.Shared.Common;
using Energy.Domain.Common;

namespace Energy.Application.Operations.Services;

/// <summary>
/// İş emri iş kuralları: durum geçişleri (geçmiş kaydıyla), zorunlu checklist
/// tamamlanmadan kapatma engeli ve kontrollü reopen.
/// </summary>
public interface IWorkOrderService
{
    /// <summary>İş emri durumunu değiştirir ve <c>WorkOrderStatusHistories</c>'e geçmiş yazar.</summary>
    Task ChangeStatusAsync(Guid workOrderId, WorkOrderStatus newStatus, string? note = null, CancellationToken ct = default);

    /// <summary>
    /// İş emrini kapatır. Zorunlu checklist kalemlerinin tümü tamamlanmadan kapatma
    /// engellenir (InvalidOperationException).
    /// </summary>
    Task CloseAsync(Guid workOrderId, string? note = null, CancellationToken ct = default);

    /// <summary>Kapalı iş emrini kontrollü biçimde yeniden açar.</summary>
    Task ReopenAsync(Guid workOrderId, string? note = null, CancellationToken ct = default);
}

