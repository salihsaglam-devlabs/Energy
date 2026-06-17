/*
 * Finance / ProgressPaymentPosting — process screen (form, Contracts flow).
 * Select an approved progress payment and post it to a receivable/payable
 * financial transaction (server-side, transaction-safe). FK is a lookup.
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
        var data = {};

        var form = $("#" + opts.formId).dxForm({
            formData: data,
            labelLocation: "top",
            colCount: 1,
            items: [
                {
                    dataField: "progressPaymentId",
                    editorType: "dxSelectBox",
                    validationRules: [{ type: "required" }],
                    editorOptions: {
                        dataSource: lookupStore("/progress-payments/progress-payments/lookup"),
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
                                        $("#" + opts.resultId).text((LP().resultTransaction || "Transaction") + ": " + (r.data ? r.data.financialTransactionId : ""));
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
    window.AppProcesses.FinanceProgressPaymentPosting = { init: init };
})(window, jQuery);

