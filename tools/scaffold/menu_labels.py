#!/usr/bin/env python3
"""Professional menu labels for the redesigned navigation tree (TR/EN/neutral)."""
import os, re

BASE = os.path.join(os.path.dirname(__file__), "..", "..", "Energy.Localization", "Resources")

# key -> (tr, en)
M = {
    # ---- L1 functional areas (unique, no abbreviations, no area/module name clash) ----
    "Menus.ProjectsArea":   ("Proje ve Saha Yönetimi", "Projects & Field"),
    "Menus.SupplyArea":     ("Tedarik ve Stok Yönetimi", "Supply & Inventory"),
    "Menus.FinanceArea":    ("Finans ve Bütçe", "Finance & Budget"),
    "Menus.HRArea":         ("İnsan Kaynakları", "Human Resources"),
    "Menus.PartnersArea":   ("İş Ortakları ve Dokümanlar", "Partners & Documents"),
    "Menus.WorkflowArea":   ("İş Akışı ve Bildirimler", "Workflow & Notifications"),
    "Menus.MasterDataArea": ("Tanımlar ve Ana Veri", "Master Data & Definitions"),
    "Menus.System":         ("Sistem Yönetimi", "System Administration"),
    # ---- L2 modules (clear, consistent) ----
    "Menus.Projects":         ("Projeler", "Projects"),
    "Menus.Operations":       ("İş Emirleri", "Work Orders"),
    "Menus.FieldOperations":  ("Saha Operasyonları", "Field Operations"),
    "Menus.Contracts":        ("Sözleşmeler", "Contracts"),
    "Menus.ProgressPayments": ("Hakedişler", "Progress Payments"),
    "Menus.Catalog":          ("Malzeme Kataloğu", "Material Catalog"),
    "Menus.Inventory":        ("Stok ve Depo", "Stock & Warehouses"),
    "Menus.Requests":         ("Talepler", "Requests"),
    "Menus.Procurement":      ("Satınalma", "Procurement"),
    "Menus.Finance":          ("Finans", "Finance"),
    "Menus.Budget":           ("Bütçe", "Budget"),
    "Menus.Organization":     ("Organizasyon ve Personel", "Organization & Staff"),
    "Menus.HR":               ("Puantaj", "Timesheets"),
    "Menus.Assets":           ("Ekipman ve Demirbaş", "Equipment & Assets"),
    "Menus.BusinessPartners": ("İş Ortakları", "Business Partners"),
    "Menus.Documents":        ("Dokümanlar", "Documents"),
    "Menus.Workflow":         ("Onay Akışları", "Approval Workflows"),
    "Menus.Notifications":    ("Bildirimler", "Notifications"),
    "Menus.Reporting":        ("Raporlama Tanımları", "Report Definitions"),
    "Menus.CoreData":         ("Temel Tanımlar", "Core Definitions"),
    # ---- System administration screens ----
    "Menus.Localization":     ("Çeviri Yönetimi", "Localization"),
    "Menus.Logs":             ("Sistem Günlükleri", "System Logs"),
    # ---- Veri Yönetimi (tüm tablolar, kısıtlı: System.DataAdmin) ----
    "Menus.DataAdminArea":              ("Veri Yönetimi", "Data Administration"),
    "Menus.DataAdmin.Core":             ("Çekirdek / Temel", "Core"),
    "Menus.DataAdmin.Organization":     ("Organizasyon", "Organization"),
    "Menus.DataAdmin.BusinessPartners": ("İş Ortakları", "Business Partners"),
    "Menus.DataAdmin.Projects":         ("Projeler", "Projects"),
    "Menus.DataAdmin.Catalog":          ("Katalog", "Catalog"),
    "Menus.DataAdmin.Inventory":        ("Stok ve Depo", "Inventory"),
    "Menus.DataAdmin.Requests":         ("Talepler", "Requests"),
    "Menus.DataAdmin.Procurement":      ("Satınalma", "Procurement"),
    "Menus.DataAdmin.Operations":       ("İş Emirleri", "Operations"),
    "Menus.DataAdmin.FieldOperations":  ("Saha Operasyonları", "Field Operations"),
    "Menus.DataAdmin.HR":               ("Puantaj / İK", "Human Resources"),
    "Menus.DataAdmin.Assets":           ("Ekipman ve Demirbaş", "Assets"),
    "Menus.DataAdmin.Finance":          ("Finans", "Finance"),
    "Menus.DataAdmin.Budget":           ("Bütçe", "Budget"),
    "Menus.DataAdmin.Contracts":        ("Sözleşmeler", "Contracts"),
    "Menus.DataAdmin.ProgressPayments": ("Hakedişler", "Progress Payments"),
    "Menus.DataAdmin.Documents":        ("Dokümanlar", "Documents"),
    "Menus.DataAdmin.Workflow":         ("Onay Akışları", "Workflow"),
    "Menus.DataAdmin.Notifications":    ("Bildirimler", "Notifications"),
    "Menus.DataAdmin.Reporting":        ("Raporlama", "Reporting"),
    # ---- Menu search box placeholder ----
    "Layout.MenuSearch":      ("Menüde ara...", "Search menu..."),
}

FILES = {"SharedResource.tr-TR.resx": 0, "SharedResource.en-US.resx": 1, "SharedResource.resx": 1}


def upsert(txt, name, value):
    value = value.replace("&", "&amp;")
    pat = re.compile(r'(<data name="' + re.escape(name) + r'"[^>]*>\s*<value>).*?(</value>)', re.S)
    if pat.search(txt):
        return pat.sub(lambda m: m.group(1) + value + m.group(2), txt, count=1)
    block = f'  <data name="{name}" xml:space="preserve">\n    <value>{value}</value>\n  </data>\n'
    return txt.replace("</root>", block + "</root>")


for fn, idx in FILES.items():
    p = os.path.join(BASE, fn)
    txt = open(p, encoding="utf-8").read()
    for name, vals in M.items():
        txt = upsert(txt, name, vals[idx])
    open(p, "w", encoding="utf-8").write(txt)
    print(f"{fn}: {len(M)} labels upserted")
print("Done.")

