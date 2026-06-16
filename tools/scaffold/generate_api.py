#!/usr/bin/env python3
"""
Energy — API controller generator (Phase 6).

One {Entity}Controller per table under Energy.Api/Controllers/Modules/{Module}/,
attribute-routed as api/v1/{module-kebab}/{entity-plural-kebab}. Controllers are
thin: they only depend on Application interfaces (I{Entity}Service / lookup) and
return the BaseResponse envelope. Permissions are enforced centrally by the
endpoint-resolving authorization middleware (seeded ApiEndpoints catalogue).
"""
from __future__ import annotations

import os
import re
import shutil

from generate_domain import ROOT, build_model

API_ROOT = os.path.join(ROOT, "Energy.Api", "Controllers")
# IAM/Chat have curated hand controllers and no CRUD permission set in PermissionCatalog.
EXCLUDE_MODULES = {"IAM", "Chat"}
MAP_FILE = os.path.join(ROOT, "Energy.Infrastructure", "System", "Services",
                        "ModulesEndpointPermissionMap.cs")


def kebab(name: str) -> str:
    s = re.sub(r"(?<!^)(?=[A-Z])", "-", name)
    return s.lower()


def write(path, content):
    os.makedirs(os.path.dirname(path), exist_ok=True)
    with open(path, "w", encoding="utf-8") as f:
        f.write(content)


def gen_controller(module, entity, table):
    """Thin, MediatR-tabanlı entity controller (Controller -> IMediator -> Command/Query
    -> Handler -> Application Service akışı). CQRS Command/Query/Handler dosyaları
    generate_cqrs.py tarafından üretilir; bu şablon onunla birebir aynı olmalıdır."""
    route = f"api/v{{version:apiVersion}}/{kebab(module)}/{kebab(table)}"
    cmd = f"Energy.Application.Modules.{module}.{entity}.Commands"
    qry = f"Energy.Application.Modules.{module}.{entity}.Queries"
    L = [
        "using Asp.Versioning;",
        "using MediatR;",
        "using Microsoft.AspNetCore.Mvc;",
        f"using {cmd}.Create{entity};",
        f"using {cmd}.Delete{entity};",
        f"using {cmd}.Update{entity};",
        f"using {qry}.Get{entity}ById;",
        f"using {qry}.Get{entity}List;",
        f"using {qry}.Get{entity}Lookup;",
        "using Energy.Shared.Models.V1.Common.Responses;",
        f"using Energy.Shared.Models.V1.{module}.{entity}.Requests;",
        f"using Energy.Shared.Models.V1.{module}.{entity}.Responses;", "",
        f"namespace Energy.Api.Controllers.{module};", "",
        "/// <summary>",
        f"/// {entity} uç noktaları (liste, detay, lookup, create, update, delete).",
        "/// Controller iş mantığı içermez; her istek ilgili Command/Query'ye map edilip",
        "/// <see cref=\"IMediator\"/> üzerinden Application use-case'ine yönlendirilir.",
        "/// </summary>",
        "[ApiController]",
        '[ApiVersion("1.0")]',
        f'[Route("{route}")]',
        f"public sealed class {entity}Controller : ControllerBase",
        "{",
        "    private readonly IMediator _mediator;", "",
        f"    public {entity}Controller(IMediator mediator)",
        "        => _mediator = mediator;", "",
        "    /// <summary>Sayfalanmış liste.</summary>",
        "    [HttpGet]",
        f"    public async Task<ActionResult<BaseResponse<PaginatedResponse<{entity}ListResponse>>>> GetList([FromQuery] Get{entity}ListRequest request, CancellationToken ct)",
        f"        => Ok(await _mediator.Send(new Get{entity}ListQuery(request), ct));", "",
        "    /// <summary>Kimliğe göre detay.</summary>",
        '    [HttpGet("{id:guid}")]',
        f"    public async Task<ActionResult<BaseResponse<{entity}DetailResponse>>> GetById(Guid id, CancellationToken ct)",
        f"        => Ok(await _mediator.Send(new Get{entity}ByIdQuery(id), ct));", "",
        "    /// <summary>Lookup listesi.</summary>",
        '    [HttpGet("lookup")]',
        f"    public async Task<ActionResult<BaseResponse<IReadOnlyList<{entity}LookupResponse>>>> Lookup([FromQuery] string? search, [FromQuery] bool activeOnly, CancellationToken ct)",
        f"        => Ok(await _mediator.Send(new Get{entity}LookupQuery(search, activeOnly), ct));", "",
        "    /// <summary>Yeni kayıt oluşturur.</summary>",
        "    [HttpPost]",
        f"    public async Task<ActionResult<BaseResponse<Guid>>> Create(Create{entity}Request request, CancellationToken ct)",
        f"        => Ok(await _mediator.Send(new Create{entity}Command(request), ct));", "",
        "    /// <summary>Var olan kaydı günceller.</summary>",
        '    [HttpPut("{id:guid}")]',
        f"    public async Task<ActionResult<BaseResponse<bool>>> Update(Guid id, Update{entity}Request request, CancellationToken ct)",
        f"        => Ok(await _mediator.Send(new Update{entity}Command(id, request), ct));", "",
        "    /// <summary>Kaydı (soft-delete) siler.</summary>",
        '    [HttpDelete("{id:guid}")]',
        "    public async Task<ActionResult<BaseResponse<bool>>> Delete(Guid id, CancellationToken ct)",
        f"        => Ok(await _mediator.Send(new Delete{entity}Command(id), ct));",
        "}", "",
    ]
    return "\n".join(L)


def gen_endpoint_permission_map(items):
    """Map each generated entity controller's actions to the module CRUD permission,
    so ApiEndpointSyncService auto-activates them on startup."""
    L = [
        "namespace Energy.Infrastructure.System.Services;", "",
        "/// <summary>",
        "/// Üretilen per-entity API controller uç noktalarının (Controller.Action) modül",
        "/// CRUD yetkilerine eşlemesi. ApiEndpointSyncService başlangıçta bunları etkinleştirir.",
        "/// </summary>",
        "public static class ModulesEndpointPermissionMap",
        "{",
        "    public static void Apply(IDictionary<string, string?> map)",
        "    {",
    ]
    for (m, e) in items:
        L.append(f'        map["{e}.GetList"] = "{m}.ReadAll";')
        L.append(f'        map["{e}.GetById"] = "{m}.Read";')
        L.append(f'        map["{e}.Lookup"] = "{m}.Read";')
        L.append(f'        map["{e}.Create"] = "{m}.Create";')
        L.append(f'        map["{e}.Update"] = "{m}.Update";')
        L.append(f'        map["{e}.Delete"] = "{m}.Delete";')
    L += ["    }", "}", ""]
    return "\n".join(L)


def main():
    # NOT: Process/Report controller'ları korunmalı; bu nedenle ağaç silinmez,
    # entity controller dosyaları yerinde yeniden yazılır. CQRS Command/Query/Handler
    # dosyaları için generate_cqrs.py çalıştırılmalıdır (controller şablonu ortaktır).
    order, table_module, table_purpose, table_columns, table_entity = build_model()
    items = []
    for t in order:
        m, e = table_module[t], table_entity[t]
        if m in EXCLUDE_MODULES:
            continue
        write(os.path.join(API_ROOT, m, f"{e}Controller.cs"), gen_controller(m, e, t))
        items.append((m, e))
    write(MAP_FILE, gen_endpoint_permission_map(items))
    print(f"Generated {len(items)} API controllers (+ endpoint permission map; IAM/Chat excluded)")


if __name__ == "__main__":
    main()

