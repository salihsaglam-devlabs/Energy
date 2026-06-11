namespace Energy.Shared.Models.V1.Common.Responses;

public class PaginatedResponse<T>
{
    public IReadOnlyCollection<T> Items { get; init; }
        = Array.Empty<T>();

    public int PageNumber { get; init; }

    public int PageSize { get; init; }

    public int TotalCount { get; init; }

    public int TotalPages =>
        PageSize <= 0
            ? 0
            : (int)Math.Ceiling((double)TotalCount / PageSize);

    public bool HasPreviousPage =>
        PageNumber > 1;

    public bool HasNextPage =>
        PageNumber < TotalPages;

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

