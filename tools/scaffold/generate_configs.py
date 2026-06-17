#!/usr/bin/env python3
"""
Energy — per-entity EF Core configuration generator (Phase 4a).

One IEntityTypeConfiguration file per table under
Energy.Infrastructure/Persistence/Configurations/Modules/{Module}/{Entity}Configuration.cs

Relationships come from the Relationship Catalogue (539 rows). Audit FK columns
(CreatedBy/UpdatedBy/DeletedBy) are intentionally skipped here; they are wired by
the shared audit-user convention in the DbContext. These configs are NOT applied
by the legacy AppDbContext (it filters them out by namespace) and become active in
the canonical context at cutover.
"""
from __future__ import annotations

import os
import shutil

from generate_domain import ROOT, build_model

CFG_ROOT = os.path.join(ROOT, "Energy.Infrastructure", "Persistence",
                        "Configurations", "Modules")

AUDIT_FK_COLUMNS = {"CreatedBy", "UpdatedBy", "DeletedBy"}
ONDELETE = {
    "Restrict": "DeleteBehavior.Restrict",
    "Cascade": "DeleteBehavior.Cascade",
    "SetNull": "DeleteBehavior.SetNull",
    "NoAction": "DeleteBehavior.NoAction",
}


def fq(module, entity):
    return f"Energy.Domain.Modules.{module}.{entity}"


def load_relationships():
    from generate_domain import load_rows
    rows = load_rows("Relationship Catalogue")[2:]
    rels = {}
    for r in rows:
        if len(r) < 8:
            continue
        src, scol, tgt, tcol, required, card, ondelete = r[0], r[1], r[2], r[3], r[4], r[5], r[6]
        if src == "SourceTable" or not src or not scol:
            continue
        rels.setdefault(src, []).append({
            "scol": scol, "tgt": tgt, "tcol": tcol or "Id",
            "ondelete": ondelete or "Restrict",
        })
    return rels


def gen_config(module, entity, table, rels, table_module, table_entity):
    etype = f"global::Energy.Domain.Modules.{module}.{entity}"
    lines = [
        "using Microsoft.EntityFrameworkCore;",
        "using Microsoft.EntityFrameworkCore.Metadata.Builders;", "",
        f"namespace Energy.Infrastructure.Persistence.Configurations.Modules.{module};", "",
        f"/// <summary>{entity} EF Core eşleştirmesi (tablo, anahtar ve ilişkiler).</summary>",
        f"public class {entity}Configuration : IEntityTypeConfiguration<{etype}>",
        "{",
        f"    public void Configure(EntityTypeBuilder<{etype}> builder)",
        "    {",
        f'        builder.ToTable("{table}");',
        "        builder.HasKey(e => e.Id);",
    ]
    for rel in rels:
        scol = rel["scol"]
        if scol in AUDIT_FK_COLUMNS:
            continue
        tgt = rel["tgt"]
        if tgt not in table_module:
            continue  # target outside generated set
        tmod, tent = table_module[tgt], table_entity[tgt]
        target_type = "global::" + fq(tmod, tent)
        behavior = ONDELETE.get(rel["ondelete"], "DeleteBehavior.Restrict")
        chain = f"        builder.HasOne<{target_type}>().WithMany().HasForeignKey(e => e.{scol})"
        if rel["tcol"] and rel["tcol"] != "Id":
            chain += f'.HasPrincipalKey("{rel["tcol"]}")'
        chain += f".OnDelete({behavior});"
        lines.append(chain)
    lines += ["    }", "}", ""]
    return "\n".join(lines)


def main():
    shutil.rmtree(CFG_ROOT, ignore_errors=True)
    order, table_module, table_purpose, table_columns, table_entity = build_model()
    rels_by_table = load_relationships()
    # build a column-name set per table to guard against FK columns not present
    colset = {t: {c["col"] for c in cs} for t, cs in table_columns.items()}
    count = 0
    for table in order:
        module = table_module[table]
        entity = table_entity[table]
        rels = [r for r in rels_by_table.get(table, []) if r["scol"] in colset.get(table, set())]
        out_dir = os.path.join(CFG_ROOT, module)
        os.makedirs(out_dir, exist_ok=True)
        with open(os.path.join(out_dir, f"{entity}Configuration.cs"), "w", encoding="utf-8") as f:
            f.write(gen_config(module, entity, table, rels, table_module, table_entity))
        count += 1
    print(f"Generated {count} EF configuration files under {CFG_ROOT}")


if __name__ == "__main__":
    main()

