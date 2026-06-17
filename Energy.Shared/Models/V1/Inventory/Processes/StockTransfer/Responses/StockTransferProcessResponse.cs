namespace Energy.Shared.Models.V1.Inventory.Processes.StockTransfer.Responses;

/// <summary>Stok transfer sürecinin sonucu: taşınan FIFO maliyet toplamı.</summary>
public sealed class StockTransferProcessResponse
{
    /// <summary>Kaynak çıkışın FIFO toplam maliyeti (hedefe taşınan değer).</summary>
    public decimal TotalCost { get; set; }

    /// <summary>Çıkışın dağıtıldığı lot/tahsis satırı sayısı.</summary>
    public int AllocationCount { get; set; }
}
