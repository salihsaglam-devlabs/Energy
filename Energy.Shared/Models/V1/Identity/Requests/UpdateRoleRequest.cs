namespace Energy.Shared.Models.V1.Identity.Requests;

/// <summary>Var olan bir rolü güncellemek için kullanılan istek.</summary>
public sealed class UpdateRoleRequest
{
    /// <summary>Rolün adı.</summary>
    public string Name { get; set; } = string.Empty;

    /// <summary>Rolün isteğe bağlı açıklaması.</summary>
    public string? Description { get; set; }
}
