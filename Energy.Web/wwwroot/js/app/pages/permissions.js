(function (window, $) {
    "use strict";
    var L = function () { return window.AppL10n.permissions; };
    var LG = function () { return window.AppL10n.grid; };
    var LN = function () { return window.AppL10n.notifications; };
    var R = function () { return window.AppResponsive; };
    var gridInstance;

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
        var store = new DevExpress.data.CustomStore({
            key: "id",
            load: function (loadOptions) {
                var params = $.param({
                    skip: loadOptions.skip || 0, take: loadOptions.take || 20,
                    sort: loadOptions.sort ? JSON.stringify(loadOptions.sort) : "",
                    searchValue: loadOptions.searchValue || ""
                });
                return window.AppHttp.get("/permissions/list?" + params);
            }
        });

        var g = R().getGridOptions();
        gridInstance = $("#permissions-grid").dxDataGrid({
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
                { dataField: "code", caption: L().code },
                { dataField: "name", caption: L().name },
                { type: "buttons", width: 120, caption: LG().actions, fixed: true, fixedPosition: "right",
                  buttons: [
                      { hint: LG().edit, icon: "edit", onClick: function (e) { openEdit(e.row.data); } },
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
        showFormPopup({ title: L().createTitle, data: { code: "", name: "" },
            onSave: function (d) { return window.AppHttp.post("/permissions", d); } });
    }
    function openEdit(row) {
        showFormPopup({ title: L().editTitle, data: { code: row.code, name: row.name },
            onSave: function (d) { return window.AppHttp.put("/permissions/" + row.id, d); } });
    }
    function showFormPopup(opts) {
        var formData = opts.data;
        var popup = $("<div>").appendTo("body").dxPopup(R().getPopupOptions({
            title: opts.title, width: 480, height: 280, showCloseButton: true,
            contentTemplate: function (host) {
                $("<div>").appendTo(host).dxForm(R().getFormOptions({
                    formData: formData, labelLocation: "top",
                    items: [
                        { dataField: "code", label: { text: L().code }, validationRules: [{ type: "required" }] },
                        { dataField: "name", label: { text: L().name }, validationRules: [{ type: "required" }] }
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
            window.AppLoading.wrap(window.AppHttp.del("/permissions/" + row.id))
                .then(function () { window.AppNotify.success(LN().deleted); gridInstance.refresh(); })
                .catch(window.AppNotify.fromHttpError);
        });
    }

    window.AppPages = window.AppPages || {};
    window.AppPages.Permissions = { init: init };
})(window, jQuery);
