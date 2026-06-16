namespace Energy.Shared.Models.V1.Inventory.Processes.StockIssue.Responses;

/// <summary>Stok çıkış sürecinin sonucu: FIFO toplam maliyet ve tahsis satır sayısı.</summary>
public sealed class StockIssueProcessResponse
{
    /// <summary>FIFO dağıtımıyla hesaplanan toplam maliyet.</summary>
    public decimal TotalCost { get; set; }

    /// <summary>Çıkışın dağıtıldığı lot/tahsis satırı sayısı.</summary>
    public int AllocationCount { get; set; }
}
