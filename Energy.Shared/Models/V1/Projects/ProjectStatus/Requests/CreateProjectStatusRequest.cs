namespace Energy.Shared.Models.V1.Projects.ProjectStatus.Requests;

/// <summary>ProjectStatus oluşturma isteği.</summary>
public class CreateProjectStatusRequest
{
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
}
