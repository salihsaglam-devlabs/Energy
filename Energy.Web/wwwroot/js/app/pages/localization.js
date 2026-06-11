(function (window, $) {
    "use strict";
    var L = function () { return window.AppL10n.localization; };
    var LG = function () { return window.AppL10n.grid; };
    var LN = function () { return window.AppL10n.notifications; };
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
            editing: {
                mode: "popup", allowAdding: true, allowUpdating: true, allowDeleting: true,
                useIcons: true,
                popup: {
                    title: L().editTitle, showTitle: true,
                    width: 640, height: "auto",
                    maxWidth: "min(92vw, 960px)", maxHeight: "min(88vh, 820px)",
                    dragEnabled: true, resizeEnabled: true, hideOnOutsideClick: true,
                    showCloseButton: true, wrapperAttr: { class: "energy-popup" }
                },
                form: { labelLocation: "top", colCount: 1, items: [
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
