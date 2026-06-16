using Energy.Domain.Common;

namespace Energy.Domain.Catalog;

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
