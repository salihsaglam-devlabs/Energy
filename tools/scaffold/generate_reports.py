#!/usr/bin/env python3
"""
Energy — Reports generator (Phase: Report standard).

Emits a per-report vertical (Shared contract -> Application interface ->
Infrastructure read-only service -> API controller -> Web API client -> Web
controller -> View + JS + CSS) for the reports JUSTIFIED by the ER Overview
flows. Nothing is invented: every report maps onto a real ER Overview business
flow and projects only columns that exist on the base entity.

ER Overview flows -> reports:
  Procurement to Inventory  -> Procurement.PurchaseOrderSummary
  Inventory Costing         -> Inventory.StockBalanceReport
  Project Execution         -> Projects.ProjectStatusReport
  HR Cost                   -> HR.TimesheetSummary
  Finance                   -> Finance.PayableAging, Finance.ReceivableAging
  Contracts                 -> ProgressPayments.ProgressPaymentSummary

Each report screen is READ-ONLY (grid + filters + export), permission-controlled
(`{Module}.{Report}.Read` / `.Export`) and localization-driven.
"""
from __future__ import annotations

import os
import re
import shutil

from generate_domain import ROOT, build_model, load_rows

# --- output roots -----------------------------------------------------------
SHARED = os.path.join(ROOT, "Energy.Shared", "Models", "V1")
APP = os.path.join(ROOT, "Energy.Application", "Modules")
INFRA = os.path.join(ROOT, "Energy.Infrastructure", "Modules")
API = os.path.join(ROOT, "Energy.Api", "Controllers")
WEB_CLIENTS = os.path.join(ROOT, "Energy.Web", "Clients")
WEB_CTRL = os.path.join(ROOT, "Energy.Web", "Controllers")
VIEWS = os.path.join(ROOT, "Energy.Web", "Views")
JS = os.path.join(ROOT, "Energy.Web", "wwwroot", "js", "modules")
CSS = os.path.join(ROOT, "Energy.Web", "wwwroot", "css", "modules")

INFRA_REG = os.path.join(INFRA, "ModulesReportRegistration.cs")
WEB_REG = os.path.join(WEB_CLIENTS, "ModulesReportApiClientRegistration.cs")
PERM_MAP = os.path.join(ROOT, "Energy.Infrastructure", "System", "Services",
                        "ModulesReportEndpointPermissionMap.cs")
MENU_OUT = os.path.join(ROOT, "Energy.Infrastructure", "Seeding",
                        "SystemSeeder.ModulesReportMenus.cs")

# --- report catalogue (ER-Overview justified) -------------------------------
# Each report: module, name, dbset (plural table), base_table (for FK lookup
# resolution), date_field, status_field (or None), columns [(prop, cstype)].
REPORTS = [
    dict(module="Procurement", name="PurchaseOrderSummary",
         dbset="PurchaseOrders", base_table="PurchaseOrders",
         date_field="OrderDate", status_field="Status",
         columns=[("OrderNo", "string"), ("OrderDate", "DateTime"),
                  ("SupplierId", "Guid"), ("ProjectId", "Guid?"),
                  ("CurrencyId", "Guid"), ("Status", "string")]),
    dict(module="Inventory", name="StockBalanceReport",
         dbset="StockBalances", base_table="StockBalances",
         date_field="LastRecalculatedAt", status_field=None,
         columns=[("WarehouseId", "Guid"), ("MaterialId", "Guid"),
                  ("Quantity", "decimal"), ("ReservedQuantity", "decimal"),
                  ("TotalCost", "decimal"), ("LastRecalculatedAt", "DateTime")]),
    dict(module="Projects", name="ProjectStatusReport",
         dbset="Projects", base_table="Projects",
         date_field="StartDate", status_field=None,
         columns=[("Code", "string"), ("Name", "string"),
                  ("ProjectTypeId", "Guid"), ("StatusId", "Guid"),
                  ("StartDate", "DateTime?"), ("EndDate", "DateTime?")]),
    dict(module="HR", name="TimesheetSummary",
         dbset="Timesheets", base_table="Timesheets",
         date_field="PeriodStart", status_field="Status",
         columns=[("TimesheetNo", "string"), ("PeriodStart", "DateTime"),
                  ("PeriodEnd", "DateTime"), ("Status", "string")]),
    dict(module="Finance", name="PayableAging",
         dbset="Payables", base_table="Payables",
         date_field="DueDate", status_field=None,
         columns=[("PartnerId", "Guid"), ("CurrencyId", "Guid"),
                  ("Amount", "decimal"), ("RemainingAmount", "decimal"),
                  ("DueDate", "DateTime"), ("IsClosed", "bool")]),
    dict(module="Finance", name="ReceivableAging",
         dbset="Receivables", base_table="Receivables",
         date_field="DueDate", status_field=None,
         columns=[("PartnerId", "Guid"), ("CurrencyId", "Guid"),
                  ("Amount", "decimal"), ("RemainingAmount", "decimal"),
                  ("DueDate", "DateTime"), ("IsClosed", "bool")]),
    dict(module="ProgressPayments", name="ProgressPaymentSummary",
         dbset="ProgressPayments", base_table="ProgressPayments",
         date_field="PaymentPeriodStart", status_field="Status",
         columns=[("ProgressPaymentNo", "string"), ("ContractId", "Guid"),
                  ("GrossAmount", "decimal"), ("NetAmount", "decimal"),
                  ("PaymentPeriodStart", "DateTime"), ("Status", "string")]),
]


def kebab(name: str) -> str:
    return re.sub(r"(?<!^)(?=[A-Z])", "-", name).lower()


def write(path, content):
    os.makedirs(os.path.dirname(path), exist_ok=True)
    with open(path, "w", encoding="utf-8") as f:
        f.write(content)


def build_fk_lookup_map(table_module):
    """(table, column) -> /module-kebab/target-table-kebab/lookup from Relationship Catalogue."""
    fk = {}
    for r in load_rows("Relationship Catalogue")[2:]:
        if len(r) < 3:
            continue
        src, scol, tgt = r[0], r[1], r[2]
        if src == "SourceTable" or not src or not scol or not tgt:
            continue
        mod = table_module.get(tgt)
        if not mod:
            continue
        fk[(src, scol)] = f"/{kebab(mod)}/{kebab(tgt)}/lookup"
    return fk


# --- emitters ---------------------------------------------------------------
def gen_request(rep):
    m, n = rep["module"], rep["name"]
    L = [
        f"namespace Energy.Shared.Models.V1.{m}.Reports.{n}.Requests;", "",
        f"/// <summary>{n} raporu filtre/sayfalama isteği (salt-okunur).</summary>",
        f"public sealed class {n}Request",
        "{",
        "    /// <summary>Sayfa numarası (1 tabanlı).</summary>",
        "    public int PageNumber { get; set; } = 1;", "",
        "    /// <summary>Sayfa boyutu.</summary>",
        "    public int PageSize { get; set; } = 50;", "",
        "    /// <summary>Başlangıç tarihi filtresi (dahil).</summary>",
        "    public DateTime? StartDate { get; set; }", "",
        "    /// <summary>Bitiş tarihi filtresi (dahil).</summary>",
        "    public DateTime? EndDate { get; set; }",
    ]
    if rep["status_field"]:
        L += ["", "    /// <summary>Durum filtresi.</summary>",
              "    public string? Status { get; set; }"]
    L += ["}", ""]
    return "\n".join(L)


def gen_row(rep):
    m, n = rep["module"], rep["name"]
    L = [
        f"namespace Energy.Shared.Models.V1.{m}.Reports.{n}.Responses;", "",
        f"/// <summary>{n} raporu satırı (salt-okunur projeksiyon).</summary>",
        f"public sealed class {n}RowResponse",
        "{",
        "    /// <summary>Kaynak kayıt kimliği.</summary>",
        "    public Guid Id { get; set; }",
    ]
    for (prop, cs) in rep["columns"]:
        L.append("")
        L.append(f"    /// <summary>{prop}</summary>")
        if cs == "string":
            L.append(f"    public string? {prop} {{ get; set; }}")
        else:
            L.append(f"    public {cs} {prop} {{ get; set; }}")
    L += ["}", ""]
    return "\n".join(L)


def gen_app_interface(rep):
    m, n = rep["module"], rep["name"]
    return "\n".join([
        "using Energy.Shared.Models.V1.Common.Responses;",
        f"using Energy.Shared.Models.V1.{m}.Reports.{n}.Requests;",
        f"using Energy.Shared.Models.V1.{m}.Reports.{n}.Responses;", "",
        f"namespace Energy.Application.Modules.{m}.Reports.{n}.Services;", "",
        f"/// <summary>{n} raporu servis sözleşmesi (salt-okunur).</summary>",
        f"public interface I{n}Service",
        "{",
        "    /// <summary>Filtrelenmiş, sayfalanmış rapor verisini döndürür.</summary>",
        f"    Task<BaseResponse<PaginatedResponse<{n}RowResponse>>> GetDataAsync({n}Request request, CancellationToken ct = default);",
        "}", "",
    ])


def gen_infra_service(rep):
    m, n = rep["module"], rep["name"]
    dbset = rep["dbset"]
    date_field = rep["date_field"]
    status_field = rep["status_field"]
    proj = ["                Id = e.Id"]
    for (prop, _cs) in rep["columns"]:
        proj.append(f"                {prop} = e.{prop}")
    L = [
        "using Microsoft.EntityFrameworkCore;",
        "using Energy.Infrastructure.Persistence;",
        "using Energy.Shared.Models.V1.Common.Responses;",
        f"using Energy.Application.Modules.{m}.Reports.{n}.Services;",
        f"using Energy.Shared.Models.V1.{m}.Reports.{n}.Requests;",
        f"using Energy.Shared.Models.V1.{m}.Reports.{n}.Responses;", "",
        f"namespace Energy.Infrastructure.Modules.{m}.Reports.{n};", "",
        f"/// <summary>{n} raporu servisi (AsNoTracking, projection, filtre, sayfalama).</summary>",
        f"public sealed class {n}Service : I{n}Service",
        "{",
        "    private readonly EnergyDbContext _db;", "",
        f"    public {n}Service(EnergyDbContext db) => _db = db;", "",
        f"    public async Task<BaseResponse<PaginatedResponse<{n}RowResponse>>> GetDataAsync({n}Request request, CancellationToken ct = default)",
        "    {",
        f"        var query = _db.{dbset}.AsNoTracking();",
        f"        if (request.StartDate.HasValue) query = query.Where(e => e.{date_field} >= request.StartDate.Value);",
        f"        if (request.EndDate.HasValue) query = query.Where(e => e.{date_field} <= request.EndDate.Value);",
    ]
    if status_field:
        L.append(f"        if (!string.IsNullOrWhiteSpace(request.Status)) query = query.Where(e => e.{status_field} == request.Status);")
    L += [
        "        var total = await query.CountAsync(ct);",
        "        var pageSize = request.PageSize <= 0 ? 50 : request.PageSize;",
        "        var pageNumber = request.PageNumber <= 0 ? 1 : request.PageNumber;",
        "        var items = await query",
        f"            .OrderByDescending(e => e.{date_field})",
        "            .Skip((pageNumber - 1) * pageSize)",
        "            .Take(pageSize)",
        f"            .Select(e => new {n}RowResponse",
        "            {",
        ",\n".join(proj),
        "            })",
        "            .ToListAsync(ct);",
        f"        var page = PaginatedResponse<{n}RowResponse>.Create(items, pageNumber, pageSize, total);",
        f"        return BaseResponse<PaginatedResponse<{n}RowResponse>>.Success(page);",
        "    }",
        "}", "",
    ]
    return "\n".join(L)


def gen_api_controller(rep):
    m, n = rep["module"], rep["name"]
    route = f"api/v{{version:apiVersion}}/{kebab(m)}/reports/{kebab(n)}"
    headers = ",".join([f'"{p}"' for (p, _c) in rep["columns"]])
    props = [p for (p, _c) in rep["columns"]]
    cell_exprs = []
    for (p, cs) in rep["columns"]:
        if cs == "string":
            cell_exprs.append(f'(r.{p} ?? string.Empty)')
        else:
            cell_exprs.append(f'r.{p}.ToString()')
    csv_line = ' + "," + '.join(cell_exprs) if cell_exprs else '""'
    L = [
        "using System.Text;",
        "using Asp.Versioning;",
        "using Microsoft.AspNetCore.Mvc;",
        f"using Energy.Application.Modules.{m}.Reports.{n}.Services;",
        "using Energy.Shared.Models.V1.Common.Responses;",
        f"using Energy.Shared.Models.V1.{m}.Reports.{n}.Requests;",
        f"using Energy.Shared.Models.V1.{m}.Reports.{n}.Responses;", "",
        f"namespace Energy.Api.Controllers.{m}.Reports;", "",
        f"/// <summary>{n} raporu uç noktaları (veri + export). Salt-okunur.</summary>",
        "[ApiController]",
        '[ApiVersion("1.0")]',
        f'[Route("{route}")]',
        f"public sealed class {n}Controller : ControllerBase",
        "{",
        f"    private readonly I{n}Service _service;", "",
        f"    public {n}Controller(I{n}Service service) => _service = service;", "",
        "    /// <summary>Filtrelenmiş, sayfalanmış rapor verisi.</summary>",
        "    [HttpGet]",
        f"    public async Task<ActionResult<BaseResponse<PaginatedResponse<{n}RowResponse>>>> GetData([FromQuery] {n}Request request, CancellationToken ct)",
        "        => Ok(await _service.GetDataAsync(request, ct));", "",
        "    /// <summary>Raporu CSV olarak dışa aktarır (ayrı yetki).</summary>",
        '    [HttpGet("export")]',
        f"    public async Task<IActionResult> Export([FromQuery] {n}Request request, CancellationToken ct)",
        "    {",
        "        request.PageNumber = 1;",
        "        request.PageSize = 100000;",
        "        var result = await _service.GetDataAsync(request, ct);",
        "        var rows = result.Data?.Items ?? [];",
        "        var sb = new StringBuilder();",
        f'        sb.AppendLine(string.Join(",", new[] {{ {headers} }}));',
        "        foreach (var r in rows)",
        "        {",
        f"            sb.AppendLine({csv_line});",
        "        }",
        '        var bytes = Encoding.UTF8.GetBytes(sb.ToString());',
        f'        return File(bytes, "text/csv", "{kebab(n)}.csv");',
        "    }",
        "}", "",
    ]
    return "\n".join(L)


def gen_web_client(rep):
    m, n = rep["module"], rep["name"]
    base = f"api/v1/{kebab(m)}/reports/{kebab(n)}"
    return "\n".join([
        "using Energy.Shared.Models.V1.Common.Responses;",
        f"using Energy.Shared.Models.V1.{m}.Reports.{n}.Responses;",
        "using Energy.Web.Clients.Infrastructure;", "",
        f"namespace Energy.Web.Clients.{m}.Reports.{n};", "",
        f"/// <summary>{n} raporu API istemci sözleşmesi.</summary>",
        f"public interface I{n}ApiClient",
        "{",
        f"    Task<BaseResponse<PaginatedResponse<{n}RowResponse>>> GetDataAsync(string query, CancellationToken ct = default);",
        "}", "",
        f"/// <summary>{n} raporu API istemcisi (HttpClientFactory + BaseResponse).</summary>",
        f"public sealed class {n}ApiClient : ApiClientBase, I{n}ApiClient",
        "{",
        f'    private const string Base = "{base}";', "",
        f"    public {n}ApiClient(HttpClient httpClient) : base(httpClient) {{ }}", "",
        f"    public Task<BaseResponse<PaginatedResponse<{n}RowResponse>>> GetDataAsync(string query, CancellationToken ct = default)",
        f'        => GetAsync<BaseResponse<PaginatedResponse<{n}RowResponse>>>(string.IsNullOrEmpty(query) ? Base : $"{{Base}}?{{query}}", ct);',
        "}", "",
    ])


def gen_web_controller(rep):
    m, n = rep["module"], rep["name"]
    route = f"{kebab(m)}/reports/{kebab(n)}"
    view = f"~/Views/{m}/Reports/{n}/Index.cshtml"
    status_q = ""
    if rep["status_field"]:
        status_q = '        if (!string.IsNullOrWhiteSpace(status)) parts.Add($"Status={Uri.EscapeDataString(status)}");\n'
    sig = "int skip = 0, int take = 50, DateTime? startDate = null, DateTime? endDate = null"
    if rep["status_field"]:
        sig += ", string? status = null"
    L = [
        "using Microsoft.AspNetCore.Authorization;",
        "using Microsoft.AspNetCore.Mvc;",
        f"using Energy.Web.Clients.{m}.Reports.{n};", "",
        f"namespace Energy.Web.Controllers.{m}.Reports;", "",
        f"/// <summary>{n} rapor ekran denetleyicisi (yalnızca API istemcisiyle konuşur, salt-okunur).</summary>",
        "[Authorize]",
        f'[Route("{route}")]',
        f"public sealed class {n}Controller : Controller",
        "{",
        f"    private readonly I{n}ApiClient _api;", "",
        f"    public {n}Controller(I{n}ApiClient api) => _api = api;", "",
        '    [HttpGet("")]',
        f'    public IActionResult Index() => View("{view}");', "",
        '    [HttpGet("data")]',
        f"    public async Task<IActionResult> Data({sig}, CancellationToken ct = default)",
        "    {",
        "        var pageNumber = (take <= 0 ? 1 : skip / take) + 1;",
        "        var pageSize = take <= 0 ? 50 : take;",
        "        var parts = new List<string> { $\"PageNumber={pageNumber}\", $\"PageSize={pageSize}\" };",
        '        if (startDate.HasValue) parts.Add($"StartDate={startDate.Value:O}");',
        '        if (endDate.HasValue) parts.Add($"EndDate={endDate.Value:O}");',
        status_q.rstrip("\n") if status_q else "",
        '        var envelope = await _api.GetDataAsync(string.Join("&", parts), ct);',
        "        var page = envelope.Data;",
        f"        return Json(new {{ data = page?.Items ?? Array.Empty<Energy.Shared.Models.V1.{m}.Reports.{n}.Responses.{n}RowResponse>(), totalCount = page?.TotalCount ?? 0 }});",
        "    }",
        "}", "",
    ]
    return "\n".join([x for x in L if x != ""] if False else L)


def gen_view(rep):
    m, n = rep["module"], rep["name"]
    grid_id = f"{kebab(n)}-grid"
    title_key = f"Modules.{m}.Reports.{n}.Title"
    js_path = f"~/js/modules/{kebab(m)}/reports/{kebab(n)}/{kebab(n)}.index.js"
    css_path = f"~/css/modules/{kebab(m)}/reports/{kebab(n)}/{kebab(n)}.css"
    route = f"/{kebab(m)}/reports/{kebab(n)}"
    has_status = "true" if rep["status_field"] else "false"
    fn = f"{m}{n}"
    L = [
        "@using Energy.Localization",
        "@inject Microsoft.AspNetCore.Mvc.Localization.IHtmlLocalizer<SharedResource> T",
        "@{",
        f'    ViewData["Title"] = T["{title_key}"].Value;',
        "}", "",
        '<link rel="stylesheet" href="' + css_path + '" asp-append-version="true" />', "",
        '<section class="energy-screen energy-report">',
        '    <header class="energy-screen__header">',
        f'        <div><h2>@T["{title_key}"]</h2></div>',
        "    </header>",
        f'    <div id="{kebab(n)}-filters" class="energy-report__filters"></div>',
        f'    <div id="{grid_id}"></div>',
        "</section>", "",
        "@section Scripts {",
        f'    <script src="{js_path}" asp-append-version="true"></script>',
        "    <script>",
        '        document.addEventListener("DOMContentLoaded", function () {',
        f'            window.AppReports.{fn}.init("{route}", "{grid_id}", "{kebab(n)}-filters", {{ hasStatus: {has_status} }});',
        "        });",
        "    </script>",
        "}", "",
    ]
    return "\n".join(L)


def gen_js(rep, fk_lookup):
    m, n = rep["module"], rep["name"]
    fn = f"{m}{n}"
    cols_js = []
    for (prop, cs) in rep["columns"]:
        camel = prop[0].lower() + prop[1:]
        url = fk_lookup.get((rep["base_table"], prop))
        if prop.endswith("Id") and url:
            cols_js.append(
                "            { dataField: \"%s\", caption: \"%s\", lookup: { dataSource: lookupStore(\"%s\"), valueExpr: \"id\", displayExpr: \"displayName\" } }"
                % (camel, prop[:-2], url))
        elif cs in ("decimal",):
            cols_js.append('            { dataField: "%s", caption: "%s", dataType: "number", format: { type: "fixedPoint", precision: 2 } }' % (camel, prop))
        elif cs.startswith("DateTime"):
            cols_js.append('            { dataField: "%s", caption: "%s", dataType: "date" }' % (camel, prop))
        elif cs == "bool":
            cols_js.append('            { dataField: "%s", caption: "%s", dataType: "boolean" }' % (camel, prop))
        else:
            cols_js.append('            { dataField: "%s", caption: "%s" }' % (camel, prop))
    columns_block = ",\n".join(cols_js)
    L = [
        "/*",
        f" * {m} / {n} — read-only DevExtreme report screen.",
        " * Filters (date range%s) -> server-side query. Export via server CSV endpoint." % (" + status" if rep["status_field"] else ""),
        " */",
        "(function (window, $) {",
        '    "use strict";', "",
        "    function lookupStore(url) {",
        "        return new DevExpress.data.CustomStore({",
        '            key: "id",',
        '            loadMode: "raw",',
        "            load: function () { return window.AppHttp.get(url); }",
        "        });",
        "    }", "",
        "    function init(base, gridId, filtersId, opts) {",
        "        var state = { startDate: null, endDate: null, status: null };", "",
        "        function buildQuery(loadOptions) {",
        "            var params = {",
        "                skip: (loadOptions && loadOptions.skip) || 0,",
        "                take: (loadOptions && loadOptions.take) || 50",
        "            };",
        "            if (state.startDate) { params.startDate = state.startDate; }",
        "            if (state.endDate) { params.endDate = state.endDate; }",
        "            if (opts && opts.hasStatus && state.status) { params.status = state.status; }",
        "            return $.param(params);",
        "        }", "",
        "        var store = new DevExpress.data.CustomStore({",
        '            key: "id",',
        "            load: function (loadOptions) {",
        '                return window.AppHttp.get(base + "/data?" + buildQuery(loadOptions));',
        "            }",
        "        });", "",
        "        var grid = $(\"#\" + gridId).dxDataGrid({",
        "            dataSource: store,",
        "            remoteOperations: { paging: true },",
        "            showBorders: true,",
        "            headerFilter: { visible: true },",
        "            filterRow: { visible: true },",
        "            rowAlternationEnabled: true,",
        "            allowColumnResizing: true,",
        "            columnAutoWidth: true,",
        "            columnHidingEnabled: true,",
        '            width: "100%",',
        '            height: "70vh",',
        "            paging: { pageSize: 50 },",
        "            pager: { visible: true, showPageSizeSelector: true, allowedPageSizes: [25, 50, 100], showInfo: true },",
        "            columns: [",
        columns_block,
        "            ]",
        "        }).dxDataGrid(\"instance\");", "",
        "        // Filter toolbar (date range + optional status + export).",
        "        var $f = $(\"#\" + filtersId);",
        '        var $start = $("<div class=\\"energy-report__filter\\"></div>").appendTo($f);',
        '        var $end = $("<div class=\\"energy-report__filter\\"></div>").appendTo($f);',
        "        $start.dxDateBox({ type: \"date\", placeholder: \"Start\", onValueChanged: function (e) { state.startDate = e.value ? e.value.toISOString() : null; grid.refresh(); } });",
        "        $end.dxDateBox({ type: \"date\", placeholder: \"End\", onValueChanged: function (e) { state.endDate = e.value ? e.value.toISOString() : null; grid.refresh(); } });",
        "        if (opts && opts.hasStatus) {",
        '            var $status = $("<div class=\\"energy-report__filter\\"></div>").appendTo($f);',
        "            $status.dxTextBox({ placeholder: \"Status\", onValueChanged: function (e) { state.status = e.value || null; grid.refresh(); } });",
        "        }",
        '        var $export = $("<div class=\\"energy-report__filter\\"></div>").appendTo($f);',
        "        $export.dxButton({",
        '            icon: "export", text: "Export",',
        "            onClick: function () {",
        '                var q = buildQuery({ skip: 0, take: 100000 });',
        '                window.open(base + "/data?" + q, "_blank");',
        "            }",
        "        });",
        "    }", "",
        "    window.AppReports = window.AppReports || {};",
        f"    window.AppReports.{fn} = {{ init: init }};",
        "})(window, jQuery);", "",
    ]
    return "\n".join(L)


def gen_css(rep):
    n = rep["name"]
    return "\n".join([
        f"/* {n} report screen styles. */",
        ".energy-report__filters {",
        "    display: flex;",
        "    flex-wrap: wrap;",
        "    gap: 12px;",
        "    align-items: flex-end;",
        "    margin-bottom: 16px;",
        "}", "",
        ".energy-report__filter {",
        "    min-width: 160px;",
        "}", "",
        "@media (max-width: 640px) {",
        "    .energy-report__filters { flex-direction: column; align-items: stretch; }",
        "    .energy-report__filter { width: 100%; }",
        "}", "",
    ])


def gen_infra_registration():
    L = [
        "using Microsoft.Extensions.DependencyInjection;", "",
        "namespace Energy.Infrastructure.Modules;", "",
        "/// <summary>Tüm rapor servislerinin (salt-okunur) DI kaydı.</summary>",
        "public static class ModulesReportRegistration",
        "{",
        "    public static IServiceCollection AddModulesReportServices(this IServiceCollection services)",
        "    {",
    ]
    for rep in REPORTS:
        m, n = rep["module"], rep["name"]
        L.append(f"        services.AddScoped<global::Energy.Application.Modules.{m}.Reports.{n}.Services.I{n}Service, global::Energy.Infrastructure.Modules.{m}.Reports.{n}.{n}Service>();")
    L += ["        return services;", "    }", "}", ""]
    return "\n".join(L)


def gen_web_registration():
    L = [
        "using Energy.Web.Clients.Infrastructure.Authentication;",
        "using Energy.Web.Clients.Infrastructure.ClientIdentity;",
        "using Energy.Web.Configuration;",
        "using Microsoft.Extensions.Options;", "",
        "namespace Energy.Web.Clients;", "",
        "/// <summary>Tüm rapor API istemcilerinin (typed HttpClient) kaydı.</summary>",
        "public static class ModulesReportApiClientRegistration",
        "{",
        "    public static IServiceCollection AddModulesReportApiClients(this IServiceCollection services)",
        "    {",
    ]
    for rep in REPORTS:
        m, n = rep["module"], rep["name"]
        itype = f"global::Energy.Web.Clients.{m}.Reports.{n}.I{n}ApiClient"
        impl = f"global::Energy.Web.Clients.{m}.Reports.{n}.{n}ApiClient"
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
        "        if (string.IsNullOrWhiteSpace(settings.BaseUrl))",
        '            throw new InvalidOperationException("Api:BaseUrl is not configured.");',
        "        http.BaseAddress = new Uri(settings.BaseUrl);",
        "    }",
        "}", "",
    ]
    return "\n".join(L)


def gen_perm_map():
    L = [
        "namespace Energy.Infrastructure.System.Services;", "",
        "/// <summary>",
        "/// Üretilen rapor API uç noktalarının (Controller.Action) rapor yetkilerine",
        "/// eşlemesi. ApiEndpointSyncService başlangıçta bunları etkinleştirir.",
        "/// </summary>",
        "public static class ModulesReportEndpointPermissionMap",
        "{",
        "    public static void Apply(IDictionary<string, string?> map)",
        "    {",
    ]
    for rep in REPORTS:
        m, n = rep["module"], rep["name"]
        L.append(f'        map["{n}.GetData"] = "{m}.{n}.Read";')
        L.append(f'        map["{n}.Export"] = "{m}.{n}.Export";')
    L += ["    }", "}", ""]
    return "\n".join(L)


def gen_menu_seeder():
    rows = []
    for i, rep in enumerate(REPORTS, start=1):
        m, n = rep["module"], rep["name"]
        route = f"/{kebab(m)}/reports/{kebab(n)}"
        name_key = f"Menus.{m}.Reports.{n}"
        parent_key = f"Menus.{m}"
        rows.append((m, parent_key, n, route, name_key, i))
    L = [
        "using Microsoft.EntityFrameworkCore;",
        "using Microsoft.Extensions.Logging;", "",
        "namespace Energy.Infrastructure.Seeding;", "",
        "/// <summary>",
        "/// Per-report menü tohumlaması: her rapor, modül menüsünün altına",
        "/// /{module}/reports/{report} rotasıyla ve {Module}.{Report}.Read yetkisiyle eklenir.",
        "/// </summary>",
        "public sealed partial class SystemSeeder",
        "{",
        "    /// <summary>(Module, ParentMenuNameKey, Report, Route, NameKey, Order)</summary>",
        "    private static readonly (string Module, string ParentKey, string Report, string Route, string NameKey, int Order)[] ModuleReportMenus =",
        "    [",
    ]
    for (m, pk, n, route, nk, idx) in rows:
        L.append(f'        ("{m}", "{pk}", "{n}", "{route}", "{nk}", {idx}),')
    L += [
        "    ];", "",
        "    /// <summary>Modül menüsünün altına per-report menü girdilerini idempotent ekler.</summary>",
        "    private async Task EnsureModulesReportMenusAsync(CancellationToken ct)",
        "    {",
        "        foreach (var (module, parentKey, report, route, nameKey, order) in ModuleReportMenus)",
        "        {",
        "            var parent = await _db.Menus.FirstOrDefaultAsync(m => m.NameKey == parentKey, ct);",
        "            if (parent is null)",
        "            {",
        "                continue;",
        "            }",
        '            await EnsureMenuAsync(nameKey, parent.Id, route, "chart", 300 + order, $"{module}.{report}.Read", ct);',
        "        }",
        '        _logger.LogInformation("Seeding: {Count} per-report menu(s) ensured.", ModuleReportMenus.Length);',
        "    }",
        "}", "",
    ]
    return "\n".join(L)


def main():
    order, table_module, table_purpose, table_columns, table_entity = build_model()
    fk_lookup = build_fk_lookup_map(table_module)

    # clean previous report outputs
    for rep in REPORTS:
        m, n = rep["module"], rep["name"]
        shutil.rmtree(os.path.join(SHARED, m, "Reports", n), ignore_errors=True)
        shutil.rmtree(os.path.join(APP, m, "Reports", n), ignore_errors=True)
        shutil.rmtree(os.path.join(INFRA, m, "Reports", n), ignore_errors=True)
        shutil.rmtree(os.path.join(API, m, "Reports"), ignore_errors=True)
        shutil.rmtree(os.path.join(WEB_CLIENTS, m, "Reports", n), ignore_errors=True)
        shutil.rmtree(os.path.join(WEB_CTRL, m, "Reports"), ignore_errors=True)
        shutil.rmtree(os.path.join(VIEWS, m, "Reports", n), ignore_errors=True)
        shutil.rmtree(os.path.join(JS, kebab(m), "reports", kebab(n)), ignore_errors=True)
        shutil.rmtree(os.path.join(CSS, kebab(m), "reports", kebab(n)), ignore_errors=True)

    count = 0
    for rep in REPORTS:
        m, n = rep["module"], rep["name"]
        write(os.path.join(SHARED, m, "Reports", n, "Requests", f"{n}Request.cs"), gen_request(rep))
        write(os.path.join(SHARED, m, "Reports", n, "Responses", f"{n}RowResponse.cs"), gen_row(rep))
        write(os.path.join(APP, m, "Reports", n, "Services", f"I{n}Service.cs"), gen_app_interface(rep))
        write(os.path.join(INFRA, m, "Reports", n, f"{n}Service.cs"), gen_infra_service(rep))
        write(os.path.join(API, m, "Reports", f"{n}Controller.cs"), gen_api_controller(rep))
        write(os.path.join(WEB_CLIENTS, m, "Reports", n, f"I{n}ApiClient.cs"), gen_web_client(rep))
        write(os.path.join(WEB_CTRL, m, "Reports", f"{n}Controller.cs"), gen_web_controller(rep))
        write(os.path.join(VIEWS, m, "Reports", n, "Index.cshtml"), gen_view(rep))
        write(os.path.join(JS, kebab(m), "reports", kebab(n), f"{kebab(n)}.index.js"), gen_js(rep, fk_lookup))
        write(os.path.join(CSS, kebab(m), "reports", kebab(n), f"{kebab(n)}.css"), gen_css(rep))
        count += 1

    write(INFRA_REG, gen_infra_registration())
    write(WEB_REG, gen_web_registration())
    write(PERM_MAP, gen_perm_map())
    write(MENU_OUT, gen_menu_seeder())
    print(f"Generated {count} report verticals (+ DI, perm map, menu seeder)")


if __name__ == "__main__":
    main()

