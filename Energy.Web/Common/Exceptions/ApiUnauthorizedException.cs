namespace Energy.Web.Common.Exceptions;

/// <summary>
/// Thrown by <see cref="Energy.Web.Clients.Infrastructure.Authentication.AuthHeaderHandler"/>
/// when an authenticated API call returns 401 Unauthorized after retry. The
/// <c>ApiExceptionFilter</c> converts this into a sign-out + redirect to the
/// configured login path.
/// </summary>
public sealed class ApiUnauthorizedException : Exception
{
    public ApiUnauthorizedException()
        : base("The API rejected the request as unauthorized.")
    {
    }
}

