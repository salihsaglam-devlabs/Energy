namespace Energy.Shared.Models.V1.Documents.Document.Requests;

/// <summary>Document oluşturma isteği.</summary>
public class CreateDocumentRequest
{
    /// <summary>DocumentFolderId</summary>
    public Guid? DocumentFolderId { get; set; }

    /// <summary>Name</summary>
    public string Name { get; set; } = string.Empty;

    /// <summary>Description</summary>
    public string? Description { get; set; }

    /// <summary>Status</summary>
    public string Status { get; set; } = string.Empty;

    /// <summary>CurrentVersionNo</summary>
    public int CurrentVersionNo { get; set; }
}
