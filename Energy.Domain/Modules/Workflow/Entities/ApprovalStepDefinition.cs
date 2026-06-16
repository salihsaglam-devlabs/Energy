using Energy.Domain.Common;

namespace Energy.Domain.Modules.Workflow;

/// <summary>
/// Onay adımı tanımları
/// </summary>
public class ApprovalStepDefinition : AuditableEntity
{
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
