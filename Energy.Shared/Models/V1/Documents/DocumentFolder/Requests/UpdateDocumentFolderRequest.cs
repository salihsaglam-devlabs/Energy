namespace Energy.Shared.Models.V1.Documents.DocumentFolder.Requests;

/// <summary>DocumentFolder güncelleme isteği.</summary>
public class UpdateDocumentFolderRequest
{
    /// <summary>Güncellenecek kaydın kimliği.</summary>
    public Guid Id { get; set; }

    /// <summary>ParentFolderId</summary>
    public Guid? ParentFolderId { get; set; }

    /// <summary>Name</summary>
    public string Name { get; set; } = string.Empty;
}
