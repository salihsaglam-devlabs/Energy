namespace Energy.Application.Common.Exceptions;

/// <summary>404 — carries a localization key plus optional placeholder args.</summary>
public sealed class NotFoundException : LocalizedException
{
    public NotFoundException(string messageKey, params object[] arguments)
        : base(messageKey, arguments)
    {
    }
}
