using Energy.Shared.Common;
namespace Energy.Shared.Models.V1.Workflow.ApprovalRequest.Responses;

/// <summary>ApprovalRequest liste satırı.</summary>
public class ApprovalRequestListResponse
{
    /// <summary>Kimlik.</summary>
    public Guid Id { get; set; }

    /// <summary>Akış versiyonu</summary>
    public Guid ApprovalDefinitionVersionId { get; set; }

    /// <summary>Kaynak modül</summary>
    public string RelatedModule { get; set; } = string.Empty;

    /// <summary>Kaynak nesne türü</summary>
    public string RelatedEntityType { get; set; } = string.Empty;

    /// <summary>Kaynak nesne</summary>
    public Guid RelatedEntityId { get; set; }

    /// <summary>Talep eden</summary>
    public Guid RequestedByUserId { get; set; }

    /// <summary>Durum</summary>
    public ApprovalRequestStatus Status { get; set; }

    /// <summary>CurrentStepNo</summary>
    public int CurrentStepNo { get; set; }

    /// <summary>Oluşturma zamanı.</summary>
    public DateTime CreatedAt { get; set; }
}
