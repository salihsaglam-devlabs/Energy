/*
 * Inventory / StockBalanceReport — read-only DevExtreme report screen.
 * Filters (date range) -> server-side query. Export via server CSV endpoint.
 */
(function (window, $) {
    "use strict";

    function lookupStore(url) {
        return new DevExpress.data.CustomStore({
            key: "id",
            loadMode: "raw",
            load: function () { return window.AppHttp.get(url); }
        });
    }

    function init(base, gridId, filtersId, opts) {
        var LR = function () { return (window.AppScreenL10n && window.AppScreenL10n.report) || {}; };
        var state = { startDate: null, endDate: null, status: null };

        function buildQuery(loadOptions) {
            var params = {
                skip: (loadOptions && loadOptions.skip) || 0,
                take: (loadOptions && loadOptions.take) || 50
            };
            if (state.startDate) { params.startDate = state.startDate; }
            if (state.endDate) { params.endDate = state.endDate; }
            if (opts && opts.hasStatus && state.status) { params.status = state.status; }
            return $.param(params);
        }

        var store = new DevExpress.data.CustomStore({
            key: "id",
            load: function (loadOptions) {
                return window.AppHttp.get(base + "/data?" + buildQuery(loadOptions));
            }
        });

        var grid = $("#" + gridId).dxDataGrid({
            dataSource: store,
            remoteOperations: { paging: true },
            showBorders: true,
            headerFilter: { visible: true },
            filterRow: { visible: true },
            rowAlternationEnabled: true,
            allowColumnResizing: true,
            columnAutoWidth: true,
            columnHidingEnabled: true,
            width: "100%",
            height: "70vh",
            paging: { pageSize: 50 },
            pager: { visible: true, showPageSizeSelector: true, allowedPageSizes: [25, 50, 100], showInfo: true },
            columns: [
            { dataField: "warehouseId" },
            { dataField: "materialId" },
            { dataField: "quantity", dataType: "number", format: { type: "fixedPoint", precision: 2 } },
            { dataField: "reservedQuantity", dataType: "number", format: { type: "fixedPoint", precision: 2 } },
            { dataField: "totalCost", dataType: "number", format: { type: "fixedPoint", precision: 2 } },
            { dataField: "lastRecalculatedAt", dataType: "date" }
            ]
        }).dxDataGrid("instance");

        // Filter toolbar (date range + optional status + export).
        var $f = $("#" + filtersId);
        var $start = $("<div class=\"energy-report__filter\"></div>").appendTo($f);
        var $end = $("<div class=\"energy-report__filter\"></div>").appendTo($f);
        $start.dxDateBox({ type: "date", placeholder: (LR().startDate || "Start"), onValueChanged: function (e) { state.startDate = e.value ? e.value.toISOString() : null; grid.refresh(); } });
        $end.dxDateBox({ type: "date", placeholder: (LR().endDate || "End"), onValueChanged: function (e) { state.endDate = e.value ? e.value.toISOString() : null; grid.refresh(); } });
        if (opts && opts.hasStatus) {
            var $status = $("<div class=\"energy-report__filter\"></div>").appendTo($f);
            $status.dxTextBox({ placeholder: (LR().status || "Status"), onValueChanged: function (e) { state.status = e.value || null; grid.refresh(); } });
        }
        var $export = $("<div class=\"energy-report__filter\"></div>").appendTo($f);
        $export.dxButton({
            icon: "export", text: (LR().export || "Export"),
            onClick: function () {
                var q = buildQuery({ skip: 0, take: 100000 });
                window.open(base + "/data?" + q, "_blank");
            }
        });
    }

    window.AppReports = window.AppReports || {};
    window.AppReports.InventoryStockBalanceReport = { init: init };
})(window, jQuery);
