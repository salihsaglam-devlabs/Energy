namespace Energy.Shared.Models.V1.Documents.DocumentFolder.Responses;

/// <summary>DocumentFolder liste satırı.</summary>
public class DocumentFolderListResponse
{
    /// <summary>Kimlik.</summary>
    public Guid Id { get; set; }

    /// <summary>ParentFolderId</summary>
    public Guid? ParentFolderId { get; set; }

    /// <summary>Name</summary>
    public string Name { get; set; } = string.Empty;

    /// <summary>Oluşturma zamanı.</summary>
    public DateTime CreatedAt { get; set; }
}
