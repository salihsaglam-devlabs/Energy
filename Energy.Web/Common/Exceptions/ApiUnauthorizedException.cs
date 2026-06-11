namespace Energy.Web.Common.Exceptions;

public sealed class ApiUnauthorizedException : Exception
{
    public ApiUnauthorizedException() : base("API returned 401 Unauthorized.") { }
    public ApiUnauthorizedException(string message) : base(message) { }
}
