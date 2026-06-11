(function (window, $) {
    "use strict";
    var L = function () { return window.AppL10n.localization; };
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

    function importFromResx() {
        DevExpress.ui.dialog.confirm(L().importConfirmMessage, L().importConfirmTitle).then(function (ok) {
            if (!ok) return;
            window.AppLoading.wrap(window.AppHttp.post("/localization/import-from-resx", {}))
                .then(function (resp) {
                    var total = resp && resp.data ? (resp.data.added + resp.data.updated) : 0;
                    window.AppNotify.success(L().imported.replace("{0}", total));
                    gridInstance.refresh();
                })
                .catch(window.AppNotify.fromHttpError);
        });
    }

    function init() {
        var store = new DevExpress.data.CustomStore({
            key: "key",
            load: function () { return window.AppHttp.get("/localization/list"); },
            insert: function (values) { return window.AppHttp.post("/localization", values); },
            update: function (key, values) {
                return window.AppHttp.get("/localization/list").then(function (all) {
                    var current = all.find(function (r) { return r.key === key; }) || { key: key };
                    var merged = Object.assign({}, current, values);
                    return window.AppHttp.post("/localization", merged);
                });
            },
            remove: function (key) { return window.AppHttp.del("/localization?key=" + encodeURIComponent(key)); }
        });

        var g = R().getGridOptions();
        gridInstance = $("#localization-grid").dxDataGrid({
            dataSource: store,
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
            editing: {
                mode: "popup", allowAdding: true, allowUpdating: true, allowDeleting: true,
                useIcons: true,
                popup: R().getPopupOptions({ title: L().editTitle, showTitle: true, width: 640, height: 460 }),
                form: { labelLocation: "top", items: [
                    { dataField: "key", isRequired: true },
                    { dataField: "tr" },
                    { dataField: "en" },
                    { dataField: "invariant" }
                ]}
            },
            columns: [
                { dataField: "key", caption: L().key },
                { dataField: "tr", caption: L().tr },
                { dataField: "en", caption: L().en },
                { dataField: "invariant", caption: L().invariant, visible: false }
            ],
            toolbar: { items: [
                { location: "before", widget: "dxButton", locateInMenu: "auto",
                  options: { icon: "import", text: L().importFromResx, stylingMode: "text", onClick: importFromResx } },
                { name: "addRowButton", location: "after", locateInMenu: "auto" },
                { location: "after", widget: "dxButton", locateInMenu: "auto",
                  options: { icon: "refresh", hint: LG().refresh, stylingMode: "text", onClick: function () { gridInstance.refresh(); } } },
                { name: "columnChooserButton", location: "after", locateInMenu: "auto" },
                { name: "exportButton", location: "after", locateInMenu: "auto" },
                { name: "searchPanel", location: "after" }
            ] },
            onRowInserted: function () { window.AppNotify.success(LN().saved); },
            onRowUpdated: function () { window.AppNotify.success(LN().saved); },
            onRowRemoved: function () { window.AppNotify.success(LN().deleted); }
        }).dxDataGrid("instance");
    }

    window.AppPages = window.AppPages || {};
    window.AppPages.Localization = { init: init };
})(window, jQuery);
