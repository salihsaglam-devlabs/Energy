/*
 * Procurement / GoodsReceipt — process screen (form).
 * Select an approved purchase receipt and convert it into a stock-in document
 * (StockDocument + StockLot + StockTransaction + StockBalance) in one server-side
 * transaction. The receipt FK is a lookup (no IDs shown).
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

    function init(base, opts) {
        var LP = function () { return (window.AppL10n && window.AppL10n.processes) || {}; };
        var notify = window.AppNotify || { success: function () {}, error: function () {} };
        var data = {};

        var form = $("#" + opts.formId).dxForm({
            formData: data,
            labelLocation: "top",
            colCount: 1,
            items: [
                {
                    dataField: "purchaseReceiptId",
                    editorType: "dxSelectBox",
                    validationRules: [{ type: "required" }],
                    editorOptions: {
                        dataSource: lookupStore("/procurement/purchase-receipts/lookup"),
                        valueExpr: "id",
                        displayExpr: "displayName",
                        searchEnabled: true
                    }
                },
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
                                        $("#" + opts.resultId).text(opts.success);
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
    window.AppProcesses.ProcurementGoodsReceipt = { init: init };
})(window, jQuery);

