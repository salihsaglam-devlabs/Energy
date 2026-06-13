namespace Energy.Application.Common.Exceptions;

/// <summary>404 (Not Found) — bir yerelleştirme anahtarı ve opsiyonel yer tutucu argümanları taşır.</summary>
public sealed class NotFoundException : LocalizedException
{
    /// <summary>Verilen yerelleştirme anahtarı ve argümanlarla bir "bulunamadı" istisnası oluşturur.</summary>
    public NotFoundException(string messageKey, params object[] arguments)
        : base(messageKey, arguments)
    {
    }
}
