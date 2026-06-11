(function (window, $) {
    "use strict";

    var LG = function () { return window.AppL10n.grid; };
    var LN = function () { return window.AppL10n.notifications; };
    var R = function () { return window.AppResponsive; };

    var gridInstance;
    var permissionLookup = [];

    function exportGrid(e, fileName) {
        if (typeof ExcelJS === "undefined" || typeof saveAs === "undefined" ||
            !DevExpress.excelExporter || !DevExpress.excelExporter.exportDataGrid) { return; }
        var workbook = new ExcelJS.Workbook();
        var worksheet = workbook.addWorksheet("Data");
        DevExpress.excelExporter.exportDataGrid({ component: e.component, worksheet: worksheet, autoFilterEnabled: true })
            .then(function () {
                workbook.xlsx.writeBuffer().then(function (buffer) {
                    saveAs(new Blob([buffer], { type: "application/octet-stream" }), (fileName || "export") + ".xlsx");
                });
            });
        e.cancel = true;
    }

    function init() {
        loadPermissionLookup().then(function () { buildGrid(); });
    }

    function loadPermissionLookup() {
        return window.AppHttp.get("/access-rules/permissions-lookup")
            .then(function (data) { permissionLookup = data || []; })
            .catch(function () { permissionLookup = []; });
    }

    function buildGrid() {
        var store = new DevExpress.data.CustomStore({
            key: "id",
            load: function (loadOptions) {
                var params = $.param({
                    skip: loadOptions.skip || 0,
                    take: loadOptions.take || 20,
                    sort: loadOptions.sort ? JSON.stringify(loadOptions.sort) : "",
                    searchValue: loadOptions.searchValue || ""
                });
                return window.AppHttp.get("/access-rules/list?" + params);
            }
        });

        var g = R().getGridOptions();
        gridInstance = $("#access-rules-grid").dxDataGrid({
            dataSource: store,
            remoteOperations: { paging: true, sorting: true },
            showBorders: true,
            headerFilter: { visible: true },
            filterRow: { visible: true },
            groupPanel: { visible: true },
            grouping: { autoExpandAll: true },
            showColumnLines: false,
            showRowLines: true,
            rowAlternationEnabled: true,
            hoverStateEnabled: true,
            allowColumnResizing: true,
            columnResizingMode: "widget",
            columnAutoWidth: g.columnAutoWidth,
            columnHidingEnabled: g.columnHidingEnabled,
            wordWrapEnabled: g.wordWrapEnabled,
            width: "100%",
            height: g.height,
            scrolling: g.scrolling,
            repaintChangesOnly: true,
            paging: { pageSize: 20 },
            pager: R().getPagerOptions(),
            searchPanel: R().getSearchPanelOptions(LG().search),
            sorting: { mode: "multiple" },
            columnChooser: { enabled: true, mode: "select", height: 320, search: { enabled: true } },
            loadPanel: { enabled: true, text: LG().loading },
            noDataText: LG().noData,
            export: { enabled: true, formats: ["xlsx"] },
            onExporting: function (e) { exportGrid(e, "AccessRules"); },
            columns: [
                { dataField: "name", caption: "Name" },
                { dataField: "scope", caption: "Scope", width: 90 },
                { dataField: "path", caption: "Path" },
                { dataField: "httpMethod", caption: "Method", width: 90 },
                { dataField: "isEnabled", caption: "Enabled", dataType: "boolean", width: 90, alignment: "center" },
                {
                    type: "buttons", width: 130, caption: LG().actions, fixed: true, fixedPosition: "right",
                    buttons: [
                        { hint: LG().edit, icon: "edit", onClick: function (e) { openEdit(e.row.data); } },
                        { hint: "Permissions", icon: "key", onClick: function (e) { openPermissions(e.row.data); } },
                        { hint: LG().delete, icon: "trash", onClick: function (e) { confirmDelete(e.row.data); } }
                    ]
                }
            ],
            toolbar: { items: [
                { location: "after", widget: "dxButton", locateInMenu: "auto",
                  options: { icon: "add", text: LG().add, type: "default", stylingMode: "contained", onClick: openCreate } },
                { location: "after", widget: "dxButton", locateInMenu: "auto",
                  options: { icon: "refresh", hint: LG().refresh, stylingMode: "text", onClick: function () { gridInstance.refresh(); } } },
                { name: "columnChooserButton", location: "after", locateInMenu: "auto" },
                { name: "exportButton", location: "after", locateInMenu: "auto" },
                { name: "searchPanel", location: "after" }
            ] }
        }).dxDataGrid("instance");
    }

    function openCreate() {
        showFormPopup({
            title: "Create Access Rule",
            data: { name: "", scope: "PAGE", path: "", httpMethod: "", description: "", isEnabled: true },
            onSave: function (data) { return window.AppHttp.post("/access-rules", data); }
        });
    }

    function openEdit(row) {
        showFormPopup({
            title: "Edit Access Rule",
            data: {
                name: row.name,
                scope: row.scope,
                path: row.path,
                httpMethod: row.httpMethod,
                description: row.description,
                isEnabled: row.isEnabled
            },
            onSave: function (data) { return window.AppHttp.put("/access-rules/" + row.id, data); }
        });
    }

    function showFormPopup(opts) {
        var formData = opts.data;

        var popup = $("<div>").appendTo("body").dxPopup(R().getPopupOptions({
            title: opts.title, width: 700, height: 500, showCloseButton: true,
            contentTemplate: function (host) {
                $("<div>").appendTo(host).dxForm(R().getFormOptions({
                    formData: formData,
                    labelLocation: "top",
                    colCount: 2,
                    items: [
                        { dataField: "name", label: { text: "Name" }, validationRules: [{ type: "required" }] },
                        { dataField: "scope", label: { text: "Scope" }, editorType: "dxSelectBox", editorOptions: { items: ["PAGE", "API"] } },
                        { dataField: "path", label: { text: "Path" }, validationRules: [{ type: "required" }] },
                        { dataField: "httpMethod", label: { text: "HTTP Method (optional)" }, editorType: "dxSelectBox", editorOptions: { items: ["", "GET", "POST", "PUT", "DELETE", "PATCH"] } },
                        { dataField: "isEnabled", label: { text: "Enabled" }, editorType: "dxCheckBox" },
                        { dataField: "description", label: { text: "Description" }, editorType: "dxTextArea", editorOptions: { height: 100 }, colSpan: 2 }
                    ]
                }));
            },
            toolbarItems: [
                { widget: "dxButton", location: "after", toolbar: "bottom",
                  options: { text: LG().save, type: "default", onClick: function () {
                      window.AppLoading.wrap(opts.onSave(formData))
                          .then(function () { window.AppNotify.success(LN().saved); popup.hide(); gridInstance.refresh(); })
                          .catch(window.AppNotify.fromHttpError);
                  }}},
                { widget: "dxButton", location: "after", toolbar: "bottom",
                  options: { text: LG().cancel, onClick: function () { popup.hide(); } }}
            ],
            onHidden: function () { popup.dispose(); $(popup.element()).remove(); }
        })).dxPopup("instance");

        popup.show();
    }

    function openPermissions(row) {
        window.AppLoading.wrap(window.AppHttp.get("/access-rules/" + row.id + "/permissions"))
            .then(function (selectedResp) {
                var selectedIds = selectedResp.selected || [];
                var formData = { permissionIds: selectedIds.slice() };

                var popup = $("<div>").appendTo("body").dxPopup(R().getPopupOptions({
                    title: "Rule Permissions - " + row.name,
                    width: 560, height: 520, showCloseButton: true,
                    contentTemplate: function (host) {
                        $("<div>").appendTo(host).dxForm(R().getFormOptions({
                            formData: formData,
                            items: [{
                                dataField: "permissionIds", label: { text: "Permissions" },
                                editorType: "dxTagBox",
                                editorOptions: {
                                    items: permissionLookup,
                                    valueExpr: "id",
                                    displayExpr: function (permission) {
                                        return permission ? permission.name + " (" + permission.code + ")" : "";
                                    },
                                    showSelectionControls: true,
                                    applyValueMode: "useButtons",
                                    searchEnabled: true
                                }
                            }]
                        }));
                    },
                    toolbarItems: [
                        { widget: "dxButton", location: "after", toolbar: "bottom",
                          options: { text: LG().save, type: "default", onClick: function () {
                              window.AppLoading.wrap(window.AppHttp.put("/access-rules/" + row.id + "/permissions", { permissionIds: formData.permissionIds }))
                                  .then(function () { window.AppNotify.success(LN().saved); popup.hide(); })
                                  .catch(window.AppNotify.fromHttpError);
                          }}},
                        { widget: "dxButton", location: "after", toolbar: "bottom",
                          options: { text: LG().cancel, onClick: function () { popup.hide(); } }}
                    ],
                    onHidden: function () { popup.dispose(); $(popup.element()).remove(); }
                })).dxPopup("instance");

                popup.show();
            })
            .catch(window.AppNotify.fromHttpError);
    }

    function confirmDelete(row) {
        DevExpress.ui.dialog.confirm(LG().confirmDelete, LG().delete).then(function (ok) {
            if (!ok) return;
            window.AppLoading.wrap(window.AppHttp.del("/access-rules/" + row.id))
                .then(function () { window.AppNotify.success(LN().deleted); gridInstance.refresh(); })
                .catch(window.AppNotify.fromHttpError);
        });
    }

    window.AppPages = window.AppPages || {};
    window.AppPages.AccessRules = { init: init };
})(window, jQuery);

