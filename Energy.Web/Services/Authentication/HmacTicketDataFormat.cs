using System.Buffers.Text;
using System.Security.Cryptography;
using System.Text;
using Microsoft.AspNetCore.Authentication;

namespace Energy.Web.Services.Authentication;

/// <summary>
/// Protects the auth cookie ticket with a static HMAC-SHA256 key taken from
/// configuration ("Auth:CookieProtectionKey") instead of the ASP.NET Core
/// DataProtection key ring.
///
/// Why: the default cookie protection needs a persisted, writable key store
/// (e.g. C:\Energy\keys\web). On servers where that folder is not writable the
/// keys become ephemeral and every restart / app-pool recycle / scaled-out
/// instance invalidates previously issued cookies -> users silently drop to 401
/// and menus/data stop loading. A static configured key is stable across
/// restarts and instances, so no writable key store is required.
///
/// The ticket payload already carries the API-signed JWT plus the effective
/// permission/role claims. We only need INTEGRITY (so a user cannot tamper with
/// their permission claims), not confidentiality: the JWT is self-protecting and
/// possession of the cookie already implies access. HttpOnly + Secure guard the
/// cookie in transit. Therefore the payload is signed, not encrypted — exactly
/// the "the JWT is already signed, no separate encryption needed" model.
/// </summary>
public sealed class HmacTicketDataFormat : ISecureDataFormat<AuthenticationTicket>
{
    private const int MacSize = 32; // HMAC-SHA256 output size in bytes.

    private readonly byte[] _key;
    private readonly TicketSerializer _serializer = TicketSerializer.Default;

    public HmacTicketDataFormat(string key)
    {
        if (string.IsNullOrWhiteSpace(key))
        {
            throw new ArgumentException("Cookie protection key must not be empty.", nameof(key));
        }

        // Normalise any reasonably long secret into a fixed-size HMAC key.
        _key = SHA256.HashData(Encoding.UTF8.GetBytes(key));
    }

    public string Protect(AuthenticationTicket data) => Protect(data, purpose: null);

    public string Protect(AuthenticationTicket data, string? purpose)
    {
        var payload = _serializer.Serialize(data);
        using var hmac = new HMACSHA256(_key);
        var mac = hmac.ComputeHash(payload);

        // Layout: [32-byte MAC][serialised ticket payload].
        var combined = new byte[MacSize + payload.Length];
        Buffer.BlockCopy(mac, 0, combined, 0, MacSize);
        Buffer.BlockCopy(payload, 0, combined, MacSize, payload.Length);

        return Base64Url.EncodeToString(combined);
    }

    public AuthenticationTicket? Unprotect(string? protectedText) => Unprotect(protectedText, purpose: null);

    public AuthenticationTicket? Unprotect(string? protectedText, string? purpose)
    {
        if (string.IsNullOrEmpty(protectedText))
        {
            return null;
        }

        try
        {
            var combined = Base64Url.DecodeFromChars(protectedText);
            if (combined.Length <= MacSize)
            {
                return null;
            }

            var mac = combined.AsSpan(0, MacSize);
            var payload = combined.AsSpan(MacSize);

            using var hmac = new HMACSHA256(_key);
            Span<byte> expected = stackalloc byte[MacSize];
            hmac.TryComputeHash(payload, expected, out _);

            // Constant-time comparison: reject any tampered / forged cookie.
            if (!CryptographicOperations.FixedTimeEquals(mac, expected))
            {
                return null;
            }

            return _serializer.Deserialize(payload.ToArray());
        }
        catch
        {
            // Malformed cookie (truncated, wrong key, format change) -> treat as
            // unauthenticated rather than throwing.
            return null;
        }
    }
}

