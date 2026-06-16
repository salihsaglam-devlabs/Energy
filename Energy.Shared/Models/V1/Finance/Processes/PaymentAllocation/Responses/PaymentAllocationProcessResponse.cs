namespace Energy.Shared.Models.V1.Finance.Processes.PaymentAllocation.Responses;

/// <summary>Ödeme tahsis sürecinin sonucu: tahsis edilen satır sayısı ve toplam tutar.</summary>
public sealed class PaymentAllocationProcessResponse
{
    /// <summary>Tahsis edilen satır sayısı.</summary>
    public int AllocatedLineCount { get; set; }

    /// <summary>Tahsis edilen toplam tutar.</summary>
    public decimal TotalAllocated { get; set; }
}
