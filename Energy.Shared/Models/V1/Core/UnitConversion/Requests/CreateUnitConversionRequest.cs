namespace Energy.Shared.Models.V1.Core.UnitConversion.Requests;

/// <summary>UnitConversion oluşturma isteği.</summary>
public class CreateUnitConversionRequest
{
    /// <summary>FromUnitOfMeasureId</summary>
    public Guid FromUnitOfMeasureId { get; set; }

    /// <summary>ToUnitOfMeasureId</summary>
    public Guid ToUnitOfMeasureId { get; set; }

    /// <summary>Factor</summary>
    public decimal Factor { get; set; }
}
