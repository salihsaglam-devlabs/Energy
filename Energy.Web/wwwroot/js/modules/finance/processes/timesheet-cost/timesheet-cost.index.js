/*
 * Finance / TimesheetCost — process screen (form, HR Cost flow).
 * Select an approved timesheet + currency and post the labour cost to a financial
 * transaction (server-side, transaction-safe). FK fields are lookups (no IDs).
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

    function selectBox(url) {
        return {
            editorType: "dxSelectBox",
            validationRules: [{ type: "required" }],
            editorOptions: {
                dataSource: lookupStore(url),
                valueExpr: "id",
                displayExpr: "displayName",
                searchEnabled: true
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
                $.extend({ dataField: "timesheetId", label: { text: "Timesheet" } }, selectBox("/hr/timesheets/lookup")),
                $.extend({ dataField: "currencyId", label: { text: "Currency" } }, selectBox("/core/currencies/lookup")),
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
                                        $("#" + opts.resultId).text("Transaction: " + (r.data ? r.data.financialTransactionId : ""));
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
    window.AppProcesses.FinanceTimesheetCost = { init: init };
})(window, jQuery);

