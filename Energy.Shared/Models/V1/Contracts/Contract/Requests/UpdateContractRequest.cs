using Energy.Shared.Common;
namespace Energy.Shared.Models.V1.Contracts.Contract.Requests;

/// <summary>Contract güncelleme isteği.</summary>
public class UpdateContractRequest
{
    /// <summary>Güncellenecek kaydın kimliği.</summary>
    public Guid Id { get; set; }

    /// <summary>Sözleşme türü</summary>
    public ContractType ContractType { get; set; }

    /// <summary>Opsiyonel proje</summary>
    public Guid? ProjectId { get; set; }

    /// <summary>Sözleşme no</summary>
    public string ContractNo { get; set; } = string.Empty;

    /// <summary>Para birimi</summary>
    public Guid CurrencyId { get; set; }

    /// <summary>Sözleşme bedeli</summary>
    public decimal? ContractAmount { get; set; }

    /// <summary>Title</summary>
    public string Title { get; set; } = string.Empty;

    /// <summary>StartDate</summary>
    public DateTime? StartDate { get; set; }

    /// <summary>EndDate</summary>
    public DateTime? EndDate { get; set; }

    /// <summary>Status</summary>
    public DocumentStatus Status { get; set; }
}
