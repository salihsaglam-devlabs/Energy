namespace Energy.Shared.Models.V1.Core.Branch.Responses;

/// <summary>Branch detay görünümü.</summary>
public class BranchDetailResponse
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

    /// <summary>Şube kodu</summary>
    public string Code { get; set; } = string.Empty;

    /// <summary>Şube adı</summary>
    public string Name { get; set; } = string.Empty;

    /// <summary>Address</summary>
    public string? Address { get; set; }

    /// <summary>IsActive</summary>
    public bool IsActive { get; set; }
}
