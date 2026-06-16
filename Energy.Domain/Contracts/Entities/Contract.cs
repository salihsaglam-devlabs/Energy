using Energy.Shared.Common;
using Energy.Domain.Common;

namespace Energy.Domain.Contracts;

/// <summary>Sözleşme.</summary>
public class Contract : AuditableEntity
{
    public ContractType ContractType { get; set; }
    public Guid? ProjectId { get; set; }
    public Guid CurrencyId { get; set; }
    public string ContractNo { get; set; } = string.Empty;
    public string Title { get; set; } = string.Empty;
    public decimal? ContractAmount { get; set; }
    public DateTime? StartDate { get; set; }
    public DateTime? EndDate { get; set; }
    public DocumentStatus Status { get; set; } = DocumentStatus.Draft;
}
