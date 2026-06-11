using Energy.Shared.Models.V1.Common.Requests;
using Energy.Shared.Models.V1.Common.Responses;

namespace Energy.Application.Common.Pagination;

public static class PaginationExtensions
{
    /// <summary>
    /// Applies the search, sort, and page slice described by <paramref name="request"/>
    /// to an in-memory sequence and wraps the result in a <see cref="PaginatedResponse{T}"/>.
    /// </summary>
    public static PaginatedResponse<T> ToPaginatedResponse<T>(
        this IEnumerable<T> source,
        PaginatedRequest request,
        Func<T, string, bool>? searchPredicate = null,
        IReadOnlyDictionary<string, Func<T, object?>>? sortSelectors = null)
    {
        IEnumerable<T> query = source;

        if (!string.IsNullOrWhiteSpace(request.Search) && searchPredicate is not null)
        {
            var term = request.Search!;
            query = query.Where(item => searchPredicate(item, term));
        }

        if (!string.IsNullOrWhiteSpace(request.SortBy)
            && sortSelectors is not null
            && sortSelectors.TryGetValue(request.SortBy!, out var selector))
        {
            query = request.IsDescending
                ? query.OrderByDescending(selector)
                : query.OrderBy(selector);
        }

        var materialized = query as IList<T> ?? query.ToList();
        var totalCount = materialized.Count;

        var items = materialized
            .Skip((request.PageNumber - 1) * request.PageSize)
            .Take(request.PageSize)
            .ToList();

        return PaginatedResponse<T>.Create(items, request.PageNumber, request.PageSize, totalCount);
    }
}
