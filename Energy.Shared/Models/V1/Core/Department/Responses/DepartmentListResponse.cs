namespace Energy.Shared.Models.V1.Core.Department.Responses;

/// <summary>Department liste satırı.</summary>
public class DepartmentListResponse
{
    /// <summary>Kimlik.</summary>
    public Guid Id { get; set; }

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

    /// <summary>Oluşturma zamanı.</summary>
    public DateTime CreatedAt { get; set; }
}
