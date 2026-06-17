/*
 * Finance / PaymentAllocation — master-detail process screen.
 * Master: pick a payment (lookup). Detail: an editable allocation grid (target
 * payable lookup + amount). Submitting posts all lines + financial transactions
 * in one server-side transaction.
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
        var LP = function () { return (window.AppScreenL10n && window.AppScreenL10n.process) || {}; };
        var notify = window.AppNotify || { success: function () {}, error: function () {} };
        var state = { paymentId: null };
        var lines = [];

        // Master: payment selector.
        $("#" + opts.masterId).dxForm({
            formData: state,
            labelLocation: "top",
            colCount: 2,
            items: [
                {
                    dataField: "paymentId",
                    editorType: "dxSelectBox",
                    validationRules: [{ type: "required" }],
                    editorOptions: {
                        dataSource: lookupStore("/finance/payments/lookup"),
                        valueExpr: "id",
                        displayExpr: "displayName",
                        searchEnabled: true,
                        onValueChanged: function (e) { state.paymentId = e.value || null; }
                    }
                }
            ]
        });

        // Detail: editable allocation lines grid.
        var grid = $("#" + opts.gridId).dxDataGrid({
            dataSource: lines,
            keyExpr: "_key",
            showBorders: true,
            columnAutoWidth: true,
            width: "100%",
            editing: {
                mode: "cell",
                allowAdding: true,
                allowUpdating: true,
                allowDeleting: true,
                useIcons: true
            },
            onInitNewRow: function (e) { e.data._key = Date.now() + Math.random(); },
            columns: [
                {
                    dataField: "targetId",
                    validationRules: [{ type: "required" }],
                    lookup: {
                        dataSource: lookupStore("/finance/payables/lookup"),
                        valueExpr: "id",
                        displayExpr: "displayName"
                    }
                },
                {
                    dataField: "amount",
                    dataType: "number",
                    format: { type: "fixedPoint", precision: 2 },
                    validationRules: [{ type: "required" }, { type: "range", min: 0.000001 }]
                }
            ]
        }).dxDataGrid("instance");

        // Submit.
        $("#" + opts.submitId).dxButton({
            text: opts.submit,
            type: "default",
            stylingMode: "contained",
            onClick: function () {
                if (!state.paymentId) { notify.error(opts.pickPayment); return; }
                grid.saveEditData().then(function () {
                    var payload = {
                        paymentId: state.paymentId,
                        lines: (grid.getDataSource().items() || []).map(function (l) {
                            return { targetId: l.targetId, amount: l.amount };
                        })
                    };
                    if (!payload.lines.length) { notify.error(opts.pickPayment); return; }
                    window.AppHttp.post(base, payload)
                        .then(function (r) {
                            if (r && r.isSuccess) {
                                notify.success(opts.success);
                                $("#" + opts.resultId).text(
                                    (LP().resultLines || "Lines") + ": " + (r.data ? r.data.allocatedLineCount : "") + " | " + (LP().resultTotal || "Total") + ": " + (r.data ? r.data.totalAllocated : ""));
                                lines.splice(0, lines.length);
                                grid.refresh();
                            } else {
                                notify.error((r && r.message) || LP().genericError || "Error");
                            }
                        })
                        .catch(function () { notify.error(LP().genericError || "Error"); });
                });
            }
        });
    }

    window.AppProcesses = window.AppProcesses || {};
    window.AppProcesses.FinancePaymentAllocation = { init: init };
})(window, jQuery);

