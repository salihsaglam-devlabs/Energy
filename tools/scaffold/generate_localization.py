#!/usr/bin/env python3
"""
Energy — localization key generator.

Injects per-entity screen + menu localization keys into the SharedResource resx
files (neutral, tr-TR, en-US), idempotently (existing names are skipped):
  * Modules.{Module}.{Entity}.Title  — screen header (all 134 entities)
  * Menus.{Module}.{Entity}          — menu label (121 web-managed entities)

Values default to the spaced PascalCase entity name (clean, human-readable);
proper Turkish wording is a follow-up content task tracked in the coverage report.
"""
from __future__ import annotations

import os
import re

from generate_domain import ROOT, build_model

RESX = [
    os.path.join(ROOT, "Energy.Localization", "Resources", "SharedResource.resx"),
    os.path.join(ROOT, "Energy.Localization", "Resources", "SharedResource.tr-TR.resx"),
    os.path.join(ROOT, "Energy.Localization", "Resources", "SharedResource.en-US.resx"),
]
EXCLUDE_MENU = {"IAM", "Chat"}


def spaced(name: str) -> str:
    return re.sub(r"(?<!^)(?=[A-Z])", " ", name)


def xml_escape(s: str) -> str:
    return (s.replace("&", "&amp;").replace("<", "&lt;").replace(">", "&gt;"))


def build_entries():
    order, table_module, _, _, table_entity = build_model()
    entries = {}  # name -> value
    for t in order:
        m, e = table_module[t], table_entity[t]
        entries[f"Modules.{m}.{e}.Title"] = spaced(e)
        if m not in EXCLUDE_MENU:
            entries[f"Menus.{m}.{e}"] = spaced(e)

    # ER Overview report screens: title, menu, filter labels.
    try:
        from generate_reports import REPORTS
    except Exception:
        REPORTS = []
    for rep in REPORTS:
        m, n = rep["module"], rep["name"]
        entries[f"Modules.{m}.Reports.{n}.Title"] = spaced(n)
        entries[f"Menus.{m}.Reports.{n}"] = spaced(n)
        entries[f"Modules.{m}.Reports.{n}.Filters.StartDate"] = "Start Date"
        entries[f"Modules.{m}.Reports.{n}.Filters.EndDate"] = "End Date"
        if rep.get("status_field"):
            entries[f"Modules.{m}.Reports.{n}.Filters.Status"] = "Status"
        entries[f"Modules.{m}.Reports.{n}.Actions.Export"] = "Export"

    # Process screens (standard process route) — currently the Approval inbox.
    entries["Modules.Workflow.Processes.Approval.Title"] = "Approval Inbox"
    entries["Modules.Workflow.Processes.Approval.Hint"] = "Pending approvals awaiting your action."
    entries["Modules.Workflow.Processes.Approval.NotePrompt"] = "Note (optional)"
    entries["Modules.Workflow.Processes.Approval.Actions.Approve"] = "Approve"
    entries["Modules.Workflow.Processes.Approval.Actions.Reject"] = "Reject"
    entries["Modules.Workflow.Processes.Approval.Actions.Cancel"] = "Cancel"
    entries["Menus.Workflow.Processes.Approval"] = "Approval Inbox"

    # Inventory / Procurement form-based process screens.
    entries["Modules.Inventory.Processes.StockIssue.Title"] = "Stock Issue"
    entries["Modules.Inventory.Processes.StockIssue.Actions.Execute"] = "Issue Stock"
    entries["Modules.Inventory.Processes.StockIssue.Messages.Completed"] = "Stock issue completed."
    entries["Menus.Inventory.Processes.StockIssue"] = "Stock Issue"

    entries["Modules.Inventory.Processes.StockTransfer.Title"] = "Stock Transfer"
    entries["Modules.Inventory.Processes.StockTransfer.Actions.Execute"] = "Transfer Stock"
    entries["Modules.Inventory.Processes.StockTransfer.Messages.Completed"] = "Stock transfer completed."
    entries["Menus.Inventory.Processes.StockTransfer"] = "Stock Transfer"

    entries["Modules.Procurement.Processes.GoodsReceipt.Title"] = "Goods Receipt"
    entries["Modules.Procurement.Processes.GoodsReceipt.Hint"] = "Convert an approved purchase receipt into a stock-in document."
    entries["Modules.Procurement.Processes.GoodsReceipt.Actions.Execute"] = "Receive"
    entries["Modules.Procurement.Processes.GoodsReceipt.Messages.Completed"] = "Goods receipt completed."
    entries["Menus.Procurement.Processes.GoodsReceipt"] = "Goods Receipt"

    # Finance process screens (HR Cost + Contracts flows).
    entries["Modules.Finance.Processes.TimesheetCost.Title"] = "Timesheet Costing"
    entries["Modules.Finance.Processes.TimesheetCost.Hint"] = "Post an approved timesheet's labour cost to a financial transaction."
    entries["Modules.Finance.Processes.TimesheetCost.Actions.Execute"] = "Post Cost"
    entries["Modules.Finance.Processes.TimesheetCost.Messages.Completed"] = "Timesheet cost posted."
    entries["Menus.Finance.Processes.TimesheetCost"] = "Timesheet Costing"

    entries["Modules.Finance.Processes.ProgressPaymentPosting.Title"] = "Progress Payment Posting"
    entries["Modules.Finance.Processes.ProgressPaymentPosting.Hint"] = "Post an approved progress payment to a receivable/payable transaction."
    entries["Modules.Finance.Processes.ProgressPaymentPosting.Actions.Execute"] = "Post"
    entries["Modules.Finance.Processes.ProgressPaymentPosting.Messages.Completed"] = "Progress payment posted."
    entries["Menus.Finance.Processes.ProgressPaymentPosting"] = "Progress Payment Posting"

    entries["Modules.Finance.Processes.PaymentAllocation.Title"] = "Payment Allocation"
    entries["Modules.Finance.Processes.PaymentAllocation.Hint"] = "Allocate a payment across one or more payables in a single transaction."
    entries["Modules.Finance.Processes.PaymentAllocation.Lines"] = "Allocation Lines"
    entries["Modules.Finance.Processes.PaymentAllocation.Actions.Execute"] = "Allocate"
    entries["Modules.Finance.Processes.PaymentAllocation.Messages.Completed"] = "Payment allocated."
    entries["Modules.Finance.Processes.PaymentAllocation.Messages.PickPayment"] = "Select a payment and add at least one line."
    entries["Menus.Finance.Processes.PaymentAllocation"] = "Payment Allocation"

    # Documents file & version management screen.
    entries["Modules.Documents.Files.Title"] = "Document Files"
    entries["Modules.Documents.Files.Hint"] = "Upload new versions and download the version history of a document."
    entries["Modules.Documents.Files.VersionHistory"] = "Version History"
    entries["Modules.Documents.Files.SelectDocument"] = "Select a document"
    entries["Modules.Documents.Files.Actions.Upload"] = "Upload New Version"
    entries["Modules.Documents.Files.Actions.Download"] = "Download"
    entries["Modules.Documents.Files.Messages.Uploaded"] = "New version uploaded."
    entries["Modules.Documents.Files.Messages.PickDocument"] = "Please select a document first."
    entries["Menus.Documents.Files"] = "Document Files"
    return entries


def inject(path, entries):
    with open(path, "r", encoding="utf-8") as f:
        text = f.read()
    existing = set(re.findall(r'<data name="([^"]+)"', text))
    additions = []
    for name, value in entries.items():
        if name in existing:
            continue
        additions.append(
            f'  <data name="{name}" xml:space="preserve"><value>{xml_escape(value)}</value></data>')
    if not additions:
        return 0
    block = "\n".join(additions) + "\n</root>"
    text = text.rstrip()
    if not text.endswith("</root>"):
        raise RuntimeError(f"Unexpected resx tail in {path}")
    text = text[: -len("</root>")].rstrip() + "\n" + block + "\n"
    with open(path, "w", encoding="utf-8") as f:
        f.write(text)
    return len(additions)


def main():
    entries = build_entries()
    total = 0
    for path in RESX:
        n = inject(path, entries)
        total += n
        print(f"{os.path.basename(path)}: +{n} keys")
    print(f"Total resx additions: {total} (unique keys: {len(entries)})")


if __name__ == "__main__":
    main()

