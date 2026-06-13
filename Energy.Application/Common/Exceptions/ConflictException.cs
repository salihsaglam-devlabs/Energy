namespace Energy.Application.Common.Exceptions;

/// <summary>409 (Conflict) — bir yerelleştirme anahtarı ve opsiyonel yer tutucu argümanları taşır.</summary>
public sealed class ConflictException : LocalizedException
{
    /// <summary>Verilen yerelleştirme anahtarı ve argümanlarla bir çakışma (conflict) istisnası oluşturur.</summary>
    public ConflictException(string messageKey, params object[] arguments)
        : base(messageKey, arguments)
    {
    }
}
