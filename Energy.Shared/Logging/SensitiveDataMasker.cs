using System;
using System.Linq;
using System.Text.Json;
using System.Text.Json.Nodes;

namespace Energy.Shared.Logging;

/// <summary>
/// Bir veri, denetim günlüğüne kalıcı olarak yazılmadan önce hassas değerleri
/// (parolalar, jeton/token, sırlar, ...) maskeler. Hem API istek günlükleme
/// ara katmanı hem de Web istek günlükleme ara katmanı tarafından kullanılır;
/// böylece maskeleme her katmanda aynıdır. Maskeleme asla hata fırlatmaz: herhangi
/// bir ayrıştırma hatasında girdi olabildiğince korunarak döndürülür (yine de
/// anahtar tabanlı metin maskelemesinden geçirilir).
/// </summary>
public static class SensitiveDataMasker
{
    /// <summary>Hassas değerlerin yerine konan maske ifadesi.</summary>
    public const string Mask = "***";

    /// <summary>Gövde başına kalıcı yazılan en fazla karakter sayısı; daha uzun veriler kırpılır.</summary>
    public const int MaxBodyLength = 16 * 1024;

    /// <summary>Kırpılmış değerlerin sonuna eklenen son ek.</summary>
    private const string TruncationSuffix = "...[truncated]";

    /// <summary>
    /// Büyük/küçük harfe duyarsız parçacıklar. Adı bunlardan herhangi birini
    /// içeren bir alan/parametre maskelenir.
    /// </summary>
    private static readonly string[] SensitiveFragments =
    [
        "password", "passwd", "pwd",
        "token", "secret", "apikey", "api_key",
        "authorization", "auth", "bearer",
        "securitystamp", "connectionstring",
        "creditcard", "cardnumber", "cvv", "pin", "ssn", "iban"
    ];

    /// <summary>Verilen anahtarın hassas bir alana işaret edip etmediğini belirler.</summary>
    public static bool IsSensitiveKey(string? key)
        => !string.IsNullOrEmpty(key)
           && Array.Exists(SensitiveFragments, f => key.Contains(f, StringComparison.OrdinalIgnoreCase));

    /// <summary>
    /// Bir istek/yanıt gövdesini maskeler. JSON nesneleri/dizileri gezilir ve
    /// hassas her özellik maskelenir; JSON olmayan gövdeler form/sorgu biçimindeki
    /// <c>anahtar=değer</c> maskelemesine düşer. Sonuç uzunluk sınırına çekilir.
    /// </summary>
    public static string? MaskBody(string? body, string? contentType = null)
    {
        if (string.IsNullOrWhiteSpace(body)) return body;

        // İkili (binary) veriler asla olduğu gibi saklanmaz.
        if (!string.IsNullOrEmpty(contentType) && IsBinary(contentType))
        {
            return $"[binary:{contentType}]";
        }

        string masked;
        var trimmed = body.TrimStart();
        if (trimmed.StartsWith('{') || trimmed.StartsWith('['))
        {
            masked = MaskJson(body) ?? MaskKeyValuePairs(body);
        }
        else
        {
            masked = MaskKeyValuePairs(body);
        }

        return Truncate(masked);
    }

    /// <summary>Bir URL sorgu dizesini maskeler (<c>?a=1&amp;password=secret</c>).</summary>
    public static string? MaskQueryString(string? query)
        => string.IsNullOrWhiteSpace(query) ? query : Truncate(MaskKeyValuePairs(query));

    /// <summary>Bir JSON metnini ayrıştırıp hassas özellikleri maskeler; ayrıştırılamazsa null döner.</summary>
    public static string? MaskJson(string? json)
    {
        if (string.IsNullOrWhiteSpace(json)) return json;
        try
        {
            var node = JsonNode.Parse(json);
            if (node is null) return json;
            MaskNode(node);
            return node.ToJsonString(new JsonSerializerOptions { WriteIndented = false });
        }
        catch (JsonException)
        {
            return null;
        }
    }

    /// <summary>Bir JSON düğümünü özyinelemeli gezerek hassas özellikleri maskeler.</summary>
    private static void MaskNode(JsonNode? node)
    {
        switch (node)
        {
            case JsonObject obj:
                foreach (var key in obj.Select(p => p.Key).ToList())
                {
                    if (IsSensitiveKey(key))
                    {
                        obj[key] = Mask;
                    }
                    else
                    {
                        MaskNode(obj[key]);
                    }
                }
                break;
            case JsonArray arr:
                foreach (var item in arr) MaskNode(item);
                break;
        }
    }

    private static string MaskKeyValuePairs(string text)
    {
        // "a=1&password=x" ve "?a=1&token=y" biçimlerini ele alır.
        var leading = text.StartsWith('?') ? "?" : string.Empty;
        var payload = leading.Length == 1 ? text[1..] : text;

        var pairs = payload.Split('&', StringSplitOptions.RemoveEmptyEntries);
        if (pairs.Length == 0) return text;

        for (var i = 0; i < pairs.Length; i++)
        {
            var eq = pairs[i].IndexOf('=');
            if (eq <= 0) continue;
            var key = pairs[i][..eq];
            if (IsSensitiveKey(key)) pairs[i] = key + "=" + Mask;
        }

        return leading + string.Join('&', pairs);
    }

    /// <summary>İçerik türünün ikili (binary) bir veri olup olmadığını belirler.</summary>
    private static bool IsBinary(string contentType)
    {
        var ct = contentType.ToLowerInvariant();
        return ct.StartsWith("image/")
               || ct.StartsWith("video/")
               || ct.StartsWith("audio/")
               || ct.Contains("octet-stream")
               || ct.Contains("pdf")
               || ct.Contains("zip");
    }

    /// <summary>Değeri <see cref="MaxBodyLength"/> sınırına çeker ve gerekirse son ek ekler.</summary>
    private static string Truncate(string value)
        => value.Length <= MaxBodyLength ? value : value[..MaxBodyLength] + TruncationSuffix;
}

