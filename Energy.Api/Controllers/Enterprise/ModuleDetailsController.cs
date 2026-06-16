using Asp.Versioning;
using Energy.Application.Common.Crud;
using Energy.Domain.Common;
using Energy.Shared.Models.V1.Common.Requests;
using Energy.Shared.Models.V1.Common.Responses;
using Microsoft.AspNetCore.Mvc;
using CatD = Energy.Domain.Catalog;
using InvD = Energy.Domain.Inventory;
using ReqD = Energy.Domain.Requests;
using ProcD = Energy.Domain.Procurement;
using OpsD = Energy.Domain.Operations;
using FieldD = Energy.Domain.FieldOperations;
using HrD = Energy.Domain.HR;
using AssetD = Energy.Domain.Assets;
using FinD = Energy.Domain.Finance;
using BudgetD = Energy.Domain.Budget;
using ContractD = Energy.Domain.Contracts;
using PpD = Energy.Domain.ProgressPayments;

namespace Energy.Api.Controllers.Enterprise;

/// <summary>
/// Ana-detay (master-detail) ekranlarının alt-koleksiyon uç noktaları. Bir başlık
/// kaydının kimliği (<c>parentId</c>) verildiğinde, ona bağlı satırları sayfalı döndürür.
/// Her eylem ayrı bir literal rotaya sahiptir; böylece "ModuleDetails.&lt;Action&gt;"
/// kuralı üzerinden her alt-koleksiyon kendi ana modülünün <c>ReadAll</c> yetkisiyle
/// (DefaultEndpointPermissionMap) bağımsız korunur.
/// </summary>
[ApiController]
[ApiVersion("1.0")]
[Route("api/v{version:apiVersion}/details")]
public sealed class ModuleDetailsController : ControllerBase
{
    private readonly IModuleDetailQueryService _service;
    private readonly IModuleDetailCommandService _command;

    /// <summary>Alt-koleksiyon sorgu ve komut servislerini enjekte eder.</summary>
    public ModuleDetailsController(IModuleDetailQueryService service, IModuleDetailCommandService command)
    {
        _service = service;
        _command = command;
    }

    /// <summary>Belirtilen başlık için, verilen yabancı anahtarla bağlı satırları sayfalı döndüren ortak yardımcı.</summary>
    private async Task<ActionResult<BaseResponse<PaginatedResponse<TChild>>>> Children<TChild>(
        string foreignKeyProperty, Guid parentId, PaginatedRequest request, CancellationToken ct)
        where TChild : AuditableEntity
        => Ok(BaseResponse<PaginatedResponse<TChild>>.Success(
            await _service.GetChildrenAsync<TChild>(foreignKeyProperty, parentId, request, ct)));

    /// <summary>Verilen başlığa bağlı yeni bir alt satır oluşturan ortak yardımcı.</summary>
    private async Task<ActionResult<BaseResponse<TChild>>> CreateChild<TChild>(
        string foreignKeyProperty, Guid parentId, TChild entity, CancellationToken ct)
        where TChild : AuditableEntity
        => Ok(BaseResponse<TChild>.Success(
            await _command.CreateChildAsync(foreignKeyProperty, parentId, entity, ct)));

    /// <summary>Bir alt satırı (başlık bağını koruyarak) güncelleyen ortak yardımcı.</summary>
    private async Task<ActionResult<BaseResponse<TChild>>> UpdateChild<TChild>(
        string foreignKeyProperty, Guid id, TChild entity, CancellationToken ct)
        where TChild : AuditableEntity
    {
        var updated = await _command.UpdateChildAsync(foreignKeyProperty, id, entity, ct);
        return updated is null
            ? NotFound(BaseResponse<TChild>.Failure("Record not found."))
            : Ok(BaseResponse<TChild>.Success(updated));
    }

    /// <summary>Bir alt satırı yumuşak silen ortak yardımcı.</summary>
    private async Task<ActionResult<BaseResponse<bool>>> DeleteChild<TChild>(Guid id, CancellationToken ct)
        where TChild : AuditableEntity
        => Ok(BaseResponse<bool>.Success(await _command.DeleteChildAsync<TChild>(id, ct)));

    // ---- Requests ----
    /// <summary>Bir talebin satırları.</summary>
    [HttpGet("request-lines")]
    public Task<ActionResult<BaseResponse<PaginatedResponse<ReqD.RequestLine>>>> RequestLines(
        [FromQuery] Guid parentId, [FromQuery] PaginatedRequest request, CancellationToken ct)
        => Children<ReqD.RequestLine>(nameof(ReqD.RequestLine.RequestId), parentId, request, ct);

    // ---- Procurement ----
    /// <summary>Bir satın alma siparişinin kalemleri.</summary>
    [HttpGet("purchase-order-lines")]
    public Task<ActionResult<BaseResponse<PaginatedResponse<ProcD.PurchaseOrderLine>>>> PurchaseOrderLines(
        [FromQuery] Guid parentId, [FromQuery] PaginatedRequest request, CancellationToken ct)
        => Children<ProcD.PurchaseOrderLine>(nameof(ProcD.PurchaseOrderLine.PurchaseOrderId), parentId, request, ct);

    // ---- Operations ----
    /// <summary>Bir iş emrinin atamaları.</summary>
    [HttpGet("work-order-assignments")]
    public Task<ActionResult<BaseResponse<PaginatedResponse<OpsD.WorkOrderAssignment>>>> WorkOrderAssignments(
        [FromQuery] Guid parentId, [FromQuery] PaginatedRequest request, CancellationToken ct)
        => Children<OpsD.WorkOrderAssignment>(nameof(OpsD.WorkOrderAssignment.WorkOrderId), parentId, request, ct);

    /// <summary>Bir iş emrinin malzeme planı.</summary>
    [HttpGet("work-order-material-plans")]
    public Task<ActionResult<BaseResponse<PaginatedResponse<OpsD.WorkOrderMaterialPlan>>>> WorkOrderMaterialPlans(
        [FromQuery] Guid parentId, [FromQuery] PaginatedRequest request, CancellationToken ct)
        => Children<OpsD.WorkOrderMaterialPlan>(nameof(OpsD.WorkOrderMaterialPlan.WorkOrderId), parentId, request, ct);

    /// <summary>Bir iş emrinin kontrol listeleri.</summary>
    [HttpGet("work-order-checklists")]
    public Task<ActionResult<BaseResponse<PaginatedResponse<OpsD.WorkOrderChecklist>>>> WorkOrderChecklists(
        [FromQuery] Guid parentId, [FromQuery] PaginatedRequest request, CancellationToken ct)
        => Children<OpsD.WorkOrderChecklist>(nameof(OpsD.WorkOrderChecklist.WorkOrderId), parentId, request, ct);

    /// <summary>Bir iş emrinin durum geçmişi.</summary>
    [HttpGet("work-order-status-histories")]
    public Task<ActionResult<BaseResponse<PaginatedResponse<OpsD.WorkOrderStatusHistory>>>> WorkOrderStatusHistories(
        [FromQuery] Guid parentId, [FromQuery] PaginatedRequest request, CancellationToken ct)
        => Children<OpsD.WorkOrderStatusHistory>(nameof(OpsD.WorkOrderStatusHistory.WorkOrderId), parentId, request, ct);

    // ---- FieldOperations ----
    /// <summary>Bir günlük saha raporunun işçi kayıtları.</summary>
    [HttpGet("daily-site-report-workers")]
    public Task<ActionResult<BaseResponse<PaginatedResponse<FieldD.DailySiteReportWorker>>>> DailySiteReportWorkers(
        [FromQuery] Guid parentId, [FromQuery] PaginatedRequest request, CancellationToken ct)
        => Children<FieldD.DailySiteReportWorker>(nameof(FieldD.DailySiteReportWorker.DailySiteReportId), parentId, request, ct);

    /// <summary>Bir günlük saha raporunun ekipman kayıtları.</summary>
    [HttpGet("daily-site-report-equipments")]
    public Task<ActionResult<BaseResponse<PaginatedResponse<FieldD.DailySiteReportEquipment>>>> DailySiteReportEquipments(
        [FromQuery] Guid parentId, [FromQuery] PaginatedRequest request, CancellationToken ct)
        => Children<FieldD.DailySiteReportEquipment>(nameof(FieldD.DailySiteReportEquipment.DailySiteReportId), parentId, request, ct);

    /// <summary>Bir günlük saha raporunun malzeme kayıtları.</summary>
    [HttpGet("daily-site-report-materials")]
    public Task<ActionResult<BaseResponse<PaginatedResponse<FieldD.DailySiteReportMaterial>>>> DailySiteReportMaterials(
        [FromQuery] Guid parentId, [FromQuery] PaginatedRequest request, CancellationToken ct)
        => Children<FieldD.DailySiteReportMaterial>(nameof(FieldD.DailySiteReportMaterial.DailySiteReportId), parentId, request, ct);

    // ---- HR ----
    /// <summary>Bir puantajın satırları.</summary>
    [HttpGet("timesheet-lines")]
    public Task<ActionResult<BaseResponse<PaginatedResponse<HrD.TimesheetLine>>>> TimesheetLines(
        [FromQuery] Guid parentId, [FromQuery] PaginatedRequest request, CancellationToken ct)
        => Children<HrD.TimesheetLine>(nameof(HrD.TimesheetLine.TimesheetId), parentId, request, ct);

    // ---- Assets ----
    /// <summary>Bir ekipmanın zimmet/atama kayıtları.</summary>
    [HttpGet("equipment-assignments")]
    public Task<ActionResult<BaseResponse<PaginatedResponse<AssetD.EquipmentAssignment>>>> EquipmentAssignments(
        [FromQuery] Guid parentId, [FromQuery] PaginatedRequest request, CancellationToken ct)
        => Children<AssetD.EquipmentAssignment>(nameof(AssetD.EquipmentAssignment.EquipmentAssetId), parentId, request, ct);

    /// <summary>Bir ekipmanın bakım kayıtları.</summary>
    [HttpGet("equipment-maintenances")]
    public Task<ActionResult<BaseResponse<PaginatedResponse<AssetD.EquipmentMaintenance>>>> EquipmentMaintenances(
        [FromQuery] Guid parentId, [FromQuery] PaginatedRequest request, CancellationToken ct)
        => Children<AssetD.EquipmentMaintenance>(nameof(AssetD.EquipmentMaintenance.EquipmentAssetId), parentId, request, ct);

    // ---- Finance ----
    /// <summary>Bir finansal hareketin satırları.</summary>
    [HttpGet("financial-transaction-lines")]
    public Task<ActionResult<BaseResponse<PaginatedResponse<FinD.FinancialTransactionLine>>>> FinancialTransactionLines(
        [FromQuery] Guid parentId, [FromQuery] PaginatedRequest request, CancellationToken ct)
        => Children<FinD.FinancialTransactionLine>(nameof(FinD.FinancialTransactionLine.FinancialTransactionId), parentId, request, ct);

    // ---- Budget ----
    /// <summary>Bir bütçenin satırları.</summary>
    [HttpGet("budget-lines")]
    public Task<ActionResult<BaseResponse<PaginatedResponse<BudgetD.BudgetLine>>>> BudgetLines(
        [FromQuery] Guid parentId, [FromQuery] PaginatedRequest request, CancellationToken ct)
        => Children<BudgetD.BudgetLine>(nameof(BudgetD.BudgetLine.BudgetId), parentId, request, ct);

    // ---- Contracts ----
    /// <summary>Bir sözleşmenin kalemleri.</summary>
    [HttpGet("contract-lines")]
    public Task<ActionResult<BaseResponse<PaginatedResponse<ContractD.ContractLine>>>> ContractLines(
        [FromQuery] Guid parentId, [FromQuery] PaginatedRequest request, CancellationToken ct)
        => Children<ContractD.ContractLine>(nameof(ContractD.ContractLine.ContractId), parentId, request, ct);

    /// <summary>Bir sözleşmenin tarafları.</summary>
    [HttpGet("contract-parties")]
    public Task<ActionResult<BaseResponse<PaginatedResponse<ContractD.ContractParty>>>> ContractParties(
        [FromQuery] Guid parentId, [FromQuery] PaginatedRequest request, CancellationToken ct)
        => Children<ContractD.ContractParty>(nameof(ContractD.ContractParty.ContractId), parentId, request, ct);

    /// <summary>Bir sözleşmenin ek/zeyilnameleri.</summary>
    [HttpGet("contract-amendments")]
    public Task<ActionResult<BaseResponse<PaginatedResponse<ContractD.ContractAmendment>>>> ContractAmendments(
        [FromQuery] Guid parentId, [FromQuery] PaginatedRequest request, CancellationToken ct)
        => Children<ContractD.ContractAmendment>(nameof(ContractD.ContractAmendment.ContractId), parentId, request, ct);

    // ---- ProgressPayments ----
    /// <summary>Bir hakedişin satırları.</summary>
    [HttpGet("progress-payment-lines")]
    public Task<ActionResult<BaseResponse<PaginatedResponse<PpD.ProgressPaymentLine>>>> ProgressPaymentLines(
        [FromQuery] Guid parentId, [FromQuery] PaginatedRequest request, CancellationToken ct)
        => Children<PpD.ProgressPaymentLine>(nameof(PpD.ProgressPaymentLine.ProgressPaymentId), parentId, request, ct);

    /// <summary>Bir hakedişin kesintileri.</summary>
    [HttpGet("progress-payment-deductions")]
    public Task<ActionResult<BaseResponse<PaginatedResponse<PpD.ProgressPaymentDeduction>>>> ProgressPaymentDeductions(
        [FromQuery] Guid parentId, [FromQuery] PaginatedRequest request, CancellationToken ct)
        => Children<PpD.ProgressPaymentDeduction>(nameof(PpD.ProgressPaymentDeduction.ProgressPaymentId), parentId, request, ct);

    // ---- Catalog ----
    /// <summary>Bir malzemenin nitelik değerleri.</summary>
    [HttpGet("material-attribute-values")]
    public Task<ActionResult<BaseResponse<PaginatedResponse<CatD.MaterialAttributeValue>>>> MaterialAttributeValues(
        [FromQuery] Guid parentId, [FromQuery] PaginatedRequest request, CancellationToken ct)
        => Children<CatD.MaterialAttributeValue>(nameof(CatD.MaterialAttributeValue.MaterialId), parentId, request, ct);

    /// <summary>Bir malzemenin birim dönüşümleri.</summary>
    [HttpGet("material-unit-conversions")]
    public Task<ActionResult<BaseResponse<PaginatedResponse<CatD.MaterialUnitConversion>>>> MaterialUnitConversions(
        [FromQuery] Guid parentId, [FromQuery] PaginatedRequest request, CancellationToken ct)
        => Children<CatD.MaterialUnitConversion>(nameof(CatD.MaterialUnitConversion.MaterialId), parentId, request, ct);

    // ---- Inventory ----
    /// <summary>Bir deponun lokasyonları.</summary>
    [HttpGet("warehouse-locations")]
    public Task<ActionResult<BaseResponse<PaginatedResponse<InvD.WarehouseLocation>>>> WarehouseLocations(
        [FromQuery] Guid parentId, [FromQuery] PaginatedRequest request, CancellationToken ct)
        => Children<InvD.WarehouseLocation>(nameof(InvD.WarehouseLocation.WarehouseId), parentId, request, ct);

    // =====================================================================================
    //  Alt-koleksiyon yazma (CRUD) uç noktaları. Her koleksiyon için Create/Update/Delete
    //  ayrı literal rotalara sahiptir; böylece her biri kendi ana modülünün
    //  Create/Update/Delete yetkisiyle (ModuleDetails.<Action>) bağımsız korunur. Denetim/iz
    //  niteliğindeki koleksiyonlar (ör. iş emri durum geçmişi) bilinçli olarak salt-okunurdur.
    // =====================================================================================

    // ---- Requests ----
    [HttpPost("request-lines")]
    public Task<ActionResult<BaseResponse<ReqD.RequestLine>>> CreateRequestLine([FromQuery] Guid parentId, [FromBody] ReqD.RequestLine entity, CancellationToken ct)
        => CreateChild(nameof(ReqD.RequestLine.RequestId), parentId, entity, ct);
    [HttpPut("request-lines/{id:guid}")]
    public Task<ActionResult<BaseResponse<ReqD.RequestLine>>> UpdateRequestLine(Guid id, [FromBody] ReqD.RequestLine entity, CancellationToken ct)
        => UpdateChild(nameof(ReqD.RequestLine.RequestId), id, entity, ct);
    [HttpDelete("request-lines/{id:guid}")]
    public Task<ActionResult<BaseResponse<bool>>> DeleteRequestLine(Guid id, CancellationToken ct)
        => DeleteChild<ReqD.RequestLine>(id, ct);

    // ---- Procurement ----
    [HttpPost("purchase-order-lines")]
    public Task<ActionResult<BaseResponse<ProcD.PurchaseOrderLine>>> CreatePurchaseOrderLine([FromQuery] Guid parentId, [FromBody] ProcD.PurchaseOrderLine entity, CancellationToken ct)
        => CreateChild(nameof(ProcD.PurchaseOrderLine.PurchaseOrderId), parentId, entity, ct);
    [HttpPut("purchase-order-lines/{id:guid}")]
    public Task<ActionResult<BaseResponse<ProcD.PurchaseOrderLine>>> UpdatePurchaseOrderLine(Guid id, [FromBody] ProcD.PurchaseOrderLine entity, CancellationToken ct)
        => UpdateChild(nameof(ProcD.PurchaseOrderLine.PurchaseOrderId), id, entity, ct);
    [HttpDelete("purchase-order-lines/{id:guid}")]
    public Task<ActionResult<BaseResponse<bool>>> DeletePurchaseOrderLine(Guid id, CancellationToken ct)
        => DeleteChild<ProcD.PurchaseOrderLine>(id, ct);

    // ---- Operations ----
    [HttpPost("work-order-assignments")]
    public Task<ActionResult<BaseResponse<OpsD.WorkOrderAssignment>>> CreateWorkOrderAssignment([FromQuery] Guid parentId, [FromBody] OpsD.WorkOrderAssignment entity, CancellationToken ct)
        => CreateChild(nameof(OpsD.WorkOrderAssignment.WorkOrderId), parentId, entity, ct);
    [HttpPut("work-order-assignments/{id:guid}")]
    public Task<ActionResult<BaseResponse<OpsD.WorkOrderAssignment>>> UpdateWorkOrderAssignment(Guid id, [FromBody] OpsD.WorkOrderAssignment entity, CancellationToken ct)
        => UpdateChild(nameof(OpsD.WorkOrderAssignment.WorkOrderId), id, entity, ct);
    [HttpDelete("work-order-assignments/{id:guid}")]
    public Task<ActionResult<BaseResponse<bool>>> DeleteWorkOrderAssignment(Guid id, CancellationToken ct)
        => DeleteChild<OpsD.WorkOrderAssignment>(id, ct);

    [HttpPost("work-order-material-plans")]
    public Task<ActionResult<BaseResponse<OpsD.WorkOrderMaterialPlan>>> CreateWorkOrderMaterialPlan([FromQuery] Guid parentId, [FromBody] OpsD.WorkOrderMaterialPlan entity, CancellationToken ct)
        => CreateChild(nameof(OpsD.WorkOrderMaterialPlan.WorkOrderId), parentId, entity, ct);
    [HttpPut("work-order-material-plans/{id:guid}")]
    public Task<ActionResult<BaseResponse<OpsD.WorkOrderMaterialPlan>>> UpdateWorkOrderMaterialPlan(Guid id, [FromBody] OpsD.WorkOrderMaterialPlan entity, CancellationToken ct)
        => UpdateChild(nameof(OpsD.WorkOrderMaterialPlan.WorkOrderId), id, entity, ct);
    [HttpDelete("work-order-material-plans/{id:guid}")]
    public Task<ActionResult<BaseResponse<bool>>> DeleteWorkOrderMaterialPlan(Guid id, CancellationToken ct)
        => DeleteChild<OpsD.WorkOrderMaterialPlan>(id, ct);

    [HttpPost("work-order-checklists")]
    public Task<ActionResult<BaseResponse<OpsD.WorkOrderChecklist>>> CreateWorkOrderChecklist([FromQuery] Guid parentId, [FromBody] OpsD.WorkOrderChecklist entity, CancellationToken ct)
        => CreateChild(nameof(OpsD.WorkOrderChecklist.WorkOrderId), parentId, entity, ct);
    [HttpPut("work-order-checklists/{id:guid}")]
    public Task<ActionResult<BaseResponse<OpsD.WorkOrderChecklist>>> UpdateWorkOrderChecklist(Guid id, [FromBody] OpsD.WorkOrderChecklist entity, CancellationToken ct)
        => UpdateChild(nameof(OpsD.WorkOrderChecklist.WorkOrderId), id, entity, ct);
    [HttpDelete("work-order-checklists/{id:guid}")]
    public Task<ActionResult<BaseResponse<bool>>> DeleteWorkOrderChecklist(Guid id, CancellationToken ct)
        => DeleteChild<OpsD.WorkOrderChecklist>(id, ct);

    // ---- FieldOperations ----
    [HttpPost("daily-site-report-workers")]
    public Task<ActionResult<BaseResponse<FieldD.DailySiteReportWorker>>> CreateDailySiteReportWorker([FromQuery] Guid parentId, [FromBody] FieldD.DailySiteReportWorker entity, CancellationToken ct)
        => CreateChild(nameof(FieldD.DailySiteReportWorker.DailySiteReportId), parentId, entity, ct);
    [HttpPut("daily-site-report-workers/{id:guid}")]
    public Task<ActionResult<BaseResponse<FieldD.DailySiteReportWorker>>> UpdateDailySiteReportWorker(Guid id, [FromBody] FieldD.DailySiteReportWorker entity, CancellationToken ct)
        => UpdateChild(nameof(FieldD.DailySiteReportWorker.DailySiteReportId), id, entity, ct);
    [HttpDelete("daily-site-report-workers/{id:guid}")]
    public Task<ActionResult<BaseResponse<bool>>> DeleteDailySiteReportWorker(Guid id, CancellationToken ct)
        => DeleteChild<FieldD.DailySiteReportWorker>(id, ct);

    [HttpPost("daily-site-report-equipments")]
    public Task<ActionResult<BaseResponse<FieldD.DailySiteReportEquipment>>> CreateDailySiteReportEquipment([FromQuery] Guid parentId, [FromBody] FieldD.DailySiteReportEquipment entity, CancellationToken ct)
        => CreateChild(nameof(FieldD.DailySiteReportEquipment.DailySiteReportId), parentId, entity, ct);
    [HttpPut("daily-site-report-equipments/{id:guid}")]
    public Task<ActionResult<BaseResponse<FieldD.DailySiteReportEquipment>>> UpdateDailySiteReportEquipment(Guid id, [FromBody] FieldD.DailySiteReportEquipment entity, CancellationToken ct)
        => UpdateChild(nameof(FieldD.DailySiteReportEquipment.DailySiteReportId), id, entity, ct);
    [HttpDelete("daily-site-report-equipments/{id:guid}")]
    public Task<ActionResult<BaseResponse<bool>>> DeleteDailySiteReportEquipment(Guid id, CancellationToken ct)
        => DeleteChild<FieldD.DailySiteReportEquipment>(id, ct);

    [HttpPost("daily-site-report-materials")]
    public Task<ActionResult<BaseResponse<FieldD.DailySiteReportMaterial>>> CreateDailySiteReportMaterial([FromQuery] Guid parentId, [FromBody] FieldD.DailySiteReportMaterial entity, CancellationToken ct)
        => CreateChild(nameof(FieldD.DailySiteReportMaterial.DailySiteReportId), parentId, entity, ct);
    [HttpPut("daily-site-report-materials/{id:guid}")]
    public Task<ActionResult<BaseResponse<FieldD.DailySiteReportMaterial>>> UpdateDailySiteReportMaterial(Guid id, [FromBody] FieldD.DailySiteReportMaterial entity, CancellationToken ct)
        => UpdateChild(nameof(FieldD.DailySiteReportMaterial.DailySiteReportId), id, entity, ct);
    [HttpDelete("daily-site-report-materials/{id:guid}")]
    public Task<ActionResult<BaseResponse<bool>>> DeleteDailySiteReportMaterial(Guid id, CancellationToken ct)
        => DeleteChild<FieldD.DailySiteReportMaterial>(id, ct);

    // ---- HR ----
    [HttpPost("timesheet-lines")]
    public Task<ActionResult<BaseResponse<HrD.TimesheetLine>>> CreateTimesheetLine([FromQuery] Guid parentId, [FromBody] HrD.TimesheetLine entity, CancellationToken ct)
        => CreateChild(nameof(HrD.TimesheetLine.TimesheetId), parentId, entity, ct);
    [HttpPut("timesheet-lines/{id:guid}")]
    public Task<ActionResult<BaseResponse<HrD.TimesheetLine>>> UpdateTimesheetLine(Guid id, [FromBody] HrD.TimesheetLine entity, CancellationToken ct)
        => UpdateChild(nameof(HrD.TimesheetLine.TimesheetId), id, entity, ct);
    [HttpDelete("timesheet-lines/{id:guid}")]
    public Task<ActionResult<BaseResponse<bool>>> DeleteTimesheetLine(Guid id, CancellationToken ct)
        => DeleteChild<HrD.TimesheetLine>(id, ct);

    // ---- Assets ----
    [HttpPost("equipment-assignments")]
    public Task<ActionResult<BaseResponse<AssetD.EquipmentAssignment>>> CreateEquipmentAssignment([FromQuery] Guid parentId, [FromBody] AssetD.EquipmentAssignment entity, CancellationToken ct)
        => CreateChild(nameof(AssetD.EquipmentAssignment.EquipmentAssetId), parentId, entity, ct);
    [HttpPut("equipment-assignments/{id:guid}")]
    public Task<ActionResult<BaseResponse<AssetD.EquipmentAssignment>>> UpdateEquipmentAssignment(Guid id, [FromBody] AssetD.EquipmentAssignment entity, CancellationToken ct)
        => UpdateChild(nameof(AssetD.EquipmentAssignment.EquipmentAssetId), id, entity, ct);
    [HttpDelete("equipment-assignments/{id:guid}")]
    public Task<ActionResult<BaseResponse<bool>>> DeleteEquipmentAssignment(Guid id, CancellationToken ct)
        => DeleteChild<AssetD.EquipmentAssignment>(id, ct);

    [HttpPost("equipment-maintenances")]
    public Task<ActionResult<BaseResponse<AssetD.EquipmentMaintenance>>> CreateEquipmentMaintenance([FromQuery] Guid parentId, [FromBody] AssetD.EquipmentMaintenance entity, CancellationToken ct)
        => CreateChild(nameof(AssetD.EquipmentMaintenance.EquipmentAssetId), parentId, entity, ct);
    [HttpPut("equipment-maintenances/{id:guid}")]
    public Task<ActionResult<BaseResponse<AssetD.EquipmentMaintenance>>> UpdateEquipmentMaintenance(Guid id, [FromBody] AssetD.EquipmentMaintenance entity, CancellationToken ct)
        => UpdateChild(nameof(AssetD.EquipmentMaintenance.EquipmentAssetId), id, entity, ct);
    [HttpDelete("equipment-maintenances/{id:guid}")]
    public Task<ActionResult<BaseResponse<bool>>> DeleteEquipmentMaintenance(Guid id, CancellationToken ct)
        => DeleteChild<AssetD.EquipmentMaintenance>(id, ct);

    // ---- Finance ----
    [HttpPost("financial-transaction-lines")]
    public Task<ActionResult<BaseResponse<FinD.FinancialTransactionLine>>> CreateFinancialTransactionLine([FromQuery] Guid parentId, [FromBody] FinD.FinancialTransactionLine entity, CancellationToken ct)
        => CreateChild(nameof(FinD.FinancialTransactionLine.FinancialTransactionId), parentId, entity, ct);
    [HttpPut("financial-transaction-lines/{id:guid}")]
    public Task<ActionResult<BaseResponse<FinD.FinancialTransactionLine>>> UpdateFinancialTransactionLine(Guid id, [FromBody] FinD.FinancialTransactionLine entity, CancellationToken ct)
        => UpdateChild(nameof(FinD.FinancialTransactionLine.FinancialTransactionId), id, entity, ct);
    [HttpDelete("financial-transaction-lines/{id:guid}")]
    public Task<ActionResult<BaseResponse<bool>>> DeleteFinancialTransactionLine(Guid id, CancellationToken ct)
        => DeleteChild<FinD.FinancialTransactionLine>(id, ct);

    // ---- Budget ----
    [HttpPost("budget-lines")]
    public Task<ActionResult<BaseResponse<BudgetD.BudgetLine>>> CreateBudgetLine([FromQuery] Guid parentId, [FromBody] BudgetD.BudgetLine entity, CancellationToken ct)
        => CreateChild(nameof(BudgetD.BudgetLine.BudgetId), parentId, entity, ct);
    [HttpPut("budget-lines/{id:guid}")]
    public Task<ActionResult<BaseResponse<BudgetD.BudgetLine>>> UpdateBudgetLine(Guid id, [FromBody] BudgetD.BudgetLine entity, CancellationToken ct)
        => UpdateChild(nameof(BudgetD.BudgetLine.BudgetId), id, entity, ct);
    [HttpDelete("budget-lines/{id:guid}")]
    public Task<ActionResult<BaseResponse<bool>>> DeleteBudgetLine(Guid id, CancellationToken ct)
        => DeleteChild<BudgetD.BudgetLine>(id, ct);

    // ---- Contracts ----
    [HttpPost("contract-lines")]
    public Task<ActionResult<BaseResponse<ContractD.ContractLine>>> CreateContractLine([FromQuery] Guid parentId, [FromBody] ContractD.ContractLine entity, CancellationToken ct)
        => CreateChild(nameof(ContractD.ContractLine.ContractId), parentId, entity, ct);
    [HttpPut("contract-lines/{id:guid}")]
    public Task<ActionResult<BaseResponse<ContractD.ContractLine>>> UpdateContractLine(Guid id, [FromBody] ContractD.ContractLine entity, CancellationToken ct)
        => UpdateChild(nameof(ContractD.ContractLine.ContractId), id, entity, ct);
    [HttpDelete("contract-lines/{id:guid}")]
    public Task<ActionResult<BaseResponse<bool>>> DeleteContractLine(Guid id, CancellationToken ct)
        => DeleteChild<ContractD.ContractLine>(id, ct);

    [HttpPost("contract-parties")]
    public Task<ActionResult<BaseResponse<ContractD.ContractParty>>> CreateContractParty([FromQuery] Guid parentId, [FromBody] ContractD.ContractParty entity, CancellationToken ct)
        => CreateChild(nameof(ContractD.ContractParty.ContractId), parentId, entity, ct);
    [HttpPut("contract-parties/{id:guid}")]
    public Task<ActionResult<BaseResponse<ContractD.ContractParty>>> UpdateContractParty(Guid id, [FromBody] ContractD.ContractParty entity, CancellationToken ct)
        => UpdateChild(nameof(ContractD.ContractParty.ContractId), id, entity, ct);
    [HttpDelete("contract-parties/{id:guid}")]
    public Task<ActionResult<BaseResponse<bool>>> DeleteContractParty(Guid id, CancellationToken ct)
        => DeleteChild<ContractD.ContractParty>(id, ct);

    [HttpPost("contract-amendments")]
    public Task<ActionResult<BaseResponse<ContractD.ContractAmendment>>> CreateContractAmendment([FromQuery] Guid parentId, [FromBody] ContractD.ContractAmendment entity, CancellationToken ct)
        => CreateChild(nameof(ContractD.ContractAmendment.ContractId), parentId, entity, ct);
    [HttpPut("contract-amendments/{id:guid}")]
    public Task<ActionResult<BaseResponse<ContractD.ContractAmendment>>> UpdateContractAmendment(Guid id, [FromBody] ContractD.ContractAmendment entity, CancellationToken ct)
        => UpdateChild(nameof(ContractD.ContractAmendment.ContractId), id, entity, ct);
    [HttpDelete("contract-amendments/{id:guid}")]
    public Task<ActionResult<BaseResponse<bool>>> DeleteContractAmendment(Guid id, CancellationToken ct)
        => DeleteChild<ContractD.ContractAmendment>(id, ct);

    // ---- ProgressPayments ----
    [HttpPost("progress-payment-lines")]
    public Task<ActionResult<BaseResponse<PpD.ProgressPaymentLine>>> CreateProgressPaymentLine([FromQuery] Guid parentId, [FromBody] PpD.ProgressPaymentLine entity, CancellationToken ct)
        => CreateChild(nameof(PpD.ProgressPaymentLine.ProgressPaymentId), parentId, entity, ct);
    [HttpPut("progress-payment-lines/{id:guid}")]
    public Task<ActionResult<BaseResponse<PpD.ProgressPaymentLine>>> UpdateProgressPaymentLine(Guid id, [FromBody] PpD.ProgressPaymentLine entity, CancellationToken ct)
        => UpdateChild(nameof(PpD.ProgressPaymentLine.ProgressPaymentId), id, entity, ct);
    [HttpDelete("progress-payment-lines/{id:guid}")]
    public Task<ActionResult<BaseResponse<bool>>> DeleteProgressPaymentLine(Guid id, CancellationToken ct)
        => DeleteChild<PpD.ProgressPaymentLine>(id, ct);

    [HttpPost("progress-payment-deductions")]
    public Task<ActionResult<BaseResponse<PpD.ProgressPaymentDeduction>>> CreateProgressPaymentDeduction([FromQuery] Guid parentId, [FromBody] PpD.ProgressPaymentDeduction entity, CancellationToken ct)
        => CreateChild(nameof(PpD.ProgressPaymentDeduction.ProgressPaymentId), parentId, entity, ct);
    [HttpPut("progress-payment-deductions/{id:guid}")]
    public Task<ActionResult<BaseResponse<PpD.ProgressPaymentDeduction>>> UpdateProgressPaymentDeduction(Guid id, [FromBody] PpD.ProgressPaymentDeduction entity, CancellationToken ct)
        => UpdateChild(nameof(PpD.ProgressPaymentDeduction.ProgressPaymentId), id, entity, ct);
    [HttpDelete("progress-payment-deductions/{id:guid}")]
    public Task<ActionResult<BaseResponse<bool>>> DeleteProgressPaymentDeduction(Guid id, CancellationToken ct)
        => DeleteChild<PpD.ProgressPaymentDeduction>(id, ct);

    // ---- Catalog ----
    [HttpPost("material-attribute-values")]
    public Task<ActionResult<BaseResponse<CatD.MaterialAttributeValue>>> CreateMaterialAttributeValue([FromQuery] Guid parentId, [FromBody] CatD.MaterialAttributeValue entity, CancellationToken ct)
        => CreateChild(nameof(CatD.MaterialAttributeValue.MaterialId), parentId, entity, ct);
    [HttpPut("material-attribute-values/{id:guid}")]
    public Task<ActionResult<BaseResponse<CatD.MaterialAttributeValue>>> UpdateMaterialAttributeValue(Guid id, [FromBody] CatD.MaterialAttributeValue entity, CancellationToken ct)
        => UpdateChild(nameof(CatD.MaterialAttributeValue.MaterialId), id, entity, ct);
    [HttpDelete("material-attribute-values/{id:guid}")]
    public Task<ActionResult<BaseResponse<bool>>> DeleteMaterialAttributeValue(Guid id, CancellationToken ct)
        => DeleteChild<CatD.MaterialAttributeValue>(id, ct);

    [HttpPost("material-unit-conversions")]
    public Task<ActionResult<BaseResponse<CatD.MaterialUnitConversion>>> CreateMaterialUnitConversion([FromQuery] Guid parentId, [FromBody] CatD.MaterialUnitConversion entity, CancellationToken ct)
        => CreateChild(nameof(CatD.MaterialUnitConversion.MaterialId), parentId, entity, ct);
    [HttpPut("material-unit-conversions/{id:guid}")]
    public Task<ActionResult<BaseResponse<CatD.MaterialUnitConversion>>> UpdateMaterialUnitConversion(Guid id, [FromBody] CatD.MaterialUnitConversion entity, CancellationToken ct)
        => UpdateChild(nameof(CatD.MaterialUnitConversion.MaterialId), id, entity, ct);
    [HttpDelete("material-unit-conversions/{id:guid}")]
    public Task<ActionResult<BaseResponse<bool>>> DeleteMaterialUnitConversion(Guid id, CancellationToken ct)
        => DeleteChild<CatD.MaterialUnitConversion>(id, ct);

    // ---- Inventory ----
    [HttpPost("warehouse-locations")]
    public Task<ActionResult<BaseResponse<InvD.WarehouseLocation>>> CreateWarehouseLocation([FromQuery] Guid parentId, [FromBody] InvD.WarehouseLocation entity, CancellationToken ct)
        => CreateChild(nameof(InvD.WarehouseLocation.WarehouseId), parentId, entity, ct);
    [HttpPut("warehouse-locations/{id:guid}")]
    public Task<ActionResult<BaseResponse<InvD.WarehouseLocation>>> UpdateWarehouseLocation(Guid id, [FromBody] InvD.WarehouseLocation entity, CancellationToken ct)
        => UpdateChild(nameof(InvD.WarehouseLocation.WarehouseId), id, entity, ct);
    [HttpDelete("warehouse-locations/{id:guid}")]
    public Task<ActionResult<BaseResponse<bool>>> DeleteWarehouseLocation(Guid id, CancellationToken ct)
        => DeleteChild<InvD.WarehouseLocation>(id, ct);
}

