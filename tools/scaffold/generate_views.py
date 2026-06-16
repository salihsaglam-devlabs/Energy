#!/usr/bin/env python3
"""
Energy — Web Views + JS generator (Phase 7 UI).

Per web-managed entity (IAM/Chat excluded):
  * Views/{Module}/{Entity}/Index.cshtml  — DevExtreme grid host + init.
  * wwwroot/js/modules/{module-kebab}/{entity-kebab}/{entity-kebab}.index.js
        — entity-specific dxDataGrid bound to the entity Web controller routes,
          reusing the shared AppHttp/AppNotify/AppAuth/AppL10n helpers.

Screen-specific grid config lives in its OWN js file (no shared screen logic).
"""
from __future__ import annotations

import os
import re
import shutil

from generate_domain import ROOT, build_model, load_rows

VIEWS_ROOT = os.path.join(ROOT, "Energy.Web", "Views")
JS_ROOT = os.path.join(ROOT, "Energy.Web", "wwwroot", "js", "modules")
EXCLUDE_MODULES = {"IAM", "Chat"}
AUDIT_FK = {"CreatedBy", "UpdatedBy", "DeletedBy"}


def kebab(name: str) -> str:
    return re.sub(r"(?<!^)(?=[A-Z])", "-", name).lower()


def camel(name: str) -> str:
    return name[:1].lower() + name[1:] if name else name


def build_fk_lookups(table_module, table_entity, table_columns):
    """source table -> { camelFkField: lookupUrl } for FKs whose target is web-managed.
    Combines the Excel Relationship Catalogue (precise) with {Entity}Id column-name
    inference (covers legacy-enriched FK columns absent from the Excel relationships)."""
    out: dict[str, dict[str, str]] = {}

    def add(src, scol, tgt):
        tmod = table_module.get(tgt)
        if tmod is None or tmod in EXCLUDE_MODULES:
            return
        out.setdefault(src, {})[camel(scol)] = f"/{kebab(tmod)}/{kebab(tgt)}/lookup"

    # (a) Excel relationships
    for r in load_rows("Relationship Catalogue")[2:]:
        if len(r) < 4:
            continue
        src, scol, tgt = r[0], r[1], r[2]
        if src == "SourceTable" or not src or not scol or scol in AUDIT_FK:
            continue
        add(src, scol, tgt)

    # (b) Column-name inference: {Entity}Id -> table whose singular == {Entity}
    singular_to_table = {table_entity[t]: t for t in table_entity}
    for src, cols in table_columns.items():
        if src not in table_module:
            continue  # skip stray/non-canonical keys
        for c in cols:
            col = c["col"]
            if col == "Id" or col in AUDIT_FK or not col.endswith("Id"):
                continue
            base = col[:-2]
            tgt = singular_to_table.get(base)
            if tgt:
                add(src, col, tgt)
    return out


def write(path, content):
    os.makedirs(os.path.dirname(path), exist_ok=True)
    with open(path, "w", encoding="utf-8") as f:
        f.write(content)


def gen_view(module, entity, table):
    token = f"{module}{entity}"
    base = f"/{kebab(module)}/{kebab(table)}"
    js = f"~/js/modules/{kebab(module)}/{kebab(entity)}/{kebab(entity)}.index.js"
    title_key = f"Modules.{module}.{entity}.Title"
    return "\n".join([
        "@using Energy.Localization",
        "@inject Microsoft.AspNetCore.Mvc.Localization.IHtmlLocalizer<SharedResource> T",
        "@{",
        f'    ViewData["Title"] = T["{title_key}"].Value;',
        "}", "",
        '<section class="energy-screen">',
        '    <header class="energy-screen__header">',
        f'        <div><h2>@T["{title_key}"]</h2></div>',
        "    </header>",
        f'    <div id="{kebab(entity)}-grid"></div>',
        "</section>", "",
        "@section Scripts {",
        f'    <script src="{js}" asp-append-version="true"></script>',
        "    <script>",
        '        document.addEventListener("DOMContentLoaded", function () {',
        f'            window.AppPages.{token}.init("{base}", "{kebab(entity)}-grid", "{module}");',
        "        });",
        "    </script>",
        "}", "",
    ])


def gen_js(module, entity, table, lookups):
    token = f"{module}{entity}"
    lookup_entries = ",\n".join(
        [f'        "{field}": "{url}"' for field, url in sorted(lookups.items())])
    return f"""/*
 * {module} / {entity} — entity-specific DevExtreme grid screen.
 * Shared helpers used: AppHttp, AppNotify, AppAuth, AppL10n. Screen-specific
 * grid columns / FK lookups / CRUD wiring live ONLY in this file.
 */
(function (window, $) {{
    "use strict";

    var LG = function () {{ return (window.AppL10n && window.AppL10n.grid) || {{}}; }};
    var LN = function () {{ return (window.AppL10n && window.AppL10n.notifications) || {{}}; }};

    var HIDDEN_FIELDS = ["id", "createdAt", "createdBy", "updatedAt", "updatedBy",
        "isDeleted", "deletedAt", "deletedBy"];

    // FK alanı -> ilişkili entity lookup endpoint'i. Kullanıcıya ID değil ad gösterilir.
    var LOOKUPS = {{
{lookup_entries}
    }};

    function lookupStore(url) {{
        return new DevExpress.data.CustomStore({{
            key: "id",
            loadMode: "raw",
            load: function () {{ return window.AppHttp.get(url); }}
        }});
    }}

    function init(base, gridId, permModule) {{
        var auth = window.AppAuth || {{ can: function () {{ return true; }} }};
        var store = new DevExpress.data.CustomStore({{
            key: "id",
            load: function (loadOptions) {{
                var params = $.param({{
                    skip: loadOptions.skip || 0,
                    take: loadOptions.take || 20,
                    searchValue: loadOptions.searchValue || ""
                }});
                return window.AppHttp.get(base + "/list?" + params);
            }},
            insert: function (values) {{ return window.AppHttp.post(base, values); }},
            update: function (key, values) {{ return window.AppHttp.put(base + "/" + key, values); }},
            remove: function (key) {{ return window.AppHttp.del(base + "/" + key); }}
        }});

        $("#" + gridId).dxDataGrid({{
            dataSource: store,
            remoteOperations: {{ paging: true }},
            showBorders: true,
            headerFilter: {{ visible: true }},
            filterRow: {{ visible: true }},
            rowAlternationEnabled: true,
            hoverStateEnabled: true,
            allowColumnResizing: true,
            columnAutoWidth: true,
            columnHidingEnabled: true,
            width: "100%",
            height: "75vh",
            paging: {{ pageSize: 20 }},
            pager: {{ visible: true, allowedPageSizes: [10, 20, 50], showPageSizeSelector: true, showInfo: true }},
            searchPanel: {{ visible: true, placeholder: (LG().search || "Ara..."), width: 240 }},
            sorting: {{ mode: "multiple" }},
            columnChooser: {{ enabled: true, mode: "select" }},
            loadPanel: {{ enabled: true, text: (LG().loading || "Yükleniyor...") }},
            noDataText: (LG().noData || "Kayıt yok"),
            export: {{ enabled: true, formats: ["xlsx"] }},
            editing: {{
                mode: "popup",
                allowAdding: auth.can(permModule + ".Create"),
                allowUpdating: auth.can(permModule + ".Update"),
                allowDeleting: auth.can(permModule + ".Delete"),
                useIcons: true,
                popup: {{ showTitle: true, width: "min(92vw, 720px)", height: "auto" }}
            }},
            onRowUpdating: function (e) {{ e.newData = $.extend({{}}, e.oldData, e.newData); }},
            customizeColumns: function (columns) {{
                columns.forEach(function (col) {{
                    if (HIDDEN_FIELDS.indexOf(col.dataField) !== -1) {{
                        col.visible = false;
                        col.formItem = {{ visible: false }};
                        col.allowEditing = false;
                    }} else if (LOOKUPS[col.dataField]) {{
                        // FK kolonu: ID yerine ilişkili kaydın görünen adını göster.
                        col.lookup = {{
                            dataSource: lookupStore(LOOKUPS[col.dataField]),
                            valueExpr: "id",
                            displayExpr: "displayName"
                        }};
                    }}
                }});
            }},
            onRowInserted: function () {{ window.AppNotify && window.AppNotify.success(LN().saved || "Kaydedildi"); }},
            onRowUpdated: function () {{ window.AppNotify && window.AppNotify.success(LN().saved || "Kaydedildi"); }},
            onRowRemoved: function () {{ window.AppNotify && window.AppNotify.success(LN().deleted || "Silindi"); }},
            onDataErrorOccurred: function (e) {{
                if (window.AppNotify && window.AppNotify.fromHttpError) {{ window.AppNotify.fromHttpError(e.error); }}
            }}
        }});
    }}

    window.AppPages = window.AppPages || {{}};
    window.AppPages.{token} = {{ init: init }};

}})(window, window.jQuery);
"""


def main():
    shutil.rmtree(VIEWS_ROOT, ignore_errors=True)
    shutil.rmtree(JS_ROOT, ignore_errors=True)
    order, table_module, table_purpose, table_columns, table_entity = build_model()
    fk_lookups = build_fk_lookups(table_module, table_entity, table_columns)
    count = 0
    for t in order:
        m, e = table_module[t], table_entity[t]
        if m in EXCLUDE_MODULES:
            continue
        write(os.path.join(VIEWS_ROOT, m, e, "Index.cshtml"), gen_view(m, e, t))
        write(os.path.join(JS_ROOT, kebab(m), kebab(e), f"{kebab(e)}.index.js"),
              gen_js(m, e, t, fk_lookups.get(t, {})))
        count += 1
    print(f"Generated {count} Index views + {count} entity JS screens (with FK lookups)")


if __name__ == "__main__":
    main()

