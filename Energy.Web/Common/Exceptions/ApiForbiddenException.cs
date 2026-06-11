namespace Energy.Web.Common.Exceptions;

/// <summary>
/// Thrown by <see cref="Energy.Web.Clients.Infrastructure.Authentication.AuthHeaderHandler"/>
/// when an authenticated API call returns 403 Forbidden. The
/// <c>ApiExceptionFilter</c> converts this into a redirect to the access-denied
/// page.
/// </summary>
public sealed class ApiForbiddenException : Exception
{
    public ApiForbiddenException()
        : base("The API rejected the request as forbidden.")
    {
    }
}

