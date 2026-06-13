using Energy.Domain.Common;

namespace Energy.Domain.Catalog;

/// <summary>Marka.</summary>
public class Brand : AuditableEntity
{
    public string Code { get; set; } = string.Empty;
    public string Name { get; set; } = string.Empty;
    public bool IsActive { get; set; } = true;
}

/// <summary>Malzeme kategori ağacı.</summary>
public class MaterialCategory : AuditableEntity
{
    public Guid? ParentCategoryId { get; set; }
    public string Code { get; set; } = string.Empty;
    public string Name { get; set; } = string.Empty;
    public bool IsActive { get; set; } = true;
}

/// <summary>Dinamik malzeme öznitelik tanımı.</summary>
public class MaterialAttributeDefinition : AuditableEntity
{
    public string Code { get; set; } = string.Empty;
    public string Name { get; set; } = string.Empty;
    /// <summary>Text, Number, Decimal, Boolean, Date, Option.</summary>
    public string DataType { get; set; } = "Text";
    public bool IsActive { get; set; } = true;
}

/// <summary>Seçimli (Option) öznitelik değeri.</summary>
public class MaterialAttributeOption : AuditableEntity
{
    public Guid MaterialAttributeDefinitionId { get; set; }
    public string Value { get; set; } = string.Empty;
    public int DisplayOrder { get; set; }
}

/// <summary>Kategori ↔ öznitelik bağlantısı.</summary>
public class MaterialCategoryAttribute : AuditableEntity
{
    public Guid MaterialCategoryId { get; set; }
    public Guid MaterialAttributeDefinitionId { get; set; }
    public bool IsRequired { get; set; }
    public int DisplayOrder { get; set; }
}

/// <summary>Malzeme kartı.</summary>
public class Material : AuditableEntity
{
    public Guid MaterialCategoryId { get; set; }
    public Guid? BrandId { get; set; }
    public Guid BaseUnitOfMeasureId { get; set; }
    public string Code { get; set; } = string.Empty;
    public string Name { get; set; } = string.Empty;
    public bool IsBatchTracked { get; set; }
    public bool IsSerialTracked { get; set; }
    public bool IsActive { get; set; }
}

/// <summary>Malzeme öznitelik değeri.</summary>
public class MaterialAttributeValue : AuditableEntity
{
    public Guid MaterialId { get; set; }
    public Guid MaterialAttributeDefinitionId { get; set; }
    public Guid? OptionId { get; set; }
    public string? ValueText { get; set; }
    public decimal? ValueNumber { get; set; }
    public bool? ValueBoolean { get; set; }
    public DateTime? ValueDate { get; set; }
}

/// <summary>Malzemeye özel birim dönüşümü.</summary>
public class MaterialUnitConversion : AuditableEntity
{
    public Guid MaterialId { get; set; }
    public Guid FromUnitOfMeasureId { get; set; }
    public Guid ToUnitOfMeasureId { get; set; }
    public decimal Factor { get; set; }
}

