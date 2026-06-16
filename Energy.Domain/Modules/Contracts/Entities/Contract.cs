using Energy.Domain.Common;

namespace Energy.Domain.Modules.Contracts;

/// <summary>
/// Sözleşmeler
/// </summary>
public class Contract : AuditableEntity
{
    /// <summary>Sözleşme türü</summary>
    public string ContractType { get; set; } = string.Empty;

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
    public string Status { get; set; } = string.Empty;
}
