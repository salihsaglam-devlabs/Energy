using Energy.Domain.Common;

namespace Energy.Domain.FieldOperations;

/// <summary>Günlük saha raporu (proje bazlı).</summary>
public class DailySiteReport : AuditableEntity
{
    public Guid ProjectId { get; set; }
    public Guid? WorkOrderId { get; set; }
    public string ReportNo { get; set; } = string.Empty;
    public DateTime ReportDate { get; set; }
    public string? Weather { get; set; }
    public string? Notes { get; set; }
    public DocumentStatus Status { get; set; } = DocumentStatus.Draft;
}

/// <summary>Günlük saha personeli.</summary>
public class DailySiteReportWorker : AuditableEntity
{
    public Guid DailySiteReportId { get; set; }
    public Guid EmployeeId { get; set; }
    public decimal HoursWorked { get; set; }
    public string? Note { get; set; }
}

/// <summary>Günlük saha ekipmanı.</summary>
public class DailySiteReportEquipment : AuditableEntity
{
    public Guid DailySiteReportId { get; set; }
    public Guid? EquipmentAssetId { get; set; }
    public string? EquipmentText { get; set; }
    public decimal Hours { get; set; }
}

/// <summary>Günlük saha malzemesi.</summary>
public class DailySiteReportMaterial : AuditableEntity
{
    public Guid DailySiteReportId { get; set; }
    public Guid MaterialId { get; set; }
    public decimal Quantity { get; set; }
}

/// <summary>Proje ilerleme kaydı (miktar ve yüzde bazlı).</summary>
public class ProgressEntry : AuditableEntity
{
    public Guid ProjectId { get; set; }
    public Guid? ProjectPhaseId { get; set; }
    public DateTime EntryDate { get; set; }
    public decimal Quantity { get; set; }
    public decimal Percentage { get; set; }
    public string? Note { get; set; }
}

/// <summary>Metraj başlığı.</summary>
public class MeasurementSheet : AuditableEntity
{
    public Guid ProjectId { get; set; }
    public Guid? ContractId { get; set; }
    public string SheetNo { get; set; } = string.Empty;
    public DateTime SheetDate { get; set; }
    public DocumentStatus Status { get; set; } = DocumentStatus.Draft;
}

/// <summary>Metraj satırı.</summary>
public class MeasurementSheetLine : AuditableEntity
{
    public Guid MeasurementSheetId { get; set; }
    public string Description { get; set; } = string.Empty;
    public decimal Quantity { get; set; }
    public decimal UnitPrice { get; set; }
}

