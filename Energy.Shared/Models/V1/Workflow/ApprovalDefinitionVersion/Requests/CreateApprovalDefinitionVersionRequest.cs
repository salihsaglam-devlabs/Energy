namespace Energy.Shared.Models.V1.Workflow.ApprovalDefinitionVersion.Requests;

/// <summary>ApprovalDefinitionVersion oluşturma isteği.</summary>
public class CreateApprovalDefinitionVersionRequest
{
    /// <summary>Onay tanımı</summary>
    public Guid ApprovalDefinitionId { get; set; }

    /// <summary>Versiyon</summary>
    public int VersionNo { get; set; }

    /// <summary>Başlangıç</summary>
    public DateTime EffectiveFrom { get; set; }

    /// <summary>Bitiş</summary>
    public DateTime? EffectiveTo { get; set; }

    /// <summary>IsActive</summary>
    public bool IsActive { get; set; }
}
