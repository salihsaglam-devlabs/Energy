namespace Energy.Shared.Models.V1.Finance.CostCenter.Responses;

/// <summary>CostCenter liste satırı.</summary>
public class CostCenterListResponse
{
    /// <summary>Kimlik.</summary>
    public Guid Id { get; set; }

    /// <summary>ParentCostCenterId</summary>
    public Guid? ParentCostCenterId { get; set; }

    /// <summary>Code</summary>
    public string Code { get; set; } = string.Empty;

    /// <summary>Name</summary>
    public string Name { get; set; } = string.Empty;

    /// <summary>IsActive</summary>
    public bool IsActive { get; set; }

    /// <summary>Oluşturma zamanı.</summary>
    public DateTime CreatedAt { get; set; }
}
