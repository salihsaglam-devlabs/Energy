namespace Energy.Application.Identity.Services;

/// <summary>İsteği yapan kullanıcı (principal) hakkındaki bağlamsal (ambient) bilgi.</summary>
public interface ICurrentUser
{
    /// <summary>Geçerli kullanıcının kimliği; anonimse null.</summary>
    Guid? UserId { get; }

    /// <summary>Geçerli kullanıcının adı; anonimse null.</summary>
    string? UserName { get; }

    /// <summary>Geçerli isteğin kimliği doğrulanmış (authenticated) bir kullanıcıya ait olup olmadığı.</summary>
    bool IsAuthenticated { get; }
}
