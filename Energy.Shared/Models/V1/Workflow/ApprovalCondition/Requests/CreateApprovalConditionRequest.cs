namespace Energy.Shared.Models.V1.Workflow.ApprovalCondition.Requests;

/// <summary>ApprovalCondition oluşturma isteği.</summary>
public class CreateApprovalConditionRequest
{
    /// <summary>Akış versiyonu</summary>
    public Guid ApprovalDefinitionVersionId { get; set; }

    /// <summary>Koşul alanı</summary>
    public string FieldName { get; set; } = string.Empty;

    /// <summary>Karşılaştırma operatörü</summary>
    public string Operator { get; set; } = string.Empty;

    /// <summary>Metin değer</summary>
    public string? ValueText { get; set; }

    /// <summary>Sayısal değer</summary>
    public decimal? ValueNumber { get; set; }
}
