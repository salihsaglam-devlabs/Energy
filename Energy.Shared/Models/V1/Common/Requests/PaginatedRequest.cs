using System.Collections.Generic;

namespace Energy.Shared.Models.V1.Common.Requests;

/// <summary>
/// Sayfalama, arama, sıralama ve filtreleme destekleyen listeleme istekleri için
/// temel sınıf. Sayfa numarası ve boyutu güvenli sınırlar içinde tutulur.
/// </summary>
public class PaginatedRequest
{
    /// <summary>Varsayılan sayfa numarası.</summary>
    private const int DefaultPageNumber = 1;

    /// <summary>Varsayılan sayfa boyutu.</summary>
    private const int DefaultPageSize = 10;

    /// <summary>İzin verilen en büyük sayfa boyutu.</summary>
    private const int MaxPageSize = 100;

    private int _pageNumber = DefaultPageNumber;
    private int _pageSize = DefaultPageSize;

    /// <summary>İstenen sayfa numarası (1'den küçük değerler varsayılana çekilir).</summary>
    public int PageNumber
    {
        get => _pageNumber;
        set => _pageNumber =
            value < 1
                ? DefaultPageNumber
                : value;
    }

    /// <summary>Sayfa başına kayıt sayısı (geçersiz değerler varsayılana, üst sınır aşılırsa MaxPageSize'a çekilir).</summary>
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

    /// <summary>İsteğe bağlı serbest metin arama terimi.</summary>
    public string? Search { get; set; }

    /// <summary>Sıralanacak alanın adı.</summary>
    public string? SortBy { get; set; }

    /// <summary>Sıralama azalan ise true, artan ise false.</summary>
    public bool IsDescending { get; set; }

    /// <summary>İsteğe bağlı alan-bazlı filtreler (alan adı → değer).</summary>
    public Dictionary<string, string>? Filters { get; set; }
}
