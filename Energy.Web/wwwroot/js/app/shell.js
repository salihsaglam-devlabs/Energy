(function (window, $) {
    "use strict";

    var NAV_STATE_KEY = "energy.nav.open";
    var DESKTOP_MEDIA_QUERY = "(min-width: 992px)";

    function readJson(attr) {
        try { return JSON.parse(attr || "[]"); }
        catch (e) { return []; }
    }

    function isDesktop() {
        return window.matchMedia(DESKTOP_MEDIA_QUERY).matches;
    }

    function normalizeUrl(url) {
        if (!url) { return ""; }
        return url.length > 1 && url.charAt(url.length - 1) === "/"
            ? url.substring(0, url.length - 1)
            : url;
    }

    function readInitialNavState() {
        try {
            var saved = window.localStorage.getItem(NAV_STATE_KEY);
            if (saved === "open") { return true; }
            if (saved === "closed") { return false; }
        } catch (e) {
            // Ignore storage access issues and fall back to viewport defaults.
        }

        return isDesktop();
    }

    function persistNavState(isOpen) {
        try {
            window.localStorage.setItem(NAV_STATE_KEY, isOpen ? "open" : "closed");
        } catch (e) {
            // Ignore storage access issues.
        }
    }

    function applyNavState(isOpen) {
        $(document.body)
            .toggleClass("energy-nav-open", isOpen)
            .toggleClass("energy-nav-closed", !isOpen);

        persistNavState(isOpen);
    }

    function findActiveItem(items, activeUrl) {
        var normalizedActiveUrl = normalizeUrl(activeUrl);
        var matches = items.filter(function (item) {
            var itemUrl = normalizeUrl(item.url);
            return itemUrl && normalizedActiveUrl &&
                (normalizedActiveUrl === itemUrl || normalizedActiveUrl.indexOf(itemUrl + "/") === 0);
        });

        matches.sort(function (left, right) {
            return normalizeUrl(right.url).length - normalizeUrl(left.url).length;
        });

        return matches.length > 0 ? matches[0] : null;
    }

    function collectExpandedIds(items, activeItem) {
        if (!activeItem) {
            return {};
        }

        var byId = {};
        items.forEach(function (item) {
            byId[item.id] = item;
        });

        var expandedIds = {};
        var currentParentId = activeItem.parentId;

        while (currentParentId && byId[currentParentId]) {
            expandedIds[currentParentId] = true;
            currentParentId = byId[currentParentId].parentId;
        }

        return expandedIds;
    }

    function setupDrawer(items, activeUrl) {
        var isOpen = readInitialNavState();
        var $tree = $("#energy-navigation-tree");
        var $toggle = $("#energy-drawer-toggle");
        var activeItem = findActiveItem(items, activeUrl);
        var expandedIds = collectExpandedIds(items, activeItem);
        var mediaQuery = window.matchMedia(DESKTOP_MEDIA_QUERY);

        var treeItems = items.map(function (item) {
            return {
                id: item.id,
                parentId: item.parentId,
                text: item.text,
                icon: item.icon || (item.parentId ? "" : "folder"),
                url: item.url || "",
                expanded: !!expandedIds[item.id]
            };
        });

        applyNavState(isOpen);

        var treeInstance = $tree.dxTreeView({
            items: treeItems,
            dataStructure: "plain",
            parentIdExpr: "parentId",
            keyExpr: "id",
            displayExpr: "text",
            selectionMode: "single",
            expandNodesRecursive: false,
            itemTemplate: function (itemData, index, itemElement) {
                var $row = $("<div>").addClass("energy-nav-row");
                if (itemData.icon) {
                    $("<i>")
                        .addClass("dx-icon dx-icon-" + itemData.icon)
                        .addClass("energy-nav-row__icon")
                        .appendTo($row);
                }
                $("<span>").addClass("energy-nav-row__text").text(itemData.text).appendTo($row);
                $(itemElement).append($row);
            },
            onItemClick: function (e) {
                var hasChildren = e.node && e.node.children && e.node.children.length > 0;
                if (hasChildren) {
                    if (e.component.isItemExpanded(e.itemData.id)) {
                        e.component.collapseItem(e.itemData.id);
                    } else {
                        e.component.expandItem(e.itemData.id);
                    }

                    return;
                }

                var url = e.itemData.url;
                if (url && url.length > 0) {
                    if (!isDesktop()) {
                        applyNavState(false);
                    }

                    window.location.href = url;
                }
            },
            onContentReady: function (e) {
                if (activeItem) {
                    e.component.selectItem(activeItem.id);
                }
            }
        }).dxTreeView("instance");

        $toggle.off("click.energyNav").on("click.energyNav", function () {
            isOpen = !$(document.body).hasClass("energy-nav-open");
            applyNavState(isOpen);
        });

        var onViewportChange = function (event) {
            if (event.matches && !$(document.body).hasClass("energy-nav-open") && !$(document.body).hasClass("energy-nav-closed")) {
                applyNavState(true);
                return;
            }

            if (!event.matches && !$(document.body).hasClass("energy-nav-open") && !$(document.body).hasClass("energy-nav-closed")) {
                applyNavState(false);
            }
        };

        if (typeof mediaQuery.addEventListener === "function") {
            mediaQuery.addEventListener("change", onViewportChange);
        } else if (typeof mediaQuery.addListener === "function") {
            mediaQuery.addListener(onViewportChange);
        }

        return treeInstance;
    }

    function setupUserMenu() {
        var $btn = $("#energy-user-menu");
        if ($btn.length === 0) return;

        $btn.dxDropDownButton({
            text: $btn.find(".energy-toolbar__user-name").text(),
            icon: "user",
            stylingMode: "text",
            displayExpr: "text",
            keyExpr: "id",
            elementAttr: { class: "energy-toolbar__user-dd" },
            items: [
                { id: "profile", text: window.AppL10n.layout.profile, icon: "user" },
                { id: "logout", text: window.AppL10n.layout.signOut, icon: "runner" }
            ],
            onItemClick: function (e) {
                if (e.itemData.id === "profile") {
                    window.location.href = "/profile";
                    return;
                }
                if (e.itemData.id === "logout") {
                    var meta = document.querySelector('meta[name="csrf-token"]');
                    var token = meta ? meta.getAttribute("content") : "";
                    var form = document.createElement("form");
                    form.method = "POST";
                    form.action = window.AppContext.urls.logout;
                    var input = document.createElement("input");
                    input.type = "hidden";
                    input.name = "__RequestVerificationToken";
                    input.value = token;
                    form.appendChild(input);
                    document.body.appendChild(form);
                    form.submit();
                }
            }
        });
    }

    window.AppShell = {
        init: function () {
            var $nav = $("#energy-navigation");
            var items = readJson($nav.attr("data-items"));
            var activeUrl = $nav.attr("data-active-url") || "/";
            if ($nav.length > 0) {
                setupDrawer(items, activeUrl);
            }
            setupUserMenu();
        }
    };
})(window, jQuery);
