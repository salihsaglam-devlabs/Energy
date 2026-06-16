namespace Energy.Shared.Models.V1.Workflow.ApprovalStepDefinition.Requests;

/// <summary>ApprovalStepDefinition güncelleme isteği.</summary>
public class UpdateApprovalStepDefinitionRequest
{
    /// <summary>Güncellenecek kaydın kimliği.</summary>
    public Guid Id { get; set; }

    /// <summary>Akış versiyonu</summary>
    public Guid ApprovalDefinitionVersionId { get; set; }

    /// <summary>Sıra</summary>
    public int StepNo { get; set; }

    /// <summary>Sequential, ParallelAny, ParallelAll, Quorum</summary>
    public string ApprovalMode { get; set; } = string.Empty;

    /// <summary>Quorum için gerekli sayı</summary>
    public int? RequiredApprovalCount { get; set; }

    /// <summary>Zorunlu adım</summary>
    public bool IsRequired { get; set; }

    /// <summary>Name</summary>
    public string Name { get; set; } = string.Empty;
}
