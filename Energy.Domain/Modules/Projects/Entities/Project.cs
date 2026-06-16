using Energy.Domain.Common;

namespace Energy.Domain.Modules.Projects;

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
