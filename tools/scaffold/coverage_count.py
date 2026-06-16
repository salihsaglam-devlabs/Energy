#!/usr/bin/env python3
"""Coverage counter — prints exact file counts for the final coverage report."""
import os, glob

ROOT = os.path.abspath(os.path.join(os.path.dirname(__file__), "..", ".."))

def count(pattern):
    return len(glob.glob(os.path.join(ROOT, pattern), recursive=True))

rows = [
    ("Domain entities (Modules)", "Energy.Domain/Modules/**/Entities/*.cs"),
    ("EF Configurations (Modules)", "Energy.Infrastructure/Persistence/Configurations/Modules/**/*Configuration.cs"),
    ("Application service interfaces", "Energy.Application/Modules/**/Services/I*Service.cs"),
    ("Application lookup interfaces", "Energy.Application/Modules/**/Lookups/I*LookupService.cs"),
    ("Infrastructure services", "Energy.Infrastructure/Modules/**/Services/*Service.cs"),
    ("Infrastructure lookups", "Energy.Infrastructure/Modules/**/Lookups/*LookupService.cs"),
    ("API controllers (Modules)", "Energy.Api/Controllers/Modules/**/*Controller.cs"),
    ("Web API clients", "Energy.Web/Clients/Modules/**/I*ApiClient.cs"),
    ("Web controllers (Modules)", "Energy.Web/Controllers/Modules/**/*Controller.cs"),
    ("Views Index.cshtml", "Energy.Web/Views/Modules/**/Index.cshtml"),
    ("JS *.index.js", "Energy.Web/wwwroot/js/modules/**/*.index.js"),
    ("Shared V1 contracts", "Energy.Shared/Models/V1/**/*.cs"),
    ("Report API controllers", "Energy.Api/Controllers/Modules/**/Reports/*Controller.cs"),
    ("Process API controllers", "Energy.Api/Controllers/Modules/**/Processes/*Controller.cs"),
    ("Test files (Modules)", "Energy.Tests/Modules/**/*Tests.cs"),
]
for label, pat in rows:
    print(f"{label:34s}: {count(pat)}")

