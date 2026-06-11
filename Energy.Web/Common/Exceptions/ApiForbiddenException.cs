namespace Energy.Web.Common.Exceptions;

public sealed class ApiForbiddenException : Exception
{
    public ApiForbiddenException() : base("API returned 403 Forbidden.") { }
    public ApiForbiddenException(string message) : base(message) { }
}
