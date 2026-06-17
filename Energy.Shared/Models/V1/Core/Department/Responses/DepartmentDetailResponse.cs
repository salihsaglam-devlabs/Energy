namespace Energy.Shared.Models.V1.Core.Department.Responses;

/// <summary>Department detay görünümü.</summary>
public class DepartmentDetailResponse
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

    /// <summary>Üst departman</summary>
    public Guid? ParentDepartmentId { get; set; }

    /// <summary>Departman kodu</summary>
    public string Code { get; set; } = string.Empty;

    /// <summary>Departman adı</summary>
    public string Name { get; set; } = string.Empty;

    /// <summary>ManagerUserId</summary>
    public Guid? ManagerUserId { get; set; }

    /// <summary>IsActive</summary>
    public bool IsActive { get; set; }
}
