namespace Energy.Shared.Models.V1.Workflow.ApprovalDefinition.Requests;

/// <summary>ApprovalDefinition güncelleme isteği.</summary>
public class UpdateApprovalDefinitionRequest
{
    /// <summary>Güncellenecek kaydın kimliği.</summary>
    public Guid Id { get; set; }

    /// <summary>Akış kodu</summary>
    public string Code { get; set; } = string.Empty;

    /// <summary>Akış adı</summary>
    public string Name { get; set; } = string.Empty;

    /// <summary>İlgili modül</summary>
    public string RelatedModule { get; set; } = string.Empty;

    /// <summary>İlgili nesne türü</summary>
    public string RelatedEntityType { get; set; } = string.Empty;

    /// <summary>Aktiflik</summary>
    public bool IsActive { get; set; }
}
