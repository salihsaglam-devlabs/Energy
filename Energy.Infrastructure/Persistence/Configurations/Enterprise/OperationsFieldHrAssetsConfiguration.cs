using Energy.Domain.Assets;
using Energy.Domain.Catalog;
using Energy.Domain.Contracts;
using Energy.Domain.Core;
using Energy.Domain.FieldOperations;
using Energy.Domain.HR;
using Energy.Domain.Identity;
using Energy.Domain.Inventory;
using Energy.Domain.Operations;
using Energy.Domain.Organization;
using Energy.Domain.Projects;
using Microsoft.EntityFrameworkCore;

namespace Energy.Infrastructure.Persistence.Configurations.Enterprise;

/// <summary>Operations, FieldOperations, HR ve Assets modülleri EF Core yapılandırması.</summary>
public static class OperationsFieldHrAssetsConfiguration
{
    public static void Configure(ModelBuilder b)
    {
        // ---- Operations ----
        b.Entity<WorkOrderType>(e => { e.ToTable("WorkOrderTypes"); e.HasIndex(x => x.Code).IsUnique(); });

        b.Entity<WorkOrder>(e =>
        {
            e.ToTable("WorkOrders");
            e.HasIndex(x => x.WorkOrderNo).IsUnique();
            e.HasOne<WorkOrderType>().WithMany().HasForeignKey(x => x.WorkOrderTypeId).OnDelete(DeleteBehavior.Restrict);
            e.HasOne<Project>().WithMany().HasForeignKey(x => x.ProjectId).OnDelete(DeleteBehavior.Restrict);
            e.HasOne<ProjectPhase>().WithMany().HasForeignKey(x => x.ProjectPhaseId).OnDelete(DeleteBehavior.Restrict);
            e.HasOne<ProjectLocation>().WithMany().HasForeignKey(x => x.ProjectLocationId).OnDelete(DeleteBehavior.Restrict);
        });

        b.Entity<WorkOrderAssignment>(e =>
        {
            e.ToTable("WorkOrderAssignments");
            e.HasOne<WorkOrder>().WithMany().HasForeignKey(x => x.WorkOrderId).OnDelete(DeleteBehavior.Cascade);
            e.HasOne<Employee>().WithMany().HasForeignKey(x => x.EmployeeId).OnDelete(DeleteBehavior.Restrict);
            e.HasOne<User>().WithMany().HasForeignKey(x => x.UserId).OnDelete(DeleteBehavior.Restrict);
        });

        b.Entity<WorkOrderMaterialPlan>(e =>
        {
            e.ToTable("WorkOrderMaterialPlans");
            e.HasOne<WorkOrder>().WithMany().HasForeignKey(x => x.WorkOrderId).OnDelete(DeleteBehavior.Cascade);
            e.HasOne<Material>().WithMany().HasForeignKey(x => x.MaterialId).OnDelete(DeleteBehavior.Restrict);
        });

        b.Entity<WorkOrderMaterialUsage>(e =>
        {
            e.ToTable("WorkOrderMaterialUsages");
            e.HasOne<WorkOrder>().WithMany().HasForeignKey(x => x.WorkOrderId).OnDelete(DeleteBehavior.Restrict);
            e.HasOne<StockDocumentLine>().WithMany().HasForeignKey(x => x.StockDocumentLineId).OnDelete(DeleteBehavior.Restrict);
            e.HasOne<Material>().WithMany().HasForeignKey(x => x.MaterialId).OnDelete(DeleteBehavior.Restrict);
        });

        b.Entity<WorkOrderChecklist>(e =>
        {
            e.ToTable("WorkOrderChecklists");
            e.HasOne<WorkOrder>().WithMany().HasForeignKey(x => x.WorkOrderId).OnDelete(DeleteBehavior.Cascade);
        });

        b.Entity<WorkOrderChecklistItem>(e =>
        {
            e.ToTable("WorkOrderChecklistItems");
            e.HasOne<WorkOrderChecklist>().WithMany().HasForeignKey(x => x.WorkOrderChecklistId).OnDelete(DeleteBehavior.Cascade);
        });

        b.Entity<WorkOrderStatusHistory>(e =>
        {
            e.ToTable("WorkOrderStatusHistories");
            e.HasOne<WorkOrder>().WithMany().HasForeignKey(x => x.WorkOrderId).OnDelete(DeleteBehavior.Cascade);
        });

        // ---- FieldOperations ----
        b.Entity<DailySiteReport>(e =>
        {
            e.ToTable("DailySiteReports");
            e.HasIndex(x => x.ReportNo).IsUnique();
            e.HasOne<Project>().WithMany().HasForeignKey(x => x.ProjectId).OnDelete(DeleteBehavior.Restrict);
            e.HasOne<WorkOrder>().WithMany().HasForeignKey(x => x.WorkOrderId).OnDelete(DeleteBehavior.Restrict);
        });

        b.Entity<DailySiteReportWorker>(e =>
        {
            e.ToTable("DailySiteReportWorkers");
            e.HasOne<DailySiteReport>().WithMany().HasForeignKey(x => x.DailySiteReportId).OnDelete(DeleteBehavior.Cascade);
            e.HasOne<Employee>().WithMany().HasForeignKey(x => x.EmployeeId).OnDelete(DeleteBehavior.Restrict);
        });

        b.Entity<DailySiteReportEquipment>(e =>
        {
            e.ToTable("DailySiteReportEquipments");
            e.HasOne<DailySiteReport>().WithMany().HasForeignKey(x => x.DailySiteReportId).OnDelete(DeleteBehavior.Cascade);
            e.HasOne<EquipmentAsset>().WithMany().HasForeignKey(x => x.EquipmentAssetId).OnDelete(DeleteBehavior.Restrict);
        });

        b.Entity<DailySiteReportMaterial>(e =>
        {
            e.ToTable("DailySiteReportMaterials");
            e.HasOne<DailySiteReport>().WithMany().HasForeignKey(x => x.DailySiteReportId).OnDelete(DeleteBehavior.Cascade);
            e.HasOne<Material>().WithMany().HasForeignKey(x => x.MaterialId).OnDelete(DeleteBehavior.Restrict);
        });

        b.Entity<ProgressEntry>(e =>
        {
            e.ToTable("ProgressEntries");
            e.HasOne<Project>().WithMany().HasForeignKey(x => x.ProjectId).OnDelete(DeleteBehavior.Restrict);
            e.HasOne<ProjectPhase>().WithMany().HasForeignKey(x => x.ProjectPhaseId).OnDelete(DeleteBehavior.Restrict);
        });

        b.Entity<MeasurementSheet>(e =>
        {
            e.ToTable("MeasurementSheets");
            e.HasIndex(x => x.SheetNo).IsUnique();
            e.HasOne<Project>().WithMany().HasForeignKey(x => x.ProjectId).OnDelete(DeleteBehavior.Restrict);
            e.HasOne<Contract>().WithMany().HasForeignKey(x => x.ContractId).OnDelete(DeleteBehavior.Restrict);
        });

        b.Entity<MeasurementSheetLine>(e =>
        {
            e.ToTable("MeasurementSheetLines");
            e.HasOne<MeasurementSheet>().WithMany().HasForeignKey(x => x.MeasurementSheetId).OnDelete(DeleteBehavior.Cascade);
        });

        // ---- HR ----
        b.Entity<Timesheet>(e => { e.ToTable("Timesheets"); e.HasIndex(x => x.TimesheetNo).IsUnique(); });

        b.Entity<TimesheetLine>(e =>
        {
            e.ToTable("TimesheetLines");
            e.HasOne<Timesheet>().WithMany().HasForeignKey(x => x.TimesheetId).OnDelete(DeleteBehavior.Cascade);
            e.HasOne<Employee>().WithMany().HasForeignKey(x => x.EmployeeId).OnDelete(DeleteBehavior.Restrict);
            e.HasOne<Project>().WithMany().HasForeignKey(x => x.ProjectId).OnDelete(DeleteBehavior.Restrict);
            e.HasOne<WorkOrder>().WithMany().HasForeignKey(x => x.WorkOrderId).OnDelete(DeleteBehavior.Restrict);
        });

        // ---- Assets ----
        b.Entity<EquipmentAsset>(e =>
        {
            e.ToTable("EquipmentAssets");
            e.HasIndex(x => x.Code).IsUnique();
            e.HasOne<Company>().WithMany().HasForeignKey(x => x.CompanyId).OnDelete(DeleteBehavior.Restrict);
        });

        b.Entity<EquipmentAssignment>(e =>
        {
            e.ToTable("EquipmentAssignments");
            e.HasOne<EquipmentAsset>().WithMany().HasForeignKey(x => x.EquipmentAssetId).OnDelete(DeleteBehavior.Cascade);
            e.HasOne<Project>().WithMany().HasForeignKey(x => x.ProjectId).OnDelete(DeleteBehavior.Restrict);
            e.HasOne<Employee>().WithMany().HasForeignKey(x => x.EmployeeId).OnDelete(DeleteBehavior.Restrict);
            e.HasOne<Warehouse>().WithMany().HasForeignKey(x => x.WarehouseId).OnDelete(DeleteBehavior.Restrict);
        });

        b.Entity<EquipmentMaintenance>(e =>
        {
            e.ToTable("EquipmentMaintenances");
            e.HasOne<EquipmentAsset>().WithMany().HasForeignKey(x => x.EquipmentAssetId).OnDelete(DeleteBehavior.Cascade);
        });
    }
}

