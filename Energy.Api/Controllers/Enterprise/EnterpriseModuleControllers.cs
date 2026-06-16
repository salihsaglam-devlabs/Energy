using Energy.Application.Common.Crud;
using Microsoft.AspNetCore.Mvc;
using CoreD = Energy.Domain.Core;
using OrgD = Energy.Domain.Organization;
using BpD = Energy.Domain.BusinessPartners;
using ProjD = Energy.Domain.Projects;
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
using DocD = Energy.Domain.Documents;
using WfD = Energy.Domain.Workflow;
using NotifD = Energy.Domain.Notifications;
using RepD = Energy.Domain.Reporting;

namespace Energy.Api.Controllers.Enterprise;

// Her modül için ana varlığı yöneten, permission ile korunan CRUD denetleyicisi.
// Denetleyici adı (Controller eki hariç) permission modülüyle birebir eşleşir; böylece
// DefaultEndpointPermissionMap'teki "<Module>.<Action>" kuralları otomatik uygulanır.

/// <summary>Core modülü — Şirketler.</summary>
[Route("api/v{version:apiVersion}/core")]
public sealed class CoreController(IGenericCrudService<CoreD.Company> service)
    : EnterpriseCrudControllerBase<CoreD.Company>(service);

/// <summary>Organization modülü — Personeller.</summary>
[Route("api/v{version:apiVersion}/organization")]
public sealed class OrganizationController(IGenericCrudService<OrgD.Employee> service)
    : EnterpriseCrudControllerBase<OrgD.Employee>(service);

/// <summary>BusinessPartners modülü — Cariler.</summary>
[Route("api/v{version:apiVersion}/business-partners")]
public sealed class BusinessPartnersController(IGenericCrudService<BpD.BusinessPartner> service)
    : EnterpriseCrudControllerBase<BpD.BusinessPartner>(service);

/// <summary>Projects modülü — Projeler.</summary>
[Route("api/v{version:apiVersion}/projects")]
public sealed class ProjectsController(IGenericCrudService<ProjD.Project> service)
    : EnterpriseCrudControllerBase<ProjD.Project>(service);

/// <summary>Catalog modülü — Malzemeler.</summary>
[Route("api/v{version:apiVersion}/catalog")]
public sealed class CatalogController(IGenericCrudService<CatD.Material> service)
    : EnterpriseCrudControllerBase<CatD.Material>(service);

/// <summary>Inventory modülü — Depolar.</summary>
[Route("api/v{version:apiVersion}/inventory")]
public sealed class InventoryController(IGenericCrudService<InvD.Warehouse> service)
    : EnterpriseCrudControllerBase<InvD.Warehouse>(service);

/// <summary>Requests modülü — Talepler.</summary>
[Route("api/v{version:apiVersion}/requests")]
public sealed class RequestsController(IGenericCrudService<ReqD.Request> service)
    : EnterpriseCrudControllerBase<ReqD.Request>(service);

/// <summary>Procurement modülü — Satın alma siparişleri.</summary>
[Route("api/v{version:apiVersion}/procurement")]
public sealed class ProcurementController(IGenericCrudService<ProcD.PurchaseOrder> service)
    : EnterpriseCrudControllerBase<ProcD.PurchaseOrder>(service);

/// <summary>Operations modülü — İş emirleri.</summary>
[Route("api/v{version:apiVersion}/operations")]
public sealed class OperationsController(IGenericCrudService<OpsD.WorkOrder> service)
    : EnterpriseCrudControllerBase<OpsD.WorkOrder>(service);

/// <summary>FieldOperations modülü — Günlük saha raporları.</summary>
[Route("api/v{version:apiVersion}/field-operations")]
public sealed class FieldOperationsController(IGenericCrudService<FieldD.DailySiteReport> service)
    : EnterpriseCrudControllerBase<FieldD.DailySiteReport>(service);

/// <summary>HR modülü — Puantajlar.</summary>
[Route("api/v{version:apiVersion}/hr")]
public sealed class HRController(IGenericCrudService<HrD.Timesheet> service)
    : EnterpriseCrudControllerBase<HrD.Timesheet>(service);

/// <summary>Assets modülü — Ekipmanlar.</summary>
[Route("api/v{version:apiVersion}/assets")]
public sealed class AssetsController(IGenericCrudService<AssetD.EquipmentAsset> service)
    : EnterpriseCrudControllerBase<AssetD.EquipmentAsset>(service);

/// <summary>Finance modülü — Finansal hareketler.</summary>
[Route("api/v{version:apiVersion}/finance")]
public sealed class FinanceController(IGenericCrudService<FinD.FinancialTransaction> service)
    : EnterpriseCrudControllerBase<FinD.FinancialTransaction>(service);

/// <summary>Budget modülü — Bütçeler.</summary>
[Route("api/v{version:apiVersion}/budget")]
public sealed class BudgetController(IGenericCrudService<BudgetD.Budget> service)
    : EnterpriseCrudControllerBase<BudgetD.Budget>(service);

/// <summary>Contracts modülü — Sözleşmeler.</summary>
[Route("api/v{version:apiVersion}/contracts")]
public sealed class ContractsController(IGenericCrudService<ContractD.Contract> service)
    : EnterpriseCrudControllerBase<ContractD.Contract>(service);

/// <summary>ProgressPayments modülü — Hakedişler.</summary>
[Route("api/v{version:apiVersion}/progress-payments")]
public sealed class ProgressPaymentsController(IGenericCrudService<PpD.ProgressPayment> service)
    : EnterpriseCrudControllerBase<PpD.ProgressPayment>(service);

/// <summary>Documents modülü — Belgeler.</summary>
[Route("api/v{version:apiVersion}/documents")]
public sealed class DocumentsController(IGenericCrudService<DocD.Document> service)
    : EnterpriseCrudControllerBase<DocD.Document>(service);

/// <summary>Workflow modülü — Onay talepleri.</summary>
[Route("api/v{version:apiVersion}/workflow")]
public sealed class WorkflowController(IGenericCrudService<WfD.ApprovalRequest> service)
    : EnterpriseCrudControllerBase<WfD.ApprovalRequest>(service);

/// <summary>Notifications modülü — Bildirimler.</summary>
[Route("api/v{version:apiVersion}/notifications")]
public sealed class NotificationsController(IGenericCrudService<NotifD.Notification> service)
    : EnterpriseCrudControllerBase<NotifD.Notification>(service);

/// <summary>Reporting modülü — Rapor tanımları.</summary>
[Route("api/v{version:apiVersion}/reporting")]
public sealed class ReportingController(IGenericCrudService<RepD.ReportDefinition> service)
    : EnterpriseCrudControllerBase<RepD.ReportDefinition>(service);

