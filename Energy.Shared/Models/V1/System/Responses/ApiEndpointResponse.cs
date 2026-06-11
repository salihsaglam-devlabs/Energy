namespace Energy.Shared.Models.V1.System.Responses;

public sealed class ApiEndpointResponse
{
    public Guid Id { get; init; }
    public string Name { get; init; } = string.Empty;
    public string? Description { get; init; }
    public string Path { get; init; } = string.Empty;
    public string HttpMethod { get; init; } = string.Empty;
    public bool IsActive { get; init; }
    public string? RequiredPermissionCode { get; init; }
}
