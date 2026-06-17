/*
 * UserAccess sayfası — kullanıcı bazlı erişim (rol + doğrudan yetki) yönetim ekranı.
 *
 * Sorumluluk:
 *   - Solda bir kullanıcı seçici grid (DevExtreme dxDataGrid), sağda o kullanıcı için
 *     rolleri ve yetkileri tamamen onay kutularıyla yöneten bir ayrıntı paneli sunar.
 *   - Rollerden devralınan (salt okunur) yetkileri ile doğrudan atanan yetkileri ayırt eder.
 *   - Tüm değişiklikleri (roller + doğrudan yetkiler) tek bir kaydetme işleminde gönderir.
 *   - Paylaşılan bileşen fabrikaları kullanmaz; kendi kendine yeten bir ekrandır.
 *
 * Genel API: window.AppPages.UserAccess.init().
 */
(function (window, $) {
    "use strict";

    // Yerelleştirme sözlüğü kısayolları (kullanıcı erişimi / grid / bildirimler / kullanıcılar).
    var UA = function () { return (window.AppL10n && window.AppL10n.userAccess) || {}; };
    var LG = function () { return window.AppL10n.userAccess.grid; };
    var LN = function () { return window.AppL10n.userAccess.notifications; };
    var LU = function () { return window.AppL10n.userAccess; };

    var usersGrid;
    var rolesLookup = [];          // [{ id, name, description, isSystem }]
    var permissionsCatalog = [];   // [{ code, name, module, action }]
    var selectedUser = null;       // o anda düzenlenen kullanıcı satırı
    var detail = null;             // canlı ayrıntı paneli bileşenlerine tutamaçlar

    function init() {
        Promise.all([loadRolesLookup(), loadPermissionsCatalog()])
            .then(function () {
                buildUsersGrid();
                renderEmptyDetail();
            });
    }

    function loadRolesLookup() {
        return window.AppHttp.get("/user-access/roles-lookup")
            .then(function (data) { rolesLookup = data || []; })
            .catch(function () { rolesLookup = []; });
    }

    function loadPermissionsCatalog() {
        return window.AppHttp.get("/user-access/permissions-catalog")
            .then(function (data) { permissionsCatalog = data || []; })
            .catch(function () { permissionsCatalog = []; });
    }

    // ---------------------------------------------------------------- Kullanıcılar grid'i
    function buildUsersGrid() {
        var store = new DevExpress.data.CustomStore({
            key: "id",
            load: function (loadOptions) {
                var params = $.param({
                    skip: loadOptions.skip || 0,
                    take: loadOptions.take || 20,
                    searchValue: loadOptions.searchValue || ""
                });
                return window.AppHttp.get("/user-access/users-list?" + params);
            }
        });

        usersGrid = $("#user-access-users").dxDataGrid({
            dataSource: store,
            remoteOperations: { paging: true, sorting: false, filtering: false },
            showBorders: true,
            showColumnLines: false,
            showRowLines: true,
            rowAlternationEnabled: true,
            hoverStateEnabled: true,
            columnAutoWidth: true,
            wordWrapEnabled: false,
            width: "100%",
            height: "78vh",
            scrolling: { mode: "standard", useNative: true, showScrollbar: "onScroll" },
            selection: { mode: "single" },
            focusedRowEnabled: true,
            keyExpr: "id",
            paging: { pageSize: 20 },
            pager: { visible: true, allowedPageSizes: [20, 50], showPageSizeSelector: true, showInfo: true, showNavigationButtons: true, displayMode: "compact" },
            searchPanel: { visible: true, placeholder: LG().search, width: 220 },
            loadPanel: { enabled: true, text: LG().loading },
            noDataText: LG().noData,
            columns: [
                { dataField: "fullName", caption: LU().firstName + " / " + LU().lastName },
                { dataField: "userName", caption: LU().userName },
                { dataField: "isActive", caption: LU().isActive, dataType: "boolean", width: 80, alignment: "center" }
            ],
            onSelectionChanged: function (e) {
                var row = (e.selectedRowsData || [])[0];
                if (row) { loadUserAccess(row); }
            }
        }).dxDataGrid("instance");
    }

    // ---------------------------------------------------------------- Ayrıntı paneli
    function renderEmptyDetail() {
        detail = null;
        $("#user-access-detail").empty().append(
            $("<div>").addClass("user-access__empty").append(
                $("<i>").addClass("dx-icon dx-icon-user user-access__empty-icon"),
                $("<p>").text(UA().selectUserPrompt || "Select a user to manage their access.")
            )
        );
    }

    function loadUserAccess(userRow) {
        selectedUser = userRow;
        window.AppLoading.wrap(window.AppHttp.get("/user-access/" + userRow.id + "/access"))
            .then(function (access) { renderDetail(userRow, access || {}); })
            .catch(window.AppNotify.fromHttpError);
    }

    function renderDetail(userRow, access) {
        var inheritedSet = {};
        (access.rolePermissionCodes || []).forEach(function (c) { inheritedSet[c] = true; });
        var directCodes = access.directPermissionCodes || [];
        var roleIds = access.roleIds || [];

        var $host = $("#user-access-detail").empty();

        // --- Başlık araç çubuğu: kim + kaydet -------------------------------------
        var $toolbar = $("<div>").appendTo($host);
        $toolbar.dxToolbar({
            items: [
                {
                    location: "before",
                    template: function () {
                        return $("<div>").addClass("user-access__who").append(
                            $("<span>").addClass("user-access__who-name").text(access.fullName || userRow.fullName || userRow.userName),
                            $("<span>").addClass("user-access__who-sub").text("@" + (access.userName || userRow.userName))
                        );
                    }
                },
                {
                    location: "after", widget: "dxButton",
                    options: {
                        icon: "save", text: LG().save, type: "default", stylingMode: "contained",
                        onClick: function () { save(userRow.id); }
                    }
                },
                {
                    location: "after", widget: "dxButton",
                    options: {
                        icon: "refresh", hint: LG().refresh, stylingMode: "text",
                        onClick: function () { loadUserAccess(userRow); }
                    }
                }
            ]
        });

        // --- Sekme paneli: Roller + Yetkiler ---------------------------------
        // Ayrıntı tutamacını sekme panelini oluşturmadan ÖNCE başlat: deferRendering:false
        // ile sekme şablonları yapım sırasında çalışır; bu yüzden detail.roleList /
        // detail.permTree'yi atayabilmeleri gerekir.
        detail = { roleList: null, permTree: null, inheritedSet: inheritedSet };

        var $tabs = $("<div>").appendTo($host);
        $tabs.dxTabPanel({
            height: "70vh",
            deferRendering: false,
            animationEnabled: true,
            swipeEnabled: false,
            items: [
                { title: UA().rolesTab || "Roles", icon: "group", template: rolesTabTemplate },
                { title: UA().permissionsTab || "Permissions", icon: "key", template: permissionsTabTemplate }
            ]
        });


        function rolesTabTemplate() {
            var $wrap = $("<div>").addClass("user-access__tab");
            $("<p>").addClass("user-access__note").text(UA().rolesNote || "Tick the roles this user should have.").appendTo($wrap);
            var $list = $("<div>").appendTo($wrap);
            detail.roleList = $list.dxList({
                dataSource: rolesLookup,
                keyExpr: "id",
                height: "58vh",
                selectionMode: "multiple",
                showSelectionControls: true,
                selectByClick: true,
                selectedItemKeys: roleIds.slice(),
                searchEnabled: true,
                searchExpr: ["name", "description"],
                pageLoadMode: "scrollBottom",
                noDataText: LG().noData,
                itemTemplate: function (item) {
                    var $row = $("<div>").addClass("user-access__role");
                    $("<span>").addClass("user-access__role-name").text(item.name).appendTo($row);
                    if (item.description) {
                        $("<span>").addClass("user-access__role-desc").text(item.description).appendTo($row);
                    }
                    if (item.isSystem) {
                        $("<span>").addClass("user-access__badge user-access__badge--system").text("system").appendTo($row);
                    }
                    return $row;
                }
            }).dxList("instance");
            return $wrap;
        }

        function permissionsTabTemplate() {
            var $wrap = $("<div>").addClass("user-access__tab");
            $("<p>").addClass("user-access__note").text(UA().permissionsNote || "Locked items are inherited from roles; tick extra permissions to grant directly.").appendTo($wrap);
            var $tree = $("<div>").appendTo($wrap);
            detail.permTree = $tree.dxTreeView({
                items: buildPermissionTreeItems(inheritedSet, directCodes),
                dataStructure: "plain",
                keyExpr: "id",
                parentIdExpr: "parentId",
                displayExpr: "text",
                height: "58vh",
                showCheckBoxesMode: "normal",
                selectNodesRecursive: true,
                selectByClick: true,
                searchEnabled: true,
                searchMode: "contains"
            }).dxTreeView("instance");
            return $wrap;
        }
    }

    function buildPermissionTreeItems(inheritedSet, directCodes) {
        var directSet = {};
        (directCodes || []).forEach(function (c) { directSet[c] = true; });

        var modules = {};
        var items = [];
        permissionsCatalog.forEach(function (p) {
            if (!modules[p.module]) {
                modules[p.module] = true;
                items.push({ id: "mod:" + p.module, parentId: null, text: p.module, expanded: false });
            }
            var inherited = !!inheritedSet[p.code];
            items.push({
                id: p.code,
                parentId: "mod:" + p.module,
                text: (p.name && p.name !== p.code ? p.name : (p.module + " " + p.action)) +
                      (inherited ? "  •  " + (UA().inherited || "from role") : ""),
                selected: inherited || !!directSet[p.code],
                disabled: inherited
            });
        });
        return items;
    }

    // ---------------------------------------------------------------- Kaydet
    function save(userId) {
        if (!detail) return;

        var roleIds = detail.roleList ? detail.roleList.option("selectedItemKeys") : [];

        var directCodes = [];
        if (detail.permTree) {
            var keys = detail.permTree.getSelectedNodeKeys() || [];
            keys.forEach(function (k) {
                if (typeof k === "string" && k.indexOf("mod:") !== 0 && !detail.inheritedSet[k]) {
                    directCodes.push(k);
                }
            });
        }

        var payload = { roleIds: roleIds, directPermissionCodes: directCodes };
        window.AppLoading.wrap(window.AppHttp.put("/user-access/" + userId + "/access", payload))
            .then(function () {
                window.AppNotify.success(UA().saved || LN().saved);
                loadUserAccess(selectedUser); // rol değişikliklerinden sonra devralınan/doğrudan durumu yenile
            })
            .catch(window.AppNotify.fromHttpError);
    }

    window.AppPages = window.AppPages || {};
    window.AppPages.UserAccess = { init: init };
})(window, jQuery);

