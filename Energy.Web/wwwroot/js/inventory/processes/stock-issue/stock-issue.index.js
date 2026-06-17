/*
 * Inventory / StockIssue — process screen (form).
 * Collects FIFO stock-out inputs (warehouse, material, unit, quantity, project,
 * note) and posts to the standard process route. FK fields are lookups (no IDs
 * shown). Transaction-safe FIFO costing happens server-side.
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
        var LP = function () { return (window.AppL10n && window.AppL10n.processes) || {}; };
        var notify = window.AppNotify || { success: function () {}, error: function () {} };
        var data = {};

        var form = $("#" + opts.formId).dxForm({
            formData: data,
            labelLocation: "top",
            colCount: 2,
            items: [
                $.extend({ dataField: "warehouseId" }, selectBox("/inventory/warehouses/lookup", true)),
                $.extend({ dataField: "materialId" }, selectBox("/catalog/materials/lookup", true)),
                $.extend({ dataField: "unitOfMeasureId" }, selectBox("/core/units-of-measure/lookup", true)),
                { dataField: "quantity", editorType: "dxNumberBox", validationRules: [{ type: "required" }, { type: "range", min: 0.000001 }], editorOptions: { showSpinButtons: true, min: 0 } },
                $.extend({ dataField: "projectId" }, selectBox("/projects/projects/lookup", false)),
                { dataField: "note", editorType: "dxTextArea", colSpan: 2 },
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
                                        $("#" + opts.resultId).text((LP().resultTotalCost || "Total cost") + ": " + (r.data ? r.data.totalCost : "") + " | " + (LP().resultAllocations || "Allocations") + ": " + (r.data ? r.data.allocationCount : ""));
                                        form.resetValues();
                                    } else {
                                        notify.error((r && r.message) || LP().genericError || "Error");
                                    }
                                })
                                .catch(function () { notify.error(LP().genericError || "Error"); });
                        }
                    }
                }
            ]
        }).dxForm("instance");
    }

    window.AppProcesses = window.AppProcesses || {};
    window.AppProcesses.InventoryStockIssue = { init: init };
})(window, jQuery);

