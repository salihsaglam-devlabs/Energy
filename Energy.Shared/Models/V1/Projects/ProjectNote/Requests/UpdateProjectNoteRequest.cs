namespace Energy.Shared.Models.V1.Projects.ProjectNote.Requests;

/// <summary>ProjectNote güncelleme isteği.</summary>
public class UpdateProjectNoteRequest
{
    /// <summary>Güncellenecek kaydın kimliği.</summary>
    public Guid Id { get; set; }

    /// <summary>ProjectId</summary>
    public Guid ProjectId { get; set; }

    /// <summary>Title</summary>
    public string Title { get; set; } = string.Empty;

    /// <summary>Body</summary>
    public string? Body { get; set; }
}
