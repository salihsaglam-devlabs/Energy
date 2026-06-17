using Energy.Shared.Common;
namespace Energy.Shared.Models.V1.Finance.FinancialTransaction.Responses;

/// <summary>FinancialTransaction liste satırı.</summary>
public class FinancialTransactionListResponse
{
    /// <summary>Kimlik.</summary>
    public Guid Id { get; set; }

    /// <summary>Finans hareket türü</summary>
    public FinancialTransactionType TransactionType { get; set; }

    /// <summary>Opsiyonel proje</summary>
    public Guid? ProjectId { get; set; }

    /// <summary>Opsiyonel cari</summary>
    public Guid? PartnerId { get; set; }

    /// <summary>Para birimi</summary>
    public Guid CurrencyId { get; set; }

    /// <summary>Tutar</summary>
    public decimal Amount { get; set; }

    /// <summary>Kaynak modül</summary>
    public string? RelatedModule { get; set; }

    /// <summary>Kaynak nesne türü</summary>
    public string? RelatedEntityType { get; set; }

    /// <summary>Kaynak nesne</summary>
    public Guid? RelatedEntityId { get; set; }

    /// <summary>FinancialAccountId</summary>
    public Guid? FinancialAccountId { get; set; }

    /// <summary>CostCenterId</summary>
    public Guid? CostCenterId { get; set; }

    /// <summary>TransactionDate</summary>
    public DateTime TransactionDate { get; set; }

    /// <summary>Description</summary>
    public string? Description { get; set; }

    /// <summary>IsReversed</summary>
    public bool IsReversed { get; set; }

    /// <summary>Oluşturma zamanı.</summary>
    public DateTime CreatedAt { get; set; }
}
