namespace Energy.Shared.Models.V1.Identity.Requests;

/// <summary>
/// Payload sent to set a user's profile image (binary content + MIME type).
/// </summary>
public sealed class UpdateProfileImageRequest
{
    public byte[] Content { get; init; } = Array.Empty<byte>();

    public string ContentType { get; init; } = "application/octet-stream";
}

