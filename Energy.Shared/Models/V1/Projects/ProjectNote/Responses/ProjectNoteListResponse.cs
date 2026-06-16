namespace Energy.Shared.Models.V1.Projects.ProjectNote.Responses;

/// <summary>ProjectNote liste satırı.</summary>
public class ProjectNoteListResponse
{
    /// <summary>Kimlik.</summary>
    public Guid Id { get; set; }

    /// <summary>ProjectId</summary>
    public Guid ProjectId { get; set; }

    /// <summary>Title</summary>
    public string Title { get; set; } = string.Empty;

    /// <summary>Body</summary>
    public string? Body { get; set; }

    /// <summary>Oluşturma zamanı.</summary>
    public DateTime CreatedAt { get; set; }
}
