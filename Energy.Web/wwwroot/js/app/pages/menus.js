(function (window, $) {
    "use strict";
    var L = function () { return window.AppL10n.menus; };
    var LG = function () { return window.AppL10n.grid; };
    var LN = function () { return window.AppL10n.notifications; };
    var R = function () { return window.AppResponsive; };
    var gridInstance, lookupItems = [], permissionsLookup = [];

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
        Promise.all([loadLookup(), loadPermissionsLookup()]).then(function () { buildGrid(); });
    }

    function loadLookup() {
        return window.AppHttp.get("/menus/lookup")
            .then(function (data) { lookupItems = data || []; })
            .catch(function () { lookupItems = []; });
    }

    function loadPermissionsLookup() {
        return window.AppHttp.get("/menus/permissions-lookup")
            .then(function (data) { permissionsLookup = data || []; })
            .catch(function () { permissionsLookup = []; });
    }

    function buildGrid() {
        var store = new DevExpress.data.CustomStore({
            key: "id",
            load: function (loadOptions) {
                var params = $.param({
                    skip: loadOptions.skip || 0, take: loadOptions.take || 20,
                    sort: loadOptions.sort ? JSON.stringify(loadOptions.sort) : "",
                    searchValue: loadOptions.searchValue || ""
                });
                return window.AppHttp.get("/menus/list?" + params);
            }
        });

        var g = R().getGridOptions();
        gridInstance = $("#menus-grid").dxDataGrid({
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
            onExporting: function (e) { exportGrid(e, L().title); },
            columns: [
                { dataField: "name", caption: L().name },
                { dataField: "nameKey", caption: L().nameKey },
                { dataField: "url", caption: L().url },
                { dataField: "icon", caption: L().icon, width: 100 },
                { dataField: "order", caption: L().order, width: 80, alignment: "center" },
                {
                    dataField: "parentId", caption: L().parent,
                    calculateDisplayValue: function (row) {
                        if (!row.parentId) return L().noParent;
                        var p = lookupItems.find(function (m) { return m.id === row.parentId; });
                        return p ? p.name : "";
                    }
                },
                { type: "buttons", width: 130, caption: LG().actions, fixed: true, fixedPosition: "right", buttons: [
                    { hint: LG().edit, icon: "edit", onClick: function (e) { openEdit(e.row.data); } },
                    { hint: window.AppL10n.roles.managePermissions, icon: "key", onClick: function (e) { openPermissions(e.row.data); } },
                    { hint: LG().delete, icon: "trash", onClick: function (e) { confirmDelete(e.row.data); } }
                ]}
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
        showFormPopup({ title: L().createTitle,
            data: { name: "", url: "", icon: "", order: 0, parentId: null },
            onSave: function (d) { return window.AppHttp.post("/menus", d).then(loadLookup); }});
    }
    function openEdit(row) {
        showFormPopup({ title: L().editTitle,
            data: { name: row.name, url: row.url, icon: row.icon, order: row.order, parentId: row.parentId || null },
            onSave: function (d) { return window.AppHttp.put("/menus/" + row.id, d).then(loadLookup); }});
    }
    function showFormPopup(opts) {
        var formData = opts.data;
        var popup = $("<div>").appendTo("body").dxPopup(R().getPopupOptions({
            title: opts.title, width: 640, height: 520, showCloseButton: true,
            contentTemplate: function (host) {
                $("<div>").appendTo(host).dxForm(R().getFormOptions({
                    formData: formData, labelLocation: "top", colCount: 2,
                    items: [
                        { dataField: "name", label: { text: L().name }, validationRules: [{ type: "required" }] },
                        { dataField: "url", label: { text: L().url } },
                        { dataField: "icon", label: { text: L().icon }, editorType: "dxSelectBox",
                          editorOptions: {
                              items: window.AppIcons || [],
                              searchEnabled: true,
                              showClearButton: true,
                              placeholder: L().icon,
                              itemTemplate: function (data) {
                                  return $("<div>").css({ display: "flex", alignItems: "center", gap: "10px" })
                                      .append($("<i>").addClass("dx-icon dx-icon-" + data).css({ fontSize: "18px", width: "18px", textAlign: "center" }))
                                      .append($("<span>").text(data));
                              },
                              fieldTemplate: function (data, container) {
                                  container.css({ display: "flex", alignItems: "center" });
                                  $("<i>").addClass("dx-icon " + (data ? "dx-icon-" + data : ""))
                                      .css({ marginLeft: "8px", fontSize: "18px", width: "18px", textAlign: "center" })
                                      .appendTo(container);
                                  $("<div>").css("flex", "1").dxTextBox({ value: data, placeholder: L().icon }).appendTo(container);
                              }
                          }
                        },
                        { dataField: "order", label: { text: L().order }, editorType: "dxNumberBox", editorOptions: { min: 0, step: 1 }},
                        { dataField: "parentId", label: { text: L().parent }, editorType: "dxSelectBox",
                          editorOptions: { items: lookupItems, valueExpr: "id", displayExpr: "name", searchEnabled: true, showClearButton: true } }
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

    function confirmDelete(row) {
        DevExpress.ui.dialog.confirm(LG().confirmDelete, LG().delete).then(function (ok) {
            if (!ok) return;
            window.AppLoading.wrap(window.AppHttp.del("/menus/" + row.id))
                .then(function () { window.AppNotify.success(LN().deleted); loadLookup().then(function () { gridInstance.refresh(); }); })
                .catch(window.AppNotify.fromHttpError);
        });
    }

    function openPermissions(row) {
        window.AppLoading.wrap(window.AppHttp.get("/menus/" + row.id + "/permissions"))
            .then(function (selectedResp) {
                var selectedIds = selectedResp.selected || [];
                var formData = { permissionIds: selectedIds.slice() };

                var popup = $("<div>").appendTo("body").dxPopup(R().getPopupOptions({
                    title: window.AppL10n.roles.permissionsTitle + " - " + row.name,
                    width: 560, height: 520, showCloseButton: true,
                    contentTemplate: function (host) {
                        $("<div>").appendTo(host).dxForm(R().getFormOptions({
                            formData: formData,
                            items: [{
                                dataField: "permissionIds", label: { text: window.AppL10n.permissions.title },
                                editorType: "dxTagBox",
                                editorOptions: {
                                    items: permissionsLookup,
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
                              window.AppLoading.wrap(window.AppHttp.put("/menus/" + row.id + "/permissions", { permissionIds: formData.permissionIds }))
                                  .then(function () { window.AppNotify.success(LN().saved); popup.hide(); gridInstance.refresh(); })
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

    window.AppPages = window.AppPages || {};
    window.AppPages.Menus = { init: init };
})(window, jQuery);
