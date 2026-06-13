namespace Energy.Shared.Models.V1.Common.Responses;

/// <summary>
/// Sayfalanmış bir liste sonucu. Öğelerle birlikte sayfalama meta verilerini
/// (toplam sayı, sayfa sayısı, önceki/sonraki sayfa bilgisi) taşır.
/// </summary>
/// <typeparam name="T">Liste öğesi türü.</typeparam>
public class PaginatedResponse<T>
{
    /// <summary>Geçerli sayfadaki öğeler.</summary>
    public IReadOnlyCollection<T> Items { get; init; }
        = Array.Empty<T>();

    /// <summary>Geçerli sayfa numarası.</summary>
    public int PageNumber { get; init; }

    /// <summary>Sayfa başına kayıt sayısı.</summary>
    public int PageSize { get; init; }

    /// <summary>Filtre uygulandıktan sonraki toplam kayıt sayısı.</summary>
    public int TotalCount { get; init; }

    /// <summary>Toplam sayfa sayısı.</summary>
    public int TotalPages =>
        PageSize <= 0
            ? 0
            : (int)Math.Ceiling((double)TotalCount / PageSize);

    /// <summary>Önceki bir sayfa olup olmadığı.</summary>
    public bool HasPreviousPage =>
        PageNumber > 1;

    /// <summary>Sonraki bir sayfa olup olmadığı.</summary>
    public bool HasNextPage =>
        PageNumber < TotalPages;

    /// <summary>Verilen öğeler ve sayfalama bilgisinden bir yanıt oluşturur.</summary>
    public static PaginatedResponse<T> Create(
        IEnumerable<T> items,
        int pageNumber,
        int pageSize,
        int totalCount)
    {
        return new PaginatedResponse<T>
        {
            Items = items.ToList(),
            PageNumber = pageNumber,
            PageSize = pageSize,
            TotalCount = totalCount
        };
    }
}
