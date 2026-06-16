using Energy.Shared.Common;
namespace Energy.Shared.Models.V1.Workflow.ApprovalStepDefinition.Responses;

/// <summary>ApprovalStepDefinition detay görünümü.</summary>
public class ApprovalStepDefinitionDetailResponse
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
