using Energy.Shared.Common;
namespace Energy.Shared.Models.V1.Workflow.ApprovalCondition.Responses;

/// <summary>ApprovalCondition detay görünümü.</summary>
public class ApprovalConditionDetailResponse
{
    /// <summary>Kimlik.</summary>
    public Guid Id { get; set; }

    /// <summary>Oluşturma zamanı</summary>
    public DateTime CreatedAt { get; set; }

    /// <summary>Oluşturan kullanıcı</summary>
    public Guid? CreatedBy { get; set; }

    /// <summary>Son güncelleme zamanı</summary>
    public DateTime? UpdatedAt { get; set; }

    /// <summary>Güncelleyen kullanıcı</summary>
    public Guid? UpdatedBy { get; set; }

    /// <summary>Soft delete bayrağı</summary>
    public bool IsDeleted { get; set; }

    /// <summary>Silinme zamanı</summary>
    public DateTime? DeletedAt { get; set; }

    /// <summary>Silen kullanıcı</summary>
    public Guid? DeletedBy { get; set; }

    /// <summary>Akış versiyonu</summary>
    public Guid ApprovalDefinitionVersionId { get; set; }

    /// <summary>Koşul alanı</summary>
    public string FieldName { get; set; } = string.Empty;

    /// <summary>Karşılaştırma operatörü</summary>
    public ConditionOperator Operator { get; set; }

    /// <summary>Metin değer</summary>
    public string? ValueText { get; set; }

    /// <summary>Sayısal değer</summary>
    public decimal? ValueNumber { get; set; }
}
