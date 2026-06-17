#!/usr/bin/env python3
"""
Energy — lookup enum generator.

Emits one C# enum per Lookup Catalogue group (15) under
Energy.Domain/Modules/Common/Enums/{Group}.cs, namespace
Energy.Domain.Modules.Common.Enums. Additive (does not touch legacy enums).
"""
from __future__ import annotations

import json
import os
import collections

from generate_domain import ROOT, load_rows

OUT = os.path.join(ROOT, "Energy.Domain", "Modules", "Common", "Enums")


def main():
    rows = load_rows("Lookup Catalogue")[2:]
    groups = collections.OrderedDict()
    for r in rows:
        if len(r) < 2 or not r[0] or r[0] == "LookupName":
            continue
        groups.setdefault(r[0], [])
        if r[1] and r[1] not in groups[r[0]]:
            groups[r[0]].append(r[1])

    os.makedirs(OUT, exist_ok=True)
    for name, values in groups.items():
        lines = [
            "namespace Energy.Domain.Modules.Common.Enums;", "",
            f"/// <summary>{name} lookup değerleri (Lookup Catalogue).</summary>",
            f"public enum {name}",
            "{",
        ]
        for i, v in enumerate(values, start=1):
            lines.append(f"    /// <summary>{v}</summary>")
            lines.append(f"    {v} = {i},")
        lines += ["}", ""]
        with open(os.path.join(OUT, f"{name}.cs"), "w", encoding="utf-8") as f:
            f.write("\n".join(lines))
    print(f"Generated {len(groups)} lookup enums under {OUT}")


if __name__ == "__main__":
    main()

