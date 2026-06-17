/*
 * ApiEndpoints sayfası — API uç noktası erişim yönetim ekranı.
 *
 * Sorumluluk:
 *   - DevExtreme dxDataGrid ile keşfedilen API uç noktalarını listeler, düzenler ve
 *     siler; her uç noktanın etkin/pasif durumunu ve gerektirdiği yetkiyi yönetir.
 *   - Uç nokta-yetki eşlemesi için yetki kataloğunu arama (lookup) olarak yükler.
 *
 * Genel API: window.AppPages.ApiEndpoints.init().
 */
(function (window, $) {
    "use strict";
    // Yerelleştirme sözlüğü kısayolları (API uç noktaları / grid / bildirimler).
    var LA = function () { return window.AppL10n.apiEndpoints; };
    var LG = function () { return window.AppL10n.apiEndpoints.grid; };
    var LN = function () { return window.AppL10n.apiEndpoints.notifications; };

    // Grid örneği ve uç nokta-yetki eşlemesi için arama verisi.
    var gridInstance, permissions = [];

    // Sayfayı başlatır: yetkileri yükler, ardından grid'i kurar.
    function init() {
        loadPermissions().then(buildGrid);
    }

    function loadPermissions() {
        return window.AppHttp.get("/api-endpoints/permissions-lookup")
            .then(function (data) { permissions = data || []; })
            .catch(function () { permissions = []; });
    }

    function buildStore() {
        return new DevExpress.data.CustomStore({
            key: "id",
            load: function (loadOptions) {
                var params = $.param({
                    skip: loadOptions.skip || 0,
                    take: loadOptions.take || 20,
                    searchValue: loadOptions.searchValue || ""
                });
                return window.AppHttp.get("/api-endpoints/list?" + params);
            },
            insert: function (values) {
                return window.AppHttp.post("/api-endpoints", values);
            },
            update: function (key, values) {
                return window.AppHttp.get("/api-endpoints/list?" + $.param({ skip: 0, take: 100 })).then(function (page) {
                    var current = (page.data || []).find(function (r) { return r.id === key; }) || {};
                    return window.AppHttp.put("/api-endpoints/" + key, Object.assign({}, current, values));
                });
            },
            remove: function (key) {
                return window.AppHttp.del("/api-endpoints/" + key);
            }
        });
    }

    function buildGrid() {
        gridInstance = $("#api-endpoints-grid").dxDataGrid({
            dataSource: buildStore(),
            remoteOperations: { paging: true },
            showBorders: true,
            headerFilter: { visible: true },
            filterRow: { visible: true },
            groupPanel: { visible: true },
            grouping: { autoExpandAll: true },
            showColumnLines: false,
            showRowLines: true,
            rowAlternationEnabled: true,
            hoverStateEnabled: true,
            allowColumnResizing: true,
            columnResizingMode: "widget",
            columnAutoWidth: true,
            columnHidingEnabled: false,
            wordWrapEnabled: false,
            width: "100%",
            height: "75vh",
            scrolling: { mode: "standard", useNative: true, showScrollbar: "onScroll", columnRenderingMode: "standard" },
            paging: { pageSize: 20 },
            pager: { visible: true, allowedPageSizes: [10, 20, 50], showPageSizeSelector: true, showInfo: true, showNavigationButtons: true, displayMode: "full" },
            searchPanel: { visible: true, placeholder: LG().search, width: 240 },
            sorting: { mode: "multiple" },
            columnChooser: { enabled: true, mode: "select", height: 320, search: { enabled: true } },
            loadPanel: { enabled: true, text: LG().loading },
            noDataText: LG().noData,
            editing: {
                mode: "popup",
                allowAdding: true,
                allowUpdating: true,
                allowDeleting: true,
                useIcons: true,
                popup: {
                    title: LA().popupTitle, showTitle: true,
                    width: 640, height: "auto",
                    maxWidth: "min(92vw, 960px)", maxHeight: "min(88vh, 820px)",
                    dragEnabled: true, resizeEnabled: true, hideOnOutsideClick: true,
                    showCloseButton: true, wrapperAttr: { class: "energy-popup" }
                },
                form: {
                    labelLocation: "top", colCount: 2,
                    items: [
                        { dataField: "name", label: { text: LA().name }, isRequired: true },
                        { dataField: "httpMethod", label: { text: LA().httpMethod }, editorType: "dxSelectBox",
                          editorOptions: { items: ["GET", "POST", "PUT", "PATCH", "DELETE"] }, isRequired: true },
                        { dataField: "path", label: { text: LA().path }, colSpan: 2, isRequired: true },
                        { dataField: "description", label: { text: LA().description }, colSpan: 2, editorType: "dxTextArea", editorOptions: { height: 80 } },
                        { dataField: "requiredPermissionCode", label: { text: LA().requiredPermission }, colSpan: 2, editorType: "dxSelectBox",
                          editorOptions: { items: permissions, valueExpr: "code", displayExpr: "name", searchEnabled: true, showClearButton: true } },
                        { dataField: "isActive", label: { text: LA().isActive }, editorType: "dxCheckBox" }
                    ]
                }
            },
            columns: [
                { dataField: "httpMethod", caption: LA().httpMethod, width: 90 },
                { dataField: "name", caption: LA().name },
                { dataField: "path", caption: LA().path },
                { dataField: "requiredPermissionCode", caption: LA().requiredPermission },
                { dataField: "isActive", caption: LA().isActive, dataType: "boolean", width: 90 }
            ],
            toolbar: { items: [
                { name: "addRowButton", location: "after" },
                { location: "after", widget: "dxButton",
                  options: { icon: "refresh", hint: LG().refresh, stylingMode: "text", onClick: function () { gridInstance.refresh(); } } },
                { name: "columnChooserButton", location: "after" },
                { name: "searchPanel", location: "after" }
            ]},
            onRowInserted: function () { window.AppNotify.success(LN().saved); },
            onRowUpdated: function () { window.AppNotify.success(LN().saved); },
            onRowRemoved: function () { window.AppNotify.success(LN().deleted); }
        }).dxDataGrid("instance");
    }

    window.AppPages = window.AppPages || {};
    window.AppPages.ApiEndpoints = { init: init };
})(window, jQuery);

