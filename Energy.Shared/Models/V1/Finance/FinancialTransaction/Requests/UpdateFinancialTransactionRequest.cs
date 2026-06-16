namespace Energy.Shared.Models.V1.Finance.FinancialTransaction.Requests;

/// <summary>FinancialTransaction güncelleme isteği.</summary>
public class UpdateFinancialTransactionRequest
{
    /// <summary>Güncellenecek kaydın kimliği.</summary>
    public Guid Id { get; set; }

    /// <summary>Finans hareket türü</summary>
    public string TransactionType { get; set; } = string.Empty;

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
}
