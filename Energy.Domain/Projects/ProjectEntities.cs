using Energy.Domain.Common;

namespace Energy.Domain.Projects;

/// <summary>Proje türü (lookup/master).</summary>
public class ProjectType : AuditableEntity
{
    public string Code { get; set; } = string.Empty;
    public string Name { get; set; } = string.Empty;
    public bool IsActive { get; set; } = true;
}

/// <summary>Proje durumu (lookup/master).</summary>
public class ProjectStatus : AuditableEntity
{
    public string Code { get; set; } = string.Empty;
    public string Name { get; set; } = string.Empty;
    public int DisplayOrder { get; set; }
    public bool IsClosedState { get; set; }
    public bool IsActive { get; set; } = true;
}

/// <summary>Proje ana kartı.</summary>
public class Project : AuditableEntity
{
    public Guid CompanyId { get; set; }
    public Guid? BranchId { get; set; }
    public Guid ProjectTypeId { get; set; }
    public Guid StatusId { get; set; }
    public Guid? CustomerId { get; set; }
    public Guid? ManagerUserId { get; set; }
    public string Code { get; set; } = string.Empty;
    public string Name { get; set; } = string.Empty;
    public DateTime? StartDate { get; set; }
    public DateTime? EndDate { get; set; }
    public string? Description { get; set; }
}

/// <summary>Proje lokasyon hiyerarşisi.</summary>
public class ProjectLocation : AuditableEntity
{
    public Guid ProjectId { get; set; }
    public Guid? ParentLocationId { get; set; }
    public string Code { get; set; } = string.Empty;
    public string Name { get; set; } = string.Empty;
}

/// <summary>Proje fazı / WBS benzeri kırılım.</summary>
public class ProjectPhase : AuditableEntity
{
    public Guid ProjectId { get; set; }
    public Guid? ParentPhaseId { get; set; }
    public string Code { get; set; } = string.Empty;
    public string Name { get; set; } = string.Empty;
    public decimal ProgressPercentage { get; set; }
    public DateTime? PlannedStart { get; set; }
    public DateTime? PlannedEnd { get; set; }
}

/// <summary>Proje üyesi (kullanıcı veya personel ataması).</summary>
public class ProjectMember : AuditableEntity
{
    public Guid ProjectId { get; set; }
    public Guid? UserId { get; set; }
    public Guid? EmployeeId { get; set; }
    public string? ProjectRole { get; set; }
}

/// <summary>Proje notu.</summary>
public class ProjectNote : AuditableEntity
{
    public Guid ProjectId { get; set; }
    public string Title { get; set; } = string.Empty;
    public string? Body { get; set; }
}

