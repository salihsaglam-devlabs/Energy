namespace Energy.Shared.Models.V1.Projects.Project.Responses;

/// <summary>Project detay görünümü.</summary>
public class ProjectDetailResponse
{
    /// <summary>Kimlik.</summary>
    public Guid Id { get; set; }

    /// <summary>Oluşturma zamanı</summary>
    public DateTime CreatedAt { get; set; }

    /// <summary>Oluşturan kullanıcı</summary>
    public Guid? CreatedBy { get; set; }

    /// <summary>Son güncelleme zamanı</summary>
    public DateTime? UpdatedAt { get; set; }

    /// <summary>Güncelleyen kullanıcı</summary>
    public Guid? UpdatedBy { get; set; }

    /// <summary>Soft delete bayrağı</summary>
    public bool IsDeleted { get; set; }

    /// <summary>Silinme zamanı</summary>
    public DateTime? DeletedAt { get; set; }

    /// <summary>Silen kullanıcı</summary>
    public Guid? DeletedBy { get; set; }

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
