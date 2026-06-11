namespace Energy.Shared.Models.V1.Identity.Requests;

public sealed class CreateRoleRequest
{
    public string Name { get; set; } = string.Empty;
    public string? Description { get; set; }
}

