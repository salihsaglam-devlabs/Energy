/*
 * Workflow / Approval — process screen (approval inbox).
 * Lists the current user's pending approvals and dispatches approve/reject/cancel
 * actions (with an optional note) to the standard process API route. Read+act
 * screen; transaction-safe business logic lives in the backend workflow service.
 */
(function (window, $) {
    "use strict";

    function init(base, gridId, labels) {
        var LP = function () { return (window.AppL10n && window.AppL10n.processes) || {}; };
        var notify = window.AppNotify || { success: function () {}, error: function () {} };

        var store = new DevExpress.data.CustomStore({
            key: "id",
            loadMode: "raw",
            load: function () {
                return window.AppHttp.get(base + "/my-pending").then(function (res) {
                    return (res && res.data) || [];
                });
            }
        });

        var grid;

        function act(verb, id) {
            var note = window.prompt(labels.notePrompt, "");
            if (note === null) { return; }
            window.AppHttp.post(base + "/" + id + "/" + verb, { note: note })
                .then(function (res) {
                    if (res && res.isSuccess) {
                        notify.success((res.message) || LP().genericSuccess || "OK");
                    } else {
                        notify.error((res && res.message) || LP().genericError || "Error");
                    }
                    grid.refresh();
                })
                .catch(function () { notify.error(LP().genericError || "Error"); });
        }

        grid = $("#" + gridId).dxDataGrid({
            dataSource: store,
            showBorders: true,
            headerFilter: { visible: true },
            filterRow: { visible: true },
            rowAlternationEnabled: true,
            columnAutoWidth: true,
            columnHidingEnabled: true,
            width: "100%",
            height: "70vh",
            paging: { pageSize: 25 },
            pager: { visible: true, showInfo: true },
            columns: [
                { dataField: "relatedModule" },
                { dataField: "relatedEntityType" },
                { dataField: "relatedEntityId" },
                { dataField: "status" },
                { dataField: "currentStepNo", dataType: "number" },
                { dataField: "createdAt", dataType: "datetime" },
                {
                    type: "buttons",
                    width: 220,
                    buttons: [
                        { hint: labels.approve, icon: "check", onClick: function (e) { act("approve", e.row.data.id); } },
                        { hint: labels.reject, icon: "close", onClick: function (e) { act("reject", e.row.data.id); } },
                        { hint: labels.cancel, icon: "trash", onClick: function (e) { act("cancel", e.row.data.id); } }
                    ]
                }
            ]
        }).dxDataGrid("instance");
    }

    window.AppProcesses = window.AppProcesses || {};
    window.AppProcesses.WorkflowApproval = { init: init };
})(window, jQuery);

