namespace Energy.Web.Models.Profile;

/// <summary>
/// Profil ekranında kullanılan salt okunur özet. Çerez (cookie) kimliğinin
/// taleplerinden (claims) ve <c>/users/{id}</c>'ye yapılan elden geldiğince bir
/// sorgudan doldurulur.
/// </summary>
public sealed class ProfileViewModel
{
    /// <summary>Kullanıcının kimliği.</summary>
    public Guid UserId { get; init; }
    /// <summary>Kullanıcı adı.</summary>
    public string UserName { get; init; } = string.Empty;
    /// <summary>Adı.</summary>
    public string FirstName { get; init; } = string.Empty;
    /// <summary>Soyadı.</summary>
    public string LastName { get; init; } = string.Empty;
    /// <summary>E-posta adresi.</summary>
    public string Email { get; init; } = string.Empty;
    /// <summary>Ad soyad.</summary>
    public string FullName { get; init; } = string.Empty;
    /// <summary>Kullanıcının etkin olup olmadığı.</summary>
    public bool IsActive { get; init; } = true;
    /// <summary>Kullanıcının profil resmi olup olmadığı.</summary>
    public bool HasProfileImage { get; init; }
    /// <summary>Kullanıcının rolleri.</summary>
    public IReadOnlyList<ProfileRoleViewModel> Roles { get; init; } = Array.Empty<ProfileRoleViewModel>();
    /// <summary>Kullanıcının etkin yetkileri.</summary>
    public IReadOnlyList<string> Permissions { get; init; } = Array.Empty<string>();
}

/// <summary>Profil ekranında gösterilen bir rolün özeti.</summary>
public sealed class ProfileRoleViewModel
{
    /// <summary>Rolün kimliği.</summary>
    public Guid Id { get; init; }
    /// <summary>Rolün adı.</summary>
    public string Name { get; init; } = string.Empty;
    /// <summary>Rolün açıklaması.</summary>
    public string? Description { get; init; }
}
