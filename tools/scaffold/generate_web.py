#!/usr/bin/env python3
"""
Energy — Web layer generator (Phase 7): per-entity API clients + MVC controllers.

Excludes IAM and Chat modules (curated hand-written controllers already exist),
avoiding controller-name collisions. Generates:
  * I{Entity}ApiClient / {Entity}ApiClient (HttpClientFactory via ApiClientBase)
  * {Table}Controller (MVC, talks ONLY to the API client; grid JSON endpoints)
  * ModulesApiClientRegistration (typed HttpClient registration)
"""
from __future__ import annotations

import os
import re
import shutil

from generate_domain import ROOT, build_model

CLIENTS_ROOT = os.path.join(ROOT, "Energy.Web", "Clients", "Modules")
WEBCTRL_ROOT = os.path.join(ROOT, "Energy.Web", "Controllers", "Modules")
EXCLUDE_MODULES = {"IAM", "Chat"}


def kebab(name: str) -> str:
    return re.sub(r"(?<!^)(?=[A-Z])", "-", name).lower()


def write(path, content):
    os.makedirs(os.path.dirname(path), exist_ok=True)
    with open(path, "w", encoding="utf-8") as f:
        f.write(content)


def gen_client(module, entity, table):
    base = f"api/v1/{kebab(module)}/{kebab(table)}"
    L = [
        "using Energy.Shared.Models.V1.Common.Responses;",
        f"using Energy.Shared.Models.V1.{module}.{entity}.Requests;",
        f"using Energy.Shared.Models.V1.{module}.{entity}.Responses;",
        "using Energy.Web.Clients.Infrastructure;", "",
        f"namespace Energy.Web.Clients.Modules.{module}.{entity};", "",
        f"/// <summary>{entity} API istemci sözleşmesi.</summary>",
        f"public interface I{entity}ApiClient",
        "{",
        f"    Task<BaseResponse<PaginatedResponse<{entity}ListResponse>>> GetListAsync(int pageNumber, int pageSize, string? search, CancellationToken ct = default);",
        f"    Task<BaseResponse<{entity}DetailResponse>> GetByIdAsync(Guid id, CancellationToken ct = default);",
        f"    Task<BaseResponse<IReadOnlyList<{entity}LookupResponse>>> GetLookupAsync(string? search, bool activeOnly, CancellationToken ct = default);",
        f"    Task<BaseResponse<Guid>> CreateAsync(Create{entity}Request request, CancellationToken ct = default);",
        f"    Task<BaseResponse<bool>> UpdateAsync(Guid id, Update{entity}Request request, CancellationToken ct = default);",
        "    Task<BaseResponse<bool>> DeleteAsync(Guid id, CancellationToken ct = default);",
        "}", "",
        f"/// <summary>{entity} API istemcisi (HttpClientFactory + BaseResponse).</summary>",
        f"public sealed class {entity}ApiClient : ApiClientBase, I{entity}ApiClient",
        "{",
        f'    private const string Base = "{base}";', "",
        f"    public {entity}ApiClient(HttpClient httpClient) : base(httpClient) {{ }}", "",
        f"    public Task<BaseResponse<PaginatedResponse<{entity}ListResponse>>> GetListAsync(int pageNumber, int pageSize, string? search, CancellationToken ct = default)",
        f'        => GetAsync<BaseResponse<PaginatedResponse<{entity}ListResponse>>>($"{{Base}}?pageNumber={{pageNumber}}&pageSize={{pageSize}}&search={{Uri.EscapeDataString(search ?? string.Empty)}}", ct);', "",
        f"    public Task<BaseResponse<{entity}DetailResponse>> GetByIdAsync(Guid id, CancellationToken ct = default)",
        f'        => GetAsync<BaseResponse<{entity}DetailResponse>>($"{{Base}}/{{id}}", ct);', "",
        f"    public Task<BaseResponse<IReadOnlyList<{entity}LookupResponse>>> GetLookupAsync(string? search, bool activeOnly, CancellationToken ct = default)",
        f'        => GetAsync<BaseResponse<IReadOnlyList<{entity}LookupResponse>>>($"{{Base}}/lookup?search={{Uri.EscapeDataString(search ?? string.Empty)}}&activeOnly={{activeOnly}}", ct);', "",
        f"    public Task<BaseResponse<Guid>> CreateAsync(Create{entity}Request request, CancellationToken ct = default)",
        f"        => PostAsync<Create{entity}Request, BaseResponse<Guid>>(Base, request, ct);", "",
        f"    public Task<BaseResponse<bool>> UpdateAsync(Guid id, Update{entity}Request request, CancellationToken ct = default)",
        f'        => PutAsync<Update{entity}Request, BaseResponse<bool>>($"{{Base}}/{{id}}", request, ct);', "",
        "    public Task<BaseResponse<bool>> DeleteAsync(Guid id, CancellationToken ct = default)",
        f'        => DeleteAsync<BaseResponse<bool>>($"{{Base}}/{{id}}", ct);',
        "}", "",
    ]
    return "\n".join(L)


def gen_controller(module, entity, table):
    route = f"{kebab(module)}/{kebab(table)}"
    view = f"~/Views/Modules/{module}/{entity}/Index.cshtml"
    L = [
        "using Microsoft.AspNetCore.Authorization;",
        "using Microsoft.AspNetCore.Mvc;",
        f"using Energy.Shared.Models.V1.{module}.{entity}.Requests;",
        f"using Energy.Web.Clients.Modules.{module}.{entity};", "",
        f"namespace Energy.Web.Controllers.Modules.{module};", "",
        f"/// <summary>{entity} ekran denetleyicisi (yalnızca API istemcisiyle konuşur).</summary>",
        "[Authorize]",
        f'[Route("{route}")]',
        f"public sealed class {table}Controller : Controller",
        "{",
        f"    private readonly I{entity}ApiClient _api;", "",
        f"    public {table}Controller(I{entity}ApiClient api) => _api = api;", "",
        '    [HttpGet("")]',
        f'    public IActionResult Index() => View("{view}");', "",
        '    [HttpGet("list")]',
        "    public async Task<IActionResult> List(int skip = 0, int take = 20, string? searchValue = null, CancellationToken ct = default)",
        "    {",
        "        var pageNumber = (take <= 0 ? 1 : skip / take) + 1;",
        "        var envelope = await _api.GetListAsync(pageNumber, take <= 0 ? 20 : take, searchValue, ct);",
        "        var page = envelope.Data;",
        f"        return Json(new {{ data = page?.Items ?? Array.Empty<Energy.Shared.Models.V1.{module}.{entity}.Responses.{entity}ListResponse>(), totalCount = page?.TotalCount ?? 0 }});",
        "    }", "",
        '    [HttpGet("lookup")]',
        "    public async Task<IActionResult> Lookup(string? search = null, bool activeOnly = true, CancellationToken ct = default)",
        "        => Json((await _api.GetLookupAsync(search, activeOnly, ct)).Data ?? []);", "",
        '    [HttpGet("{id:guid}")]',
        "    public async Task<IActionResult> Detail(Guid id, CancellationToken ct)",
        "        => Json(await _api.GetByIdAsync(id, ct));", "",
        '    [HttpPost("")]',
        "    [IgnoreAntiforgeryToken]",
        f"    public async Task<IActionResult> Create([FromBody] Create{entity}Request request, CancellationToken ct)",
        "        => Json(await _api.CreateAsync(request, ct));", "",
        '    [HttpPut("{id:guid}")]',
        "    [IgnoreAntiforgeryToken]",
        f"    public async Task<IActionResult> Update(Guid id, [FromBody] Update{entity}Request request, CancellationToken ct)",
        "        => Json(await _api.UpdateAsync(id, request, ct));", "",
        '    [HttpDelete("{id:guid}")]',
        "    [IgnoreAntiforgeryToken]",
        "    public async Task<IActionResult> Delete(Guid id, CancellationToken ct)",
        "        => Json(await _api.DeleteAsync(id, ct));",
        "}", "",
    ]
    return "\n".join(L)


def gen_registration(items):
    L = [
        "using Energy.Web.Clients.Infrastructure.Authentication;",
        "using Energy.Web.Clients.Infrastructure.ClientIdentity;",
        "using Energy.Web.Configuration;",
        "using Microsoft.Extensions.Options;", "",
        "namespace Energy.Web.Clients.Modules;", "",
        "/// <summary>Tüm per-entity Modules API istemcilerinin (typed HttpClient) kaydı.</summary>",
        "public static class ModulesApiClientRegistration",
        "{",
        "    public static IServiceCollection AddModulesApiClients(this IServiceCollection services)",
        "    {",
    ]
    for (m, e) in items:
        itype = f"global::Energy.Web.Clients.Modules.{m}.{e}.I{e}ApiClient"
        impl = f"global::Energy.Web.Clients.Modules.{m}.{e}.{e}ApiClient"
        L += [
            f"        services.AddHttpClient<{itype}, {impl}>(Configure)",
            "            .AddHttpMessageHandler<ClientIdentityHeaderHandler>()",
            "            .AddHttpMessageHandler<AuthHeaderHandler>();",
        ]
    L += [
        "        return services;",
        "    }", "",
        "    private static void Configure(IServiceProvider sp, HttpClient http)",
        "    {",
        "        var settings = sp.GetRequiredService<IOptions<ApiSettings>>().Value;",
        '        if (string.IsNullOrWhiteSpace(settings.BaseUrl))',
        '            throw new InvalidOperationException("Api:BaseUrl is not configured.");',
        "        http.BaseAddress = new Uri(settings.BaseUrl);",
        "    }",
        "}", "",
    ]
    return "\n".join(L)


def main():
    shutil.rmtree(CLIENTS_ROOT, ignore_errors=True)
    shutil.rmtree(WEBCTRL_ROOT, ignore_errors=True)
    order, table_module, table_purpose, table_columns, table_entity = build_model()
    items = []
    for t in order:
        m, e = table_module[t], table_entity[t]
        if m in EXCLUDE_MODULES:
            continue
        write(os.path.join(CLIENTS_ROOT, m, e, f"I{e}ApiClient.cs"), gen_client(m, e, t))
        # client interface + impl are in same file; controller separate
        write(os.path.join(WEBCTRL_ROOT, m, f"{t}Controller.cs"), gen_controller(m, e, t))
        items.append((m, e))
    write(os.path.join(CLIENTS_ROOT, "ModulesApiClientRegistration.cs"), gen_registration(items))
    print(f"Web clients+controllers generated for {len(items)} entities (IAM/Chat excluded)")


if __name__ == "__main__":
    main()

