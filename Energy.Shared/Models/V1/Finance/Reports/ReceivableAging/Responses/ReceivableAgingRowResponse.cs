namespace Energy.Shared.Models.V1.Finance.Reports.ReceivableAging.Responses;

/// <summary>ReceivableAging raporu satırı (salt-okunur projeksiyon).</summary>
public sealed class ReceivableAgingRowResponse
{
    /// <summary>Kaynak kayıt kimliği.</summary>
    public Guid Id { get; set; }

    /// <summary>PartnerId</summary>
    public Guid PartnerId { get; set; }

    /// <summary>CurrencyId</summary>
    public Guid CurrencyId { get; set; }

    /// <summary>Amount</summary>
    public decimal Amount { get; set; }

    /// <summary>RemainingAmount</summary>
    public decimal RemainingAmount { get; set; }

    /// <summary>DueDate</summary>
    public DateTime DueDate { get; set; }

    /// <summary>IsClosed</summary>
    public bool IsClosed { get; set; }
}
