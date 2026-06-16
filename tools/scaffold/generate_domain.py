#!/usr/bin/env python3
"""
Energy — deterministic Domain scaffolding generator.

Reads the authoritative Excel design document and emits ONE C# entity file per
table under Energy.Domain/Modules/{Module}/Entities/{Entity}.cs.

This is a BUILD-TIME authoring tool only (like T4 / Roslyn source scaffolding).
The runtime architecture is NOT generic: every table gets its own dedicated
class, file and namespace, exactly as required by the project design rules.

Conventions (mirrors the existing Energy.Domain codebase):
  * Tables that carry the full audit column set derive from AuditableEntity
    (Id + CreatedAt/By, UpdatedAt/By, IsDeleted, DeletedAt/By live on the base).
  * Other tables become plain classes with an explicit Id (+ their own columns).
  * Foreign keys are stored as scalar Guid properties; navigations are wired in
    EF Fluent configurations (separate generator phase), never on the entity.
"""
from __future__ import annotations

import os
import shutil
import openpyxl

ROOT = os.path.abspath(os.path.join(os.path.dirname(__file__), "..", ".."))
XLSX = os.path.join(ROOT, "Energy.Web", "wwwroot", "docs",
                    "Energy_Teknik_Tasarim_Dokumani-v1.xlsx")
DOMAIN_ROOT = os.path.join(ROOT, "Energy.Domain", "Modules")

AUDIT_COLUMNS = {
    "Id", "CreatedAt", "CreatedBy", "UpdatedAt", "UpdatedBy",
    "IsDeleted", "DeletedAt", "DeletedBy",
}

# Table (plural) -> Entity (singular). Rules + irregular exceptions for this schema.
SINGULAR_EXCEPTIONS = {
    "UnitsOfMeasure": "UnitOfMeasure",
    "Warehouses": "Warehouse",
}


def singularize(name: str) -> str:
    """Plural table name -> singular PascalCase entity name."""
    if name in SINGULAR_EXCEPTIONS:
        return SINGULAR_EXCEPTIONS[name]
    if name.endswith("ies"):
        return name[:-3] + "y"
    for suf in ("sses", "ches", "shes", "xes", "zes", "ses"):
        if name.endswith(suf):
            return name[:-2]
    if name.endswith("s") and not name.endswith("ss"):
        return name[:-1]
    return name


def load_rows(sheet):
    wb = openpyxl.load_workbook(XLSX, data_only=True)
    ws = wb[sheet]
    rows = []
    for r in ws.iter_rows(values_only=True):
        if any(c is not None for c in r):
            rows.append([("" if c is None else str(c).strip()) for c in r])
    return rows


def map_type(data_type: str, nullable: bool) -> tuple[str, bool]:
    """Return (csharp_type, is_reference_string)."""
    dt = data_type.lower()
    if "uniqueidentifier" in dt:
        base = "Guid"
    elif "datetime" in dt or dt == "date":
        base = "DateTime"
    elif dt == "bit" or "bool" in dt:
        base = "bool"
    elif dt in ("int", "integer"):
        base = "int"
    elif dt in ("bigint",):
        base = "long"
    elif any(k in dt for k in ("decimal", "money", "numeric")):
        base = "decimal"
    elif "float" in dt or "double" in dt or "real" in dt:
        base = "double"
    else:
        return ("string", True)  # textual
    if nullable:
        base += "?"
    return (base, False)


# Primitive C# types we accept verbatim when enriching from legacy entities.
PRIMITIVE_CS = {
    "Guid", "string", "bool", "int", "long", "short", "byte",
    "decimal", "double", "float", "DateTime", "DateOnly", "TimeOnly", "TimeSpan",
}


def csharp_type(col) -> tuple[str, bool]:
    """Return (csharp_type, is_string) for a column, honoring a pre-resolved 'cstype'."""
    cs = col.get("cstype")
    if cs:
        return cs, cs.rstrip("?") == "string"
    return map_type(col["dtype"], col["nullable"])


def parse_legacy_entities() -> dict[str, list[tuple[str, str]]]:
    """Extract scalar properties from legacy *Entities.cs (authoritative real schema
    that is richer than the Excel for many tables). Returns Entity -> [(type, name)]."""
    import glob
    import re
    out: dict[str, list[tuple[str, str]]] = {}
    prop_re = re.compile(r"public\s+([\w<>?\[\]]+)\s+(\w+)\s*\{\s*get;\s*set;")
    class_re = re.compile(r"public\s+(?:sealed\s+|abstract\s+)?class\s+(\w+)")
    paths = glob.glob(os.path.join(ROOT, "Energy.Domain", "*", "*Entities.cs"))
    # Standalone legacy entity files (tables whose columns live in their own file,
    # not a *Entities.cs aggregate). Without these, the Excel-thin tables
    # (LocalizationResources, UserSettings, AuditLogs, ChatGroups) stay column-poor.
    standalone = [
        os.path.join(ROOT, "Energy.Domain", "Localization", "Resource.cs"),
        os.path.join(ROOT, "Energy.Domain", "Identity", "UserSetting.cs"),
        os.path.join(ROOT, "Energy.Domain", "Logger", "AuditLog.cs"),
        os.path.join(ROOT, "Energy.Domain", "Chat", "ChatGroup.cs"),
    ]
    paths.extend(p for p in standalone if os.path.exists(p))
    for path in paths:
        with open(path, "r", encoding="utf-8") as f:
            text = f.read()
        current = None
        for line in text.splitlines():
            cm = class_re.search(line)
            if cm:
                current = cm.group(1)
                out.setdefault(current, [])
                continue
            pm = prop_re.search(line)
            if pm and current:
                out[current].append((pm.group(1), pm.group(2)))
    # Alias: the new entity name differs from the legacy class name for some tables.
    # Map the new singular entity name to the legacy class' property set.
    aliases = {"LocalizationResource": "Resource"}
    for new_name, legacy_name in aliases.items():
        if legacy_name in out and new_name not in out:
            out[new_name] = out[legacy_name]
    return out


def build_model():
    tables = load_rows("Table Catalogue")[2:]  # skip title + header
    table_module = {}
    table_purpose = {}
    table_entity = {}
    order = []
    for row in tables:
        module, table, purpose = row[0], row[1], (row[2] if len(row) > 2 else "")
        if not module or not table:
            continue
        if module == "Module" and table == "Table":
            continue  # skip the column-header row
        table_module[table] = module
        table_purpose[table] = purpose
        table_entity[table] = singularize(table)
        order.append(table)

    columns = load_rows("Column Catalogue")[2:]
    table_columns: dict[str, list] = {}
    for row in columns:
        table, col, dtype = row[0], row[1], row[2]
        nullable = (row[3].lower() == "yes") if len(row) > 3 else True
        key = row[4] if len(row) > 4 else ""
        lookup = row[6] if len(row) > 6 else ""
        desc = row[7] if len(row) > 7 else ""
        if not table or not col:
            continue
        table_columns.setdefault(table, []).append(
            {"col": col, "dtype": dtype, "nullable": nullable,
             "key": key, "lookup": lookup, "desc": desc})

    # Inject FK columns that are declared ONLY in the Relationship Catalogue
    # (common for junction tables whose FK columns are absent from Column Catalogue).
    # Ensure every table from the Table Catalogue has an entry (some have no rows in
    # the Column Catalogue at all, e.g. reference tables keyed only by alternate keys).
    for t in order:
        table_columns.setdefault(t, [])
    coltype = {(t, c["col"]): c["dtype"]
               for t, cs in table_columns.items() for c in cs}
    existing = {t: {c["col"] for c in cs} for t, cs in table_columns.items()}
    for r in load_rows("Relationship Catalogue")[2:]:
        if len(r) < 5:
            continue
        src, scol, tgt = r[0], r[1], r[2]
        tcol = (r[3] or "Id") if len(r) > 3 else "Id"
        required = r[4] if len(r) > 4 else "No"
        if src == "SourceTable" or not src or not scol:
            continue
        if src not in table_columns:
            continue

        # (a) Ensure the TARGET principal-key column exists on the target entity when
        # the relationship references a non-Id alternate key (e.g. Permissions.Code,
        # LocalizationResources.Key). Done independently of the source column, since the
        # source FK may already be a real Column Catalogue column.
        if tcol != "Id" and tgt in table_columns and tcol not in existing.get(tgt, set()):
            tdt = coltype.get((tgt, tcol), "nvarchar")
            table_columns[tgt].append({
                "col": tcol, "dtype": tdt, "nullable": False,
                "key": "AK", "lookup": "", "desc": "Alternatif anahtar",
            })
            existing.setdefault(tgt, set()).add(tcol)

        # (b) Inject the SOURCE FK column only when absent from the Column Catalogue.
        if scol in AUDIT_COLUMNS or scol in existing.get(src, set()):
            continue
        dt = "uniqueidentifier" if tcol == "Id" else coltype.get((tgt, tcol), "nvarchar")
        table_columns[src].append({
            "col": scol, "dtype": dt, "nullable": (required != "Yes"),
            "key": "FK", "lookup": f"{tgt}.{tcol}", "desc": f"{tgt} referansı",
        })
        existing[src].add(scol)


    # Enrich thin entities with scalar properties present in the authoritative legacy
    # *Entities.cs but missing from the Excel Column Catalogue. Nothing is invented:
    # columns come from the existing domain. Enum/value-object types fall back to string
    # (consistent with the lookup-as-string approach); navigation collections are skipped.
    legacy = parse_legacy_entities()
    for t in order:
        ent = table_entity[t]
        for ptype, pname in legacy.get(ent, []):
            if pname in AUDIT_COLUMNS or pname == "Id":
                continue
            if pname in existing.get(t, set()):
                continue
            if "<" in ptype or "[" in ptype:
                continue  # navigation / collection
            base = ptype.rstrip("?")
            nullable = ptype.endswith("?")
            if base in PRIMITIVE_CS:
                cstype = ptype
            else:
                cstype, nullable = "string", False  # enum / value object -> string
            table_columns[t].append({
                "col": pname, "cstype": cstype, "dtype": None,
                "nullable": nullable, "key": "", "lookup": "", "desc": pname,
            })
            existing.setdefault(t, set()).add(pname)

    return order, table_module, table_purpose, table_columns, table_entity


def gen_entity(entity, module, purpose, cols):
    has_audit = AUDIT_COLUMNS.issubset({c["col"] for c in cols})
    lines = []
    lines.append("using Energy.Domain.Common;")
    lines.append("")
    lines.append(f"namespace Energy.Domain.Modules.{module};")
    lines.append("")
    summary = purpose or entity
    lines.append("/// <summary>")
    lines.append(f"/// {summary}")
    lines.append("/// </summary>")
    base = " : AuditableEntity" if has_audit else ""
    lines.append(f"public class {entity}{base}")
    lines.append("{")
    body = []
    if not has_audit:
        # explicit Id for non-auditable tables (junction/log/reference)
        body.append("    /// <summary>Birincil anahtar.</summary>")
        body.append("    public Guid Id { get; set; }")
        body.append("")
    for c in cols:
        col = c["col"]
        if has_audit and col in AUDIT_COLUMNS:
            continue
        if not has_audit and col == "Id":
            continue
        cstype, is_str = csharp_type(c)
        doc = c["desc"] or col
        body.append(f"    /// <summary>{doc}</summary>")
        if is_str:
            if c["nullable"]:
                body.append(f"    public string? {col} {{ get; set; }}")
            else:
                body.append(f"    public string {col} {{ get; set; }} = string.Empty;")
        else:
            body.append(f"    public {cstype} {col} {{ get; set; }}")
        body.append("")
    if body and body[-1] == "":
        body.pop()
    lines.extend(body)
    lines.append("}")
    lines.append("")
    return "\n".join(lines)


def main():
    shutil.rmtree(DOMAIN_ROOT, ignore_errors=True)
    order, table_module, table_purpose, table_columns, table_entity = build_model()
    count = 0
    for table in order:
        module = table_module[table]
        entity = table_entity[table]
        cols = table_columns.get(table, [])
        out_dir = os.path.join(DOMAIN_ROOT, module, "Entities")
        os.makedirs(out_dir, exist_ok=True)
        path = os.path.join(out_dir, f"{entity}.cs")
        with open(path, "w", encoding="utf-8") as f:
            f.write(gen_entity(entity, module, table_purpose[table], cols))
        count += 1
    print(f"Generated {count} entity files under {DOMAIN_ROOT}")


if __name__ == "__main__":
    main()


