using Energy.Shared.Models.V1.Common.Requests;

namespace Energy.Web.Common;

/// <summary>
/// Translates the query string parameters DevExtreme's <c>CustomStore</c> sends
/// (<c>skip</c>, <c>take</c>, <c>sort</c>, <c>searchValue</c>) into the API's
/// <see cref="PaginatedRequest"/> shape.
/// </summary>
public sealed class GridLoadOptions
{
    public int Skip { get; set; }

    public int Take { get; set; } = 20;

    /// <summary>
    /// DevExtreme sends sort as a JSON array, e.g.
    /// <c>[{"selector":"name","desc":false}]</c>. We accept the raw string and
    /// parse the first column.
    /// </summary>
    public string? Sort { get; set; }

    public string? SearchValue { get; set; }

    public PaginatedRequest ToPaginatedRequest()
    {
        var pageSize = Take <= 0 ? 20 : Take;
        var pageNumber = (Skip / pageSize) + 1;

        var request = new PaginatedRequest
        {
            PageNumber = pageNumber,
            PageSize = pageSize,
            Search = string.IsNullOrWhiteSpace(SearchValue) ? null : SearchValue
        };

        if (!string.IsNullOrWhiteSpace(Sort))
        {
            try
            {
                using var doc = System.Text.Json.JsonDocument.Parse(Sort);
                if (doc.RootElement.ValueKind == System.Text.Json.JsonValueKind.Array
                    && doc.RootElement.GetArrayLength() > 0)
                {
                    var first = doc.RootElement[0];
                    if (first.TryGetProperty("selector", out var selector))
                    {
                        request.SortBy = selector.GetString();
                    }
                    if (first.TryGetProperty("desc", out var desc))
                    {
                        request.IsDescending = desc.GetBoolean();
                    }
                }
            }
            catch (System.Text.Json.JsonException)
            {
                // Non-JSON sort value (e.g. plain field name) — accept as-is.
                request.SortBy = Sort;
            }
        }

        return request;
    }
}

