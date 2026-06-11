namespace Energy.Shared.Models.V1.Common.Requests;

public class PaginatedRequest
{
    private const int DefaultPageNumber = 1;
    private const int DefaultPageSize = 10;
    private const int MaxPageSize = 100;

    private int _pageNumber = DefaultPageNumber;
    private int _pageSize = DefaultPageSize;

    public int PageNumber
    {
        get => _pageNumber;
        set => _pageNumber =
            value < 1
                ? DefaultPageNumber
                : value;
    }

    public int PageSize
    {
        get => _pageSize;
        set => _pageSize =
            value < 1
                ? DefaultPageSize
                : value > MaxPageSize
                    ? MaxPageSize
                    : value;
    }

    public string? Search { get; set; }

    public string? SortBy { get; set; }

    public bool IsDescending { get; set; }

    public Dictionary<string, string>? Filters { get; set; }
}

