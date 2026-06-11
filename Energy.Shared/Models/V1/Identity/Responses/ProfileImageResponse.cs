namespace Energy.Shared.Models.V1.Identity.Responses;

public sealed class ProfileImageResponse
{
    public byte[] Content { get; init; } = Array.Empty<byte>();
    public string ContentType { get; init; } = "application/octet-stream";
}
