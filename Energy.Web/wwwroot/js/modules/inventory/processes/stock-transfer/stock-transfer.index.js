/*
 * Inventory / StockTransfer — process screen (form).
 * Source warehouse FIFO-out + target warehouse in, in a single server-side
 * transaction. FK fields are lookups (no IDs shown).
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

    function selectBox(url, required) {
        return {
            editorType: "dxSelectBox",
            validationRules: required ? [{ type: "required" }] : [],
            editorOptions: {
                dataSource: lookupStore(url),
                valueExpr: "id",
                displayExpr: "displayName",
                searchEnabled: true,
                showClearButton: !required
            }
        };
    }

    function init(base, opts) {
        var notify = window.AppNotify || { success: function () {}, error: function () {} };
        var data = {};

        var form = $("#" + opts.formId).dxForm({
            formData: data,
            labelLocation: "top",
            colCount: 2,
            items: [
                $.extend({ dataField: "sourceWarehouseId", label: { text: "Source Warehouse" } }, selectBox("/inventory/warehouses/lookup", true)),
                $.extend({ dataField: "targetWarehouseId", label: { text: "Target Warehouse" } }, selectBox("/inventory/warehouses/lookup", true)),
                $.extend({ dataField: "materialId", label: { text: "Material" } }, selectBox("/catalog/materials/lookup", true)),
                $.extend({ dataField: "unitOfMeasureId", label: { text: "Unit" } }, selectBox("/core/units-of-measure/lookup", true)),
                { dataField: "quantity", label: { text: "Quantity" }, editorType: "dxNumberBox", validationRules: [{ type: "required" }, { type: "range", min: 0.000001 }], editorOptions: { showSpinButtons: true, min: 0 } },
                { dataField: "note", label: { text: "Note" }, editorType: "dxTextArea", colSpan: 2 },
                {
                    itemType: "button",
                    horizontalAlignment: "left",
                    buttonOptions: {
                        text: opts.submit, type: "default", stylingMode: "contained",
                        onClick: function () {
                            var res = form.validate();
                            if (!res.isValid) { return; }
                            window.AppHttp.post(base, data)
                                .then(function (r) {
                                    if (r && r.isSuccess) {
                                        notify.success(opts.success);
                                        $("#" + opts.resultId).text("Total cost: " + (r.data ? r.data.totalCost : "") + " | Allocations: " + (r.data ? r.data.allocationCount : ""));
                                        form.resetValues();
                                    } else {
                                        notify.error((r && r.message) || "Error");
                                    }
                                })
                                .catch(function () { notify.error("Error"); });
                        }
                    }
                }
            ]
        }).dxForm("instance");
    }

    window.AppProcesses = window.AppProcesses || {};
    window.AppProcesses.InventoryStockTransfer = { init: init };
})(window, jQuery);

