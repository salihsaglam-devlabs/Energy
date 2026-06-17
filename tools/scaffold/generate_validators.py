#!/usr/bin/env python3
"""
Energy — per-entity FluentValidation validator generator (Phase: Validation).

Emits Create{Entity}RequestValidator and Update{Entity}RequestValidator under
Energy.Application/Modules/{Module}/{Entity}/Validators/, with conservative rules
derived from the Column Catalogue:
  * non-nullable string columns  -> NotEmpty
  * non-nullable Guid FK columns -> NotEmpty (rejects Guid.Empty)
  * Update requests              -> Id NotEmpty
Also emits a registration marker class so AddValidatorsFromAssembly finds them.
"""
from __future__ import annotations

import os
import shutil

from generate_domain import ROOT, AUDIT_COLUMNS, build_model, csharp_type

APP = os.path.join(ROOT, "Energy.Application", "Modules")


def write(path, content):
    os.makedirs(os.path.dirname(path), exist_ok=True)
    with open(path, "w", encoding="utf-8") as f:
        f.write(content)


def input_columns(cols):
    return [c for c in cols if c["col"] not in AUDIT_COLUMNS and c["col"] != "Id"]


def rules_for(cols):
    """Return list of (col, ruleexpr) for required string/Guid columns."""
    rules = []
    for c in input_columns(cols):
        if c["nullable"]:
            continue
        cstype, is_str = csharp_type(c)
        base = cstype.rstrip("?")
        if is_str:
            rules.append(c["col"])
        elif base == "Guid":
            rules.append(c["col"])
    return rules


def gen_validator(module, entity, kind, cols):
    """kind = 'Create' or 'Update'."""
    req = f"{kind}{entity}Request"
    ns = f"Energy.Application.Modules.{module}.{entity}.Validators"
    rule_cols = rules_for(cols)
    L = [
        "using FluentValidation;",
        f"using Energy.Shared.Models.V1.{module}.{entity}.Requests;", "",
        f"namespace {ns};", "",
        f"/// <summary>{req} için doğrulama kuralları (zorunlu alanlar).</summary>",
        f"public sealed class {req}Validator : AbstractValidator<{req}>",
        "{",
        f"    public {req}Validator()",
        "    {",
    ]
    if kind == "Update":
        L.append("        RuleFor(x => x.Id).NotEmpty();")
    for col in rule_cols:
        L.append(f"        RuleFor(x => x.{col}).NotEmpty();")
    if not rule_cols and kind == "Create":
        L.append("        // Zorunlu iş alanı yok; yapısal doğrulama için yer tutucu.")
    L += ["    }", "}", ""]
    return "\n".join(L)


def gen_marker():
    return "\n".join([
        "namespace Energy.Application.Modules;", "",
        "/// <summary>",
        "/// FluentValidation kayıt çıpası. AddValidatorsFromAssemblyContaining bu tipi",
        "/// kullanarak tüm per-entity validator'ları (Create/Update) tarar ve kaydeder.",
        "/// </summary>",
        "public sealed class ModulesValidatorMarker",
        "{",
        "}", "",
    ])


def main():
    order, table_module, table_purpose, table_columns, table_entity = build_model()
    count = 0
    for t in order:
        m, e = table_module[t], table_entity[t]
        cols = table_columns.get(t, [])
        vdir = os.path.join(APP, m, e, "Validators")
        shutil.rmtree(vdir, ignore_errors=True)
        write(os.path.join(vdir, f"Create{e}RequestValidator.cs"),
              gen_validator(m, e, "Create", cols))
        write(os.path.join(vdir, f"Update{e}RequestValidator.cs"),
              gen_validator(m, e, "Update", cols))
        count += 2
    write(os.path.join(APP, "ModulesValidatorMarker.cs"), gen_marker())
    print(f"Generated {count} per-entity validators (+ registration marker)")


if __name__ == "__main__":
    main()

