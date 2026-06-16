namespace Energy.Shared.Models.V1.Projects.Project.Requests;

/// <summary>Project güncelleme isteği.</summary>
public class UpdateProjectRequest
{
    /// <summary>Güncellenecek kaydın kimliği.</summary>
    public Guid Id { get; set; }

    /// <summary>Şirket</summary>
    public Guid CompanyId { get; set; }

    /// <summary>Şube</summary>
    public Guid? BranchId { get; set; }

    /// <summary>Proje türü</summary>
    public Guid ProjectTypeId { get; set; }

    /// <summary>Durum</summary>
    public Guid StatusId { get; set; }

    /// <summary>Müşteri</summary>
    public Guid? CustomerId { get; set; }

    /// <summary>Proje yöneticisi</summary>
    public Guid? ManagerUserId { get; set; }

    /// <summary>Proje kodu</summary>
    public string Code { get; set; } = string.Empty;

    /// <summary>Proje adı</summary>
    public string Name { get; set; } = string.Empty;

    /// <summary>StartDate</summary>
    public DateTime? StartDate { get; set; }

    /// <summary>EndDate</summary>
    public DateTime? EndDate { get; set; }

    /// <summary>Description</summary>
    public string? Description { get; set; }
}
