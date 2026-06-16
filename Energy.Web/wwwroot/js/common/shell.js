/*
 * AppShell — ana yerleşim (layout) kabuğu: kenar gezinme menüsü + kullanıcı menüsü.
 *
 * Sorumluluk:
 *   - Sunucudan gelen düz (flat) menü öğelerinden DevExtreme dxTreeView gezinme
 *     ağacını kurar; aktif sayfaya giden dalı otomatik açar ve seçer.
 *   - Gezinme çekmecesinin (drawer) açık/kapalı durumunu yönetir ve localStorage'da
 *     kalıcılaştırır; görünüm alanı (viewport) değişimlerine duyarlıdır
 *     (masaüstünde varsayılan açık, mobilde kapalı).
 *   - Araç çubuğundaki kullanıcı menüsünü (profil / çıkış) kurar; çıkış, CSRF jetonlu
 *     gizli bir form ile POST edilir.
 *
 * Genel API (window.AppShell): init().
 */
(function (window, $) {
    "use strict";

    // localStorage anahtarı ve masaüstü görünüm alanı eşiği.
    var NAV_STATE_KEY = "energy.nav.open";
    var DESKTOP_MEDIA_QUERY = "(min-width: 992px)";

    // Bir data-* özniteliğindeki JSON dizisini güvenli şekilde ayrıştırır (hata olursa boş dizi).
    function readJson(attr) {
        try { return JSON.parse(attr || "[]"); }
        catch (e) { return []; }
    }

    // Geçerli görünüm alanının masaüstü genişliğinde olup olmadığını döndürür.
    function isDesktop() {
        return window.matchMedia(DESKTOP_MEDIA_QUERY).matches;
    }

    // URL'i karşılaştırma için normalize eder (sondaki "/" karakterini kaldırır).
    function normalizeUrl(url) {
        if (!url) { return ""; }
        return url.length > 1 && url.charAt(url.length - 1) === "/"
            ? url.substring(0, url.length - 1)
            : url;
    }

    // Çekmecenin başlangıç durumunu çözer: kayıtlı tercih varsa onu, yoksa masaüstü/mobil varsayılanını kullanır.
    function readInitialNavState() {
        try {
            var saved = window.localStorage.getItem(NAV_STATE_KEY);
            if (saved === "open") { return true; }
            if (saved === "closed") { return false; }
        } catch (e) {
            // Depolama erişim sorunlarını yok say ve görünüm alanı varsayılanlarına geri dön.
        }

        return isDesktop();
    }

    // Çekmece durumunu localStorage'a yazar (güvenli, hata fırlatmaz).
    function persistNavState(isOpen) {
        try {
            window.localStorage.setItem(NAV_STATE_KEY, isOpen ? "open" : "closed");
        } catch (e) {
            // Depolama erişim sorunlarını yok say.
        }
    }

    // Çekmece durumunu gövdeye CSS sınıfı olarak uygular ve kalıcılaştırır.
    function applyNavState(isOpen) {
        $(document.body)
            .toggleClass("energy-nav-open", isOpen)
            .toggleClass("energy-nav-closed", !isOpen);

        persistNavState(isOpen);
    }

    // Geçerli URL'e en iyi eşleşen menü öğesini bulur (en uzun önek eşleşmesini tercih eder).
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

    // Aktif öğeden köke doğru ilerleyerek açık tutulması gereken üst düğüm kimliklerini toplar.
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

    // Gezinme çekmecesini kurar: ağaç görünümünü oluşturur, aç/kapa düğmesini ve
    // görünüm alanı değişikliklerini bağlar.
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
                    // dxTreeView'da "isItemExpanded" metodu yoktur; düğümün genişleme
                    // durumu doğrudan e.node.expanded üzerinden okunur. (Aksi hâlde
                    // "isItemExpanded is not a function" hatası verir ve menü açılıp
                    // kapanmazdı.)
                    if (e.node.expanded) {
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
            // Çekmece kapalıyken gezinme alanı sıfır genişliktedir; bu sırada oluşturulan
            // dxTreeView boyut ölçemediği için DevExtreme bir zamanlayıcıyla yeniden dener
            // (konsolda tekrarlayan W0004 uyarısı). Görünür olunca bir kez yeniden boyutlandır.
            if (isOpen) { try { treeInstance.repaint(); } catch (e) { /* yok say */ } }
        });

        var onViewportChange = function (event) {
            if (event.matches && !$(document.body).hasClass("energy-nav-open") && !$(document.body).hasClass("energy-nav-closed")) {
                applyNavState(true);
                try { treeInstance.repaint(); } catch (e) { /* yok say */ }
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

    // Araç çubuğundaki kullanıcı menüsünü kurar (profil bağlantısı + CSRF korumalı çıkış).
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
        // Kabuğu başlatır: gezinme ağacını (varsa) ve kullanıcı menüsünü kurar.
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
