namespace Energy.Web.Common;

/// <summary>
/// Helpers for distinguishing AJAX/JSON (XHR/fetch) requests from full-page
/// browser navigations. Auth filters use this to decide whether to emit a
/// machine-readable JSON envelope (so the client can redirect itself) or a
/// classic 302 redirect for top-level navigations.
/// </summary>
public static class RequestExtensions
{
    /// <summary>
    /// True when the request was issued by the in-app HTTP helper / DevExtreme
    /// (it sets <c>X-Requested-With: XMLHttpRequest</c> and/or
    /// <c>Accept: application/json</c>). For those, returning HTML redirects is
    /// useless because <c>fetch</c> follows them transparently and the grid
    /// then tries to parse an HTML page as JSON.
    /// </summary>
    public static bool WantsJson(this HttpRequest request)
    {
        if (string.Equals(request.Headers.XRequestedWith, "XMLHttpRequest", StringComparison.OrdinalIgnoreCase))
        {
            return true;
        }

        var accept = request.Headers.Accept.ToString();
        return accept.Contains("application/json", StringComparison.OrdinalIgnoreCase);
    }
}

