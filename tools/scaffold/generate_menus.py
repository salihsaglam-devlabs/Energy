#!/usr/bin/env python3
"""
Energy — per-entity menu seed generator (Phase: Menu standard).

Emits Energy.Infrastructure/Seeding/SystemSeeder.ModulesMenus.cs: an idempotent
seeder that nests one menu entry per web-managed entity UNDER its module menu,
pointing at the new per-entity route /{module-kebab}/{entity-plural-kebab} and
guarded by {Module}.ReadAll. IAM/Chat excluded (curated screens).
"""
from __future__ import annotations

import os
import re

from generate_domain import ROOT, build_model

OUT = os.path.join(ROOT, "Energy.Infrastructure", "Seeding", "SystemSeeder.ModulesMenus.cs")
EXCLUDE_MODULES = {"IAM", "Chat"}

# KÜRATÖRLÜ NAVİGASYON: Menüye yalnızca gezilebilir iş ekranları (aggregate root /
# ana veri + ana işlem belgeleri) eklenir. Aşağıdaki alt tablolar (satır/detay,
# junction, versiyon, geçmiş, dağıtım, adım/onaylayan) menüye AYRI öğe olarak
# EKLENMEZ; bunlar daima ait oldukları üst ekran içinde yönetilir. NameKey biçimi:
# "Menus.<Module>.<Entity>".
EXCLUDE_ENTITY_KEYS = {
    "Menus.Organization.EmployeeSkillAssignment", "Menus.Organization.ExpenseClaimLine",
    "Menus.BusinessPartners.BusinessPartnerContact", "Menus.BusinessPartners.BusinessPartnerAddress",
    "Menus.BusinessPartners.BusinessPartnerBankAccount",
    "Menus.Projects.ProjectPhase", "Menus.Projects.ProjectMember", "Menus.Projects.ProjectNote",
    "Menus.Catalog.MaterialAttributeOption", "Menus.Catalog.MaterialCategoryAttribute",
    "Menus.Catalog.MaterialAttributeValue", "Menus.Catalog.MaterialUnitConversion",
    "Menus.Inventory.StockDocumentLine", "Menus.Inventory.StockIssueAllocation",
    "Menus.Inventory.StockCountLine", "Menus.Inventory.WarehouseTransferLine",
    "Menus.Requests.RequestLine",
    "Menus.Procurement.SupplierQuoteLine", "Menus.Procurement.PurchaseOrderLine",
    "Menus.Procurement.PurchaseReceiptLine", "Menus.Procurement.SupplierInvoiceLine",
    "Menus.Operations.WorkOrderAssignment", "Menus.Operations.WorkOrderMaterialPlan",
    "Menus.Operations.WorkOrderMaterialUsage", "Menus.Operations.WorkOrderChecklist",
    "Menus.Operations.WorkOrderChecklistItem", "Menus.Operations.WorkOrderStatusHistory",
    "Menus.FieldOperations.DailySiteReportWorker", "Menus.FieldOperations.DailySiteReportEquipment",
    "Menus.FieldOperations.DailySiteReportMaterial", "Menus.FieldOperations.MeasurementSheetLine",
    "Menus.HR.TimesheetLine",
    "Menus.Finance.FinancialTransactionLine", "Menus.Finance.PaymentAllocation",
    "Menus.Finance.CollectionAllocation",
    "Menus.Budget.BudgetLine",
    "Menus.Contracts.ContractParty", "Menus.Contracts.ContractLine", "Menus.Contracts.ContractAmendment",
    "Menus.ProgressPayments.ProgressPaymentLine", "Menus.ProgressPayments.ProgressPaymentDeduction",
    "Menus.Documents.DocumentVersion", "Menus.Documents.DocumentRelation",
    "Menus.Documents.DocumentPermission",
    "Menus.Workflow.ApprovalDefinitionVersion", "Menus.Workflow.ApprovalStepDefinition",
    "Menus.Workflow.ApprovalStepApprover", "Menus.Workflow.ApprovalCondition",
    "Menus.Workflow.ApprovalRequestStep", "Menus.Workflow.ApprovalRequestApprover",
    "Menus.Workflow.ApprovalAction",
    "Menus.Notifications.NotificationRecipient",
}


def kebab(name: str) -> str:
    return re.sub(r"(?<!^)(?=[A-Z])", "-", name).lower()


def main():
    order, table_module, table_purpose, table_columns, table_entity = build_model()
    rows = []
    per_module_index = {}
    for t in order:
        m, e = table_module[t], table_entity[t]
        if m in EXCLUDE_MODULES:
            continue
        name_key = f"Menus.{m}.{e}"
        if name_key in EXCLUDE_ENTITY_KEYS:
            continue  # küratörlü: alt tablolar menüye eklenmez
        idx = per_module_index.get(m, 0) + 1
        per_module_index[m] = idx
        route = f"/{kebab(m)}/{kebab(t)}"
        parent_key = "Menus.CoreData" if m == "Core" else f"Menus.{m}"
        rows.append((m, parent_key, e, route, name_key, idx))

    L = [
        "using Microsoft.EntityFrameworkCore;",
        "using Microsoft.Extensions.Logging;", "",
        "namespace Energy.Infrastructure.Seeding;", "",
        "/// <summary>",
        "/// Per-entity menü tohumlaması: her web-yönetimli entity, modül menüsünün altına",
        "/// /{module}/{entity} rotasıyla ve {Module}.ReadAll yetkisiyle idempotent eklenir.",
        "/// </summary>",
        "public sealed partial class SystemSeeder",
        "{",
        "    /// <summary>(Module, ParentMenuNameKey, Entity, Route, NameKey, Order)</summary>",
        "    private static readonly (string Module, string ParentKey, string Entity, string Route, string NameKey, int Order)[] ModuleEntityMenus =",
        "    [",
    ]
    for (m, pk, e, route, nk, idx) in rows:
        L.append(f'        ("{m}", "{pk}", "{e}", "{route}", "{nk}", {idx}),')
    L += [
        "    ];", "",
        "    /// <summary>Modül menüsünün altına per-entity menü girdilerini idempotent ekler.</summary>",
        "    private async Task EnsureModulesEntityMenusAsync(CancellationToken ct)",
        "    {",
        "        foreach (var (module, parentKey, _, route, nameKey, order) in ModuleEntityMenus)",
        "        {",
        "            var parent = await _db.Menus.FirstOrDefaultAsync(m => m.NameKey == parentKey, ct);",
        "            if (parent is null)",
        "            {",
        "                continue;",
        "            }",
        '            await EnsureMenuAsync(nameKey, parent.Id, route, "doc", 100 + order, $"{module}.ReadAll", ct);',
        "        }",
        '        _logger.LogInformation("Seeding: {Count} per-entity module menu(s) ensured.", ModuleEntityMenus.Length);',
        "    }",
        "}", "",
    ]
    with open(OUT, "w", encoding="utf-8") as f:
        f.write("\n".join(L))
    print(f"Generated menu seeder with {len(rows)} entity menu entries")


if __name__ == "__main__":
    main()

