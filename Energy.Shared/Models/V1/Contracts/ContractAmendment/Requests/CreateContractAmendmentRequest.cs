namespace Energy.Shared.Models.V1.Contracts.ContractAmendment.Requests;

/// <summary>ContractAmendment oluşturma isteği.</summary>
public class CreateContractAmendmentRequest
{
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
}
