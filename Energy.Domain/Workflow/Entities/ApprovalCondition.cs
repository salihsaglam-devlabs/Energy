using Energy.Shared.Common;
using Energy.Domain.Common;

namespace Energy.Domain.Workflow;

/// <summary>Onay koşulu (tutar, proje, belge türü vb.).</summary>
public class ApprovalCondition : AuditableEntity
{
    public Guid ApprovalDefinitionVersionId { get; set; }
    public string FieldName { get; set; } = string.Empty;
    public ConditionOperator Operator { get; set; }
    public string? ValueText { get; set; }
    public decimal? ValueNumber { get; set; }
}
