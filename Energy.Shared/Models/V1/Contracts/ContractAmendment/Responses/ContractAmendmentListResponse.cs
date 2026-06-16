namespace Energy.Shared.Models.V1.Contracts.ContractAmendment.Responses;

/// <summary>ContractAmendment liste satırı.</summary>
public class ContractAmendmentListResponse
{
    /// <summary>Kimlik.</summary>
    public Guid Id { get; set; }

    /// <summary>ContractId</summary>
    public Guid ContractId { get; set; }

    /// <summary>AmendmentNo</summary>
    public string AmendmentNo { get; set; } = string.Empty;

    /// <summary>AmendmentDate</summary>
    public DateTime AmendmentDate { get; set; }

    /// <summary>Description</summary>
    public string? Description { get; set; }

    /// <summary>AmountDelta</summary>
    public decimal AmountDelta { get; set; }

    /// <summary>Oluşturma zamanı.</summary>
    public DateTime CreatedAt { get; set; }
}
