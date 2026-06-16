/*
 * Documents / Files — document file & version management screen.
 * Select a document, upload a new version (multipart -> Web controller -> API),
 * and view/download the version history. The document FK is a lookup (no IDs).
 * File handling runs through the service abstraction server-side.
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
        var notify = window.AppNotify || { success: function () {}, error: function () {} };
        var labels = opts.labels;
        var selectedDocumentId = null;
        var pendingFile = null;
        var grid;

        function refreshVersions() {
            if (grid) { grid.refresh(); }
        }

        $("#" + opts.selectId).dxSelectBox({
            dataSource: lookupStore("/documents/documents/lookup"),
            valueExpr: "id",
            displayExpr: "displayName",
            searchEnabled: true,
            placeholder: labels.selectDocument,
            onValueChanged: function (e) {
                selectedDocumentId = e.value || null;
                refreshVersions();
            }
        });

        // Hidden native file input driven by a dxButton.
        var $file = $('<input type="file" style="display:none" />').appendTo("#" + opts.uploadId);
        $file.on("change", function () {
            pendingFile = this.files && this.files[0] ? this.files[0] : null;
            if (pendingFile && selectedDocumentId) {
                doUpload();
            } else if (!selectedDocumentId) {
                notify.error(labels.pickFirst);
            }
        });

        $('<div></div>').appendTo("#" + opts.uploadId).dxButton({
            icon: "upload",
            text: labels.upload,
            type: "default",
            onClick: function () {
                if (!selectedDocumentId) { notify.error(labels.pickFirst); return; }
                $file.trigger("click");
            }
        });

        function doUpload() {
            var fd = new FormData();
            fd.append("documentId", selectedDocumentId);
            fd.append("file", pendingFile);
            $.ajax({
                url: base + "/upload",
                type: "POST",
                data: fd,
                processData: false,
                contentType: false
            }).done(function (res) {
                if (res && res.isSuccess) {
                    notify.success(labels.uploaded);
                    refreshVersions();
                } else {
                    notify.error((res && res.message) || "Error");
                }
                $file.val("");
                pendingFile = null;
            }).fail(function () {
                notify.error("Error");
                $file.val("");
                pendingFile = null;
            });
        }

        var store = new DevExpress.data.CustomStore({
            key: "id",
            loadMode: "raw",
            load: function () {
                if (!selectedDocumentId) { return []; }
                return window.AppHttp.get(base + "/versions/" + selectedDocumentId);
            }
        });

        grid = $("#" + opts.gridId).dxDataGrid({
            dataSource: store,
            showBorders: true,
            columnAutoWidth: true,
            columnHidingEnabled: true,
            width: "100%",
            height: "55vh",
            columns: [
                { dataField: "versionNo", caption: "Version", dataType: "number", width: 90 },
                { dataField: "fileName", caption: "File Name" },
                { dataField: "fileSize", caption: "Size (bytes)", dataType: "number" },
                { dataField: "contentType", caption: "Type" },
                { dataField: "uploadedAt", caption: "Uploaded", dataType: "datetime" },
                {
                    type: "buttons",
                    width: 130,
                    buttons: [
                        {
                            hint: labels.download, icon: "download",
                            onClick: function (e) {
                                window.open(base + "/download/" + e.row.data.id, "_blank");
                            }
                        }
                    ]
                }
            ]
        }).dxDataGrid("instance");
    }

    window.AppDocuments = window.AppDocuments || {};
    window.AppDocuments.Files = { init: init };
})(window, jQuery);

