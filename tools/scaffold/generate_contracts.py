#!/usr/bin/env python3
"""
Energy — Shared contracts + Application interfaces scaffolding generator.

Phase 2: Energy.Shared per-entity Request/Response contracts.
Phase 3: Energy.Application per-entity Service + Lookup interfaces.

Additive (new namespaces), one file per type. Entity type names are SINGULAR
PascalCase (Company, Material), while DbSet/route names remain plural elsewhere.
"""
from __future__ import annotations

import os
import shutil

from generate_domain import ROOT, AUDIT_COLUMNS, build_model, map_type, csharp_type

SHARED_ROOT = os.path.join(ROOT, "Energy.Shared", "Models", "V1")
APP_ROOT = os.path.join(ROOT, "Energy.Application", "Modules")


def write(path: str, content: str):
    os.makedirs(os.path.dirname(path), exist_ok=True)
    with open(path, "w", encoding="utf-8") as f:
        f.write(content)


def prop(c):
    col, doc = c["col"], (c["desc"] or c["col"])
    cstype, is_str = csharp_type(c)
    nullable = c["nullable"]
    out = [f"    /// <summary>{doc}</summary>"]
    if is_str:
        out.append(f"    public string? {col} {{ get; set; }}" if nullable
                   else f"    public string {col} {{ get; set; }} = string.Empty;")
    else:
        out.append(f"    public {cstype} {col} {{ get; set; }}")
    return out


def input_columns(cols):
    return [c for c in cols if c["col"] not in AUDIT_COLUMNS]


def gen_create(module, entity, cols):
    body = [f"namespace Energy.Shared.Models.V1.{module}.{entity}.Requests;", "",
            f"/// <summary>{entity} oluşturma isteği.</summary>",
            f"public class Create{entity}Request", "{"]
    for c in input_columns(cols):
        body += prop(c); body.append("")
    if body[-1] == "": body.pop()
    return "\n".join(body + ["}", ""])


def gen_update(module, entity, cols):
    body = [f"namespace Energy.Shared.Models.V1.{module}.{entity}.Requests;", "",
            f"/// <summary>{entity} güncelleme isteği.</summary>",
            f"public class Update{entity}Request", "{",
            "    /// <summary>Güncellenecek kaydın kimliği.</summary>",
            "    public Guid Id { get; set; }", ""]
    for c in input_columns(cols):
        body += prop(c); body.append("")
    if body[-1] == "": body.pop()
    return "\n".join(body + ["}", ""])


def gen_list_request(module, entity):
    return "\n".join([
        "using Energy.Shared.Models.V1.Common.Requests;", "",
        f"namespace Energy.Shared.Models.V1.{module}.{entity}.Requests;", "",
        f"/// <summary>{entity} listeleme isteği (sayfalama, arama, sıralama, filtre).</summary>",
        f"public class Get{entity}ListRequest : PaginatedRequest", "{", "}", ""])


def gen_list_response(module, entity, cols):
    body = [f"namespace Energy.Shared.Models.V1.{module}.{entity}.Responses;", "",
            f"/// <summary>{entity} liste satırı.</summary>",
            f"public class {entity}ListResponse", "{",
            "    /// <summary>Kimlik.</summary>",
            "    public Guid Id { get; set; }", ""]
    for c in input_columns(cols):
        body += prop(c); body.append("")
    body += ["    /// <summary>Oluşturma zamanı.</summary>",
             "    public DateTime CreatedAt { get; set; }", "}", ""]
    return "\n".join(body)


def gen_detail_response(module, entity, cols):
    body = [f"namespace Energy.Shared.Models.V1.{module}.{entity}.Responses;", "",
            f"/// <summary>{entity} detay görünümü.</summary>",
            f"public class {entity}DetailResponse", "{",
            "    /// <summary>Kimlik.</summary>",
            "    public Guid Id { get; set; }", ""]
    for c in cols:
        if c["col"] == "Id":
            continue
        body += prop(c); body.append("")
    if body[-1] == "": body.pop()
    return "\n".join(body + ["}", ""])


def gen_lookup_response(module, entity):
    return "\n".join([
        f"namespace Energy.Shared.Models.V1.{module}.{entity}.Responses;", "",
        f"/// <summary>{entity} lookup öğesi (Id, Code, Name, DisplayName, IsActive standardı).</summary>",
        f"public class {entity}LookupResponse", "{",
        "    /// <summary>Kimlik.</summary>", "    public Guid Id { get; set; }", "",
        "    /// <summary>Kod.</summary>", "    public string? Code { get; set; }", "",
        "    /// <summary>Ad.</summary>", "    public string? Name { get; set; }", "",
        "    /// <summary>Görünen ad.</summary>", "    public string DisplayName { get; set; } = string.Empty;", "",
        "    /// <summary>Aktif mi.</summary>", "    public bool IsActive { get; set; }", "}", ""])


def gen_service_interface(module, entity):
    return "\n".join([
        "using Energy.Shared.Models.V1.Common.Responses;",
        f"using Energy.Shared.Models.V1.{module}.{entity}.Requests;",
        f"using Energy.Shared.Models.V1.{module}.{entity}.Responses;", "",
        f"namespace Energy.Application.Modules.{module}.{entity}.Services;", "",
        f"/// <summary>{entity} CRUD use-case sözleşmesi.</summary>",
        f"public interface I{entity}Service", "{",
        f"    /// <summary>Sayfalanmış {entity} listesi.</summary>",
        f"    Task<BaseResponse<PaginatedResponse<{entity}ListResponse>>> GetListAsync(Get{entity}ListRequest request, CancellationToken ct = default);", "",
        "    /// <summary>Kimliğe göre detay.</summary>",
        f"    Task<BaseResponse<{entity}DetailResponse>> GetByIdAsync(Guid id, CancellationToken ct = default);", "",
        "    /// <summary>Yeni kayıt oluşturur; yeni kimliği döndürür.</summary>",
        f"    Task<BaseResponse<Guid>> CreateAsync(Create{entity}Request request, CancellationToken ct = default);", "",
        "    /// <summary>Var olan kaydı günceller.</summary>",
        f"    Task<BaseResponse<bool>> UpdateAsync(Guid id, Update{entity}Request request, CancellationToken ct = default);", "",
        "    /// <summary>Kaydı (gerekiyorsa soft-delete) siler.</summary>",
        "    Task<BaseResponse<bool>> DeleteAsync(Guid id, CancellationToken ct = default);", "}", ""])


def gen_lookup_interface(module, entity):
    return "\n".join([
        "using Energy.Shared.Models.V1.Common.Responses;",
        f"using Energy.Shared.Models.V1.{module}.{entity}.Responses;", "",
        f"namespace Energy.Application.Modules.{module}.{entity}.Lookups;", "",
        f"/// <summary>{entity} lookup sözleşmesi (aktif kayıt + arama filtreli).</summary>",
        f"public interface I{entity}LookupService", "{",
        f"    /// <summary>{entity} lookup listesi döndürür.</summary>",
        f"    Task<BaseResponse<IReadOnlyList<{entity}LookupResponse>>> GetLookupAsync(string? search = null, bool activeOnly = true, CancellationToken ct = default);",
        "}", ""])


# Module top-level folders that are exclusively generated (no legacy Shared content).
GENERATED_SHARED_MODULES = {
    "Assets", "Budget", "BusinessPartners", "Catalog", "Contracts", "Core",
    "Documents", "FieldOperations", "Finance", "HR", "IAM", "Inventory",
    "Notifications", "Operations", "Organization", "Procurement",
    "ProgressPayments", "Projects", "Reporting", "Requests", "Workflow",
}


def main():
    order, table_module, table_purpose, table_columns, table_entity = build_model()

    # Non-destructive cleanup: remove ONLY the per-entity generated folders, so
    # hand-written / separately-generated subfolders (Reports, Processes, Files,
    # Common) survive a regen.
    for table in order:
        module = table_module[table]
        entity = table_entity[table]
        shutil.rmtree(os.path.join(SHARED_ROOT, module, entity), ignore_errors=True)
        shutil.rmtree(os.path.join(SHARED_ROOT, module, table), ignore_errors=True)
        shutil.rmtree(os.path.join(APP_ROOT, module, entity), ignore_errors=True)

    shared = app = 0
    for table in order:
        module = table_module[table]
        entity = table_entity[table]
        cols = table_columns.get(table, [])
        sreq = os.path.join(SHARED_ROOT, module, entity, "Requests")
        sres = os.path.join(SHARED_ROOT, module, entity, "Responses")
        write(os.path.join(sreq, f"Create{entity}Request.cs"), gen_create(module, entity, cols))
        write(os.path.join(sreq, f"Update{entity}Request.cs"), gen_update(module, entity, cols))
        write(os.path.join(sreq, f"Get{entity}ListRequest.cs"), gen_list_request(module, entity))
        write(os.path.join(sres, f"{entity}ListResponse.cs"), gen_list_response(module, entity, cols))
        write(os.path.join(sres, f"{entity}DetailResponse.cs"), gen_detail_response(module, entity, cols))
        write(os.path.join(sres, f"{entity}LookupResponse.cs"), gen_lookup_response(module, entity))
        shared += 6
        write(os.path.join(APP_ROOT, module, entity, "Services", f"I{entity}Service.cs"),
              gen_service_interface(module, entity))
        write(os.path.join(APP_ROOT, module, entity, "Lookups", f"I{entity}LookupService.cs"),
              gen_lookup_interface(module, entity))
        app += 2
    print(f"Shared contract files: {shared}")
    print(f"Application interface files: {app}")


if __name__ == "__main__":
    main()

