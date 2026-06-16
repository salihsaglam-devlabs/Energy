#!/usr/bin/env python3
"""
'Modules' sarmalından türetilmiş yardımcı sınıf/dosya/metot adlarını sadeleştirir
ve localization anahtarlarındaki 'Modules.' önekini kaldırır. Domain kavramı olan
'module' (CrudModules, PermissionModules, BusinessRoles.Modules) korunur.
"""
import os, re

ROOT = os.path.abspath(os.path.join(os.path.dirname(__file__), "..", ".."))
SKIP = (os.sep + "bin" + os.sep, os.sep + "obj" + os.sep, os.sep + "lib" + os.sep)

# (eski, yeni) — uzun adlar önce (substring çakışmasını önlemek için)
IDENT = [
    ("ModulesReportEndpointPermissionMap", "ReportEndpointPermissionMap"),
    ("ModulesEndpointPermissionMap", "EntityEndpointPermissionMap"),
    ("ModulesReportApiClientRegistration", "ReportApiClientRegistration"),
    ("ModulesApiClientRegistration", "EntityApiClientRegistration"),
    ("ModulesReportRegistration", "ReportRegistration"),
    ("ModulesServiceRegistration", "EntityServiceRegistration"),
    ("ModulesValidatorMarker", "ApplicationAssemblyMarker"),
    ("AddModulesReportApiClients", "AddReportApiClients"),
    ("AddModulesApiClients", "AddEntityApiClients"),
    ("AddModulesReportServices", "AddReportServices"),
    ("AddModulesEntityServices", "AddEntityServices"),
    ("EnsureModulesReportMenusAsync", "EnsureReportMenusAsync"),
    ("EnsureModulesProcessMenusAsync", "EnsureProcessMenusAsync"),
    ("EnsureModulesEntityMenusAsync", "EnsureEntityMenusAsync"),
]

FILE_RENAMES = [
    ("Energy.Application/ModulesValidatorMarker.cs", "Energy.Application/ApplicationAssemblyMarker.cs"),
    ("Energy.Infrastructure/System/Services/ModulesEndpointPermissionMap.cs",
     "Energy.Infrastructure/System/Services/EntityEndpointPermissionMap.cs"),
    ("Energy.Infrastructure/System/Services/ModulesReportEndpointPermissionMap.cs",
     "Energy.Infrastructure/System/Services/ReportEndpointPermissionMap.cs"),
    ("Energy.Infrastructure/ModulesServiceRegistration.cs",
     "Energy.Infrastructure/EntityServiceRegistration.cs"),
    ("Energy.Infrastructure/ModulesReportRegistration.cs",
     "Energy.Infrastructure/ReportRegistration.cs"),
    ("Energy.Web/Clients/ModulesApiClientRegistration.cs",
     "Energy.Web/Clients/EntityApiClientRegistration.cs"),
    ("Energy.Web/Clients/ModulesReportApiClientRegistration.cs",
     "Energy.Web/Clients/ReportApiClientRegistration.cs"),
    ("Energy.Infrastructure/Seeding/SystemSeeder.ModulesMenus.cs",
     "Energy.Infrastructure/Seeding/SystemSeeder.EntityMenus.cs"),
    ("Energy.Infrastructure/Seeding/SystemSeeder.ModulesProcessMenus.cs",
     "Energy.Infrastructure/Seeding/SystemSeeder.ProcessMenus.cs"),
    ("Energy.Infrastructure/Seeding/SystemSeeder.ModulesReportMenus.cs",
     "Energy.Infrastructure/Seeding/SystemSeeder.ReportMenus.cs"),
]


def walk_files(exts):
    for dp, _, fs in os.walk(ROOT):
        if any(s in dp + os.sep for s in SKIP) or "tools/scaffold" in dp:
            continue
        for fn in fs:
            if fn.endswith(exts):
                yield os.path.join(dp, fn)


def main():
    # 1) Identifier rename (.cs)
    cs_changed = 0
    for p in walk_files((".cs",)):
        t = open(p, encoding="utf-8").read()
        o = t
        for a, b in IDENT:
            t = t.replace(a, b)
        if t != o:
            open(p, "w", encoding="utf-8").write(t)
            cs_changed += 1

    # 2) Localization anahtar öneki: "Modules.  -> "
    loc_changed = 0
    for p in walk_files((".cs", ".cshtml", ".js", ".resx")):
        t = open(p, encoding="utf-8").read()
        if '"Modules.' not in t:
            continue
        t2 = t.replace('"Modules.', '"')
        if t2 != t:
            open(p, "w", encoding="utf-8").write(t2)
            loc_changed += 1

    # 3) Dosya adlarını değiştir
    renamed = 0
    for src, dst in FILE_RENAMES:
        s = os.path.join(ROOT, src)
        d = os.path.join(ROOT, dst)
        if os.path.exists(s):
            os.rename(s, d)
            renamed += 1

    print(f"identifier-changed files: {cs_changed}")
    print(f"localization-key files: {loc_changed}")
    print(f"renamed files: {renamed}")


if __name__ == "__main__":
    main()

