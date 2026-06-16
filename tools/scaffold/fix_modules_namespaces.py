#!/usr/bin/env python3
"""Modules düzleştirmesi sonrası namespace ve asset yollarını günceller.
.cs/.cshtml/.js dosyalarında:
  '.Modules.'  -> '.'     (Energy.* namespace'leri + Configurations.Modules)
  '.Modules;'  -> ';'     (using/namespace satır sonları)
  'js/modules/'  -> 'js/'  ve  'css/modules/' -> 'css/'  (wwwroot asset yolları)
Localization anahtarları ("Modules.X") '.Modules.' desenine uymadığından etkilenmez.
"""
import os

ROOT = os.path.abspath(os.path.join(os.path.dirname(__file__), "..", ".."))
EXTS = (".cs", ".cshtml", ".js")
SKIP = (os.sep + "bin" + os.sep, os.sep + "obj" + os.sep,
        os.sep + "lib" + os.sep, os.sep + "node_modules" + os.sep)

repls = [
    (".Modules.", "."),
    (".Modules;", ";"),
    ("js/modules/", "js/"),
    ("css/modules/", "css/"),
]

changed = 0
for dp, _, fs in os.walk(ROOT):
    if any(s in dp + os.sep for s in SKIP):
        continue
    for fn in fs:
        if not fn.endswith(EXTS):
            continue
        p = os.path.join(dp, fn)
        try:
            t = open(p, encoding="utf-8").read()
        except (UnicodeDecodeError, OSError):
            continue
        orig = t
        for a, b in repls:
            t = t.replace(a, b)
        if t != orig:
            open(p, "w", encoding="utf-8").write(t)
            changed += 1

print(f"updated {changed} files")

