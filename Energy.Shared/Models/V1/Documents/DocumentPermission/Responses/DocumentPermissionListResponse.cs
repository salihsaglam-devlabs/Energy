namespace Energy.Shared.Models.V1.Documents.DocumentPermission.Responses;

/// <summary>DocumentPermission liste satırı.</summary>
public class DocumentPermissionListResponse
{
    /// <summary>Kimlik.</summary>
    public Guid Id { get; set; }

    /// <summary>DocumentId</summary>
    public Guid DocumentId { get; set; }

    /// <summary>UserId</summary>
    public Guid? UserId { get; set; }

    /// <summary>RoleId</summary>
    public Guid? RoleId { get; set; }

    /// <summary>AccessType</summary>
    public string AccessType { get; set; } = string.Empty;

    /// <summary>Oluşturma zamanı.</summary>
    public DateTime CreatedAt { get; set; }
}
