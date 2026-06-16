namespace Energy.Shared.Models.V1.Contracts.ContractLine.Requests;

/// <summary>ContractLine oluşturma isteği.</summary>
public class CreateContractLineRequest
{
    /// <summary>ContractId</summary>
    public Guid ContractId { get; set; }

    /// <summary>Description</summary>
    public string Description { get; set; } = string.Empty;

    /// <summary>Quantity</summary>
    public decimal Quantity { get; set; }

    /// <summary>UnitPrice</summary>
    public decimal UnitPrice { get; set; }
}
