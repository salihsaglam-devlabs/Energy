using Energy.Domain.Common;

namespace Energy.Domain.Operations;

/// <summary>İş emri türü.</summary>
public class WorkOrderType : AuditableEntity
{
    public string Code { get; set; } = string.Empty;
    public string Name { get; set; } = string.Empty;
    public bool IsActive { get; set; } = true;
}

/// <summary>İş emri (proje bazlı veya bağımsız).</summary>
public class WorkOrder : AuditableEntity
{
    public Guid WorkOrderTypeId { get; set; }
    public Guid? ProjectId { get; set; }
    public Guid? ProjectPhaseId { get; set; }
    public Guid? ProjectLocationId { get; set; }
    public WorkOrderStatus Status { get; set; } = WorkOrderStatus.Draft;
    public string WorkOrderNo { get; set; } = string.Empty;
    public string Title { get; set; } = string.Empty;
    public string? Description { get; set; }
    public DateTime? PlannedStart { get; set; }
    public DateTime? PlannedEnd { get; set; }
}

/// <summary>İş emri görev ataması (kullanıcı veya personel).</summary>
public class WorkOrderAssignment : AuditableEntity
{
    public Guid WorkOrderId { get; set; }
    public Guid? EmployeeId { get; set; }
    public Guid? UserId { get; set; }
    public string? AssignmentRole { get; set; }
}

/// <summary>Planlanan iş emri malzemesi.</summary>
public class WorkOrderMaterialPlan : AuditableEntity
{
    public Guid WorkOrderId { get; set; }
    public Guid MaterialId { get; set; }
    public decimal PlannedQuantity { get; set; }
}

/// <summary>Gerçekleşen iş emri malzeme kullanımı.</summary>
public class WorkOrderMaterialUsage : AuditableEntity
{
    public Guid WorkOrderId { get; set; }
    public Guid? StockDocumentLineId { get; set; }
    public Guid MaterialId { get; set; }
    public decimal UsedQuantity { get; set; }
}

/// <summary>İş emri kontrol listesi.</summary>
public class WorkOrderChecklist : AuditableEntity
{
    public Guid WorkOrderId { get; set; }
    public string Name { get; set; } = string.Empty;
    public bool IsRequired { get; set; } = true;
}

/// <summary>Kontrol listesi satırı.</summary>
public class WorkOrderChecklistItem : AuditableEntity
{
    public Guid WorkOrderChecklistId { get; set; }
    public string Description { get; set; } = string.Empty;
    public bool IsRequired { get; set; } = true;
    public bool IsCompleted { get; set; }
}

/// <summary>İş emri durum geçmişi.</summary>
public class WorkOrderStatusHistory : AuditableEntity
{
    public Guid WorkOrderId { get; set; }
    public WorkOrderStatus FromStatus { get; set; }
    public WorkOrderStatus ToStatus { get; set; }
    public DateTime ChangedAt { get; set; }
    public string? Note { get; set; }
}

