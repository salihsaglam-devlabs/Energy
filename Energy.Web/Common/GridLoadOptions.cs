using Energy.Shared.Models.V1.Common.Requests;

namespace Energy.Web.Common;

/// <summary>
/// DevExtreme'in <c>CustomStore</c> bileşeninin gönderdiği sorgu dizesi
/// parametrelerini (<c>skip</c>, <c>take</c>, <c>sort</c>, <c>searchValue</c>)
/// API'nin <see cref="PaginatedRequest"/> biçimine çevirir.
/// </summary>
public sealed class GridLoadOptions
{
    /// <summary>Atlanacak kayıt sayısı (sayfalama ofseti).</summary>
    public int Skip { get; set; }

    /// <summary>Alınacak kayıt sayısı (sayfa boyutu).</summary>
    public int Take { get; set; } = 20;

    /// <summary>
    /// DevExtreme sıralamayı bir JSON dizisi olarak gönderir; ör.
    /// <c>[{"selector":"name","desc":false}]</c>. Ham dizeyi kabul edip ilk sütunu
    /// ayrıştırırız.
    /// </summary>
    public string? Sort { get; set; }

    /// <summary>Serbest metin arama değeri.</summary>
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
                // JSON olmayan sıralama değeri (örn. düz alan adı) — olduğu gibi kabul et.
                request.SortBy = Sort;
            }
        }

        return request;
    }
}

