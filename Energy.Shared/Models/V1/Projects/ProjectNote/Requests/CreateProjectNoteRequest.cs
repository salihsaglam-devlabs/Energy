namespace Energy.Shared.Models.V1.Projects.ProjectNote.Requests;

/// <summary>ProjectNote oluşturma isteği.</summary>
public class CreateProjectNoteRequest
{
    /// <summary>ProjectId</summary>
    public Guid ProjectId { get; set; }

    /// <summary>Title</summary>
    public string Title { get; set; } = string.Empty;

    /// <summary>Body</summary>
    public string? Body { get; set; }
}
