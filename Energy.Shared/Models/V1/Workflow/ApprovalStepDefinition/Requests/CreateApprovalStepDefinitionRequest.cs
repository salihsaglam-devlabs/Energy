using Energy.Shared.Common;
namespace Energy.Shared.Models.V1.Workflow.ApprovalStepDefinition.Requests;

/// <summary>ApprovalStepDefinition oluşturma isteği.</summary>
public class CreateApprovalStepDefinitionRequest
{
    /// <summary>Akış versiyonu</summary>
    public Guid ApprovalDefinitionVersionId { get; set; }

    /// <summary>Sıra</summary>
    public int StepNo { get; set; }

    /// <summary>Sequential, ParallelAny, ParallelAll, Quorum</summary>
    public ApprovalMode ApprovalMode { get; set; }

    /// <summary>Quorum için gerekli sayı</summary>
    public int? RequiredApprovalCount { get; set; }

    /// <summary>Zorunlu adım</summary>
    public bool IsRequired { get; set; }

    /// <summary>Name</summary>
    public string Name { get; set; } = string.Empty;
}
