(function (window, $) {
    "use strict";
    var L = function () { return window.AppL10n.roles; };
    var LG = function () { return window.AppL10n.grid; };
    var LN = function () { return window.AppL10n.notifications; };

    var gridInstance, permissionsLookup = [];

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
        loadPermissionsLookup().then(function () { buildGrid(); });
    }

    function loadPermissionsLookup() {
        return window.AppHttp.get("/roles/permissions-lookup")
            .then(function (data) { permissionsLookup = data || []; })
            .catch(function () { permissionsLookup = []; });
    }

    function buildGrid() {
        var auth = window.AppAuth || { can: function () { return true; } };
        var canCreate = auth.can("Role.Create");
        var canUpdate = auth.can("Role.Update");
        var canDelete = auth.can("Role.Delete");

        var rowButtons = [];
        if (canUpdate) {
            rowButtons.push({ hint: LG().edit, icon: "edit", onClick: function (e) { openEdit(e.row.data); } });
            rowButtons.push({ hint: L().managePermissions, icon: "key", onClick: function (e) { openPermissions(e.row.data); } });
        }
        if (canDelete) {
            rowButtons.push({ hint: LG().delete, icon: "trash", onClick: function (e) { confirmDelete(e.row.data); } });
        }

        var toolbarItems = [];
        if (canCreate) {
            toolbarItems.push({ location: "after", widget: "dxButton", locateInMenu: "auto",
                options: { icon: "add", text: LG().add, type: "default", stylingMode: "contained", onClick: openCreate } });
        }
        toolbarItems.push(
            { location: "after", widget: "dxButton", locateInMenu: "auto",
              options: { icon: "refresh", hint: LG().refresh, stylingMode: "text", onClick: function () { gridInstance.refresh(); } } },
            { name: "columnChooserButton", location: "after", locateInMenu: "auto" },
            { name: "exportButton", location: "after", locateInMenu: "auto" },
            { name: "searchPanel", location: "after" }
        );

        var store = new DevExpress.data.CustomStore({
            key: "id",
            load: function (loadOptions) {
                var params = $.param({
                    skip: loadOptions.skip || 0, take: loadOptions.take || 20,
                    sort: loadOptions.sort ? JSON.stringify(loadOptions.sort) : "",
                    searchValue: loadOptions.searchValue || ""
                });
                return window.AppHttp.get("/roles/list?" + params);
            }
        });

        gridInstance = $("#roles-grid").dxDataGrid({
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
            columnAutoWidth: true,
            columnHidingEnabled: false,
            wordWrapEnabled: false,
            width: "100%",
            height: "75vh",
            scrolling: { mode: "standard", useNative: true, showScrollbar: "onScroll", columnRenderingMode: "standard" },
            repaintChangesOnly: true,
            paging: { pageSize: 20 },
            pager: { visible: true, allowedPageSizes: [10, 20, 50], showPageSizeSelector: true, showInfo: true, showNavigationButtons: true, displayMode: "full" },
            searchPanel: { visible: true, placeholder: LG().search, width: 240 },
            sorting: { mode: "multiple" },
            columnChooser: { enabled: true, mode: "select", height: 320, search: { enabled: true } },
            loadPanel: { enabled: true, text: LG().loading },
            noDataText: LG().noData,
            export: { enabled: true, formats: ["xlsx"] },
            onExporting: function (e) { exportGrid(e, L().title); },
            columns: [
                { dataField: "name", caption: L().name },
                { dataField: "description", caption: L().description },
                {
                    type: "buttons", width: 180, caption: LG().actions, fixed: true, fixedPosition: "right",
                    visible: rowButtons.length > 0,
                    buttons: rowButtons
                }
            ],
            toolbar: { items: toolbarItems }
        }).dxDataGrid("instance");
    }

    function openCreate() {
        showFormPopup({ title: L().createTitle, data: { name: "", description: "" }, onSave: function (d) { return window.AppHttp.post("/roles", d); }});
    }
    function openEdit(row) {
        showFormPopup({ title: L().editTitle, data: { name: row.name, description: row.description },
            onSave: function (d) { return window.AppHttp.put("/roles/" + row.id, d); }});
    }

    function showFormPopup(opts) {
        var formData = opts.data;
        var popup = $("<div>").appendTo("body").dxPopup({
            title: opts.title,
            width: 520,
            height: "auto",
            maxWidth: "min(92vw, 960px)",
            maxHeight: "min(88vh, 820px)",
            dragEnabled: true,
            resizeEnabled: true,
            hideOnOutsideClick: true,
            showCloseButton: true,
            wrapperAttr: { class: "energy-popup" },
            contentTemplate: function (host) {
                $("<div>").appendTo(host).dxForm({
                    formData: formData, labelLocation: "top", colCount: 1,
                    items: [
                        { dataField: "name", label: { text: L().name }, validationRules: [{ type: "required" }] },
                        { dataField: "description", label: { text: L().description }, editorType: "dxTextArea", editorOptions: { height: 100 } }
                    ]
                });
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
        }).dxPopup("instance");
        popup.show();
    }

    function openPermissions(row) {
        window.AppLoading.wrap(window.AppHttp.get("/roles/" + row.id + "/permissions"))
            .then(function (selectedResp) {
                var selectedIds = selectedResp.selected || [];
                var formData = { permissionIds: selectedIds.slice() };
                var popup = $("<div>").appendTo("body").dxPopup({
                    title: L().permissionsTitle + " — " + row.name,
                    width: 560,
                    height: "auto",
                    maxWidth: "min(92vw, 960px)",
                    maxHeight: "min(88vh, 820px)",
                    dragEnabled: true,
                    resizeEnabled: true,
                    hideOnOutsideClick: true,
                    showCloseButton: true,
                    wrapperAttr: { class: "energy-popup" },
                    contentTemplate: function (host) {
                        $("<div>").appendTo(host).dxForm({
                            formData: formData, labelLocation: "top", colCount: 1,
                            items: [{
                                dataField: "permissionIds", label: { text: window.AppL10n.permissions.title },
                                editorType: "dxTagBox",
                                editorOptions: {
                                    items: permissionsLookup, valueExpr: "id", displayExpr: function (p) { return p ? p.name + " (" + p.code + ")" : ""; },
                                    showSelectionControls: true, applyValueMode: "useButtons", searchEnabled: true
                                }
                            }]
                        });
                    },
                    toolbarItems: [
                        { widget: "dxButton", location: "after", toolbar: "bottom",
                          options: { text: LG().save, type: "default", onClick: function () {
                              window.AppLoading.wrap(window.AppHttp.put("/roles/" + row.id + "/permissions", { permissionIds: formData.permissionIds }))
                                  .then(function () { window.AppNotify.success(LN().saved); popup.hide(); })
                                  .catch(window.AppNotify.fromHttpError);
                          }}},
                        { widget: "dxButton", location: "after", toolbar: "bottom",
                          options: { text: LG().cancel, onClick: function () { popup.hide(); } }}
                    ],
                    onHidden: function () { popup.dispose(); $(popup.element()).remove(); }
                }).dxPopup("instance");
                popup.show();
            })
            .catch(window.AppNotify.fromHttpError);
    }

    function confirmDelete(row) {
        DevExpress.ui.dialog.confirm(LG().confirmDelete, LG().delete).then(function (ok) {
            if (!ok) return;
            window.AppLoading.wrap(window.AppHttp.del("/roles/" + row.id))
                .then(function () { window.AppNotify.success(LN().deleted); gridInstance.refresh(); })
                .catch(window.AppNotify.fromHttpError);
        });
    }

    window.AppPages = window.AppPages || {};
    window.AppPages.Roles = { init: init };
})(window, jQuery);
