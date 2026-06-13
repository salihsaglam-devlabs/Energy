using System.Buffers.Text;
using System.Security.Cryptography;
using System.Text;
using Microsoft.AspNetCore.Authentication;

namespace Energy.Web.Services.Authentication;

/// <summary>
/// Kimlik doğrulama çerez biletini, ASP.NET Core DataProtection anahtar halkası yerine
/// yapılandırmadan ("Auth:CookieProtectionKey") alınan statik bir HMAC-SHA256 anahtarıyla korur.
///
/// Neden: varsayılan çerez koruması, kalıcı ve yazılabilir bir anahtar deposuna
/// (örn. C:\Energy\keys\web) ihtiyaç duyar. Bu klasörün yazılamadığı sunucularda anahtarlar
/// geçici olur ve her yeniden başlatma / app-pool geri dönüşümü / ölçeklenmiş örnek, daha
/// önce verilmiş çerezleri geçersiz kılar -> kullanıcılar sessizce 401'e düşer ve menüler/veri
/// yüklenmeyi durdurur. Yapılandırılmış statik bir anahtar, yeniden başlatmalar ve örnekler
/// arasında kararlıdır; bu yüzden yazılabilir bir anahtar deposu gerekmez.
///
/// Bilet yükü zaten API tarafından imzalanmış JWT'yi ve etkin yetki/rol taleplerini taşır.
/// Yalnızca BÜTÜNLÜĞE ihtiyacımız var (bir kullanıcı yetki taleplerini değiştiremesin diye),
/// gizliliğe değil: JWT kendini korur ve çereze sahip olmak zaten erişim anlamına gelir.
/// HttpOnly + Secure çerezi aktarım sırasında korur. Bu nedenle yük şifrelenmez, imzalanır —
/// tam olarak "JWT zaten imzalı, ayrı şifreleme gerekmez" modeli.
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
            // Bozuk çerez (kesik, yanlış anahtar, biçim değişikliği) -> istisna fırlatmak
            // yerine kimliği doğrulanmamış olarak değerlendir.
            return null;
        }
    }
}

