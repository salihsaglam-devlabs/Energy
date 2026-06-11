namespace Energy.Application.Common.Exceptions;

/// <summary>409 — carries a localization key plus optional placeholder args.</summary>
public sealed class ConflictException : LocalizedException
{
    public ConflictException(string messageKey, params object[] arguments)
        : base(messageKey, arguments)
    {
    }
}
