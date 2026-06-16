namespace Energy.Shared.Models.V1.Catalog.MaterialAttributeValue.Requests;

/// <summary>MaterialAttributeValue güncelleme isteği.</summary>
public class UpdateMaterialAttributeValueRequest
{
    /// <summary>Güncellenecek kaydın kimliği.</summary>
    public Guid Id { get; set; }

    /// <summary>Malzeme</summary>
    public Guid MaterialId { get; set; }

    /// <summary>Öznitelik</summary>
    public Guid MaterialAttributeDefinitionId { get; set; }

    /// <summary>Seçimli değer</summary>
    public Guid? OptionId { get; set; }

    /// <summary>Metin değer</summary>
    public string? ValueText { get; set; }

    /// <summary>Sayısal değer</summary>
    public decimal? ValueNumber { get; set; }

    /// <summary>ValueBoolean</summary>
    public bool? ValueBoolean { get; set; }

    /// <summary>ValueDate</summary>
    public DateTime? ValueDate { get; set; }
}
