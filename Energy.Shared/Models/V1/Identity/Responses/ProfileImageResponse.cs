namespace Energy.Shared.Models.V1.Identity.Responses;

/// <summary>Bir kullanıcının profil resminin ikili (binary) içeriği.</summary>
public sealed class ProfileImageResponse
{
    /// <summary>Resmin ham bayt içeriği.</summary>
    public byte[] Content { get; init; } = Array.Empty<byte>();

    /// <summary>Resmin MIME türü.</summary>
    public string ContentType { get; init; } = "application/octet-stream";
}
