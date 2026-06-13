namespace Energy.Shared.Models.V1.Identity.Requests;

/// <summary>Yeni bir rol oluşturmak için kullanılan istek.</summary>
public sealed class CreateRoleRequest
{
    /// <summary>Rolün adı.</summary>
    public string Name { get; set; } = string.Empty;

    /// <summary>Rolün isteğe bağlı açıklaması.</summary>
    public string? Description { get; set; }
}
