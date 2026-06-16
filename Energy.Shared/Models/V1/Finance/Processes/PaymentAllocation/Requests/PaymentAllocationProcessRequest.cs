using System.ComponentModel.DataAnnotations;

namespace Energy.Shared.Models.V1.Finance.Processes.PaymentAllocation.Requests;

/// <summary>Bir ödemenin tek bir borca (payable) tahsis satırı.</summary>
public sealed class PaymentAllocationLineRequest
{
    /// <summary>Tahsisin uygulanacağı hedef borç (Payable) kimliği.</summary>
    [Required]
    public Guid TargetId { get; set; }

    /// <summary>Bu hedefe tahsis edilen tutar (pozitif).</summary>
    [Range(0.000001, double.MaxValue)]
    public decimal Amount { get; set; }
}

/// <summary>Ödeme tahsis (allocation) süreç isteği (Finance akışı).</summary>
public sealed class PaymentAllocationProcessRequest
{
    /// <summary>Tahsis edilecek ödeme (Payment) kimliği.</summary>
    [Required]
    public Guid PaymentId { get; set; }

    /// <summary>Tahsis satırları (hedef borç + tutar).</summary>
    [Required]
    [MinLength(1)]
    public List<PaymentAllocationLineRequest> Lines { get; set; } = [];
}
