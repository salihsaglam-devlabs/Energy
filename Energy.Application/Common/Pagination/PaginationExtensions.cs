using Energy.Shared.Models.V1.Common.Requests;
using Energy.Shared.Models.V1.Common.Responses;

namespace Energy.Application.Common.Pagination;

/// <summary>Bellek içi (in-memory) dizilere sayfalama/arama/sıralama uygulayan yardımcı metotlar.</summary>
public static class PaginationExtensions
{
    /// <summary>
    /// <paramref name="request"/> ile tarif edilen arama, sıralama ve sayfa dilimini
    /// bellek içi bir diziye uygular ve sonucu bir <see cref="PaginatedResponse{T}"/>
    /// içine sarar.
    /// </summary>
    public static PaginatedResponse<T> ToPaginatedResponse<T>(
        this IEnumerable<T> source,
        PaginatedRequest request,
        Func<T, string, bool>? searchPredicate = null,
        IReadOnlyDictionary<string, Func<T, object?>>? sortSelectors = null)
    {
        IEnumerable<T> query = source;

        // Arama terimi ve bir arama yüklemi (predicate) verildiyse filtreyi uygula.
        if (!string.IsNullOrWhiteSpace(request.Search) && searchPredicate is not null)
        {
            var term = request.Search!;
            query = query.Where(item => searchPredicate(item, term));
        }

        // İstenen sütun için bir sıralama seçici (selector) varsa sıralamayı uygula.
        if (!string.IsNullOrWhiteSpace(request.SortBy)
            && sortSelectors is not null
            && sortSelectors.TryGetValue(request.SortBy!, out var selector))
        {
            query = request.IsDescending
                ? query.OrderByDescending(selector)
                : query.OrderBy(selector);
        }

        // Toplam sayıyı bir kez hesaplamak için diziyi tek seferde maddeleştir.
        var materialized = query as IList<T> ?? query.ToList();
        var totalCount = materialized.Count;

        // İstenen sayfa dilimini al.
        var items = materialized
            .Skip((request.PageNumber - 1) * request.PageSize)
            .Take(request.PageSize)
            .ToList();

        return PaginatedResponse<T>.Create(items, request.PageNumber, request.PageSize, totalCount);
    }
}
