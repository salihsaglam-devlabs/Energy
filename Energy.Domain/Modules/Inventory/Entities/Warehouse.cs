using Energy.Shared.Common;
using Energy.Domain.Common;

namespace Energy.Domain.Modules.Inventory;

/// <summary>Depo. Şirkete bağlı; şube ve proje opsiyonel.</summary>
public class Warehouse : AuditableEntity
{
    public Guid CompanyId { get; set; }
    public Guid? BranchId { get; set; }
    public Guid? ProjectId { get; set; }
    public WarehouseType WarehouseType { get; set; }
    public string Code { get; set; } = string.Empty;
    public string Name { get; set; } = string.Empty;
    public bool IsActive { get; set; } = true;
}
