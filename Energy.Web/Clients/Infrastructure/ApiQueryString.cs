using System.Globalization;
using System.Text;
using Energy.Shared.Models.V1.Common.Requests;

namespace Energy.Web.Clients.Infrastructure;

/// <summary>
/// Lightweight, allocation-friendly query string builder for outbound API calls.
/// Supports paginated request envelopes and free-form key/value parameters.
/// </summary>
internal static class ApiQueryString
{
    public static string Append(string basePath, PaginatedRequest? request)
    {
        if (request is null)
        {
            return basePath;
        }

        var builder = new Builder(basePath)
            .Add("pageNumber", request.PageNumber)
            .Add("pageSize", request.PageSize)
            .Add("search", request.Search)
            .Add("sortBy", request.SortBy);

        if (!string.IsNullOrWhiteSpace(request.SortBy))
        {
            builder.Add("isDescending", request.IsDescending);
        }

        if (request.Filters is { Count: > 0 })
        {
            foreach (var filter in request.Filters)
            {
                builder.Add($"filters[{filter.Key}]", filter.Value);
            }
        }

        return builder.ToString();
    }

    /// <summary>Temel yola serbest biçimli anahtar/değer parametreleri ekler.</summary>
    public static string Append(string basePath, params (string Key, object? Value)[] parameters)
    {
        var builder = new Builder(basePath);

        foreach (var (key, value) in parameters)
        {
            builder.Add(key, value);
        }

        return builder.ToString();
    }

    private sealed class Builder
    {
        private readonly StringBuilder _sb;
        private bool _hasQuery;

        public Builder(string basePath)
        {
            _sb = new StringBuilder(basePath);
            _hasQuery = basePath.Contains('?');
        }

        public Builder Add(string key, object? value)
        {
            if (value is null)
            {
                return this;
            }

            var stringValue = value switch
            {
                string s => s,
                bool b => b ? "true" : "false",
                IFormattable f => f.ToString(null, CultureInfo.InvariantCulture),
                _ => value.ToString()
            };

            if (string.IsNullOrEmpty(stringValue))
            {
                return this;
            }

            _sb.Append(_hasQuery ? '&' : '?');
            _hasQuery = true;
            _sb.Append(Uri.EscapeDataString(key))
                .Append('=')
                .Append(Uri.EscapeDataString(stringValue));

            return this;
        }

        public override string ToString() => _sb.ToString();
    }
}

