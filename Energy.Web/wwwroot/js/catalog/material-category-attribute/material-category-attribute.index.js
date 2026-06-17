/*
 * Catalog / MaterialCategoryAttribute — entity-specific DevExtreme grid screen.
 * Shared helpers used: AppHttp, AppNotify, AppAuth. Per-screen texts: window.AppScreenL10n (this screen only). Screen-specific
 * grid columns / FK lookups / CRUD wiring live ONLY in this file.
 */
(function (window, $) {
    "use strict";

    var LG = function () { return (window.AppScreenL10n && window.AppScreenL10n.grid) || {}; };
    var LN = function () { return (window.AppScreenL10n && window.AppScreenL10n.notifications) || {}; };

    var HIDDEN_FIELDS = ["id", "createdAt", "createdBy", "updatedAt", "updatedBy",
        "isDeleted", "deletedAt", "deletedBy"];

    // FK alanı -> ilişkili entity lookup endpoint'i. Kullanıcıya ID değil ad gösterilir.
    var LOOKUPS = {
        "materialAttributeDefinitionId": "/catalog/material-attribute-definitions/lookup",
        "materialCategoryId": "/catalog/material-categories/lookup"
    };

    function lookupStore(url) {
        return new DevExpress.data.CustomStore({
            key: "id",
            loadMode: "raw",
            load: function () { return window.AppHttp.get(url); }
        });
    }

    function init(base, gridId, permModule) {
        var auth = window.AppAuth || { can: function () { return true; } };
        var store = new DevExpress.data.CustomStore({
            key: "id",
            load: function (loadOptions) {
                var params = $.param({
                    skip: loadOptions.skip || 0,
                    take: loadOptions.take || 20,
                    searchValue: loadOptions.searchValue || ""
                });
                return window.AppHttp.get(base + "/list?" + params);
            },
            insert: function (values) { return window.AppHttp.post(base, values); },
            update: function (key, values) { return window.AppHttp.put(base + "/" + key, values); },
            remove: function (key) { return window.AppHttp.del(base + "/" + key); }
        });

        $("#" + gridId).dxDataGrid({
            dataSource: store,
            remoteOperations: { paging: true },
            showBorders: true,
            headerFilter: { visible: true },
            filterRow: { visible: true },
            rowAlternationEnabled: true,
            hoverStateEnabled: true,
            allowColumnResizing: true,
            columnAutoWidth: true,
            columnHidingEnabled: true,
            width: "100%",
            height: "75vh",
            paging: { pageSize: 20 },
            pager: { visible: true, allowedPageSizes: [10, 20, 50], showPageSizeSelector: true, showInfo: true },
            searchPanel: { visible: true, placeholder: (LG().search || "Ara..."), width: 240 },
            sorting: { mode: "multiple" },
            columnChooser: { enabled: true, mode: "select" },
            loadPanel: { enabled: true, text: (LG().loading || "Yükleniyor...") },
            noDataText: (LG().noData || "Kayıt yok"),
            export: { enabled: true, formats: ["xlsx"] },
            editing: {
                mode: "popup",
                allowAdding: auth.can(permModule + ".Create"),
                allowUpdating: auth.can(permModule + ".Update"),
                allowDeleting: auth.can(permModule + ".Delete"),
                useIcons: true,
                popup: { showTitle: true, width: "min(92vw, 720px)", height: "auto" }
            },
            onRowUpdating: function (e) { e.newData = $.extend({}, e.oldData, e.newData); },
            customizeColumns: function (columns) {
                columns.forEach(function (col) {
                    if (HIDDEN_FIELDS.indexOf(col.dataField) !== -1) {
                        col.visible = false;
                        col.formItem = { visible: false };
                        col.allowEditing = false;
                    } else if (LOOKUPS[col.dataField]) {
                        // FK kolonu: ID yerine ilişkili kaydın görünen adını göster.
                        col.lookup = {
                            dataSource: lookupStore(LOOKUPS[col.dataField]),
                            valueExpr: "id",
                            displayExpr: "displayName"
                        };
                    }
                });
            },
            onRowInserted: function () { window.AppNotify && window.AppNotify.success(LN().saved || "Kaydedildi"); },
            onRowUpdated: function () { window.AppNotify && window.AppNotify.success(LN().saved || "Kaydedildi"); },
            onRowRemoved: function () { window.AppNotify && window.AppNotify.success(LN().deleted || "Silindi"); },
            onDataErrorOccurred: function (e) {
                if (window.AppNotify && window.AppNotify.fromHttpError) { window.AppNotify.fromHttpError(e.error); }
            }
        });
    }

    window.AppPages = window.AppPages || {};
    window.AppPages.CatalogMaterialCategoryAttribute = { init: init };

})(window, window.jQuery);
