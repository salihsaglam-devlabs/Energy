namespace Energy.Shared.Models.V1.Finance.CostCenter.Requests;

/// <summary>CostCenter güncelleme isteği.</summary>
public class UpdateCostCenterRequest
{
    /// <summary>Güncellenecek kaydın kimliği.</summary>
    public Guid Id { get; set; }

    /// <summary>ParentCostCenterId</summary>
    public Guid? ParentCostCenterId { get; set; }

    /// <summary>Code</summary>
    public string Code { get; set; } = string.Empty;

    /// <summary>Name</summary>
    public string Name { get; set; } = string.Empty;

    /// <summary>IsActive</summary>
    public bool IsActive { get; set; }
}
