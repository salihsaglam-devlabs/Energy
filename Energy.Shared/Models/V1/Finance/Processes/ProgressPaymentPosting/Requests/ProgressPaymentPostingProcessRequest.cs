using System.ComponentModel.DataAnnotations;

namespace Energy.Shared.Models.V1.Finance.Processes.ProgressPaymentPosting.Requests;

/// <summary>Hakediş muhasebeleştirme süreç isteği (Contracts akışı).</summary>
public sealed class ProgressPaymentPostingProcessRequest
{
    /// <summary>Muhasebeleştirilecek hakediş (ProgressPayment) kimliği.</summary>
    [Required]
    public Guid ProgressPaymentId { get; set; }
}
