namespace Energy.Shared.Models.V1.Projects.ProjectType.Requests;

/// <summary>ProjectType oluşturma isteği.</summary>
public class CreateProjectTypeRequest
{
    /// <summary>Code</summary>
    public string Code { get; set; } = string.Empty;

    /// <summary>Name</summary>
    public string Name { get; set; } = string.Empty;

    /// <summary>IsActive</summary>
    public bool IsActive { get; set; }
}
