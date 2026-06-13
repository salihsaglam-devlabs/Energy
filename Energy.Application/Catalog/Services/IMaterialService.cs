namespace Energy.Application.Catalog.Services;

/// <summary>
/// Malzeme iş kuralları: kategoriye bağlı dinamik öznitelik doğrulaması, zorunlu
/// öznitelikler tamamlanmadan aktive etme engeli ve stok hareketi görmüş malzemenin
/// baz biriminin değiştirilememesi.
/// </summary>
public interface IMaterialService
{
    /// <summary>Malzemenin kategorisindeki zorunlu/öznitelik kurallarını doğrular; hata listesini döndürür.</summary>
    Task<IReadOnlyList<string>> ValidateAttributesAsync(Guid materialId, CancellationToken ct = default);

    /// <summary>Zorunlu öznitelikler eksiksizse malzemeyi aktive eder; aksi halde hata fırlatır.</summary>
    Task ActivateAsync(Guid materialId, CancellationToken ct = default);

    /// <summary>Baz ölçü birimini değiştirir; malzeme stok hareketi görmüşse engellenir.</summary>
    Task ChangeBaseUnitOfMeasureAsync(Guid materialId, Guid newUnitOfMeasureId, CancellationToken ct = default);
}

