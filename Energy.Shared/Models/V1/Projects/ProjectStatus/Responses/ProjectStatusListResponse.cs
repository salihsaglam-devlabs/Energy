namespace Energy.Shared.Models.V1.Projects.ProjectStatus.Responses;

/// <summary>ProjectStatus liste satırı.</summary>
public class ProjectStatusListResponse
{
    /// <summary>Kimlik.</summary>
    public Guid Id { get; set; }

    /// <summary>Code</summary>
    public string Code { get; set; } = string.Empty;

    /// <summary>Name</summary>
    public string Name { get; set; } = string.Empty;

    /// <summary>DisplayOrder</summary>
    public int DisplayOrder { get; set; }

    /// <summary>IsClosedState</summary>
    public bool IsClosedState { get; set; }

    /// <summary>IsActive</summary>
    public bool IsActive { get; set; }

    /// <summary>Oluşturma zamanı.</summary>
    public DateTime CreatedAt { get; set; }
}
