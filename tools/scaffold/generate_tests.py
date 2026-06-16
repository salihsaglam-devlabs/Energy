#!/usr/bin/env python3
"""
Energy — module-based test generator.

One {Entity}ServiceTests per table under Energy.Tests/Modules/{Module}/, verifying
the generated CRUD service round-trips (Create -> GetById -> Update -> Delete)
against EnergyDbContext on the EF InMemory provider (FK-agnostic, fast, isolated).
"""
from __future__ import annotations

import os
import shutil

from generate_domain import ROOT, build_model

TESTS_ROOT = os.path.join(ROOT, "Energy.Tests", "Modules")


def write(path, content):
    os.makedirs(os.path.dirname(path), exist_ok=True)
    with open(path, "w", encoding="utf-8") as f:
        f.write(content)


def gen_test(module, entity):
    return "\n".join([
        "using Energy.Infrastructure.Persistence;",
        f"using Energy.Infrastructure.Modules.{module}.{entity}.Services;",
        f"using Energy.Shared.Models.V1.{module}.{entity}.Requests;",
        "using Microsoft.EntityFrameworkCore;",
        "using Xunit;", "",
        f"namespace Energy.Tests.Modules.{module};", "",
        f"/// <summary>{entity} CRUD servisi round-trip testi (EF InMemory).</summary>",
        f"public sealed class {entity}ServiceTests",
        "{",
        "    private static EnergyDbContext NewContext()",
        "    {",
        "        var options = new DbContextOptionsBuilder<EnergyDbContext>()",
        "            .UseInMemoryDatabase(Guid.NewGuid().ToString())",
        "            .Options;",
        "        return new EnergyDbContext(options);",
        "    }", "",
        "    [Fact]",
        "    public async Task Create_Get_Update_Delete_RoundTrips()",
        "    {",
        "        await using var db = NewContext();",
        f"        var service = new {entity}Service(db);", "",
        f"        var created = await service.CreateAsync(new Create{entity}Request());",
        "        Assert.True(created.IsSuccess);",
        "        var id = created.Data;", "",
        "        var detail = await service.GetByIdAsync(id);",
        "        Assert.True(detail.IsSuccess);", "",
        f"        var updated = await service.UpdateAsync(id, new Update{entity}Request {{ Id = id }});",
        "        Assert.True(updated.IsSuccess);", "",
        "        var deleted = await service.DeleteAsync(id);",
        "        Assert.True(deleted.IsSuccess);",
        "    }", "",
        "    [Fact]",
        "    public async Task GetById_Unknown_ReturnsFailure()",
        "    {",
        "        await using var db = NewContext();",
        f"        var service = new {entity}Service(db);",
        "        var result = await service.GetByIdAsync(Guid.NewGuid());",
        "        Assert.False(result.IsSuccess);",
        "    }",
        "}", "",
    ])


def main():
    # Hand-written tests living under the generated tree are preserved across regens.
    HAND_WRITTEN = ["Documents/DocumentFileServiceTests.cs"]
    preserved = {}
    for rel in HAND_WRITTEN:
        p = os.path.join(TESTS_ROOT, rel)
        if os.path.exists(p):
            with open(p, "r", encoding="utf-8") as f:
                preserved[rel] = f.read()

    shutil.rmtree(TESTS_ROOT, ignore_errors=True)

    for rel, content in preserved.items():
        write(os.path.join(TESTS_ROOT, rel), content)
    order, table_module, _, _, table_entity = build_model()
    count = 0
    for t in order:
        m, e = table_module[t], table_entity[t]
        write(os.path.join(TESTS_ROOT, m, f"{e}ServiceTests.cs"), gen_test(m, e))
        count += 1
    print(f"Generated {count} module-based service tests under {TESTS_ROOT}")

    # ER Overview report read-only service tests.
    try:
        from generate_reports import REPORTS
    except Exception:
        REPORTS = []
    rcount = 0
    for rep in REPORTS:
        m, n = rep["module"], rep["name"]
        write(os.path.join(TESTS_ROOT, m, "Reports", f"{n}Tests.cs"),
              gen_report_test(m, n, bool(rep.get("status_field"))))
        rcount += 1
    print(f"Generated {rcount} report service tests")


def gen_report_test(module, report, has_status):
    L = [
        "using Energy.Infrastructure.Persistence;",
        f"using Energy.Infrastructure.Modules.{module}.Reports.{report};",
        f"using Energy.Shared.Models.V1.{module}.Reports.{report}.Requests;",
        "using Microsoft.EntityFrameworkCore;",
        "using Xunit;", "",
        f"namespace Energy.Tests.Modules.{module}.Reports;", "",
        f"/// <summary>{report} raporu (salt-okunur) servis testi (EF InMemory).</summary>",
        f"public sealed class {report}Tests",
        "{",
        "    private static EnergyDbContext NewContext()",
        "    {",
        "        var options = new DbContextOptionsBuilder<EnergyDbContext>()",
        "            .UseInMemoryDatabase(Guid.NewGuid().ToString())",
        "            .Options;",
        "        return new EnergyDbContext(options);",
        "    }", "",
        "    [Fact]",
        "    public async Task GetData_Empty_ReturnsSuccessWithNoRows()",
        "    {",
        "        await using var db = NewContext();",
        f"        var service = new {report}Service(db);",
        f"        var result = await service.GetDataAsync(new {report}Request());",
        "        Assert.True(result.IsSuccess);",
        "        Assert.NotNull(result.Data);",
        "        Assert.Empty(result.Data!.Items);",
        "    }", "",
        "    [Fact]",
        "    public async Task GetData_WithDateFilter_ReturnsSuccess()",
        "    {",
        "        await using var db = NewContext();",
        f"        var service = new {report}Service(db);",
        f"        var request = new {report}Request",
        "        {",
        "            StartDate = DateTime.UtcNow.AddYears(-1),",
        "            EndDate = DateTime.UtcNow,",
    ]
    if has_status:
        L.append('            Status = "Test",')
    L += [
        "        };",
        "        var result = await service.GetDataAsync(request);",
        "        Assert.True(result.IsSuccess);",
        "    }",
        "}", "",
    ]
    return "\n".join(L)


if __name__ == "__main__":
    main()

