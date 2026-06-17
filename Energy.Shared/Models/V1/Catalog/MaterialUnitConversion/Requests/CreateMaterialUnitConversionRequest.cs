namespace Energy.Shared.Models.V1.Catalog.MaterialUnitConversion.Requests;

/// <summary>MaterialUnitConversion oluşturma isteği.</summary>
public class CreateMaterialUnitConversionRequest
{
    /// <summary>MaterialId</summary>
    public Guid MaterialId { get; set; }

    /// <summary>FromUnitOfMeasureId</summary>
    public Guid FromUnitOfMeasureId { get; set; }

    /// <summary>ToUnitOfMeasureId</summary>
    public Guid ToUnitOfMeasureId { get; set; }

    /// <summary>Factor</summary>
    public decimal Factor { get; set; }
}
