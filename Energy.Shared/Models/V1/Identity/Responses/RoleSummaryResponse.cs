namespace Energy.Shared.Models.V1.Identity.Responses;

public sealed class RoleSummaryResponse
{
    public Guid Id { get; init; }

    public string? Name { get; init; }

    public string Description { get; init; } = string.Empty;
}
