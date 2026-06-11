namespace Energy.Shared.Models.V1.System.Requests;

public sealed class UpdateMenuRequest
{
    public Guid? ParentId { get; set; }
    public string NameKey { get; set; } = string.Empty;
    public string? Url { get; set; }
    public string? Icon { get; set; }
    public int DisplayOrder { get; set; }
    public bool IsVisible { get; set; } = true;
    public bool IsActive { get; set; } = true;
    public string? RequiredPermissionCode { get; set; }
}
