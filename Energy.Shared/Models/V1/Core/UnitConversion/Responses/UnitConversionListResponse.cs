namespace Energy.Shared.Models.V1.Core.UnitConversion.Responses;

/// <summary>UnitConversion liste satırı.</summary>
public class UnitConversionListResponse
{
    /// <summary>Kimlik.</summary>
    public Guid Id { get; set; }

    /// <summary>FromUnitOfMeasureId</summary>
    public Guid FromUnitOfMeasureId { get; set; }

    /// <summary>ToUnitOfMeasureId</summary>
    public Guid ToUnitOfMeasureId { get; set; }

    /// <summary>Factor</summary>
    public decimal Factor { get; set; }

    /// <summary>Oluşturma zamanı.</summary>
    public DateTime CreatedAt { get; set; }
}
