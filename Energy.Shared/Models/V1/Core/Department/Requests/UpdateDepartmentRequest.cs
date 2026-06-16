namespace Energy.Shared.Models.V1.Core.Department.Requests;

/// <summary>Department güncelleme isteği.</summary>
public class UpdateDepartmentRequest
{
    /// <summary>Güncellenecek kaydın kimliği.</summary>
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
}
