namespace Energy.Shared.Models.V1.Core.UnitConversion.Requests;

/// <summary>UnitConversion güncelleme isteği.</summary>
public class UpdateUnitConversionRequest
{
    /// <summary>Güncellenecek kaydın kimliği.</summary>
    public Guid Id { get; set; }

    /// <summary>FromUnitOfMeasureId</summary>
    public Guid FromUnitOfMeasureId { get; set; }

    /// <summary>ToUnitOfMeasureId</summary>
    public Guid ToUnitOfMeasureId { get; set; }

    /// <summary>Factor</summary>
    public decimal Factor { get; set; }
}
