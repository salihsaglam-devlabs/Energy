namespace Energy.Shared.Models.V1.Reporting.DashboardWidget.Responses;

/// <summary>DashboardWidget detay görünümü.</summary>
public class DashboardWidgetDetailResponse
{
    /// <summary>Kimlik.</summary>
    public Guid Id { get; set; }

    /// <summary>Oluşturma zamanı</summary>
    public DateTime CreatedAt { get; set; }

    /// <summary>Oluşturan kullanıcı</summary>
    public Guid? CreatedBy { get; set; }

    /// <summary>Son güncelleme zamanı</summary>
    public DateTime? UpdatedAt { get; set; }

    /// <summary>Güncelleyen kullanıcı</summary>
    public Guid? UpdatedBy { get; set; }

    /// <summary>Soft delete bayrağı</summary>
    public bool IsDeleted { get; set; }

    /// <summary>Silinme zamanı</summary>
    public DateTime? DeletedAt { get; set; }

    /// <summary>Silen kullanıcı</summary>
    public Guid? DeletedBy { get; set; }

    /// <summary>Code</summary>
    public string Code { get; set; } = string.Empty;

    /// <summary>Name</summary>
    public string Name { get; set; } = string.Empty;

    /// <summary>Module</summary>
    public string Module { get; set; } = string.Empty;

    /// <summary>WidgetType</summary>
    public string WidgetType { get; set; } = string.Empty;

    /// <summary>RequiredPermissionCode</summary>
    public string? RequiredPermissionCode { get; set; }

    /// <summary>DisplayOrder</summary>
    public int DisplayOrder { get; set; }

    /// <summary>IsActive</summary>
    public bool IsActive { get; set; }
}
