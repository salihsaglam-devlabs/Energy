namespace Energy.Shared.Models.V1.Core.Branch.Requests;

/// <summary>Branch oluşturma isteği.</summary>
public class CreateBranchRequest
{
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
