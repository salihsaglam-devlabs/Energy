namespace Energy.Shared.Models.V1.Identity.Requests;

/// <summary>
/// Bir kullanıcının profil resmini ayarlar. Web katmanı yüklenen dosyayı okur ve
/// ikili veriyi JSON API üzerinden taşımak için base64 ile kodlayarak iletir.
/// </summary>
public sealed class SetProfileImageRequest
{
    /// <summary>Resmin MIME türü.</summary>
    public string ContentType { get; set; } = string.Empty;

    /// <summary>Resim içeriği, Base64 olarak kodlanmış.</summary>
    public string ContentBase64 { get; set; } = string.Empty;
}
