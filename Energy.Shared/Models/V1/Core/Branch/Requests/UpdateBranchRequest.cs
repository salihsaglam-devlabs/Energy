namespace Energy.Shared.Models.V1.Core.Branch.Requests;

/// <summary>Branch güncelleme isteği.</summary>
public class UpdateBranchRequest
{
    /// <summary>Güncellenecek kaydın kimliği.</summary>
    public Guid Id { get; set; }

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
