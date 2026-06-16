namespace Energy.Shared.Models.V1.Finance.FinancialTransaction.Responses;

/// <summary>FinancialTransaction detay görünümü.</summary>
public class FinancialTransactionDetailResponse
{
    /// <summary>Kimlik.</summary>
    public Guid Id { get; set; }

    /// <summary>Oluşturma zamanı</summary>
    public DateTime CreatedAt { get; set; }

    /// <summary>Oluşturan kullanıcı</summary>
    public Guid? CreatedBy { get; set; }

    /// <summary>Son güncelleme zamanı</summary>
    public DateTime? UpdatedAt { get; set; }

    /// <summary>Güncelleyen kullanıcı</summary>
    public Guid? UpdatedBy { get; set; }

    /// <summary>Soft delete bayrağı</summary>
    public bool IsDeleted { get; set; }

    /// <summary>Silinme zamanı</summary>
    public DateTime? DeletedAt { get; set; }

    /// <summary>Silen kullanıcı</summary>
    public Guid? DeletedBy { get; set; }

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
