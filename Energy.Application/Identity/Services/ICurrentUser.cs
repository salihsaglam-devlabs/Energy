namespace Energy.Application.Identity.Services;

/// <summary>Ambient information about the principal making the request.</summary>
public interface ICurrentUser
{
    Guid? UserId { get; }
    string? UserName { get; }
    bool IsAuthenticated { get; }
}
