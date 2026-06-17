/*
 * Permissions sayfası — yetki kataloğu görüntüleme ekranı.
 *
 * Sorumluluk:
 *   - DevExtreme dxDataGrid ile merkezi, derleme zamanına ait salt okunur yetki
 *     kataloğunu (modül/eylem/kod/ad) istemci tarafında listeler.
 *   - Grid içeriğini ExcelJS + FileSaver ile .xlsx olarak dışa aktarır.
 *
 * Genel API: window.AppPages.Permissions.init().
 */
(function (window, $) {
    "use strict";
    // Yerelleştirme sözlüğü kısayolları (yetkiler / grid / bildirimler).
    var L = function () { return window.AppL10n.permissions; };
    var LG = function () { return window.AppL10n.permissions.grid; };
    var LN = function () { return window.AppL10n.permissions.notifications; };
    // Grid örneği.
    var gridInstance;

    // Grid'i ExcelJS ile bir .xlsx çalışma kitabına aktarır ve indirilmesini sağlar.
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
        // Yetki kataloğu derleme zamanına ait, salt okunur bir listedir. Uç nokta tüm
        // kümeyi düz bir dizi olarak döndürür; bu yüzden grid istemci tarafında çalışır.
        var store = new DevExpress.data.CustomStore({
            key: "code",
            loadMode: "raw",
            load: function () {
                return window.AppHttp.get("/permissions/list");
            }
        });

        gridInstance = $("#permissions-grid").dxDataGrid({
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
                { dataField: "module", caption: L().module, groupIndex: 0 },
                { dataField: "code", caption: L().code },
                { dataField: "name", caption: L().name },
                { dataField: "action", caption: L().action },
                { dataField: "roleCount", caption: L().roleCount, dataType: "number", width: 110, alignment: "center" },
                { dataField: "menuCount", caption: L().menuCount, dataType: "number", width: 110, alignment: "center" },
                { dataField: "endpointCount", caption: L().endpointCount, dataType: "number", width: 130, alignment: "center" }
            ],
            toolbar: { items: [
                { location: "after", widget: "dxButton", locateInMenu: "auto",
                  options: { icon: "refresh", hint: LG().refresh, stylingMode: "text", onClick: function () { gridInstance.refresh(); } } },
                { name: "columnChooserButton", location: "after", locateInMenu: "auto" },
                { name: "exportButton", location: "after", locateInMenu: "auto" },
                { name: "searchPanel", location: "after" }
            ] }
        }).dxDataGrid("instance");
    }


    window.AppPages = window.AppPages || {};
    window.AppPages.Permissions = { init: init };
})(window, jQuery);
