namespace Energy.Shared.Models.V1.Documents.DocumentFolder.Requests;

/// <summary>DocumentFolder oluşturma isteği.</summary>
public class CreateDocumentFolderRequest
{
    /// <summary>ParentFolderId</summary>
    public Guid? ParentFolderId { get; set; }

    /// <summary>Name</summary>
    public string Name { get; set; } = string.Empty;
}
