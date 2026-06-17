namespace Energy.Shared.Models.V1.Core.Department.Requests;

/// <summary>Department oluşturma isteği.</summary>
public class CreateDepartmentRequest
{
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
