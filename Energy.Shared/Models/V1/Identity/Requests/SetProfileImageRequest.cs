namespace Energy.Shared.Models.V1.Identity.Requests;

/// <summary>
/// Sets a user's profile image. The Web layer reads the uploaded file and
/// forwards it base64-encoded so the binary travels over the JSON API.
/// </summary>
public sealed class SetProfileImageRequest
{
    public string ContentType { get; set; } = string.Empty;
    public string ContentBase64 { get; set; } = string.Empty;
}

