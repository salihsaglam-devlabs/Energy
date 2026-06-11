(function (window, $) {
    "use strict";

    var L = function () { return window.AppL10n.users; };
    var LG = function () { return window.AppL10n.grid; };
    var LN = function () { return window.AppL10n.notifications; };

    var gridInstance, rolesLookup = [];

    function exportGrid(e, fileName) {
        if (typeof ExcelJS === "undefined" || typeof saveAs === "undefined" ||
            !DevExpress.excelExporter || !DevExpress.excelExporter.exportDataGrid) { return; }
        var workbook = new ExcelJS.Workbook();
        var worksheet = workbook.addWorksheet("Data");
        DevExpress.excelExporter.exportDataGrid({ component: e.component, worksheet: worksheet, autoFilterEnabled: true })
            .then(function () {
                workbook.xlsx.writeBuffer().then(function (buffer) {
                    saveAs(new Blob([buffer], { type: "application/octet-stream" }), (fileName || "export") + ".xlsx");
                });
            });
        e.cancel = true;
    }

    function init() {
        loadRolesLookup().then(function () { buildGrid(); });
    }

    function loadRolesLookup() {
        return window.AppHttp.get("/users/roles-lookup").then(function (data) {
            rolesLookup = data || [];
        }).catch(function () { rolesLookup = []; });
    }

    function buildGrid() {
        var auth = window.AppAuth || { can: function () { return true; } };
        var canCreate = auth.can("User.Create");
        var canUpdate = auth.can("User.Update");
        var canDelete = auth.can("User.Delete");

        var rowButtons = [];
        if (canUpdate) {
            rowButtons.push({ hint: LG().edit, icon: "edit", onClick: function (e) { openEdit(e.row.data.id); } });
            rowButtons.push({ hint: L().manageRoles, icon: "group", onClick: function (e) { openRoles(e.row.data); } });
            rowButtons.push({ hint: L().changePassword, icon: "key", onClick: function (e) { openPassword(e.row.data); } });
        }
        if (canDelete) {
            rowButtons.push({ hint: LG().delete, icon: "trash", onClick: function (e) { confirmDelete(e.row.data); } });
        }

        var toolbarItems = [];
        if (canCreate) {
            toolbarItems.push({ location: "after", widget: "dxButton", locateInMenu: "auto",
                options: { icon: "add", text: LG().add, type: "default", stylingMode: "contained", onClick: openCreate } });
        }
        toolbarItems.push(
            { location: "after", widget: "dxButton", locateInMenu: "auto",
              options: { icon: "refresh", hint: LG().refresh, stylingMode: "text", onClick: function () { gridInstance.refresh(); } } },
            { name: "columnChooserButton", location: "after", locateInMenu: "auto" },
            { name: "exportButton", location: "after", locateInMenu: "auto" },
            { name: "searchPanel", location: "after" }
        );

        var store = new DevExpress.data.CustomStore({
            key: "id",
            load: function (loadOptions) {
                var params = $.param({
                    skip: loadOptions.skip || 0,
                    take: loadOptions.take || 20,
                    sort: loadOptions.sort ? JSON.stringify(loadOptions.sort) : "",
                    searchValue: loadOptions.searchValue || ""
                });
                return window.AppHttp.get("/users/list?" + params).then(function (resp) { return resp; });
            }
        });

        gridInstance = $("#users-grid").dxDataGrid({
            dataSource: store,
            remoteOperations: { paging: true, sorting: true, filtering: false },
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
            repaintChangesOnly: true,
            paging: { pageSize: 20 },
            pager: { visible: true, allowedPageSizes: [10, 20, 50], showPageSizeSelector: true, showInfo: true, showNavigationButtons: true, displayMode: "full" },
            searchPanel: { visible: true, placeholder: LG().search, width: 240 },
            sorting: { mode: "multiple" },
            columnChooser: { enabled: true, mode: "select", height: 320, search: { enabled: true } },
            loadPanel: { enabled: true, text: LG().loading },
            noDataText: LG().noData,
            export: { enabled: true, formats: ["xlsx"] },
            onExporting: function (e) { exportGrid(e, L().title); },
            columns: [
                { dataField: "firstName", caption: L().firstName },
                { dataField: "lastName", caption: L().lastName },
                { dataField: "userName", caption: L().userName },
                { dataField: "email", caption: L().email },
                { dataField: "isActive", caption: L().isActive, dataType: "boolean", width: 90, alignment: "center" },
                {
                    type: "buttons", width: 150, caption: LG().actions, fixed: true, fixedPosition: "right",
                    visible: rowButtons.length > 0,
                    buttons: rowButtons
                }
            ],
            toolbar: { items: toolbarItems }
        }).dxDataGrid("instance");
    }

    function openCreate() {
        showFormPopup({
            title: L().createTitle,
            data: { firstName: "", lastName: "", userName: "", email: "", password: "", isActive: true, roleIds: [] },
            includePassword: true,
            includeRoles: true,
            onSave: function (data) {
                return window.AppHttp.post("/users", data);
            }
        });
    }

    function openEdit(id) {
        window.AppLoading.wrap(window.AppHttp.get("/users/" + id))
            .then(function (resp) {
                var u = resp.data;
                showFormPopup({
                    title: L().editTitle,
                    data: {
                        firstName: u.firstName, lastName: u.lastName, userName: u.userName,
                        email: u.email, phoneNumber: u.phoneNumber,
                        isActive: u.isActive, emailConfirmed: u.emailConfirmed,
                        phoneNumberConfirmed: u.phoneNumberConfirmed,
                        twoFactorEnabled: u.twoFactorEnabled,
                        lockoutEnabled: u.lockoutEnabled
                    },
                    onSave: function (data) {
                        return window.AppHttp.put("/users/" + id, data);
                    }
                });
            })
            .catch(window.AppNotify.fromHttpError);
    }

    function openRoles(row) {
        window.AppLoading.wrap(window.AppHttp.get("/users/" + row.id))
            .then(function (resp) {
                var selectedIds = (resp.data.roles || []).map(function (r) { return r.id; });
                var formData = { roleIds: selectedIds };

                var popup = $("<div>").appendTo("body").dxPopup({
                    title: L().rolesTitle + " — " + row.userName,
                    width: 480,
                    height: "auto",
                    maxWidth: "min(92vw, 960px)",
                    maxHeight: "min(88vh, 820px)",
                    dragEnabled: true,
                    resizeEnabled: true,
                    hideOnOutsideClick: true,
                    showCloseButton: true,
                    wrapperAttr: { class: "energy-popup" },
                    contentTemplate: function (host) {
                        $("<div>").appendTo(host).dxForm({
                            formData: formData,
                            labelLocation: "top",
                            colCount: 1,
                            items: [{
                                dataField: "roleIds",
                                label: { text: L().roles },
                                editorType: "dxTagBox",
                                editorOptions: {
                                    items: rolesLookup, valueExpr: "id", displayExpr: "name",
                                    showSelectionControls: true, applyValueMode: "useButtons",
                                    searchEnabled: true
                                }
                            }]
                        });
                    },
                    toolbarItems: [
                        { widget: "dxButton", location: "after", toolbar: "bottom",
                          options: { text: LG().save, type: "default",
                              onClick: function () {
                                  window.AppLoading.wrap(window.AppHttp.put("/users/" + row.id + "/roles", { roleIds: formData.roleIds }))
                                      .then(function () { window.AppNotify.success(LN().saved); popup.hide(); gridInstance.refresh(); })
                                      .catch(window.AppNotify.fromHttpError);
                              }
                          }},
                        { widget: "dxButton", location: "after", toolbar: "bottom",
                          options: { text: LG().cancel, onClick: function () { popup.hide(); } }}
                    ],
                    onHidden: function () { popup.dispose(); $(popup.element()).remove(); }
                }).dxPopup("instance");
                popup.show();
            })
            .catch(window.AppNotify.fromHttpError);
    }

    function openPassword(row) {
        var formData = { newPassword: "" };
        var popup = $("<div>").appendTo("body").dxPopup({
            title: L().passwordTitle + " — " + row.userName,
            width: 420,
            height: "auto",
            maxWidth: "min(92vw, 960px)",
            maxHeight: "min(88vh, 820px)",
            dragEnabled: true,
            resizeEnabled: true,
            hideOnOutsideClick: true,
            showCloseButton: true,
            wrapperAttr: { class: "energy-popup" },
            contentTemplate: function (host) {
                $("<div>").appendTo(host).dxForm({
                    formData: formData,
                    labelLocation: "top",
                    colCount: 1,
                    items: [{
                        dataField: "newPassword", label: { text: L().password },
                        editorOptions: { mode: "password", stylingMode: "outlined" },
                        validationRules: [{ type: "required", message: window.AppL10n.auth.fieldRequired }]
                    }]
                });
            },
            toolbarItems: [
                { widget: "dxButton", location: "after", toolbar: "bottom",
                  options: { text: LG().save, type: "default", onClick: function () {
                      if (!formData.newPassword) return;
                      window.AppLoading.wrap(window.AppHttp.put("/users/" + row.id + "/password", formData))
                          .then(function () { window.AppNotify.success(LN().saved); popup.hide(); })
                          .catch(window.AppNotify.fromHttpError);
                  }}},
                { widget: "dxButton", location: "after", toolbar: "bottom",
                  options: { text: LG().cancel, onClick: function () { popup.hide(); } }}
            ],
            onHidden: function () { popup.dispose(); $(popup.element()).remove(); }
        }).dxPopup("instance");
        popup.show();
    }

    function confirmDelete(row) {
        DevExpress.ui.dialog.confirm(LG().confirmDelete, LG().delete).then(function (ok) {
            if (!ok) return;
            window.AppLoading.wrap(window.AppHttp.del("/users/" + row.id))
                .then(function () { window.AppNotify.success(LN().deleted); gridInstance.refresh(); })
                .catch(window.AppNotify.fromHttpError);
        });
    }

    function showFormPopup(opts) {
        var formData = opts.data;
        var items = [
            { itemType: "group", colCount: 2, items: [
                { dataField: "firstName", label: { text: L().firstName }, validationRules: [{ type: "required" }] },
                { dataField: "lastName", label: { text: L().lastName }, validationRules: [{ type: "required" }] },
                { dataField: "userName", label: { text: L().userName }, validationRules: [{ type: "required" }] },
                { dataField: "email", label: { text: L().email } },
                { dataField: "phoneNumber", label: { text: L().phoneNumber } },
                { dataField: "isActive", editorType: "dxCheckBox", label: { text: L().isActive } }
            ]}
        ];
        if (opts.includePassword) {
            items[0].items.push({
                dataField: "password", label: { text: L().password },
                editorOptions: { mode: "password" },
                validationRules: [{ type: "required", message: window.AppL10n.auth.fieldRequired }]
            });
        }
        if (opts.includeRoles) {
            items.push({
                dataField: "roleIds", label: { text: L().roles },
                editorType: "dxTagBox",
                editorOptions: { items: rolesLookup, valueExpr: "id", displayExpr: "name", showSelectionControls: true, applyValueMode: "useButtons" }
            });
        }

        var popup = $("<div>").appendTo("body").dxPopup({
            title: opts.title,
            width: 720,
            height: "auto",
            maxWidth: "min(92vw, 960px)",
            maxHeight: "min(88vh, 820px)",
            dragEnabled: true,
            resizeEnabled: true,
            hideOnOutsideClick: true,
            showCloseButton: true,
            wrapperAttr: { class: "energy-popup" },
            contentTemplate: function (host) {
                $("<div>").appendTo(host).dxForm({ formData: formData, labelLocation: "top", colCount: 2, items: items });
            },
            toolbarItems: [
                { widget: "dxButton", location: "after", toolbar: "bottom",
                  options: { text: LG().save, type: "default", onClick: function () {
                      window.AppLoading.wrap(opts.onSave(formData))
                          .then(function () { window.AppNotify.success(LN().saved); popup.hide(); gridInstance.refresh(); })
                          .catch(window.AppNotify.fromHttpError);
                  }}},
                { widget: "dxButton", location: "after", toolbar: "bottom",
                  options: { text: LG().cancel, onClick: function () { popup.hide(); } }}
            ],
            onHidden: function () { popup.dispose(); $(popup.element()).remove(); }
        }).dxPopup("instance");
        popup.show();
    }

    window.AppPages = window.AppPages || {};
    window.AppPages.Users = { init: init };
})(window, jQuery);
