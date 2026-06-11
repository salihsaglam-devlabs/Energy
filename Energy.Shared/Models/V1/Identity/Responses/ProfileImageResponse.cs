namespace Energy.Shared.Models.V1.Identity.Responses;

/// <summary>
/// Raw profile image payload returned by the dedicated image endpoint.
/// </summary>
public sealed class ProfileImageResponse
{
    public byte[] Content { get; init; } = Array.Empty<byte>();

    public string ContentType { get; init; } = "application/octet-stream";
}

