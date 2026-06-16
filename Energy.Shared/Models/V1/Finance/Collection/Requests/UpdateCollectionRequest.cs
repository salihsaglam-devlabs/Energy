using Energy.Shared.Common;
namespace Energy.Shared.Models.V1.Finance.Collection.Requests;

/// <summary>Collection güncelleme isteği.</summary>
public class UpdateCollectionRequest
{
    /// <summary>Güncellenecek kaydın kimliği.</summary>
    public Guid Id { get; set; }

    /// <summary>PartnerId</summary>
    public Guid PartnerId { get; set; }

    /// <summary>CurrencyId</summary>
    public Guid CurrencyId { get; set; }

    /// <summary>FinancialAccountId</summary>
    public Guid? FinancialAccountId { get; set; }

    /// <summary>Amount</summary>
    public decimal Amount { get; set; }

    /// <summary>CollectionDate</summary>
    public DateTime CollectionDate { get; set; }

    /// <summary>CollectionNo</summary>
    public string CollectionNo { get; set; } = string.Empty;

    /// <summary>Status</summary>
    public ApprovalRequestStatus Status { get; set; }

    /// <summary>ApprovalRequestId</summary>
    public Guid? ApprovalRequestId { get; set; }
}
