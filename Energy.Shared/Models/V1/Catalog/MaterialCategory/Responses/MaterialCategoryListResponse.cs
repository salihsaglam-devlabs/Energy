namespace Energy.Shared.Models.V1.Catalog.MaterialCategory.Responses;

/// <summary>MaterialCategory liste satırı.</summary>
public class MaterialCategoryListResponse
{
    /// <summary>Kimlik.</summary>
    public Guid Id { get; set; }

    /// <summary>ParentCategoryId</summary>
    public Guid? ParentCategoryId { get; set; }

    /// <summary>Code</summary>
    public string Code { get; set; } = string.Empty;

    /// <summary>Name</summary>
    public string Name { get; set; } = string.Empty;

    /// <summary>IsActive</summary>
    public bool IsActive { get; set; }

    /// <summary>Oluşturma zamanı.</summary>
    public DateTime CreatedAt { get; set; }
}
