/*
 * Logs sayfası — denetim günlüğü (audit log) görüntüleme ekranı.
 *
 * Sorumluluk:
 *   - DevExtreme dxDataGrid ile API'ye vekillenen denetim kayıtlarını sunucu tarafı
 *     sayfalama/filtreleme/sıralama ile salt okunur olarak listeler.
 *   - Kayıt ayrıntılarını (istek/yanıt gövdeleri, istisna, ilişkilendirme kimliği) gösterir.
 *   - Grid içeriğini ExcelJS + FileSaver ile .xlsx olarak dışa aktarır.
 *
 * Genel API: window.AppPages.Logs.init().
 */
(function (window, $) {
    "use strict";
    // Yerelleştirme sözlüğü kısayolları (günlükler / grid).
    var LL = function () { return window.AppL10n.logs; };
    var LG = function () { return window.AppL10n.logs.grid; };

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
        var store = new DevExpress.data.CustomStore({
            key: "id",
            load: function (loadOptions) {
                var params = $.param({ skip: loadOptions.skip || 0, take: loadOptions.take || 25 });
                return window.AppHttp.get("/logs/list?" + params);
            }
        });

        gridInstance = $("#logs-grid").dxDataGrid({
            dataSource: store,
            remoteOperations: { paging: true },
            showBorders: true,
            showColumnLines: false,
            showRowLines: true,
            rowAlternationEnabled: true,
            hoverStateEnabled: true,
            allowColumnResizing: true,
            columnResizingMode: "widget",
            columnAutoWidth: true,
            wordWrapEnabled: false,
            width: "100%",
            height: "75vh",
            scrolling: { mode: "standard", useNative: true, showScrollbar: "onScroll", columnRenderingMode: "standard" },
            paging: { pageSize: 25 },
            pager: { visible: true, allowedPageSizes: [25, 50, 100], showPageSizeSelector: true, showInfo: true, showNavigationButtons: true, displayMode: "full" },
            sorting: { mode: "multiple" },
            columnChooser: { enabled: true, mode: "select", height: 320, search: { enabled: true } },
            loadPanel: { enabled: true, text: LG().loading },
            noDataText: LG().noData,
            export: { enabled: true, formats: ["xlsx"] },
            onExporting: function (e) { exportGrid(e, LL().title); },
            columns: [
                { dataField: "occurredAt", caption: LL().occurred, dataType: "datetime", sortIndex: 0, sortOrder: "desc" },
                { dataField: "source", caption: LL().source, width: 80 },
                { dataField: "userName", caption: LL().user },
                { dataField: "ipAddress", caption: LL().ipAddress },
                { dataField: "httpMethod", caption: LL().httpMethod, width: 90 },
                { dataField: "path", caption: LL().path },
                { dataField: "statusCode", caption: LL().statusCode, dataType: "number", width: 90 },
                { dataField: "isSuccess", caption: LL().isSuccess, dataType: "boolean", width: 70 },
                { dataField: "hasException", caption: LL().hasException, dataType: "boolean", width: 70 },
                { dataField: "durationMs", caption: LL().durationMs, dataType: "number", width: 90 },
                {
                    type: "buttons", width: 90, caption: LG().actions, fixed: true, fixedPosition: "right",
                    buttons: [
                        { hint: LL().details, icon: "info", onClick: function (e) { window.location.href = "/logs/" + e.row.data.id; } }
                    ]
                }
            ],
            toolbar: { items: [
                { location: "after", widget: "dxButton", locateInMenu: "auto",
                  options: { icon: "refresh", hint: LG().refresh, stylingMode: "text", onClick: function () { gridInstance.refresh(); } } },
                { name: "columnChooserButton", location: "after", locateInMenu: "auto" },
                { name: "exportButton", location: "after", locateInMenu: "auto" }
            ] }
        }).dxDataGrid("instance");
    }

    function initDetail(id) {
        window.AppHttp.get("/logs/" + id + "/detail").then(function (response) {
            var data = response && response.data;
            if (!data) { $("#log-detail").text(LL().notFound); return; }
            $("#log-detail").dxForm({
                formData: data, readOnly: true, labelLocation: "top",
                items: [
                    { dataField: "id", label: { text: LL().id } },
                    { dataField: "occurredAt", label: { text: LL().occurred } },
                    { dataField: "source", label: { text: LL().source } },
                    { dataField: "userName", label: { text: LL().user } },
                    { dataField: "ipAddress", label: { text: LL().ipAddress } },
                    { dataField: "httpMethod", label: { text: LL().httpMethod } },
                    { dataField: "path", label: { text: LL().path } },
                    { dataField: "queryString", label: { text: LL().queryString } },
                    { dataField: "statusCode", label: { text: LL().statusCode } },
                    { dataField: "isSuccess", label: { text: LL().isSuccess } },
                    { dataField: "hasException", label: { text: LL().hasException } },
                    { dataField: "exceptionType", label: { text: LL().exceptionType } },
                    { dataField: "exceptionMessage", label: { text: LL().exceptionMessage } },
                    { dataField: "correlationId", label: { text: LL().correlationId } },
                    { dataField: "durationMs", label: { text: LL().durationMs } },
                    { dataField: "requestBody", label: { text: LL().requestBody }, editorType: "dxTextArea", editorOptions: { height: 160 } },
                    { dataField: "responseBody", label: { text: LL().responseBody }, editorType: "dxTextArea", editorOptions: { height: 220 } }
                ]
            });
        });
    }

    window.AppPages = window.AppPages || {};
    window.AppPages.Logs = { init: init, initDetail: initDetail };
})(window, jQuery);

