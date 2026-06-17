namespace Energy.Shared.Models.V1.Finance.CostCenter.Requests;

/// <summary>CostCenter oluşturma isteği.</summary>
public class CreateCostCenterRequest
{
    /// <summary>ParentCostCenterId</summary>
    public Guid? ParentCostCenterId { get; set; }

    /// <summary>Code</summary>
    public string Code { get; set; } = string.Empty;

    /// <summary>Name</summary>
    public string Name { get; set; } = string.Empty;

    /// <summary>IsActive</summary>
    public bool IsActive { get; set; }
}
