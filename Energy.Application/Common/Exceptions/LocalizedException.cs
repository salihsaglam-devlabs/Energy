namespace Energy.Application.Common.Exceptions;

/// <summary>
/// Base class for application exceptions whose user-facing message is a
/// localization KEY (resolved at the API boundary), never a hard-coded string.
/// Optional <see cref="Arguments"/> feed the resource's <c>{0}</c> placeholders.
/// </summary>
public abstract class LocalizedException : Exception
{
    protected LocalizedException(string messageKey, object[]? arguments = null)
        : base(messageKey)
    {
        MessageKey = messageKey;
        Arguments = arguments ?? Array.Empty<object>();
    }

    /// <summary>Localization key resolved against the shared resource.</summary>
    public string MessageKey { get; }

    /// <summary>Arguments for the resource's composite-format placeholders.</summary>
    public object[] Arguments { get; }
}

