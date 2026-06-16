namespace Energy.Shared.Models.V1.Finance.Processes.ProgressPaymentPosting.Responses;

/// <summary>Hakediş muhasebeleştirme sürecinin sonucu: üretilen finansal hareket kimliği.</summary>
public sealed class ProgressPaymentPostingProcessResponse
{
    /// <summary>Üretilen finansal hareketin kimliği.</summary>
    public Guid FinancialTransactionId { get; set; }
}
