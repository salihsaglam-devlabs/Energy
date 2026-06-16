using Energy.Domain.Common;

namespace Energy.Domain.Workflow;

/// <summary>Onay akışı tanımı.</summary>
public class ApprovalDefinition : AuditableEntity
{
    public string Code { get; set; } = string.Empty;
    public string Name { get; set; } = string.Empty;
    public string RelatedModule { get; set; } = string.Empty;
    public string RelatedEntityType { get; set; } = string.Empty;
    public bool IsActive { get; set; } = true;
}

/// <summary>Onay akışı versiyonu.</summary>
public class ApprovalDefinitionVersion : AuditableEntity
{
    public Guid ApprovalDefinitionId { get; set; }
    public int VersionNo { get; set; }
    public DateTime EffectiveFrom { get; set; }
    public DateTime? EffectiveTo { get; set; }
    public bool IsActive { get; set; } = true;
}

/// <summary>Onay adımı tanımı.</summary>
public class ApprovalStepDefinition : AuditableEntity
{
    public Guid ApprovalDefinitionVersionId { get; set; }
    public int StepNo { get; set; }
    public string Name { get; set; } = string.Empty;
    public ApprovalMode ApprovalMode { get; set; } = ApprovalMode.Sequential;
    public int? RequiredApprovalCount { get; set; }
    public bool IsRequired { get; set; } = true;
}

/// <summary>Adım bazlı onaycı (kullanıcı, rol veya departman).</summary>
public class ApprovalStepApprover : AuditableEntity
{
    public Guid ApprovalStepDefinitionId { get; set; }
    public ApproverType ApproverType { get; set; }
    public Guid? ApproverUserId { get; set; }
    public Guid? ApproverRoleId { get; set; }
    public Guid? ApproverDepartmentId { get; set; }
}

/// <summary>Onay koşulu (tutar, proje, belge türü vb.).</summary>
public class ApprovalCondition : AuditableEntity
{
    public Guid ApprovalDefinitionVersionId { get; set; }
    public string FieldName { get; set; } = string.Empty;
    public ConditionOperator Operator { get; set; }
    public string? ValueText { get; set; }
    public decimal? ValueNumber { get; set; }
}

/// <summary>Çalışan onay talebi.</summary>
public class ApprovalRequest : AuditableEntity
{
    public Guid ApprovalDefinitionVersionId { get; set; }
    public string RelatedModule { get; set; } = string.Empty;
    public string RelatedEntityType { get; set; } = string.Empty;
    public Guid RelatedEntityId { get; set; }
    public Guid RequestedByUserId { get; set; }
    public ApprovalRequestStatus Status { get; set; } = ApprovalRequestStatus.Draft;
    public int CurrentStepNo { get; set; }
}

/// <summary>Talebe bağlı adım örneği.</summary>
public class ApprovalRequestStep : AuditableEntity
{
    public Guid ApprovalRequestId { get; set; }
    public Guid ApprovalStepDefinitionId { get; set; }
    public int StepNo { get; set; }
    public ApprovalMode ApprovalMode { get; set; } = ApprovalMode.Sequential;
    public int? RequiredApprovalCount { get; set; }
    public ApprovalStepStatus Status { get; set; } = ApprovalStepStatus.Waiting;
}

/// <summary>Adımın gerçek onaycıları (snapshot).</summary>
public class ApprovalRequestApprover : AuditableEntity
{
    public Guid ApprovalRequestStepId { get; set; }
    public Guid UserId { get; set; }
    public ApprovalApproverStatus Status { get; set; } = ApprovalApproverStatus.Waiting;
    public DateTime? ActionAt { get; set; }
    /// <summary>Devralan kullanıcı (delegation çözümlendiyse).</summary>
    public Guid? DelegatedFromUserId { get; set; }
}

/// <summary>Onay, ret, iade ve iptal hareketleri.</summary>
public class ApprovalAction : AuditableEntity
{
    public Guid ApprovalRequestId { get; set; }
    public Guid? ApprovalRequestStepId { get; set; }
    public Guid UserId { get; set; }
    public ApprovalActionType ActionType { get; set; }
    public DateTime ActionAt { get; set; }
    public string? Note { get; set; }
}

/// <summary>Geçici onay yetkisi devri.</summary>
public class ApprovalDelegation : AuditableEntity
{
    public Guid DelegatorUserId { get; set; }
    public Guid DelegateUserId { get; set; }
    public DateTime StartDate { get; set; }
    public DateTime EndDate { get; set; }
    public bool IsActive { get; set; } = true;
}

