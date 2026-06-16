namespace Energy.Shared.Models.V1.Catalog.MaterialCategory.Requests;

/// <summary>MaterialCategory güncelleme isteği.</summary>
public class UpdateMaterialCategoryRequest
{
    /// <summary>Güncellenecek kaydın kimliği.</summary>
    public Guid Id { get; set; }

    /// <summary>ParentCategoryId</summary>
    public Guid? ParentCategoryId { get; set; }

    /// <summary>Code</summary>
    public string Code { get; set; } = string.Empty;

    /// <summary>Name</summary>
    public string Name { get; set; } = string.Empty;

    /// <summary>IsActive</summary>
    public bool IsActive { get; set; }
}
