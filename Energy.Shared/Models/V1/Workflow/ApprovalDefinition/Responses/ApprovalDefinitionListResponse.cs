namespace Energy.Shared.Models.V1.Workflow.ApprovalDefinition.Responses;

/// <summary>ApprovalDefinition liste satırı.</summary>
public class ApprovalDefinitionListResponse
{
    /// <summary>Kimlik.</summary>
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

    /// <summary>Oluşturma zamanı.</summary>
    public DateTime CreatedAt { get; set; }
}
