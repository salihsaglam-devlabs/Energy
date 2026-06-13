namespace Energy.Shared.Models.V1.Identity.Requests;

/// <summary>
/// Bir kullanıcının tüm erişimini tek seferde değiştirir: istenen rol kümesi ve
/// istenen doğrudan (kullanıcıya özel) yetki tanımları. Bilinmeyen yetki kodları
/// servis tarafından yok sayılır; rollerden zaten miras alınan yetkilerin burada
/// tekrar belirtilmesine gerek yoktur.
/// </summary>
public sealed class SetUserAccessRequest
{
    /// <summary>Kullanıcıya atanacak rollerin kimlikleri.</summary>
    public IReadOnlyList<Guid> RoleIds { get; init; } = Array.Empty<Guid>();

    /// <summary>Kullanıcıya doğrudan verilecek yetki kodları.</summary>
    public IReadOnlyList<string> DirectPermissionCodes { get; init; } = Array.Empty<string>();
}
