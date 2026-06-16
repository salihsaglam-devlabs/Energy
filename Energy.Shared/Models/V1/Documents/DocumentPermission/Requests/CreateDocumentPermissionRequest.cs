namespace Energy.Shared.Models.V1.Documents.DocumentPermission.Requests;

/// <summary>DocumentPermission oluşturma isteği.</summary>
public class CreateDocumentPermissionRequest
{
    /// <summary>DocumentId</summary>
    public Guid DocumentId { get; set; }

    /// <summary>UserId</summary>
    public Guid? UserId { get; set; }

    /// <summary>RoleId</summary>
    public Guid? RoleId { get; set; }

    /// <summary>AccessType</summary>
    public string AccessType { get; set; } = string.Empty;
}
