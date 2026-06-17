using Energy.Shared.Common;
namespace Energy.Shared.Models.V1.Documents.Document.Responses;

/// <summary>Document liste satırı.</summary>
public class DocumentListResponse
{
    /// <summary>Kimlik.</summary>
    public Guid Id { get; set; }

    /// <summary>DocumentFolderId</summary>
    public Guid? DocumentFolderId { get; set; }

    /// <summary>Name</summary>
    public string Name { get; set; } = string.Empty;

    /// <summary>Description</summary>
    public string? Description { get; set; }

    /// <summary>Status</summary>
    public DocumentStatus Status { get; set; }

    /// <summary>CurrentVersionNo</summary>
    public int CurrentVersionNo { get; set; }

    /// <summary>Oluşturma zamanı.</summary>
    public DateTime CreatedAt { get; set; }
}
