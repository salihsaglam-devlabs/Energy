namespace Energy.Application.Common.Exceptions;

/// <summary>
/// Kullanıcıya gösterilen mesajı sabit metin yerine bir yerelleştirme ANAHTARI
/// olan uygulama istisnalarının temel sınıfı (anahtar, API sınırında çözümlenir).
/// Opsiyonel <see cref="Arguments"/>, kaynaktaki <c>{0}</c> yer tutucularını besler.
/// </summary>
public abstract class LocalizedException : Exception
{
    /// <summary>Verilen yerelleştirme anahtarı ve opsiyonel argümanlarla istisnayı oluşturur.</summary>
    protected LocalizedException(string messageKey, object[]? arguments = null)
        : base(messageKey)
    {
        MessageKey = messageKey;
        Arguments = arguments ?? Array.Empty<object>();
    }

    /// <summary>Paylaşılan kaynağa (shared resource) karşı çözümlenecek yerelleştirme anahtarı.</summary>
    public string MessageKey { get; }

    /// <summary>Kaynağın bileşik biçim (composite-format) yer tutucuları için argümanlar.</summary>
    public object[] Arguments { get; }
}
