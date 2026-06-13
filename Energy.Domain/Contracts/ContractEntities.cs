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

/// <summary>Sözleşme tarafı.</summary>
public class ContractParty : AuditableEntity
{
    public Guid ContractId { get; set; }
    public Guid BusinessPartnerId { get; set; }
    public string PartyRole { get; set; } = string.Empty;
}

/// <summary>Sözleşme kalemi.</summary>
public class ContractLine : AuditableEntity
{
    public Guid ContractId { get; set; }
    public string Description { get; set; } = string.Empty;
    public decimal Quantity { get; set; }
    public decimal UnitPrice { get; set; }
}

/// <summary>Ek protokol.</summary>
public class ContractAmendment : AuditableEntity
{
    public Guid ContractId { get; set; }
    public string AmendmentNo { get; set; } = string.Empty;
    public DateTime AmendmentDate { get; set; }
    public string? Description { get; set; }
    public decimal AmountDelta { get; set; }
}

