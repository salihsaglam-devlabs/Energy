namespace Energy.Shared.Models.V1.System.Requests;

public sealed class UpdateApiEndpointRequest
{
    public string Name { get; set; } = string.Empty;
    public string? Description { get; set; }
    public string Path { get; set; } = string.Empty;
    public string HttpMethod { get; set; } = string.Empty;
    public bool IsActive { get; set; } = true;
    public string? RequiredPermissionCode { get; set; }
}
