using System.Text.Json;
using System.Text.Json.Nodes;

namespace Energy.Shared.Logging;

/// <summary>
/// Redacts sensitive values (passwords, tokens, secrets, ...) before a payload
/// is persisted to the audit log. Used by both the API request-logging
/// middleware and the Web request-logging middleware so masking is identical
/// across every layer. Masking never throws: on any parse failure the input is
/// returned best-effort (still passed through key-based text redaction).
/// </summary>
public static class SensitiveDataMasker
{
    public const string Mask = "***";

    /// <summary>Max characters persisted per body; longer payloads are truncated.</summary>
    public const int MaxBodyLength = 16 * 1024;

    private const string TruncationSuffix = "...[truncated]";

    /// <summary>
    /// Case-insensitive fragments. A field/parameter whose name contains any of
    /// these is redacted.
    /// </summary>
    private static readonly string[] SensitiveFragments =
    [
        "password", "passwd", "pwd",
        "token", "secret", "apikey", "api_key",
        "authorization", "auth", "bearer",
        "securitystamp", "connectionstring",
        "creditcard", "cardnumber", "cvv", "pin", "ssn", "iban"
    ];

    public static bool IsSensitiveKey(string? key)
        => !string.IsNullOrEmpty(key)
           && Array.Exists(SensitiveFragments, f => key.Contains(f, StringComparison.OrdinalIgnoreCase));

    /// <summary>
    /// Masks a request/response body. JSON objects/arrays are walked and any
    /// sensitive property is redacted; non-JSON bodies fall back to
    /// form/query-style <c>key=value</c> redaction. Result is length-capped.
    /// </summary>
    public static string? MaskBody(string? body, string? contentType = null)
    {
        if (string.IsNullOrWhiteSpace(body)) return body;

        // Binary payloads are never stored verbatim.
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

    /// <summary>Masks an URL query string (<c>?a=1&amp;password=secret</c>).</summary>
    public static string? MaskQueryString(string? query)
        => string.IsNullOrWhiteSpace(query) ? query : Truncate(MaskKeyValuePairs(query));

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
        // Handles "a=1&password=x" and "?a=1&token=y" shapes.
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

    private static string Truncate(string value)
        => value.Length <= MaxBodyLength ? value : value[..MaxBodyLength] + TruncationSuffix;
}

