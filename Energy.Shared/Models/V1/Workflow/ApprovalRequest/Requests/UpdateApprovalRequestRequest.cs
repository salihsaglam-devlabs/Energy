namespace Energy.Shared.Models.V1.Workflow.ApprovalRequest.Requests;

/// <summary>ApprovalRequest güncelleme isteği.</summary>
public class UpdateApprovalRequestRequest
{
    /// <summary>Güncellenecek kaydın kimliği.</summary>
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
    public string Status { get; set; } = string.Empty;

    /// <summary>CurrentStepNo</summary>
    public int CurrentStepNo { get; set; }
}
